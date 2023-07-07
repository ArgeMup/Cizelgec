// Copyright ArgeMup GNU GENERAL PUBLIC LICENSE Version 3 <http://www.gnu.org/licenses/> <https://github.com/ArgeMup>

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace Çizelgeç
{
    public class Ekranlama_Ölü
    {
        public Dictionary<double, string> dosyalar = null;
        public int önceki_dosya_sırano = 0, sonraki_dosya_sırano = 0;

        public Ekranlama_Ölü(string DosyaYolu)
        {
            Günlük.Ekle("Ayıklanıyor -> " + DosyaYolu, "Bilgi");
            if (!File.Exists(DosyaYolu)) return;

            #region Diğer dosyaların tespit edilmesi
            string klasör = Path.GetDirectoryName(DosyaYolu);
            string[] gecici_dosyalar;
            dosyalar = new Dictionary<double, string>();
            önceki_dosya_sırano = -1;
            sonraki_dosya_sırano = -1;
            Ayıkla(DosyaYolu);
            if (DosyaYolu.ToLower().EndsWith("csv"))
            {
                gecici_dosyalar = Directory.GetFiles(klasör, "*.csv", SearchOption.TopDirectoryOnly);
            }
            else
            {
                gecici_dosyalar = Directory.GetFiles(klasör, "*.mup", SearchOption.TopDirectoryOnly);
            }

            foreach (var ds in gecici_dosyalar)
            {
                try { dosyalar.Add(S.Tarih.Sayıya(Path.GetFileNameWithoutExtension(ds)), ds); }
                catch (Exception) { }   
            }
            dosyalar = dosyalar.OrderBy(obj => obj.Key).ToDictionary(obj => obj.Key, obj => obj.Value);
            for (int i = 0; i < dosyalar.Values.Count; i++)
            {
                if (dosyalar.Values.ElementAt(i).ToLower() == DosyaYolu.ToLower())
                {
                    önceki_dosya_sırano = i - 1;
                    sonraki_dosya_sırano = i + 1;
                    break;
                }
            }
            #endregion
        }
        void Ayıkla(string DosyaYolu)
        {
            if (DosyaYolu.ToLower().EndsWith("csv")) Csv(DosyaYolu);
            else Mup(DosyaYolu);

            //ram tüketimi azaltmak için birbirinin aynı olan satır varmı kontrolü
            double[][] dizi_değer = new double[Sinyaller.Tümü.Count][];
            for (int i = 0; i < Sinyaller.Tümü.Count; i++) dizi_değer[i] = new double[S.ZamanEkseni.Length];                                          
            double[] dizi_zaman = new double[S.ZamanEkseni.Length];
            int Kaydedilen_adet = 0;
            int hatalı_zamandamgası_sebebiyle_atlanılan = 0;
            bool EnAz1AdetBoşGeçti = false;
       
            //ilk değer dizisinin eklenmesi
            for (int b = 0; b < Sinyaller.Tümü.Count && S.Çalışşsın; b++)
            {
                dizi_değer[b][Kaydedilen_adet] = Sinyaller.Tümü.Values.ElementAt(b).Değeri.DeğerEkseni[0];
            }
            dizi_zaman[Kaydedilen_adet] = S.ZamanEkseni[0];
            Kaydedilen_adet++;

            for (int a = 1; a < S.ZamanEkseni.Length && S.Çalışşsın; a++)
            {
                if (S.ZamanEkseni[a] <= S.ZamanEkseni[a - 1])
                {
                    Günlük.Ekle("Problemli zaman damgası -> " + S.Tarih.Yazıya(S.ZamanEkseni[a]) + " anı " + S.Tarih.Yazıya(S.ZamanEkseni[a - 1]) + " anından sonra gelmiş");
                    hatalı_zamandamgası_sebebiyle_atlanılan++;
                    continue;
                }

                bool farklı = false;
                for (int b = 0; b < Sinyaller.Tümü.Count && !farklı && S.Çalışşsın; b++)
                {
                    if (Sinyaller.Tümü.Values.ElementAt(b).Değeri.DeğerEkseni[a - 1] != Sinyaller.Tümü.Values.ElementAt(b).Değeri.DeğerEkseni[a])
                    {
                        farklı = true;
                    }
                }

                if (farklı)
                {
                    if (EnAz1AdetBoşGeçti)
                    {
                        //merdiven görüntüsü olabilmesi icin bir önceki elamanın eklenmesi
                        for (int b = 0; b < Sinyaller.Tümü.Count && S.Çalışşsın; b++)
                        {
                            dizi_değer[b][Kaydedilen_adet] = Sinyaller.Tümü.Values.ElementAt(b).Değeri.DeğerEkseni[a - 1];
                        }
                        dizi_zaman[Kaydedilen_adet] = S.ZamanEkseni[a - 1];
                        Kaydedilen_adet++;

                        EnAz1AdetBoşGeçti = false;
                    }

                    //farklı olan elemanın eklenmesi
                    for (int b = 0; b < Sinyaller.Tümü.Count && S.Çalışşsın; b++)
                    {
                        dizi_değer[b][Kaydedilen_adet] = Sinyaller.Tümü.Values.ElementAt(b).Değeri.DeğerEkseni[a];
                    }
                    dizi_zaman[Kaydedilen_adet] = S.ZamanEkseni[a];
                    Kaydedilen_adet++;
                }
                else EnAz1AdetBoşGeçti = true;
            }
            
            if (Kaydedilen_adet != S.ZamanEkseni.Length)
            {
                Günlük.Ekle("Toplam " + S.ZamanEkseni.Length + " girdinin " + hatalı_zamandamgası_sebebiyle_atlanılan + " adedi zaman damgası hatalı olduğundan ve " + (S.ZamanEkseni.Length - Kaydedilen_adet - hatalı_zamandamgası_sebebiyle_atlanılan) + " adedi birbirinin aynısı olduğundan ram den tasarruf etmek için elendi");

                for (int b = 0; b < Sinyaller.Tümü.Count && S.Çalışşsın; b++)
                {
                    Array.Resize(ref Sinyaller.Tümü.Values.ElementAt(b).Değeri.DeğerEkseni, Kaydedilen_adet);
                    Array.Copy(dizi_değer[b], Sinyaller.Tümü.Values.ElementAt(b).Değeri.DeğerEkseni, Kaydedilen_adet);
                }
                Array.Resize(ref S.ZamanEkseni, Kaydedilen_adet);
                Array.Copy(dizi_zaman, S.ZamanEkseni, Kaydedilen_adet);
            }

            S.CanliÇizdirme_ÖlçümSayısı = S.ZamanEkseni.Length;
        }
        void Csv(string CsvDosyasıYolu)
        {
            string Başlıklar = "";
            string Doğrulama = "";
            string HesaplananDoğrulama = "";
            int ÖlçümSayısı = 0;
            string SonÖlçüm = "";

            #region İçeriğin Sayılması
            using (StreamReader sr = new StreamReader(CsvDosyasıYolu))
            {
                bool hesabadahilet = true;
                while (sr.Peek() >= 0 && S.Çalışşsın)
                {
                    string okunan = sr.ReadLine();
                    if (okunan.Contains(";Sinyaller;")) { SonÖlçüm = okunan; ÖlçümSayısı++; }
                    else if (okunan.Contains(";Doğrulama;")) { Doğrulama = okunan; hesabadahilet = false; }
                    else if (okunan.Contains(";Başlıklar;")) Başlıklar = okunan;

                    if (hesabadahilet) HesaplananDoğrulama = ArgeMup.HazirKod.Dönüştürme.D_GeriDönülemezKarmaşıklaştırmaMetodu.Yazıdan(HesaplananDoğrulama + okunan + Environment.NewLine);
                }
            }

            if (string.IsNullOrEmpty(Doğrulama) || HesaplananDoğrulama != Doğrulama.Split(';')[2]) Günlük.Ekle("Dosya bütünlüğü doğrulanamadı, içerik eksik yada değiştirilmiş olabilir.");
            S.CanliÇizdirme_ÖlçümSayısı = ÖlçümSayısı;
            S.ZamanEkseni = new double[S.CanliÇizdirme_ÖlçümSayısı];
            #endregion

            #region Eksik Başlıkların Üretilmesi
            int adet_ölçüm = string.IsNullOrEmpty(SonÖlçüm) ? 0 : SonÖlçüm.Split(';').Length - 2 /*tarih ve başlık*/;
            if (adet_ölçüm == 0)
            {
                Günlük.Ekle("Hiç sinyal okunamadı");
                return;
            }

            int adet_başlık = 0;
            if (!string.IsNullOrEmpty(Başlıklar)) adet_başlık = Başlıklar.Split(';').Length;
            else Başlıklar = "t;t";

            for (; adet_başlık < adet_ölçüm; adet_başlık++) Başlıklar += ";s" + (adet_başlık + 1);
            #endregion

            #region Sinyaller
            string[] _sinyaller = Başlıklar.Split(';');

            #region Birbirinin aynı sinyal adı var ise numaralandirilmasi
            for(int a = 0; a < _sinyaller.Length; a++)
            {
                int adet = _sinyaller.Count(x => x == _sinyaller[a]);
                if (adet > 1)
                {
                    adet = 1;
                    for (int b = a + 1; b < _sinyaller.Length; b++)
                    {
                        if (_sinyaller[b] == _sinyaller[a])
                        {
                            _sinyaller[b] = _sinyaller[b] + adet;
                            adet++;
                        }
                    }
                }
            }
            #endregion  

            for (int i = 2; i < _sinyaller.Length; i++)
            {
                string salkım = "", görünenadı = "", sinyaladı = "";
                int son_ayraç = _sinyaller[i].LastIndexOf('|');
                if (son_ayraç < 0) görünenadı = _sinyaller[i];
                else
                {
                    görünenadı = _sinyaller[i].Substring(son_ayraç + 1);
                    salkım = _sinyaller[i].Substring(0, son_ayraç);
                }
                sinyaladı = "<" + _sinyaller[i].Trim('<', '>', ' ') + ">";

                Sinyal_ sinyal = Sinyaller.Ekle(sinyaladı);
                sinyal.Güncelle_Adı(sinyaladı, salkım, görünenadı);
                if (string.IsNullOrEmpty(salkım))
                {
                    sinyal.Tür = Tür_.Sinyal;
                    sinyal.Adı.Salkım = "";
                    sinyal.Adı.Csv = görünenadı;
                }

                sinyal.Değeri.DeğerEkseni = new double[S.CanliÇizdirme_ÖlçümSayısı];
            }
            #endregion

            #region Tamponların doldurulması
            int işlenen = 0, beklenenden_fazla_bilgi_içeren_hatalı_satır_sayısı = 0;
            int tik = Environment.TickCount;
            using (StreamReader sr = new StreamReader(CsvDosyasıYolu))
            {
                int SatırNo = 0;
                while (sr.Peek() >= 0 && S.Çalışşsın)
                {
                    if (Environment.TickCount - tik > 1000)
                    {
                        if (S.Ağaç.Nodes.Count == 0) S.Ağaç.Nodes.Add("Bekleyiniz");
                        S.Ağaç.Nodes[0].Text = Path.GetFileNameWithoutExtension(CsvDosyasıYolu) + " - %" + (işlenen * 100) / ÖlçümSayısı;
                        tik = Environment.TickCount;
                        Application.DoEvents();
                    }

                    string okunan = sr.ReadLine();
                    SatırNo++;
                    try
                    {
                        string[] bir_satırdakiler = okunan.Split(';');
                        double zaman = S.Tarih.Sayıya(bir_satırdakiler[0]);

                        if (okunan.Contains(";Sinyaller;"))
                        {
                            S.ZamanEkseni[işlenen] = zaman;

                            int eleman_sayısı = bir_satırdakiler.Length;
                            if (eleman_sayısı > Sinyaller.Tümü.Count + 2) eleman_sayısı = Sinyaller.Tümü.Count + 2;
                            for (int i = 2; i < eleman_sayısı; i++)
                            {
                                double okunan_sayı = S.Sayı.Yazıdan(bir_satırdakiler[i]);
                                if (double.IsNaN(okunan_sayı) || double.IsInfinity(okunan_sayı))
                                {
                                    Günlük.Ekle("Satır : " + SatırNo + ", eleman : " + (i + 1) + " içeriği ( " + bir_satırdakiler[i] + " ) sayı değil, 0 olarak değiştirildi.");
                                    okunan_sayı = 0;
                                }
                                
                                Sinyaller.Tümü.Values.ElementAt(i - 2).Değeri.DeğerEkseni[işlenen] = okunan_sayı;
                            }

                            işlenen++;

                            if (bir_satırdakiler.Length > Sinyaller.Tümü.Count + 2)
                            {
                                beklenenden_fazla_bilgi_içeren_hatalı_satır_sayısı++;
                            }
                        }
                        else if (okunan.Contains(";Uyarı;"))
                        {
                            //double y = S.Sayı.Yazıdan(bir_satırdakiler[3]);
                            //Sinyal_ sinyal = Sinyaller.Bul("<" + bir_satırdakiler[2] + ">");
                            //sinyal.Uyarı_Yazıları.Add(S.Çizelge.plt.PlotText(bir_satırdakiler[4], zaman, y, color: sinyal.Çizikler.color, rotation: 270));
                            
                            Günlük.Ekle(Path.GetFileName(CsvDosyasıYolu) + " satır " + SatırNo + " " + okunan);
                        }
                    }
                    catch (Exception ex) { Günlük.Ekle("Problemli satır -> (" + SatırNo + ") " + okunan + " -> " + ex.ToString()); }
                }

                if (beklenenden_fazla_bilgi_içeren_hatalı_satır_sayısı > 0)
                {
                    Günlük.Ekle("Beklenenden fazla bilgi içeren toplam " + beklenenden_fazla_bilgi_içeren_hatalı_satır_sayısı + " adet girdinin fazlalık kısımları atlandı");
                }
            }
            #endregion
        }
        void Mup(string MupDosyasıYolu)
        {
            int ÖlçümSayısı = 1;
            int tik = Environment.TickCount;

            #region ayarlar dosyası varmı kontrolü
            string kla = Path.GetDirectoryName(MupDosyasıYolu) + "\\Ayarlar.json";
            if (File.Exists(kla)) json_Ayıkla.Ayarlar(kla, false);
            foreach (var biri in Sinyaller.Tümü.Values)
            {
               biri.Değeri.DeğerEkseni = new double[S.CanliÇizdirme_ÖlçümSayısı];
            }
            S.ZamanEkseni = new double[S.CanliÇizdirme_ÖlçümSayısı];
            #endregion

            bool Enazbirtanedüzgüntarihbulundu = false;
            double bir_zamanlar = S.Tarih.Sayıya(new DateTime(2000, 1, 1));
            using (StreamReader sr = new StreamReader(MupDosyasıYolu))
            {
                int SatırNo = 0;
                while (sr.Peek() >= 0 && S.Çalışşsın)
                {
                    if (Environment.TickCount - tik > 1000)
                    {
                        if (S.Ağaç.Nodes.Count == 0) S.Ağaç.Nodes.Add("Bekleyiniz");
                        S.Ağaç.Nodes[0].Text = Path.GetFileNameWithoutExtension(MupDosyasıYolu) + " - %" + (ÖlçümSayısı * 100) / S.CanliÇizdirme_ÖlçümSayısı;
                        tik = Environment.TickCount;
                        Application.DoEvents();
                    }

                    string okunan = sr.ReadLine();
                    SatırNo++;
                    try
                    {
                        int başlangıç = okunan.IndexOf(S.MupDosyasındanOkuma_CümleBaşlangıcı);
                        if (başlangıç >= 0)
                        {
                            if (ÖlçümSayısı >= S.CanliÇizdirme_ÖlçümSayısı)
                            {
                                S.CanliÇizdirme_ÖlçümSayısı += 10000;
                                foreach (var biri in Sinyaller.Tümü.Values)
                                {
                                    Array.Resize(ref biri.Değeri.DeğerEkseni, S.CanliÇizdirme_ÖlçümSayısı);
                                }

                                Array.Resize(ref S.ZamanEkseni, S.CanliÇizdirme_ÖlçümSayısı);
                            }

                            foreach (var biri in Sinyaller.Tümü.Values)
                            {
                                biri.Değeri.DeğerEkseni[ÖlçümSayısı] = biri.Değeri.DeğerEkseni[ÖlçümSayısı - 1];
                            }

                            string gelen = okunan.Substring(başlangıç + S.MupDosyasındanOkuma_CümleBaşlangıcı.Length).Trim(' ', S.MupDosyasındanOkuma_KelimeAyracı);
                            string[] dizi = gelen.Split(S.MupDosyasındanOkuma_KelimeAyracı);
                            for (int i = 1; i < dizi.Length; i++)
                            {
                                string sinyal_yazı = "<" + dizi[0] + "[" + (i - 1).ToString() + "]>";
                                Sinyal_ sinyal = Sinyaller.Ekle(sinyal_yazı);
                                if (sinyal.Değeri.DeğerEkseni == null) sinyal.Değeri.DeğerEkseni = new double[S.CanliÇizdirme_ÖlçümSayısı];

                                double okunan_sayı = S.Sayı.Yazıdan(dizi[i]);
                                if (double.IsNaN(okunan_sayı) || double.IsInfinity(okunan_sayı))
                                {
                                    Günlük.Ekle("Satır : " + SatırNo + ", eleman : " + (i + 1) + " içeriği ( " + dizi[i] + " ) sayı değil, 0 olarak değiştirildi.");
                                    okunan_sayı = 0;
                                }

                                sinyal.Değeri.DeğerEkseni[ÖlçümSayısı] = okunan_sayı;
                            }

                            //tarihi okumaya calış
                            if (!S.Tarih.Sayıya(okunan.Split(S.MupDosyasındanOkuma_KelimeAyracı)[0], out S.ZamanEkseni[ÖlçümSayısı])) S.ZamanEkseni[ÖlçümSayısı] = ÖlçümSayısı;
                            else
                            {
                                if (S.ZamanEkseni[ÖlçümSayısı] > bir_zamanlar) Enazbirtanedüzgüntarihbulundu = true;
                            }

                            ÖlçümSayısı++;
                        }
                    }
                    catch (Exception ex) { Günlük.Ekle("Problemli satır -> (" + SatırNo + ") " + okunan + " -> " + ex.ToString()); }
                }
            }

            if (Sinyaller.Tümü.Count == 0 || ÖlçümSayısı <= 1)
            {
                throw new Exception("Hiç bilgi alınamadı. Cümle başlangıcı ve kelime ayracı uygun olmayabilir. Kaynak dosyanın olduğu klasörün içerisine kaynak dosya için düzenlenmiş bir Ayarlar.json dosyası kopyalayın ve Mup Dosyasından Okuma anahtarlarını doldurun");
            }

            #region daraltma
            ÖlçümSayısı--; //ilk değer boş
            S.CanliÇizdirme_ÖlçümSayısı = ÖlçümSayısı; 
            foreach (var biri in Sinyaller.Tümü.Values)
            {
                Array.Copy(biri.Değeri.DeğerEkseni, 1, biri.Değeri.DeğerEkseni, 0, S.CanliÇizdirme_ÖlçümSayısı);
                Array.Resize(ref biri.Değeri.DeğerEkseni, S.CanliÇizdirme_ÖlçümSayısı);
            }
            Array.Copy(S.ZamanEkseni, 1, S.ZamanEkseni, 0, S.CanliÇizdirme_ÖlçümSayısı);
            Array.Resize(ref S.ZamanEkseni, S.CanliÇizdirme_ÖlçümSayısı);
            #endregion

            #region Tarih Saatte olabilecek Eksiklikleri Giderme
            if (Enazbirtanedüzgüntarihbulundu)
            {
                DateTime ilk_zaman = DateTime.MinValue;
                double son_zaman = 0;
                for (int i = S.CanliÇizdirme_ÖlçümSayısı - 1; i >= 0; i--)
                {
                    if (S.ZamanEkseni[i] >= bir_zamanlar) { son_zaman = S.ZamanEkseni[i]; continue; }

                    ilk_zaman = S.Tarih.Tarihe(son_zaman) - TimeSpan.FromMilliseconds(S.ZamanEkseni[i] + 1000 /*son ölçümden 1sn önceye*/);
                    break;
                }

                if (ilk_zaman != DateTime.MinValue)
                {
                    for (int i = 0; i < S.CanliÇizdirme_ÖlçümSayısı; i++)
                    {
                        if (S.ZamanEkseni[i] >= bir_zamanlar) break;

                        S.ZamanEkseni[i] = S.Tarih.Sayıya(ilk_zaman + TimeSpan.FromMilliseconds(S.ZamanEkseni[i]));
                    }
                }
            }
            #endregion  
        }

        public bool Ekle_Önceki()
        {
            Ekle(önceki_dosya_sırano, false);
            return --önceki_dosya_sırano > -1;
        }
        public bool Ekle_Sonraki()
        {
            Ekle(sonraki_dosya_sırano, true);
            return ++sonraki_dosya_sırano < dosyalar.Values.Count;
        }
        void Ekle(int SıraNo, bool sonuna_ekle)
        {
            if (SıraNo < 0 || SıraNo >= dosyalar.Count) return;

            Dictionary<string, Sinyal_> Önceki_Sinyaller = new Dictionary<string, Sinyal_>(Sinyaller.Tümü);
            double[] Önceki_ZamanEkseni = S.ZamanEkseni;
            S.CanliÇizdirme_ÖlçümSayısı = 10000;
            Sinyaller.Tümü.Clear();

            //double tarih = dosyalar.Keys.ElementAt(SıraNo);
            string dosya = dosyalar.Values.ElementAt(SıraNo);
            Günlük.Ekle("Ayıklanıyor -> " + dosya, "Bilgi");
            
            try
            {
                Ayıkla(dosya);
            }
            catch (Exception ex) 
            {
                Sinyaller.Tümü = Önceki_Sinyaller;
                S.ZamanEkseni = Önceki_ZamanEkseni;
                S.CanliÇizdirme_ÖlçümSayısı = S.ZamanEkseni.Length;

                Günlük.Ekle(ex.Message);
                return;
            }

            int önceki_ölçüm_sayısı = Önceki_ZamanEkseni.Length;
            int şimdiki_ölçüm_sayısı = S.ZamanEkseni.Length;
            int ToplamÖlçümSaysı = önceki_ölçüm_sayısı + şimdiki_ölçüm_sayısı;

            foreach (var eski in Önceki_Sinyaller)
            {
                if (!Sinyaller.MevcutMu(eski.Key)) Sinyaller.Doğrudan_Ekle(eski.Key, eski.Value);

                Sinyal_ yeni = Sinyaller.Bul(eski.Key);
                Array.Resize(ref yeni.Değeri.DeğerEkseni, ToplamÖlçümSaysı);
                if (sonuna_ekle)
                {
                    Array.Copy(yeni.Değeri.DeğerEkseni, 0, yeni.Değeri.DeğerEkseni, önceki_ölçüm_sayısı, şimdiki_ölçüm_sayısı);
                    Array.Copy(eski.Value.Değeri.DeğerEkseni, 0, yeni.Değeri.DeğerEkseni, 0, önceki_ölçüm_sayısı);
                }
                else
                {
                    Array.Copy(eski.Value.Değeri.DeğerEkseni, 0, yeni.Değeri.DeğerEkseni, şimdiki_ölçüm_sayısı, önceki_ölçüm_sayısı);
                }
            }

            foreach (var biri in Sinyaller.Tümü.Values)
            {
                if (biri.Değeri.DeğerEkseni == null || biri.Değeri.DeğerEkseni.Length < ToplamÖlçümSaysı)
                {
                    Array.Resize(ref biri.Değeri.DeğerEkseni, ToplamÖlçümSaysı);
                    if (sonuna_ekle) Array.Copy(biri.Değeri.DeğerEkseni, 0, biri.Değeri.DeğerEkseni, önceki_ölçüm_sayısı, şimdiki_ölçüm_sayısı);
                }
            }

            Array.Resize(ref S.ZamanEkseni, ToplamÖlçümSaysı);
            if (sonuna_ekle)
            {
                Array.Copy(S.ZamanEkseni, 0, S.ZamanEkseni, önceki_ölçüm_sayısı, şimdiki_ölçüm_sayısı);
                Array.Copy(Önceki_ZamanEkseni, 0, S.ZamanEkseni, 0, önceki_ölçüm_sayısı);
            }
            else
            {
                Array.Copy(Önceki_ZamanEkseni, 0, S.ZamanEkseni, şimdiki_ölçüm_sayısı, önceki_ölçüm_sayısı);
            }

            S.CanliÇizdirme_ÖlçümSayısı = ToplamÖlçümSaysı;
        }
    }
}

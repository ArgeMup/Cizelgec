// Copyright ArgeMup GNU GENERAL PUBLIC LICENSE Version 3 <http://www.gnu.org/licenses/> <https://github.com/ArgeMup>

using ArgeMup.HazirKod;
using ArgeMup.HazirKod.DonanımHaberleşmesi;
using ArgeMup.HazirKod.Ekİşlemler;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace Çizelgeç
{
    #region Bağlantılar
    public enum Bağlantı_Türü_ { Boşta, SeriPort, KomutSatırı, Tcpİstemci, TcpSunucu, Udpİstemci, UdpSunucu };
    public class Bağlantı_Ayıklama_
    {
        public string CümleBaşlangıcı_Sinyaller = ">Sinyaller";
        public string CümleBaşlangıcı_Baslıklar = ">Basliklar";
        public char KelimeAyracı = ';';
        public bool Kaydedilsin = true;
        public Yardımcıİşlemler.Sinyaller.Tanımlanmamış_Sinyali TanımlanmamışSinyalleri = Yardımcıİşlemler.Sinyaller.Tanımlanmamış_Sinyali.Kullan;
    }

    public class Bağlantı_
    {
        public string Adı = "";
        public Bağlantı_Türü_ Türü = Bağlantı_Türü_.Boşta;
        public Bağlantı_Ayıklama_[] Ayıklama = null;
        public bool Kaydedilsin = false;
        public string P1 = "";
        public string P2 = "";
        public List<string> SonGelenBilgiler = new List<string>();
        public TreeNode Dal = null;
        
        public bool DaldakiYazıyıGüncelleVeKabart = true;
        public DateTime SonDeğerinAlındığıAn = DateTime.Now;
        public UInt64 Sayac_Güncelleme = 0;

        #region Bağlantının İçeriğinin Dosyaya Kaydedilmesi
        public string Kaydet_DosyaAdı = "";
        public int Kaydet_DosyaBoyutu = 0;
        #endregion

        public IDonanımHaberlleşmesi Haberleşme = null;

        public GelenBilgiler_ GelenBilgiler = new GelenBilgiler_();

        public void Ayıklayıcı_Ekle(string CümleBaşlangıcı_Sinyaller = ">Sinyaller", char KelimeAyracı = ';', bool Kaydedilsin = true, Yardımcıİşlemler.Sinyaller.Tanımlanmamış_Sinyali TanımlanmamışSinyalleri = Yardımcıİşlemler.Sinyaller.Tanımlanmamış_Sinyali.Kullan, string CümleBaşlangıcı_Başlıklar = ">Basliklar")
        {
            if (Ayıklama == null) Ayıklama = new Bağlantı_Ayıklama_[1];
            else Array.Resize(ref Ayıklama, Ayıklama.Length + 1);

            Bağlantı_Ayıklama_ yeni = new Bağlantı_Ayıklama_();
            Ayıklama[Ayıklama.Length - 1] = yeni;
            yeni.CümleBaşlangıcı_Sinyaller = CümleBaşlangıcı_Sinyaller;
            yeni.CümleBaşlangıcı_Baslıklar = CümleBaşlangıcı_Başlıklar;
            yeni.KelimeAyracı = KelimeAyracı;
            yeni.Kaydedilsin = Kaydedilsin;
            yeni.TanımlanmamışSinyalleri = TanımlanmamışSinyalleri;
        }
        public void Başlat()
        {
            Kaydedilsin = false;
            foreach (Bağlantı_Ayıklama_ a in Ayıklama)
            {
                if (a.Kaydedilsin)
                {
                    if (!string.IsNullOrEmpty(Yardımcıİşlemler.BilgiToplama.Kayıt_Klasörü))
                    {
                        Kaydedilsin = true;
                    }
                    break;
                }
            }

            if (Kaydedilsin)
            {
                Yardımcıİşlemler.BilgiToplama.Kayıt_Klasörü = Yardımcıİşlemler.BilgiToplama.Kayıt_Klasörü.Trim('\\', ' ') + "\\";
                if (!Klasör.Oluştur(Yardımcıİşlemler.BilgiToplama.Kayıt_Klasörü)) throw new Exception(Yardımcıİşlemler.BilgiToplama.Kayıt_Klasörü + " klasörü oluşturulamadı");
                
                File.WriteAllText(Yardımcıİşlemler.BilgiToplama.Kayıt_Klasörü + "silinecek.mup", "deneme");
                if (!File.Exists(Yardımcıİşlemler.BilgiToplama.Kayıt_Klasörü + "silinecek.mup")) throw new Exception(Yardımcıİşlemler.BilgiToplama.Kayıt_Klasörü + " içerisinde dosya oluşturma denemesi başarısız");
                File.Delete(Yardımcıİşlemler.BilgiToplama.Kayıt_Klasörü + "silinecek.mup");
                
                string Kaydet_DosyaAdı = Yardımcıİşlemler.BilgiToplama.Kayıt_Klasörü + "Bağlantılar\\" + S.DosyaKlasörAdınıDüzelt(Adı);
                //cs dosyasını üret
                string ayıklayıcılar = null;
                foreach (Bağlantı_Ayıklama_ ayklayc in Ayıklama)
                {
                    ayıklayıcılar += @"Yardımcıİşlemler.MupDosyasındanOkuma.CümleBaşlangıcıVeKelimeAyraçları.Add(new string[] { """ + ayklayc.CümleBaşlangıcı_Sinyaller + @""" , """ + ayklayc.KelimeAyracı + @""" , """ + ayklayc.CümleBaşlangıcı_Baslıklar + @""" });" + Environment.NewLine;
                }
                Properties.Resources.ÖrnekKaynakKod_ÖlüEkranlama_mup_dosyası.Replace("??? [[[ Detaylar ]]] %%%", ayıklayıcılar).Dosyaİçeriği_Yaz(Kaydet_DosyaAdı + "\\Ölü_Ekranlama_Ayarlar.cs");
            }
            else Günlük.Ekle("Dosyalama KayıtKlasörü seçilmedi, ölçümler kaydedilmeyecek. Bağlantı :" + Adı);

            _Aç();

            GelenBilgiler.Başlat(this);
        }
        public void Gönder(string Mesaj)
        {
            Haberleşme.Gönder(Mesaj, Türü == Bağlantı_Türü_.Udpİstemci ? P1 + ":" + P2 : null);

            GelenBilgiler.Ekle(Mesaj, "Giden");
        }
        public void Gönder(byte[] Mesaj)
        {
            Haberleşme.Gönder(Mesaj, Türü == Bağlantı_Türü_.Udpİstemci ? P1 + ":" + P2 : null);

            GelenBilgiler.Ekle(Mesaj.Length + "B ham blgi", "Giden");
        }
        public void Sil()
        {
            if (Dal != null)
            {
                if (S.AnaEkran.InvokeRequired)
                {
                    S.AnaEkran.Invoke(new Action(() => { Dal?.Remove(); }));
                }
                else Dal?.Remove();
            }

            Haberleşme.Durdur();
            GelenBilgiler.Çalışsın = false;
            Bağlantılar.Tümü.Remove(Adı);
        }
        void BilgiGeldiVeyaDurumDeğişikliği(string Kaynak, GeriBildirim_Türü_ Tür, object İçerik, object Hatırlatıcı)
        {
            Bağlantı_ kendi = (Bağlantı_)Hatırlatıcı;

            if (Tür == GeriBildirim_Türü_.BilgiGeldi) kendi.GelenBilgiler.Ekle((string)İçerik);
            else if (Tür == GeriBildirim_Türü_.BağlantıKurulmasıTekrarDenecek) return;
            else
            {
                Günlük.Ekle(kendi.Adı + " " + Tür.ToString() + " (Eğer sürekli olarak tekrar ediyorsa ayarlar hatalı olabilir)");
                kendi.GelenBilgiler.Ekle(kendi.Adı + " " + Kaynak + " " + Tür.ToString(), "Açıklama");
            }
        }
    
        public void _Aç()
        {
            if (Haberleşme != null) return;

            switch (Türü)
            {
                case Bağlantı_Türü_.KomutSatırı:
                    if (!File.Exists(P1))
                    {
                        string[] lst = Directory.GetFiles(S.AnaKlasör, P1, SearchOption.AllDirectories);
                        if (lst.Length == 0)
                        {
                            lst = Directory.GetFiles(Yardımcıİşlemler.BilgiToplama.Kayıt_Klasörü, P1, SearchOption.AllDirectories);
                            if (lst.Length == 0) throw new Exception(P1 + " bulunamadı");
                        }

                        P1 = lst[0];
                        Günlük.Ekle(Adı + " bağlantısının P1 parametresi değiştirildi -> " + P1, "Bilgi");
                    }

                    Haberleşme = new KomutSatırıUygulaması_(P1, P2, BilgiGeldiVeyaDurumDeğişikliği, this);
                    break;

                case Bağlantı_Türü_.SeriPort:
                    Haberleşme = new SeriPort_(P1, int.Parse(P2), BilgiGeldiVeyaDurumDeğişikliği, this);
                    break;

                case Bağlantı_Türü_.Tcpİstemci:
                    Haberleşme = new Tcpİstemci_(int.Parse(P2), P1, BilgiGeldiVeyaDurumDeğişikliği, this);
                    break;

                case Bağlantı_Türü_.TcpSunucu:
                    Haberleşme = new TcpSunucu_(int.Parse(P1), BilgiGeldiVeyaDurumDeğişikliği, this, SadeceYerel: false);
                    break;

                case Bağlantı_Türü_.UdpSunucu:
                    Haberleşme = new UdpDinleyici_(int.Parse(this.P1), BilgiGeldiVeyaDurumDeğişikliği, this, SadeceYerel: false);
                    break;

                default:
                    break;
            }
        }
        public void _Kapat()
        {
            if (Haberleşme == null) return;
            
            Haberleşme.Durdur();
            Haberleşme = null;
        }
    };
    public class Bağlantılar
    {
        static Mutex Mtx = new Mutex();
        public static Dictionary<string, Bağlantı_> Tümü = new Dictionary<string, Bağlantı_>();
        public static bool UygunMu(string Adı)
        {
            return Adı.Trim().Length > 0;
        }
        public static bool MevcutMu(string Adı)
        {
            return Tümü.ContainsKey(Adı);
        }
        public static Bağlantı_ Ekle(string Adı)
        {
            if (!UygunMu(Adı)) throw new Exception(Adı + " bağlantı adı olarak uygun değil");
            if (MevcutMu(Adı)) throw new Exception("Bağlantı adı " + Adı + " zaten eklendi");

            Bağlantı_ yeni = new Bağlantı_();
            yeni.Adı = Adı;

            Mtx.WaitOne();
            Tümü[Adı] = yeni;
            Mtx.ReleaseMutex();

            return yeni;
        }
        public static Bağlantı_ Bul(string Adı)
        {
            Bağlantı_ girdi;
            bool sonuç = Tümü.TryGetValue(Adı, out girdi);

            if (sonuç) return girdi;

            throw new Exception(Adı + " isimli bağlantı listede bulunamadı");
        }
        public static void Tamamen_Durdur()
        {
            foreach (var biri in Tümü.Values)
            {
                biri._Kapat();
            }

            Tümü.Clear();
        }

        public static void _Aç()
        {
            foreach (var biri in Tümü.Values)
            {
                biri._Aç();
            }
        }
        public static void _Kapat()
        {
            foreach (var biri in Tümü.Values)
            {
                biri._Kapat();
            }
        }
    }
    #endregion

    #region Bağlantılardan Gelen Bilgilerin Ayıklanması, Kaydedilmesi ve Depolanması
    public class GelenBilgi_
    {
        public string Bilgi;

        public DateTime Zaman;
        public string Tür;
    }
    public class GelenBilgiler_
    {
        public bool Çalışsın = true;
        Bağlantı_ Bağlantı;
        public void Başlat(Bağlantı_ Bağlantı)
        {
            this.Bağlantı = Bağlantı;

            if (Bağlantı.Kaydedilsin)
            {
                Görev_Nesnesi_Kaydetme = new Thread(() => Görev_İşlemi_Kaydetme());
                Görev_Nesnesi_Kaydetme.Start();
            }

            Görev_Nesnesi_Ayıklama = new Thread(() => Görev_İşlemi_Ayıklama());
            Görev_Nesnesi_Ayıklama.Start();
        }

        /// <param name="Tür">Gelen bilgi için null olmalı</param>
        public void Ekle(string Bilgi, string Tür = null)
        {
            if (Bilgi == null) return;

            GelenBilgi_ yeni = new GelenBilgi_();
            yeni.Bilgi = Bilgi;

            if (Tür == null && Bilgi != "")
            {
                Mtx_Ayıklama.WaitOne();
                Tümü_Ayıklama.Add(yeni);
                Mtx_Ayıklama.ReleaseMutex();
            }

            if (string.IsNullOrEmpty(Tür)) Tür = "Gelen";

            if (Bağlantı.Kaydedilsin)
            {
                yeni.Tür = Tür;
                yeni.Zaman = DateTime.Now;

                Mtx_Kaydetme.WaitOne();
                Tümü_Kaydetme.Add(yeni);
                Mtx_Kaydetme.ReleaseMutex();
            }

            if (Bağlantı.SonGelenBilgiler.Count < 10) Bağlantı.SonGelenBilgiler.Add(Tür + " " + Bilgi.Trim('\r', '\n'));

            Bağlantı.DaldakiYazıyıGüncelleVeKabart = true;
            Bağlantı.SonDeğerinAlındığıAn = DateTime.Now;
            Bağlantı.Sayac_Güncelleme++;
        }

        #region Ayıklama
        Mutex Mtx_Ayıklama = new Mutex();
        public List<GelenBilgi_> Tümü_Ayıklama = new List<GelenBilgi_>();
        Thread Görev_Nesnesi_Ayıklama = null;
        void Görev_İşlemi_Ayıklama()
        {
            while (S.Çalışşsın && Çalışsın)
            {
                try
                {
                    if (Tümü_Ayıklama.Count == 0)
                    {
                        Thread.Sleep(Yardımcıİşlemler.BilgiToplama.ZamanDilimi_msn <= 1 ? 1 : Yardımcıİşlemler.BilgiToplama.ZamanDilimi_msn);
                        continue;
                    }
                    else if (Tümü_Ayıklama.Count < 100) Thread.Sleep(1); //cpu yüzdesini düşürmek için

                    Mtx_Ayıklama.WaitOne();

                    GelenBilgi_ sıradaki = Tümü_Ayıklama[0];
                    if (Yardımcıİşlemler.BilgiToplama.BaşlatBeklet)
                    {
                        if (Tümü_Ayıklama.Count > 100000)
                        {
                            Tümü_Ayıklama.RemoveRange(0, 90000);

                            string mesaj = Bağlantı.Adı + " bağlantısı çok fazla ayıklanamamış girdiye sahip oldugundan ilk 90000 girdi silindi.";
                            Kaydedici.Ekle(DateTime.Now, null, mesaj);
                            Günlük.Ekle(mesaj);
                        }
                        else Tümü_Ayıklama.RemoveAt(0);
                    }
                    else Tümü_Ayıklama.Clear();
                    Mtx_Ayıklama.ReleaseMutex();

                    foreach (Bağlantı_Ayıklama_ a in Bağlantı.Ayıklama)
                    {
                        int başlangıç = sıradaki.Bilgi.LastIndexOf(a.CümleBaşlangıcı_Sinyaller);
                        if (başlangıç >= 0)
                        {
                            string gelen = sıradaki.Bilgi.Substring(başlangıç + a.CümleBaşlangıcı_Sinyaller.Length).Trim(' ', a.KelimeAyracı);
                            string[] dizi = gelen.Split(a.KelimeAyracı);
                            for (int i = 1; i < dizi.Length; i++)
                            {
                                string sinyal_yazı = "<" + dizi[0] + "[" + (i - 1).ToString() + "]>";

                                bool TanımlanmamışVeKaydetme = false;
                                if (a.TanımlanmamışSinyalleri != Yardımcıİşlemler.Sinyaller.Tanımlanmamış_Sinyali.Kullan)
                                {
                                    if (!Sinyaller.MevcutMu(sinyal_yazı))
                                    {
                                        if (a.TanımlanmamışSinyalleri == Yardımcıİşlemler.Sinyaller.Tanımlanmamış_Sinyali.Atla) continue;
                                        else TanımlanmamışVeKaydetme = true;
                                    }
                                }

                                try
                                {
                                    Sinyal_ sny = Sinyaller.Ekle(sinyal_yazı);
                                    sny.Güncelle_SonDeğer(dizi[i]);
                                    if (TanımlanmamışVeKaydetme) sny.Değeri.Kaydedilsin = false;
                                }
                                catch (Exception) { Günlük.Ekle(Bağlantı.Adı + " bağlantısının " + (i + 1) + ". elemanı -> " + dizi[i] + "  <- sayıya dönüştürülemedi."); }
                            }

                            break;
                        }
                        else
                        {
                            başlangıç = sıradaki.Bilgi.LastIndexOf(a.CümleBaşlangıcı_Baslıklar);
                            if (başlangıç >= 0)
                            {
                                string gelen = sıradaki.Bilgi.Substring(başlangıç + a.CümleBaşlangıcı_Baslıklar.Length).Trim(' ', a.KelimeAyracı);
                                string[] dizi = gelen.Split(a.KelimeAyracı);
                                for (int i = 1; i < dizi.Length; i++)
                                {
                                    string sinyal_yazı = "<" + dizi[0] + "[" + (i - 1).ToString() + "]>";

                                    bool TanımlanmamışVeKaydetme = false;
                                    if (a.TanımlanmamışSinyalleri != Yardımcıİşlemler.Sinyaller.Tanımlanmamış_Sinyali.Kullan)
                                    {
                                        if (!Sinyaller.MevcutMu(sinyal_yazı))
                                        {
                                            if (a.TanımlanmamışSinyalleri == Yardımcıİşlemler.Sinyaller.Tanımlanmamış_Sinyali.Atla) continue;
                                            else TanımlanmamışVeKaydetme = true;
                                        }
                                    }

                                    try
                                    {
                                        Sinyal_ sny = Sinyaller.Ekle(sinyal_yazı);
                                        sny.Güncelle_Adı(sinyal_yazı, dizi[0], dizi[i]);
                                        sny.Güncelle_SonDeğer(0);
                                        if (TanımlanmamışVeKaydetme) sny.Değeri.Kaydedilsin = false;
                                    }
                                    catch (Exception) { Günlük.Ekle(Bağlantı.Adı + " bağlantısının " + (i + 1) + ". elemanı -> " + dizi[i] + "  <- eklenemedi."); }
                                }

                                Kaydedici.Basliklari_Yenile();

                                break;
                            }
                        }
                    }
                }
                catch (Exception ex) { Günlük.Ekle(ex.ToString()); Thread.Sleep(1000); }
            }
        }
        #endregion

        #region Kaydetme
        Mutex Mtx_Kaydetme = new Mutex();
        public List<GelenBilgi_> Tümü_Kaydetme = new List<GelenBilgi_>();
        Thread Görev_Nesnesi_Kaydetme = null;
        void Görev_İşlemi_Kaydetme()
        {
            DateTime EnSonKayıtAnı = DateTime.Now;
    
            while (S.Çalışşsın && Çalışsın)
            {
                try
                {
                    if (Tümü_Kaydetme.Count == 0)
                    {
                        //kayıt yok
                        EnSonKayıtAnı = DateTime.Now;
                        Thread.Sleep(1500);
                        continue;
                    }
                    else if ((DateTime.Now - EnSonKayıtAnı).TotalSeconds < 60 && Tümü_Kaydetme.Count < 350)
                    {
                        //şimdilik bekle
                        Thread.Sleep(1500);
                        continue;
                    }
                    EnSonKayıtAnı = DateTime.Now;

                    while (Tümü_Kaydetme.Count > 15 && S.Çalışşsın && Çalışsın)
                    {
                        Mtx_Kaydetme.WaitOne();
                        GelenBilgi_ sıradaki = Tümü_Kaydetme[0];
                        if (Yardımcıİşlemler.BilgiToplama.BaşlatBeklet)
                        {
                            if (Tümü_Kaydetme.Count > 100000)
                            {
                                Tümü_Kaydetme.RemoveRange(0, 90000);

                                string mesaj = Bağlantı.Adı + " bağlantısı çok fazla kaydedilememiş girdiye sahip oldugundan ilk 90000 girdi silindi.";
                                Görev_İşlemi_DosyayaKaydet(mesaj);
                                Günlük.Ekle(mesaj);
                            }
                            else Tümü_Kaydetme.RemoveAt(0);
                        }
                        else Tümü_Kaydetme.Clear();
                        Mtx_Kaydetme.ReleaseMutex();

                        Görev_İşlemi_DosyayaKaydet(S.Tarih.Yazıya(sıradaki.Zaman) + " " + sıradaki.Tür + " " + sıradaki.Bilgi.Trim('\r', '\n') + Environment.NewLine);

                        Thread.Sleep(1); //cpu yüzdesini düşürmek için
                    }
                }
                catch (Exception ex) { Günlük.Ekle(ex.ToString()); Thread.Sleep(1000); }
            }

            //çıkış öncesi elde kalan bilgilerin kaydedilmesi
            try
            {
                if (Tümü_Kaydetme.Count > 10000) Tümü_Kaydetme.RemoveRange(0, Tümü_Kaydetme.Count - 10000);

                while (Tümü_Kaydetme.Count > 0)
                {
                    GelenBilgi_ sıradaki = Tümü_Kaydetme[0];
                    if (Yardımcıİşlemler.BilgiToplama.BaşlatBeklet) Tümü_Kaydetme.RemoveAt(0);
                    else Tümü_Kaydetme.Clear();

                    Görev_İşlemi_DosyayaKaydet(S.Tarih.Yazıya(sıradaki.Zaman) + " " + sıradaki.Tür + " " + sıradaki.Bilgi.Trim('\r', '\n') + Environment.NewLine);
                }
            }
            catch (Exception) { }
        }
        void Görev_İşlemi_DosyayaKaydet(string Mesaj)
        {
            if (!File.Exists(Bağlantı.Kaydet_DosyaAdı))
            {
                Bağlantı.Kaydet_DosyaAdı = Yardımcıİşlemler.BilgiToplama.Kayıt_Klasörü + "Bağlantılar\\" + S.DosyaKlasörAdınıDüzelt(Bağlantı.Adı);
                Directory.CreateDirectory(Bağlantı.Kaydet_DosyaAdı);

                Bağlantı.Kaydet_DosyaAdı += "\\" + S.Tarih.Yazıya(DateTime.Now, S.Tarih._Şablon_dosyaadı) + ".mup";
                Bağlantı.Kaydet_DosyaBoyutu = 0;
            }

            File.AppendAllText(Bağlantı.Kaydet_DosyaAdı, Mesaj);
            Bağlantı.Kaydet_DosyaBoyutu += Mesaj.Length;
            if (Bağlantı.Kaydet_DosyaBoyutu > Yardımcıİşlemler.BilgiToplama.Kayıt_AzamiDosyaBoyutu_Bayt) Bağlantı.Kaydet_DosyaAdı = "";
        }
        #endregion
    }
    #endregion

    #region Düzenlenmiş Sinyallerin Dosyaya Kaydedilmesi
    public class Kayıt_
    {
        public DateTime TarihSaat;
        public string mesaj;
        public double[] sinyaller;
    }
    public class Kaydedici
    {
        static int BaşlıkSayısı = 0;
        public static void Ekle(DateTime TarihSaat, double[] Sinyaller = null, string Mesaj = "")
        {
            if (Görev_Nesnesi == null)
            {
                if (string.IsNullOrEmpty(Yardımcıİşlemler.BilgiToplama.Kayıt_Klasörü)) return;

                Görev_Nesnesi = new Thread(() => Görev_İşlemi());
                Görev_Nesnesi.Start();
            }

            Kayıt_ yeni = new Kayıt_();
            yeni.TarihSaat = TarihSaat;
            yeni.sinyaller = Sinyaller;
            yeni.mesaj = Mesaj;

            Mtx.WaitOne();
            Tümü.Add(yeni);
            Mtx.ReleaseMutex();
        }
        public static void Basliklari_Yenile()
        {
            BaşlıkSayısı = 0;
        }

        static Mutex Mtx = new Mutex();
        static public List<Kayıt_> Tümü = new List<Kayıt_>();
        static public int YeniKayıtDosyasıSayısı = 1;

        static string DosyaBütünlüğüKodu = "";
        static Thread Görev_Nesnesi = null;
        static void Görev_İşlemi()
        {
            DateTime EnSonKayıtAnı = DateTime.Now;
            string DosyaYolu = "";
            
            while (S.Çalışşsın)
            {
                try
                {
                    if (Tümü.Count == 0)
                    {
                        //kayıt yok
                        EnSonKayıtAnı = DateTime.Now;
                        Thread.Sleep(1500);
                        continue;
                    }
                    else if (Yardımcıİşlemler.BilgiToplama.BaşlatBeklet && 
                            (DateTime.Now - EnSonKayıtAnı).TotalSeconds < 60 && 
                            Tümü.Count < 350)
                    {
                        //şimdilik bekle
                        Thread.Sleep(1500);
                        continue;
                    }
                    EnSonKayıtAnı = DateTime.Now;

                    if (!File.Exists(DosyaYolu))
                    {
                        DosyaYolu = Yardımcıİşlemler.BilgiToplama.Kayıt_Klasörü + S.Tarih.Yazıya(DateTime.Now, S.Tarih._Şablon_dosyaadı) + ".csv";
                        BaşlıkSayısı = 0;
                        DosyaBütünlüğüKodu = "";
                    }

                    int işlenen = 0;
                    using (FileStream fs = File.OpenWrite(DosyaYolu))
                    {
                        fs.Position = fs.Length;

                        if (BaşlıkSayısı != Sinyaller.Tümü.Count)
                        {
                            Thread.Sleep(3500); //Yeni sinyallerin tüm özelliklerinin eklenemsi adımlarının tamamlanabilmesi için

                            BaşlıkSayısı = Sinyaller.Tümü.Count;

                            string yazı = S.Tarih.Yazıya(DateTime.Now) + ";Başlıklar";
                            foreach (var biri in Sinyaller.Tümü)
                            {
                                if (biri.Value.Değeri.Kaydedilsin)
                                {
                                    yazı += ";" + biri.Value.Adı.Csv;
                                }
                            }
                            yazı += Environment.NewLine;
                            Ekle(fs, yazı);
                        }

                        while (işlenen < Tümü.Count && S.Çalışşsın)
                        {
                            Kayıt_ sıradaki = Tümü[işlenen];
                            işlenen++;

                            string yazı = S.Tarih.Yazıya(sıradaki.TarihSaat);

                            if (string.IsNullOrEmpty(sıradaki.mesaj))
                            {
                                //sinyaller 
                                if (sıradaki.sinyaller == null) continue;

                                yazı += ";Sinyaller";
                                for (int i = 0; i < sıradaki.sinyaller.Length; i++)
                                {
                                    if (Sinyaller.Tümü.ElementAt(i).Value.Değeri.Kaydedilsin)
                                    {
                                        yazı += ";" + S.Sayı.Yazıya(sıradaki.sinyaller[i]);
                                    }
                                }
                            }
                            else
                            {
                                //Açıklama
                                yazı += ";Açıklama;" + sıradaki.mesaj;
                            }
                            yazı += Environment.NewLine;
                            Ekle(fs, yazı);

                            if (Yardımcıİşlemler.BilgiToplama.BaşlatBeklet && işlenen > 350)
                            {
                                Thread.Sleep(1); //cpu yüzdesini düşürmek için
                                break;
                            }
                        }

                        if (fs.Length > Yardımcıİşlemler.BilgiToplama.Kayıt_AzamiDosyaBoyutu_Bayt || !Yardımcıİşlemler.BilgiToplama.BaşlatBeklet)
                        {
                            DosyaYolu = "";

                            string yazı = S.Tarih.Yazıya(DateTime.Now) + ";Bilgi;ArGeMuP Çizelgeç V" + Application.ProductVersion + Environment.NewLine;
                            Ekle(fs, yazı);
                            yazı = S.Tarih.Yazıya(DateTime.Now) + ";Bilgi;" + S.SonDurumMesajı.Replace(Environment.NewLine, " ") + Environment.NewLine;
                            Ekle(fs, yazı);
                            yazı = S.Tarih.Yazıya(DateTime.Now) + ";Bilgi;" + (Yardımcıİşlemler.BilgiToplama.BaşlatBeklet ? "Azami dosya boyutuna ulaşıldığı için " : "Uygulama gecici olarak durdurulduğu için ") + YeniKayıtDosyasıSayısı++ + ". dosya olarak kaydedildi" + Environment.NewLine;
                            Ekle(fs, yazı);
                            yazı = S.Tarih.Yazıya(DateTime.Now) + ";Doğrulama;" + DosyaBütünlüğüKodu;
                            Ekle(fs, yazı);
                        }
                    }

                    if (işlenen > 0)
                    {
                        Mtx.WaitOne();
                        Tümü.RemoveRange(0, işlenen);
                        Mtx.ReleaseMutex();
                    }
                }
                catch (Exception ex) { Günlük.Ekle(ex.ToString()); Thread.Sleep(1000); }
            }

            if (Tümü.Count > 0)
            {
                try
                {
                    if (!File.Exists(DosyaYolu))
                    {
                        DosyaYolu = Yardımcıİşlemler.BilgiToplama.Kayıt_Klasörü + S.Tarih.Yazıya(DateTime.Now, S.Tarih._Şablon_dosyaadı) + ".csv";
                        BaşlıkSayısı = 0;
                        DosyaBütünlüğüKodu = "";
                    }

                    int işlenen = 0;
                    string yazı = "";
                    using (FileStream fs = File.OpenWrite(DosyaYolu))
                    {
                        fs.Position = fs.Length;

                        if (BaşlıkSayısı != Sinyaller.Tümü.Count)
                        {
                            Thread.Sleep(350); //Yeni sinyallerin tüm özelliklerinin eklenemsi adımlarının tamamlanabilmesi için

                            yazı = S.Tarih.Yazıya(DateTime.Now) + ";Başlıklar";
                            foreach (var biri in Sinyaller.Tümü)
                            {
                                if (biri.Value.Değeri.Kaydedilsin)
                                {
                                    yazı += ";" + biri.Value.Adı.Csv;
                                }
                            }
                            yazı += Environment.NewLine;
                            Ekle(fs, yazı);
                        }

                        while (işlenen < Tümü.Count)
                        {
                            Kayıt_ sıradaki = Tümü[işlenen];
                            işlenen++;

                            yazı = S.Tarih.Yazıya(sıradaki.TarihSaat);

                            if (string.IsNullOrEmpty(sıradaki.mesaj))
                            {
                                //sinyaller 
                                if (sıradaki.sinyaller == null) continue;

                                yazı += ";Sinyaller";
                                for (int i = 0; i < sıradaki.sinyaller.Length; i++)
                                {
                                    if (Sinyaller.Tümü.ElementAt(i).Value.Değeri.Kaydedilsin)
                                    {
                                        yazı += ";" + S.Sayı.Yazıya(sıradaki.sinyaller[i]);
                                    }
                                }
                            }
                            else
                            {
                                //Açıklama
                                yazı += ";Açıklama;" + sıradaki.mesaj;
                            }
                            yazı += Environment.NewLine;
                            Ekle(fs, yazı);
                        }

                        yazı = S.Tarih.Yazıya(DateTime.Now) + ";Bilgi;ArGeMuP Çizelgeç V" + Application.ProductVersion + Environment.NewLine;
                        Ekle(fs, yazı);
                        yazı = S.Tarih.Yazıya(DateTime.Now) + ";Bilgi;" + S.SonDurumMesajı.Replace(Environment.NewLine, " ") + Environment.NewLine;
                        Ekle(fs, yazı);
                        yazı = S.Tarih.Yazıya(DateTime.Now) + ";Bilgi;Uygulama kapatıldığı için " + YeniKayıtDosyasıSayısı + ". dosya olarak kaydedildi" + Environment.NewLine;
                        Ekle(fs, yazı);
                        yazı = S.Tarih.Yazıya(DateTime.Now) + ";Doğrulama;" + DosyaBütünlüğüKodu;
                        Ekle(fs, yazı);
                    }
                }
                catch (Exception) {  }
            }
            else if (File.Exists(DosyaYolu) && BaşlıkSayısı > 0 && !string.IsNullOrEmpty(DosyaBütünlüğüKodu))
            {
                string yazı = S.Tarih.Yazıya(DateTime.Now) + ";Bilgi;ArGeMuP Çizelgeç V" + Application.ProductVersion + Environment.NewLine;
                Ekle(DosyaYolu, yazı);
                yazı = S.Tarih.Yazıya(DateTime.Now) + ";Bilgi;" + S.SonDurumMesajı.Replace(Environment.NewLine, " ") + Environment.NewLine;
                Ekle(DosyaYolu, yazı);
                yazı = S.Tarih.Yazıya(DateTime.Now) + ";Bilgi;Uygulama kapatıldığı için " + YeniKayıtDosyasıSayısı + ". dosya olarak kaydedildi" + Environment.NewLine;
                Ekle(DosyaYolu, yazı);
                yazı = S.Tarih.Yazıya(DateTime.Now) + ";Doğrulama;" + DosyaBütünlüğüKodu;
                Ekle(DosyaYolu, yazı);
            }
        }
        static void Ekle(FileStream fs, string yazı)
        {
            byte[] dizi = Encoding.UTF8.GetBytes(yazı);
            fs.Write(dizi, 0, dizi.Length);

            DosyaBütünlüğüKodu = ArgeMup.HazirKod.Dönüştürme.D_GeriDönülemezKarmaşıklaştırmaMetodu.Yazıdan(DosyaBütünlüğüKodu + yazı);
        }
        static void Ekle(string DosyaYolu, string yazı)
        {
            File.AppendAllText(DosyaYolu, yazı);

            DosyaBütünlüğüKodu = ArgeMup.HazirKod.Dönüştürme.D_GeriDönülemezKarmaşıklaştırmaMetodu.Yazıdan(DosyaBütünlüğüKodu + yazı);
        }
    }
    #endregion
}

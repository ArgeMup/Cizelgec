// Copyright ArgeMup GNU GENERAL PUBLIC LICENSE Version 3 <http://www.gnu.org/licenses/> <https://github.com/ArgeMup>

using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

namespace Çizelgeç
{
    public class Ekranlama_Canlı
    {
        UInt64 Sayac_Ölçüm = 0, Sayac_BirbirininAynısıOlduğuİçinAtlandı = 0;
        TreeNode tn_Senaryolar = null;
        public string İşAdı = "";
        public Ekranlama_Canlı(string İşAdı, string AyarlarDosyasıYolu)
        {
            Günlük.Ekle("Ayıklanıyor -> " + AyarlarDosyasıYolu, "Bilgi");
            S.Ağaç.Nodes.Clear();
            S.Ağaç.CheckBoxes = false;
            S.Ağaç.Nodes.Add("Bekleyiniz");
            Application.DoEvents();

            Günlük.Ekle("Ayıklanıyor : " + AyarlarDosyasıYolu + "\\Ayarlar.json", "Bilgi");
            json_Ayıkla.Ayarlar(AyarlarDosyasıYolu + "\\Ayarlar.json");

            string[] snylar = Directory.GetFiles(AyarlarDosyasıYolu + "\\", "Senaryo*.json", SearchOption.TopDirectoryOnly);
            foreach (var sny in snylar) { Günlük.Ekle("Ayıklanıyor : " + sny, "Bilgi"); json_Ayıkla.Senaryo(sny); }

            Çevirici.Yazıdan_NoktalıSayıya(S.BilgiToplama_Kıstas);
            Çevirici.Yazıdan_NoktalıSayıya(S.BilgiToplama_ZamanAralığı_Sn);
            Çevirici.Yazıdan_NoktalıSayıya(S.Dosyalama_AzamiDosyaBoyutu_Bayt);

            if (!string.IsNullOrEmpty(S.Dosyalama_KayıtKlasörü))
            {
                S.Dosyalama_KayıtKlasörü = S.Dosyalama_KayıtKlasörü.Trim('\\', ' ') + "\\";
                Directory.CreateDirectory(S.Dosyalama_KayıtKlasörü);
                File.WriteAllText(S.Dosyalama_KayıtKlasörü + "silinecek.mup", "deneme");
                if (!File.Exists(S.Dosyalama_KayıtKlasörü + "silinecek.mup")) throw new Exception(S.Dosyalama_KayıtKlasörü + " içerisinde dosya oluşturma denemesi başarısız");
                File.Delete(S.Dosyalama_KayıtKlasörü + "silinecek.mup");
            }
            else Günlük.Ekle("Dosyalama KayıtKlasörü seçilmedi, ölçümler kaydedilmeyecek.");

            this.İşAdı = İşAdı;
            Ekranlama.AğaçVeÇizelge_Görsellerini_Üret(İşAdı);
            TreeNode[] snyler  = S.Ağaç.Nodes[0].Nodes.Find("Senaryolar", false);
            if (snyler != null && snyler.Length > 0) tn_Senaryolar = snyler[0];

            new Thread(() => Çalıştır_Ölçme_Değerlendirme()).Start();
            new Thread(() => Çalıştır_Ekranlama()).Start();

            foreach (var biri in Bağlantılar.Tümü)
            {
                if (biri.Value.Kaydedilsin && !string.IsNullOrEmpty(S.Dosyalama_KayıtKlasörü))
                {
                    string Kaydet_DosyaAdı = S.Dosyalama_KayıtKlasörü + "Bağlantılar\\" + S.DosyaKlasörAdınıDüzelt(biri.Value.Adı);
                    Directory.CreateDirectory(Kaydet_DosyaAdı);
                    Kaydet_DosyaAdı += "\\Ayarlar.json";

                    File.WriteAllText(Kaydet_DosyaAdı, json_Üret.MupDosyasıİçin_Ayarlar(AyarlarDosyasıYolu + "\\Ayarlar.json", biri.Value.CümleBaşlangıcı, biri.Value.KelimeAyracı.ToString()));
                }

                Günlük.Ekle("Çalıştırılıyor -> " + biri.Value.Türü.ToString() + " -> " + biri.Value.P1 + " -> " + biri.Value.P2, "Bilgi");
                Bağlantılar.Başlat(biri.Value.Adı);
            }

            foreach (var biri in Senaryolar.Tümü.Values)
            {
                if (biri.HemenÇalıştır)
                {
                    Günlük.Ekle("Çalıştırılıyor -> " + biri.Adı, "Bilgi");
                    biri.Çalıştır();
                }
            }
        }

        public void Çalıştır_Ölçme_Değerlendirme()
        {
            double[] kayıt_dizisi = new double[1];

            while (S.Çalışşsın)
            {
                try
                {
                    if (S.BaşlatDurdur && Çevirici.Yazıdan_NoktalıSayıya(S.BilgiToplama_Kıstas) > 0.0)
                    {
                        double şimdi = S.Tarih.Sayıya(DateTime.Now);
                        int gecikme = 1000;
                        int toplam = Sinyaller.Tümü.Count;

                        if (toplam > 0)
                        {
                            bool ekle = false;

                            if (S.BilgiToplama_BirbirininAynısıOlanZamanDilimleriniAtla)
                            {
                                if (Sinyaller.EnAzBirSinyalDeğişti_KaydedilmesiGereken)
                                {
                                    Sinyaller.EnAzBirSinyalDeğişti_KaydedilmesiGereken = false;

                                    #if MerdivenGörünümüİçin
                                        kayıt_dizisi[kayıt_dizisi.Length - 1] = şimdi;
                                        Kaydedici.Ekle(kayıt_dizisi);
                                        Sayac_Ölçüm++;
                                    #endif

                                    ekle = true;
                                }

                                #if MerdivenGörünümüİçin
                                    S.ZamanEkseni[S.ZamanEkseni.Length - 1] = şimdi; //atlandığı zamanlarda ekranın çizdirmeye devam etmesi için
                                #endif
                            }
                            else ekle = true;

                            if (ekle)
                            {
                                kayıt_dizisi = new double[toplam + 1];

                                kayıt_dizisi[toplam] = şimdi;

                                for (int i = 0; i < toplam; i++)
                                {
                                    Sinyal_ biri = Sinyaller.Tümü.ElementAt(i).Value;

                                    kayıt_dizisi[i] = biri.Güncelle_Dizi();
                                }

                                #if MerdivenGörünümüİçin
                                    if (!S.BilgiToplama_BirbirininAynısıOlanZamanDilimleriniAtla)
                                    {        
                                #endif
                                        Array.Copy(S.ZamanEkseni, 1, S.ZamanEkseni, 0, S.ZamanEkseni.Length - 1);
                                        S.ZamanEkseni[S.ZamanEkseni.Length - 1] = şimdi;
                                #if MerdivenGörünümüİçin
                                    }
                                    else
                                    {
                                        Array.Copy(S.ZamanEkseni, 2, S.ZamanEkseni, 0, S.ZamanEkseni.Length - 2);
                                        S.ZamanEkseni[S.ZamanEkseni.Length - 1] = şimdi;
                                        S.ZamanEkseni[S.ZamanEkseni.Length - 2] = şimdi;
                                    }                              
                                #endif

                                Kaydedici.Ekle(kayıt_dizisi);
                                Sayac_Ölçüm++;
                            }
                            else Sayac_BirbirininAynısıOlduğuİçinAtlandı++;

                            foreach (var biri in Sinyaller.Tümü.Values)
                            {
                                if (!biri.Güncelle_ZamanAşımıOlduMu_SadeceTekYerdenÇağırılabilir())
                                {
                                    if (Sinyaller.EnAzBirSinyalDeğişti)
                                    {
                                        if (biri.Hesaplamalar != null)
                                        {
                                            foreach (var hsp in biri.Hesaplamalar)
                                            {
                                                Sinyaller.Yaz(hsp.Değişken, Çevirici.Yazıdan_NoktalıSayıya(hsp.İşlem.Replace("<Sinyal>", S.Sayı.Yazıya(biri.Değeri.SonDeğeri))));
                                            }
                                        }

                                        if (biri.Uyarılar != null)
                                        {
                                            foreach (var uyr in biri.Uyarılar)
                                            {
                                                string kıstas = uyr.Kıstas.Replace("<Sinyal>", S.Sayı.Yazıya(biri.Değeri.SonDeğeri));
                                                if (Çevirici.Yazıdan_NoktalıSayıya(kıstas) > 0.0)
                                                {
                                                    string mesaj = Çevirici.Uyarıdan_Yazıya(uyr.Açıklama, biri.Değeri.SonDeğeri);
                                                    mesaj = biri.Adı.Csv + ";" + S.Sayı.Yazıya(biri.Değeri.SonDeğeri) + ";" + mesaj;

                                                    Kaydedici.Ekle(new double[1] { şimdi }, mesaj);
                                                    Günlük.Ekle(mesaj);
                                                }
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    string mesaj = biri.Adı.Csv + ";" + S.Sayı.Yazıya(biri.Değeri.SonDeğeri) + ";Zaman Aşımı";

                                    //Kaydedici.Ekle(new double[1] { şimdi }, mesaj);
                                    Günlük.Ekle(mesaj);
                                }
                            }
                            Sinyaller.EnAzBirSinyalDeğişti = false;

                            gecikme = (int)(Çevirici.Yazıdan_NoktalıSayıya(S.BilgiToplama_ZamanAralığı_Sn) * 1000);
                            while (gecikme > 1500 && S.Çalışşsın)
                            {
                                Thread.Sleep(1500);
                                gecikme -= 1500;
                            }
                        }
                        Thread.Sleep(gecikme);
                    }
                    else Thread.Sleep(1); //cpu yüzdesini düşürmek için
                }
                catch (Exception ex) { Günlük.Ekle(ex.ToString()); Thread.Sleep(1000); }
            }
        }
        public void Çalıştır_Ekranlama()
        {
            UInt64 Önceki_Sayac_Ölçüm = 0;
            int tik_kaydetmiyor_uyarısı = 0;
            DateTime BaşladığıAn = DateTime.Now;
            TimeSpan fark;
            bool _1sn_gecti;
            int _SonZamanDamgası = 0;
            string SonDurumMesajı = "";
            S.Çizdir_Ortalama.Ortalaması = 1000;

            while (S.Çalışşsın)
            {
                try
                {
                    if (_SonZamanDamgası < Environment.TickCount)
                    {
                        _1sn_gecti = true;
                        _SonZamanDamgası = Environment.TickCount + 1000;
                    }
                    else _1sn_gecti = false;

                    if (S.Çizdir_Ortalama.Ortalaması > 3500) S.Çizdir_Ortalama.Ortalaması = 3500;
                    Thread.Sleep((int)S.Çizdir_Ortalama.Ortalaması);
                    
                    if (_1sn_gecti)
                    {
                        fark = DateTime.Now - BaşladığıAn;
                        SonDurumMesajı = İşAdı + " - " + Sayac_Ölçüm + " - " + S.Tarih.Yazıya(DateTime.Now);
                        S.SonDurumMesajı = SonDurumMesajı + Environment.NewLine +
                                            S.Tarih.Yazıya(BaşladığıAn) + " zamanından beri" + Environment.NewLine +
                                            ArgeMup.HazirKod.Dönüştürme.D_Süre.Yazıya.SaatDakikaSaniye(0, 0, (int)fark.TotalSeconds) + " boyunca" + Environment.NewLine +
                                            Sinyaller.Tümü.Count + " adet sinyal" + Environment.NewLine +
                                            Kaydedici.Tümü.Count + " adet yazılmayı bekleyen ölçüm elde edildi ve" + Environment.NewLine +
                                            Sayac_BirbirininAynısıOlduğuİçinAtlandı + " adet girdi birbirinin aynısı olduğu için atlandı";
                    }

                    S.Çizelge.Invoke((Action)(() =>
                    {
                        if (S.AnaEkran.WindowState != FormWindowState.Minimized)
                        {
                            if (!S.Ayraç_Ana.Panel1Collapsed)
                            {
                                if (_1sn_gecti)
                                {
                                    S.Ağaç.BeginUpdate();

                                    if (S.Ağaç.Nodes[0].ImageIndex > 0) S.Ağaç.Nodes[0].ImageIndex--;
                                    else
                                    {
                                        if (S.Ağaç.Nodes[0].IsVisible)
                                        {
                                            S.Ağaç.Nodes[0].Text = SonDurumMesajı;
                                            S.Ağaç.Nodes[0].ToolTipText = S.SonDurumMesajı;

                                            S.İmleçKonumuTarihi.Visible = false;
                                        }
                                        else
                                        {
                                            S.İmleçKonumuTarihi.Text = SonDurumMesajı;
                                            S.İmleçKonumuTarihi.Visible = true;
                                        }
                                    }

                                    for (int i = 0; i < Sinyaller.Tümü.Count; i++)
                                    {
                                        Sinyal_ biri = Sinyaller.Tümü.Values.ElementAt(i);

                                        if (biri.Görseller.Dal == null) Ekranlama.AğaçVeÇizelge_SonradanEkle(Sinyaller.Tümü.ElementAt(i));

                                        if (biri.Görseller.Dal.IsVisible && S.Ağaç.Nodes[0].ImageIndex <= 0) //Ağaç değerlerinin dışarıdan çizdirildiği durumda güncelleme
                                        {
                                            biri.Görseller.Dal.Text = biri.Adı.GörünenAdı + " : " + S.Sayı.Yazıya(biri.Değeri.SonDeğeri);

                                            if (biri.Değeri.ZamanAşımıOldu)
                                            {
                                                biri.Görseller.Dal.Text += " -> Zaman Aşımı -> ";

                                                fark = DateTime.Now - biri.Değeri.SonDeğerinAlındığıAn;
                                                biri.Görseller.Dal.Text += ArgeMup.HazirKod.Dönüştürme.D_Süre.Yazıya.SaatDakikaSaniye(0, 0, (int)fark.TotalSeconds);
                                            }

                                            biri.Görseller.Dal.ToolTipText = Sinyaller.Tümü.Keys.ElementAt(i) + (biri.Tür == Tür_.Sinyal ? " sinyali" : " değişkeni") + Environment.NewLine +
                                                                    biri.Değeri.Sayac_Güncelleme + ". kez " + S.Tarih.Yazıya(biri.Değeri.SonDeğerinAlındığıAn) + " tarihinde güncellendi" + Environment.NewLine +
                                                                    "ve " + (biri.Değeri.Kaydedilsin ? "kaydediliyor" : "KAYDEDİLMİYOR");

                                            if (biri.Görseller.DaldakiYazıyıGüncelleVeKabart)
                                            {
                                                biri.Görseller.DaldakiYazıyıGüncelleVeKabart = false;
                                                biri.Görseller.Dal.ImageIndex = 10;
                                            }
                                            else if (biri.Görseller.Dal.ImageIndex > 0) biri.Görseller.Dal.ImageIndex--;
                                        }
                                    }

                                    foreach (var biri in Bağlantılar.Tümü.Values)
                                    {
                                        if (biri.Dal.IsVisible)
                                        {
                                            if (biri.DaldakiYazıyıGüncelleVeKabart)
                                            {
                                                biri.DaldakiYazıyıGüncelleVeKabart = false;
                                                biri.Dal.ImageIndex = 10;

                                                biri.Dal.ToolTipText = biri.Adı + Environment.NewLine +
                                                                        biri.Türü.ToString() + Environment.NewLine +
                                                                        biri.P1 + Environment.NewLine +
                                                                        biri.P2 + Environment.NewLine +
                                                                        biri.Sayac_Güncelleme + ". kez " + S.Tarih.Yazıya(biri.SonDeğerinAlındığıAn) + " tarihinde güncellendi," + Environment.NewLine +
                                                                        biri.GelenBilgiler.Tümü_Ayıklama.Count + "/" + biri.GelenBilgiler.Tümü_Kaydetme.Count + " adet ayıklanmayı bekleyen girdi";
                                            }
                                            else if (biri.Dal.ImageIndex > 0) biri.Dal.ImageIndex--;

                                            if (biri.Dal.Nodes[0].IsVisible)
                                            {
                                                int adet = biri.SonGelenBilgiler.Count;
                                                for (int i = 0; i < adet; i++)
                                                {
                                                    biri.Dal.Nodes.Add(biri.SonGelenBilgiler[i]);
                                                    if (biri.Dal.Nodes.Count > 10) biri.Dal.Nodes.RemoveAt(0);
                                                }
                                                biri.SonGelenBilgiler.Clear();
                                            }
                                        }
                                    }

                                    if (tn_Senaryolar != null && tn_Senaryolar.IsVisible)
                                    {
                                        foreach (var sny in Senaryolar.Tümü.Values)
                                        {
                                            sny.Dal.Text = sny.Adı + sny.Durum;
                                        }
                                    }

                                    S.Ağaç.EndUpdate();
                                }
                            }

                            if (S.BaşlatDurdur && (Önceki_Sayac_Ölçüm != Sayac_Ölçüm) )
                            {
                                Önceki_Sayac_Ölçüm = Sayac_Ölçüm;

                                S.Çizdir();
                            }
                        }

                        if (_1sn_gecti)
                        {
                            if (!S.BaşlatDurdur && !string.IsNullOrEmpty(S.Dosyalama_KayıtKlasörü) && tik_kaydetmiyor_uyarısı < Environment.TickCount)
                            {
                                S.SolMenu_BaşlatDurdur.Image = Properties.Resources.D_Tamam;
                                S.SolMenu_BaşlatDurdur.GetCurrentParent().Refresh();
                                System.Media.SystemSounds.Asterisk.Play();
                                Thread.Sleep(250);
                                S.SolMenu_BaşlatDurdur.Image = Properties.Resources.D_Hata;
                                tik_kaydetmiyor_uyarısı = Environment.TickCount + 5000;
                            }
                        }
                    }));
                }
                catch (Exception ex) { Günlük.Ekle(ex.ToString()); Thread.Sleep(1000); }
            }
        }
    }
}

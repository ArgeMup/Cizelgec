using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Çizelgeç
{
    public class Ekranlama_Canlı
    {
        UInt64 Sayac_Ölçüm = 0;
        TreeNode tn_Senaryolar = null;
        public string İşAdı = "";
        public Ekranlama_Canlı(string İşAdı, string AyarlarDosyasıYolu)
        {
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
                    string Kaydet_DosyaAdı = S.Dosyalama_KayıtKlasörü + S.DosyaKlasörAdınıDüzelt(biri.Value.Adı);
                    Directory.CreateDirectory(Kaydet_DosyaAdı);
                    Kaydet_DosyaAdı += "\\Ayarlar.json";

                    File.WriteAllText(Kaydet_DosyaAdı, json_Üret.MupDosyasıİçin_Ayarlar(AyarlarDosyasıYolu + "\\Ayarlar.json", biri.Value.CümleBaşlangıcı, biri.Value.KelimeAyracı.ToString()));
                }

                Günlük.Ekle("Çalıştırılıyor -> " + biri.Value.Yöntem + " -> " + biri.Value.P1 + " -> " + biri.Value.P2, "Bilgi");
                biri.Value.Başlat();
            }
        }

        public void Çalıştır_Ölçme_Değerlendirme()
        {
            while (S.Çalışşsın)
            {
                try
                {
                    if (S.BaşlatDurdur && Çevirici.Yazıdan_NoktalıSayıya(S.BilgiToplama_Kıstas) > 0.0)
                    {
                        int toplam = Sinyaller.Tümü.Count;
                        double[] kayıt_dizisi = new double[toplam + 1];
                        double şimdi = S.Tarih.Sayıya(DateTime.Now);
                        kayıt_dizisi[toplam] = şimdi;

                        for (int i = 0; i < toplam; i++)
                        {
                            Sinyal_ biri = Sinyaller.Tümü.ElementAt(i).Value;

                            kayıt_dizisi[i] = biri.Güncelle_Dizi();

                            if (!biri.Değeri.Kaydedilsin) continue;

                            if (!biri.Değeri.ZamanAşımıOldu)
                            {
                                if (biri.Hesaplamalar != null)
                                {
                                    foreach (var hsp in biri.Hesaplamalar)
                                    {
                                        Sinyaller.Yaz(hsp.Değişken, Çevirici.Yazıdan_NoktalıSayıya(hsp.İşlem.Replace("<Sinyal>", S.Sayı.Yazıya(kayıt_dizisi[i]))));
                                    }
                                }

                                if (biri.Uyarılar != null)
                                {
                                    foreach (var uyr in biri.Uyarılar)
                                    {
                                        string kıstas = uyr.Kıstas.Replace("<Sinyal>", S.Sayı.Yazıya(kayıt_dizisi[i]));
                                        if (Çevirici.Yazıdan_NoktalıSayıya(kıstas) > 0.0)
                                        {
                                            string mesaj = Çevirici.Uyarıdan_Yazıya(uyr.Açıklama, kayıt_dizisi[i]);
                                            mesaj = biri.Adı.Csv + ";" + S.Sayı.Yazıya(kayıt_dizisi[i]) + ";" + mesaj;

                                            Kaydedici.Ekle(new double[1] { kayıt_dizisi[kayıt_dizisi.Length - 1] }, mesaj);
                                            Günlük.Ekle(mesaj);
                                        }
                                    }
                                }
                            }
                            else
                            {
                                string mesaj = biri.Adı.Csv + ";" + S.Sayı.Yazıya(kayıt_dizisi[i]) + ";Zaman Aşımı";

                                Kaydedici.Ekle(new double[1] { kayıt_dizisi[kayıt_dizisi.Length - 1] }, mesaj);
                                Günlük.Ekle(mesaj);
                            }
                        }

                        Array.Copy(S.ZamanEkseni, 1, S.ZamanEkseni, 0, S.CanliÇizdirme_ÖlçümSayısı - 1);
                        S.ZamanEkseni[S.CanliÇizdirme_ÖlçümSayısı - 1] = şimdi;
                        Kaydedici.Ekle(kayıt_dizisi);

                        int gecikme = (int)(Çevirici.Yazıdan_NoktalıSayıya(S.BilgiToplama_ZamanAralığı_Sn) * 1000);
                        //if (gecikme < 100) gecikme = 100;
                        while (gecikme > 1500 && S.Çalışşsın)
                        {
                            Thread.Sleep(1500);
                            gecikme -= 1500;
                        }
                        Thread.Sleep(gecikme);

                        Sayac_Ölçüm++;
                    }
                    else Thread.Sleep(1);
                }
                catch (Exception ex) { Günlük.Ekle(ex.ToString()); Thread.Sleep(1000); }
            }
        }
        public void Çalıştır_Ekranlama()
        {
            int son_grafik_tarama_süresi = 1000, tik_kaydetmiyor_uyarısı = 0;
            DateTime BaşladığıAn = DateTime.Now;
            TimeSpan fark;

            while (S.Çalışşsın)
            {
                try
                {
                    Thread.Sleep(1000);

                    if (S.AnaEkran.WindowState == FormWindowState.Minimized) continue;

                    S.EkranıGüncelle_me = true;
                    S.Çizelge.Invoke((Action)(() =>
                    {
                        S.Ağaç.BeginUpdate();

                        if (S.Çizdirme_Koordinat_Tick < Environment.TickCount)
                        {
                            fark = DateTime.Now - BaşladığıAn;

                            S.Ağaç.Nodes[0].Text = İşAdı + " - " + Sayac_Ölçüm + " - " + S.Tarih.Yazıya(DateTime.Now);
                            S.Ağaç.Nodes[0].ToolTipText = ArgeMup.HazirKod.Dönüştürme.D_Süre.Metne.SaatDakikaSaniye(0, 0, (int)fark.TotalSeconds) + " boyunca" + Environment.NewLine +
                                                            Sinyaller.Tümü.Count + " adet sinyal" + Environment.NewLine +
                                                            GelenBilgiler.Tümü.Count + " adet ayıklanmayı bekleyen girdi" + Environment.NewLine +
                                                            Kaydedici.Tümü.Count + " adet yazılmayı bekleyen ölçüm elde edildi";
                        }
                        else S.Ağaç.Nodes[0].Text = "- " + S.Ağaç.Nodes[0].Text;

                        for (int i = 0; i < Sinyaller.Tümü.Count; i++)
                        {
                            Sinyal_ biri = Sinyaller.Tümü.Values.ElementAt(i);

                            if (biri.Dal == null) Ekranlama.AğaçVeÇizelge_SonradanEkle(biri);

                            if (biri.Dal.IsVisible && S.Çizdirme_Koordinat_Tick < Environment.TickCount) //Ağaç değerlerinin dışarıdan çizdirildiği durumda güncelleme
                            {
                                biri.Dal.Text = biri.Adı.GörünenAdı + " : " + S.Sayı.Yazıya(biri.Değeri.SonDeğeri);
                                if (biri.Değeri.ZamanAşımıOldu)
                                {
                                    biri.Dal.Text += " -> Zaman Aşımı -> ";

                                    fark = DateTime.Now - biri.Değeri.SonDeğerinAlındığıAn;
                                    biri.Dal.Text += ArgeMup.HazirKod.Dönüştürme.D_Süre.Metne.SaatDakikaSaniye(0, 0, (int)fark.TotalSeconds);
                                }

                                biri.Dal.ToolTipText = Sinyaller.Tümü.Keys.ElementAt(i) + (biri.Tür == Tür_.Sinyal ? " sinyali" : " değişkeni") + Environment.NewLine +
                                                        biri.Değeri.Sayac_Güncelleme + ". kez " + S.Tarih.Yazıya(biri.Değeri.SonDeğerinAlındığıAn) + " tarihinde güncellendi";                     
                            }
                        }

                        foreach (var biri in Bağlantılar.Tümü.Values)
                        {
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

                        if (tn_Senaryolar != null && tn_Senaryolar.IsVisible)
                        {
                            foreach (var sny in Senaryolar.Tümü.Values)
                            {
                                sny.Dal.Text = sny.Adı + sny.Durum;
                            }
                        }

                        S.Ağaç.EndUpdate();
                        
                        if (!S.BaşlatDurdur && !string.IsNullOrEmpty(S.Dosyalama_KayıtKlasörü) && tik_kaydetmiyor_uyarısı < Environment.TickCount)
                        {
                            S.SolMenu_BaşlatDurdur.Image = Properties.Resources.D_Tamam;
                            S.SolMenu_BaşlatDurdur.GetCurrentParent().Refresh();
                            System.Media.SystemSounds.Asterisk.Play();
                            Thread.Sleep(250);
                            S.SolMenu_BaşlatDurdur.Image = Properties.Resources.D_Hata;
                            tik_kaydetmiyor_uyarısı = Environment.TickCount + 5000;
                        }

                        if (son_grafik_tarama_süresi >= 1000) son_grafik_tarama_süresi -= 1000;
                        else
                        {
                            son_grafik_tarama_süresi = Environment.TickCount;

                            if (S.BaşlatDurdur) S.Çizdir();
                            son_grafik_tarama_süresi = Environment.TickCount - son_grafik_tarama_süresi;

                            if (son_grafik_tarama_süresi > 5000) son_grafik_tarama_süresi = 5000;
                        }
                    }));
                }
                catch (Exception ex) { Günlük.Ekle(ex.ToString()); Thread.Sleep(1000); }

                S.EkranıGüncelle_me = false;
            }
        }
    }
}

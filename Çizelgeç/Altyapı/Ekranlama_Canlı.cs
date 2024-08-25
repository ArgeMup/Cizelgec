// Copyright ArgeMup GNU GENERAL PUBLIC LICENSE Version 3 <http://www.gnu.org/licenses/> <https://github.com/ArgeMup>

using ArgeMup.HazirKod;
using System;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

namespace Çizelgeç
{
    public class Ekranlama_Canlı
    {
        UInt64 Sayac_Ölçüm = 0, Sayac_BirbirininAynısıOlduğuİçinAtlandı = 0, Sayac_BilinçliOlarakKaydedilmedi = 0;
        public string İşAdı = "";
        KodKümesi_Dll_ KodKümesi = null;

        public void Başlat(string İşAdı, string AyarlarDosyasıYolu)
        {
            Günlük.Ekle("Ayıklanıyor -> " + AyarlarDosyasıYolu, "Bilgi");
            S.Ağaç.Nodes.Clear();
            S.Ağaç.CheckBoxes = false;
            S.Ağaç.Nodes.Add("Bekleyiniz");
            Application.DoEvents();

            Günlük.Ekle("Derleniyor : " + AyarlarDosyasıYolu + "\\*.cs", "Bilgi");
            Klasör_ kls_cs_ler = new Klasör_(AyarlarDosyasıYolu, Filtre_Dosya:new string[] { "*.cs" }, DoğrulamaKodunuÜret:false, Filtre_BüyükKüçükHarfDuyarlı:false);
            if (kls_cs_ler == null || kls_cs_ler.Dosyalar.Count == 0) throw new Exception("Hiç cs dosyası bulunamadı");
            string[] dsy_cs_ler = new string[kls_cs_ler.Dosyalar.Count];
            for (int i = 0; i < dsy_cs_ler.Length; i++) { dsy_cs_ler[i] = kls_cs_ler.Kök + "\\" + kls_cs_ler.Dosyalar[i].Yolu; }
            KodKümesi = S.Derle(dsy_cs_ler);
            KodKümesi.Çağır("Yardımcıİşlemler.Kontrolcü", "İlkAyarlamalarıYap");

            this.İşAdı = İşAdı;
            Ekranlama.AğaçVeÇizelge_Görsellerini_Üret(İşAdı);

            new Thread(() => Çalıştır_Ölçme_Değerlendirme()).Start();
            new Thread(() => Çalıştır_Ekranlama()).Start();
        }

        public void Çalıştır_Ölçme_Değerlendirme()
        {
            double[] kayıt_dizisi = null;

            while (S.Çalışşsın)
            {
                try
                {
                    if (Yardımcıİşlemler.BilgiToplama.BaşlatBeklet)
                    {
                        int gecikme = 1000;
                        DateTime şimdi_t = DateTime.Now;
                        double şimdi_d = S.Tarih.Sayıya(şimdi_t);
                        bool ekle = false;

                        if (Yardımcıİşlemler.BilgiToplama.ZamanDilimi_BirbirininAynısıOlanlarıAtla)
                        {
                            if (Sinyaller.EnAzBirSinyalDeğişti_KaydedilmesiGereken)
                            {
                                Sinyaller.EnAzBirSinyalDeğişti_KaydedilmesiGereken = false;

                                ekle = true;
                            }
                            else Yardımcıİşlemler.Sinyaller.ZamanEkseni[Yardımcıİşlemler.Sinyaller.ZamanEkseni.Length - 1] = şimdi_d; //atlandığı zamanlarda ekranın çizdirmeye devam etmesi için
                        }
                        else ekle = true;

                        foreach (var biri in Sinyaller.Tümü.Values)
                        {
                            biri.Güncelle_ZamanAşımıOlduMu_SadeceTekYerdenÇağırılabilir();
                        }

                        if (ekle)
                        {
                            kayıt_dizisi = new double[Sinyaller.Tümü.Count];

                            for (int i = 0; i < kayıt_dizisi.Length; i++)
                            {
                                Sinyal_ biri = Sinyaller.Tümü.ElementAt(i).Value;

                                kayıt_dizisi[i] = biri.Güncelle_Dizi();
                            }

                            Array.Copy(Yardımcıİşlemler.Sinyaller.ZamanEkseni, 1, Yardımcıİşlemler.Sinyaller.ZamanEkseni, 0, Yardımcıİşlemler.Sinyaller.ZamanEkseni.Length - 1);
                            Yardımcıİşlemler.Sinyaller.ZamanEkseni[Yardımcıİşlemler.Sinyaller.ZamanEkseni.Length - 1] = şimdi_d;

                            bool Kaydedilsin = true;
                            if (Yardımcıİşlemler.Sinyaller.GeriBildirimİşlemi_Kaydedilecek != null)
                            {
                                try
                                {
                                    Kaydedilsin = Yardımcıİşlemler.Sinyaller.GeriBildirimİşlemi_Kaydedilecek();
                                }
                                catch (Exception ex) { Yardımcıİşlemler.ÖnYüz.Günlük(ex, "Sinyaller.GeriBildirimİşlemi_Kaydedilecek"); }
                            }

                            if (Kaydedilsin)
                            {
                                Kaydedici.Ekle(şimdi_t, kayıt_dizisi);
                                Sayac_Ölçüm++;
                            }
                            else Sayac_BilinçliOlarakKaydedilmedi++;
                        }
                        else Sayac_BirbirininAynısıOlduğuİçinAtlandı++;

                        gecikme = Yardımcıİşlemler.BilgiToplama.ZamanDilimi_msn;
                        if (gecikme <= 1)
                        {
                            şimdi_t = şimdi_t.AddMilliseconds(1);
                            while (şimdi_t >= DateTime.Now) Thread.Yield(); //mümkün olan en kısa bekleme
                        }
                        else
                        {
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

                    if (S.Çizdir_Ortalama.Ortalaması > 1500) S.Çizdir_Ortalama.Ortalaması = 1500;
                    else if (S.Çizdir_Ortalama.Ortalaması < 30) S.Çizdir_Ortalama.Ortalaması = 30;
                    Thread.Sleep((int)S.Çizdir_Ortalama.Ortalaması);

                    if (_1sn_gecti)
                    {
                        fark = DateTime.Now - BaşladığıAn;
                        SonDurumMesajı = İşAdı + " - " + Sayac_Ölçüm + " - " + S.Tarih.Yazıya(DateTime.Now);
                        S.SonDurumMesajı = SonDurumMesajı + Environment.NewLine +
                                            S.Tarih.Yazıya(BaşladığıAn) + " zamanından beri" + Environment.NewLine +
                                            ArgeMup.HazirKod.Dönüştürme.D_Süre.Yazıya.SaatDakikaSaniye(0, 0, (int)fark.TotalSeconds) + " boyunca" + Environment.NewLine +
                                            Sinyaller.Tümü.Count + " adet sinyal" + Environment.NewLine +
                                            Kaydedici.Tümü.Count + " adet yazılmayı bekleyen ölçüm elde edildi," + Environment.NewLine +
                                            Sayac_BirbirininAynısıOlduğuİçinAtlandı + " adet zaman dilimi birbirinin aynısı olduğu için atlandı ve" + Environment.NewLine +
                                            Sayac_BilinçliOlarakKaydedilmedi + " adet zaman dilimi bilinçli olarak kaydedilmedi";
                    }

                    S.Çizelge.Invoke(new Action(() =>
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
                                        }
                                    }

                                    for (int i = 0; i < Sinyaller.Tümü.Count; i++)
                                    {
                                        Sinyal_ biri = Sinyaller.Tümü.Values.ElementAt(i);

                                        if (biri.Görseller.Dal == null) Ekranlama.AğaçVeÇizelge_SonradanEkle(Sinyaller.Tümü.ElementAt(i));

                                        if (biri.Görseller.Dal.IsVisible && S.Ağaç.Nodes[0].ImageIndex <= 0) //Ağaç değerlerinin dışarıdan çizdirildiği durumda güncelleme
                                        {
                                            biri.Görseller.Dal.Text = biri.Adı.GörünenAdı + " : " + S.Sayı.Yazıya(biri.Değeri.SonDeğeri);

                                            if (biri.Değeri.ZamanAşımı_Oldu)
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
                                        if (biri.Dal == null)
                                        {
                                            TreeNode tn = Ekranlama.AğaçDalındakiDalıBul(S.Ağaç.Nodes[0].Nodes, "Bağlantılar");
                                            tn = tn.Nodes.Add(biri.Adı);
                                            tn.Tag = biri;
                                            biri.Dal = tn; 
                                            tn.Nodes.Add("Son alınan bilgiler"); 
                                        }

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

                                    #region İşlemler
                                    if (Yardımcıİşlemler.İşlemler.DeğişiklikYapıldı)
                                    {
                                        TreeNode üstteki = Ekranlama.AğaçDalındakiDalıBul(S.Ağaç.Nodes[0].Nodes, "İşlemler");
                                        üstteki.Nodes.Clear();

                                        foreach (Yardımcıİşlemler.İşlemler.İşlem_ işl in Yardımcıİşlemler.İşlemler.Tümü)
                                        {
                                            TreeNode tn = Ekranlama.AğacaDalEkle(işl.Adı, üstteki);
                                            tn.ContextMenuStrip = S.AnaEkran.SağTuşMenü_İşlem;
                                            tn.Tag = işl;
                                        }

                                        Yardımcıİşlemler.İşlemler.DeğişiklikYapıldı = false;
                                    }
                                    #endregion

                                    S.Ağaç.EndUpdate();
                                }
                            }

                            if (Yardımcıİşlemler.BilgiToplama.BaşlatBeklet)
                            {
                                if (S.Çizdir_msnBoyuncaHızlıcaÇizdirmeyeDevamEt > Environment.TickCount)
                                {
                                    S.Çizdir();
                                }
                                else if (_1sn_gecti) S.Çizdir();
                            }
                        }

                        if (_1sn_gecti)
                        {
                            if (!Yardımcıİşlemler.BilgiToplama.BaşlatBeklet && tik_kaydetmiyor_uyarısı < Environment.TickCount)
                            {
                                S.SolMenu_BaşlatBekletDurdur.Image = Properties.Resources.D_Tamam;
                                S.SolMenu_BaşlatBekletDurdur.GetCurrentParent().Refresh();
                                System.Media.SystemSounds.Asterisk.Play();
                                Thread.Sleep(250);
                                S.SolMenu_BaşlatBekletDurdur.Image = Properties.Resources.D_Hata;
                                tik_kaydetmiyor_uyarısı = Environment.TickCount + 5000;
                            }

                            Açıklamalar.EskiyenleriSil();
                        }
                    }));
                }
                catch (Exception ex) { Günlük.Ekle(ex.ToString()); Thread.Sleep(1000); }
            }
        }
    }
}

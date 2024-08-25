// Copyright ArgeMup GNU GENERAL PUBLIC LICENSE Version 3 <http://www.gnu.org/licenses/> <https://github.com/ArgeMup>

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.IO;
using System.Threading;
using ArgeMup.HazirKod;
using ScottPlot;
using ArgeMup.HazirKod.Ekİşlemler;

namespace Çizelgeç
{
    public partial class AnaEkran : Form
    {
        YeniYazılımKontrolü_ YeniYazılımKontrolü = new YeniYazılımKontrolü_();
        public Ekranlama_Ölü Ölü_Ekranlama = null;
        public Ekranlama_Canlı Canlı_Ekranlama = null;
        string SolMenu_Kaydet_DosyaBütünlüğüKodu;

        public AnaEkran()
        {
            InitializeComponent();
        }
        private void AnaEkran_Load(object sender, EventArgs e)
        {
            Icon = Properties.Resources.user;
            Text = S.AnaEkran_ÇubuktakiYazı;
            Refresh();

            try
            {
                S.Ayarlar = new ArgeMup.HazirKod.Ayarlar_(AyarlarDosyası: S.Kulanıcı_Klasörü + "Çizelgeç.exe.Ayarlar");
            }
            catch (Exception) 
            {
                Dosya.Sil(S.Kulanıcı_Klasörü + "Çizelgeç.exe.Ayarlar");
                S.Ayarlar = new ArgeMup.HazirKod.Ayarlar_(AyarlarDosyası: S.Kulanıcı_Klasörü + "Çizelgeç.exe.Ayarlar");
            }
            
            S.AnaEkran = this;
            S._Günlük_YeniMesajaGit = Günlük_YeniMesajaGit;
            S._Günlük_MetinKutusu = Günlük_MetinKutusu;
            S._Günlük_Buton = SolMenu_Gunluk;
            S.Çizelge = Çizelge;
            S.SağTuşMenü_Çizelge_TümSinyalleriEkranaSığdır = SağTuşMenü_Çizelge_TümSinyalleriEkranaSığdır;
            S.SağTuşMenü_Çizelge_Etkin = SağTuşMenü_Çizelge_Etkin;
            S.Ağaç = Ağaç;
            S.AralıkSeçici_Baştan = AralıkSeçici_Baştan;
            S.AralıkSeçici_Sondan = AralıkSeçici_Sondan;
            S.SolMenu_BaşlatBekletDurdur = SolMenu_BaşlatBekletDurdur;
            S.Ayraç_Ana = Ayraç_Ana;

            Ayraç_AğaçAçıklama_Günlük.Panel2Collapsed = true;
            Ayraç_Ağaç_Açıklama.Panel2Collapsed = true;

            Directory.CreateDirectory(S.Şablon_Klasörü);
            Directory.CreateDirectory(S.Kulanıcı_Klasörü);
            S.Derle_Başlat();
            Günlük.Ekle("Başladı", "Bilgi");
        }
        private void AnaEkran_Shown(object sender, EventArgs e)
        {
            int.TryParse(S.Ayarlar.Oku("S.Çizelge_ÇizgiKalınlığı", "1"), out S.Çizelge_ÇizgiKalınlığı);
            SağTuşMenü_Çizelge_ÇizgiKalınlığı_Değer.Text = S.Çizelge_ÇizgiKalınlığı.ToString();

            Font = new Font(Font.FontFamily, (float)S.Ayarlar.Oku_Sayı("BuyutmeOrani", Font.Size));
            Ayraç_Ana.SplitterDistance = Convert.ToInt32(S.Ayarlar.Oku("Ayraç_Ana.SplitterDistance", "250"));

            string ayarlar_cs = Properties.Resources.ÖrnekKaynakKod_CanlıEkranlama +
                    Environment.NewLine + Environment.NewLine +
                    Properties.Resources.ÖrnekKaynakKod_Ortak;
            File.WriteAllText(S.Şablon_Klasörü + "Ayarlar.cs", ayarlar_cs);
            var cs_ler = Directory.GetFiles(S.Kulanıcı_Klasörü, "*.cs", SearchOption.AllDirectories);
            if (cs_ler == null || cs_ler.Length == 0) ayarlar_cs.Dosyaİçeriği_Yaz(S.Kulanıcı_Klasörü + "\\Örnek İş\\Ayarlar.cs");
            AnaEkran_Ağacı_Başlat(false);

            YeniYazılımKontrolü.Başlat(new Uri("https://github.com/ArgeMup/Cizelgec/blob/main/%C3%87izelge%C3%A7/bin/Release/%C3%87izelge%C3%A7.exe?raw=true"), YeniYazılımKontrolü_GeriBildirim);

            Ağaç.NodeMouseDoubleClick += Ağaç_NodeMouseDoubleClick_AçılışEkranı;

            if (S.BaşlangıçParametreleri != null && 
                S.BaşlangıçParametreleri.Length == 1 && 
                File.Exists(S.BaşlangıçParametreleri[0]))
            {
                if (Path.GetExtension(S.BaşlangıçParametreleri[0]) == ".csv" || Path.GetExtension(S.BaşlangıçParametreleri[0]) == ".mup")
                {
                    S.Çalışşsın = true;
                    ÖlüEkranlamayı_Başlat();
                }
                else if (Path.GetExtension(S.BaşlangıçParametreleri[0]).ToLower() == ".cs")
                {
                    S.BaşlangıçParametreleri[0] = Path.GetDirectoryName(S.BaşlangıçParametreleri[0]);
                    DirectoryInfo iş_klasörü = new DirectoryInfo(S.BaşlangıçParametreleri[0]);
                    CanlıEkranlama_Başlat(iş_klasörü.Name);
                }
            }

            Ayraç_Ana.Refresh();
            for (int i = 0; i < 10; i++)
            {
                Application.DoEvents();
                Thread.Sleep(10);
            } 

            Ayraç_Ana.Visible = true;
            BackgroundImage = null;
        }
        private void AnaEkran_FormClosed(object sender, FormClosedEventArgs e)
        {
            Yardımcıİşlemler.BilgiToplama.BaşlatBeklet = true;
            S.Çalışşsın = false;
            Bağlantılar.Tamamen_Durdur();
            
            S.Ayarlar.Yaz("Ayraç_Ana.SplitterDistance", Ayraç_Ana.SplitterDistance.ToString());
            S.Ayarlar.Yaz("S.Çizelge_ÇizgiKalınlığı", S.Çizelge_ÇizgiKalınlığı.ToString());
            S.Ayarlar.DeğişiklikleriKaydet();

            UygulamaOncedenCalistirildiMi_Basit.Durdur();
            YeniYazılımKontrolü.Durdur();
        }
        private void AnaEkran_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
                if (files.Length == 1 && 
                    File.Exists(files[0]) && 
                    (Path.GetExtension(files[0]) == ".csv" || Path.GetExtension(files[0]) == ".mup"))
                {
                    e.Effect = DragDropEffects.Copy;
                    return;
                }
            }
            
            e.Effect = DragDropEffects.None;
        }
        private void AnaEkran_DragDrop(object sender, DragEventArgs e)
        {
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
            if (files == null || files.Length != 1 || 
                !File.Exists(files[0]) || 
                (Path.GetExtension(files[0]) != ".csv" && Path.GetExtension(files[0]) != ".mup")) return;
            
            if (S.Ağaç.Nodes[0].Text != "Kayıtlı İşler")
            {
                //yeni uygulamada aç
                S.Çalıştır.UygulamayıDoğrudanÇalıştır(Kendi.DosyaYolu, files, false, false);
                return;
            }
            else
            {
                //bu uygulamada aç
                try 
                {
                    S.Çalışşsın = true;
                    S.BaşlangıçParametreleri = files;
                    ÖlüEkranlamayı_Başlat();
                }
                catch (Exception ex) 
                { 
                    Günlük.Ekle(ex.ToString());
                    AnaEkran_Ağacı_Başlat();
                }
            }
        }
        void AnaEkran_Ağacı_Başlat(bool Sıfırla = true)
        {
            if (Sıfırla)
            {
                S.BaşlangıçParametreleri = null;
                S.Çalışşsın = false;
                Sinyaller.Tümü.Clear();
                Bağlantılar.Tamamen_Durdur();
                Canlı_Ekranlama = null;
                Ölü_Ekranlama = null;
                Yardımcıİşlemler.Görevler.Sil(null);

                string h = S.Derle_Hatalar();
                if (h != null) Günlük.Ekle(h);
            }

            Ağaç.BeginUpdate();
            Ağaç.Nodes.Clear();
            Ağaç.CheckBoxes = false;
            Ağaç.ImageList = null;

            string[] kaynaklar = Directory.GetDirectories(S.Kulanıcı_Klasörü, "*", SearchOption.TopDirectoryOnly);
            TreeNode tn = Ağaç.Nodes.Add("Kayıtlı İşler");
            for (int i = 0; i < kaynaklar.Length; i++)
            {
                TreeNode yeni = tn.Nodes.Add(kaynaklar[i].Substring(S.Kulanıcı_Klasörü.Length));
                yeni.ToolTipText = "Başlatmak için çift tıklayın.";
            }
            tn.ExpandAll();
            Ağaç.EndUpdate();
        }
        void YeniYazılımKontrolü_GeriBildirim(bool Sonuç, string Açıklama)
        {
            if (Açıklama == "Durduruldu") return;

            Günlük.Ekle("Güncel yazılım kontrolü sonucu " + (Sonuç ? "başarılı" : "hatalı") + ". " + Açıklama, (Sonuç ? "Bilgi" : "HATA"));
        }

        //İş klasörünün yolu -> S.BaşlangıçParametreleri[0]
        void CanlıEkranlama_Başlat(string İşAdı)
        {
            try
            {
                if (!Directory.Exists(S.BaşlangıçParametreleri[0])) throw new Exception(S.BaşlangıçParametreleri[0] + " klasörüne erişilemiyor");

                if (UygulamaOncedenCalistirildiMi_Basit.KontrolEt(S.BaşlangıçParametreleri[0] + "\\Kilit.mup"))
                {
                    throw new Exception(İşAdı + " işi başka bir uygulamada açık olduğundan işlem durduruldu");
                }

                S.Çalışşsın = true;

                ResimListesi.Images.Clear();
                ResimListesi.ImageSize = new Size( (int)(Font.Size * 3), (int)(Font.Size * 1.5) );
                ResimListesi.Images.Add(Properties.Resources._0);
                ResimListesi.Images.Add(Properties.Resources._1);
                ResimListesi.Images.Add(Properties.Resources._2);
                ResimListesi.Images.Add(Properties.Resources._3);
                ResimListesi.Images.Add(Properties.Resources._4);
                ResimListesi.Images.Add(Properties.Resources._5);
                ResimListesi.Images.Add(Properties.Resources._6);
                ResimListesi.Images.Add(Properties.Resources._7);
                ResimListesi.Images.Add(Properties.Resources._8);
                ResimListesi.Images.Add(Properties.Resources._9);
                ResimListesi.Images.Add(Properties.Resources._10);
                ResimListesi.Images.Add(Properties.Resources.M_Cizelge);
                Ağaç.ImageList = ResimListesi;
                Ağaç.SelectedImageIndex = 11;
                Ağaç.ImageIndex = 0;

                Canlı_Ekranlama = new Ekranlama_Canlı();
                Canlı_Ekranlama.Başlat(İşAdı, S.BaşlangıçParametreleri[0]);

                S.AnaEkran_ÇubuktakiYazı += " >>> " + İşAdı;
                Text = S.AnaEkran_ÇubuktakiYazı;

                Ağaç.NodeMouseDoubleClick -= Ağaç_NodeMouseDoubleClick_AçılışEkranı;
                Ağaç.NodeMouseDoubleClick += Ağaç_NodeMouseDoubleClick_ÇalışmaEkranı;
                Ağaç.AfterCheck += new TreeViewEventHandler(Ağaç_AfterCheck);
                Ağaç.NodeMouseClick += new TreeNodeMouseClickEventHandler(Ağaç_NodeMouseClick);
                Ağaç.MouseEnter += new EventHandler(Ağaç_MouseEnter);

                SolMenu_BaşlatBekletDurdur.Visible = true;
                SolMenu_BaşlatBekletDurdur_Ayraç.Visible = true;
                if (Directory.Exists(Yardımcıİşlemler.BilgiToplama.Kayıt_Klasörü)) SağTuşMenü_Çizelge_KayıtKlasörünüAç.Visible = true;

                AralıkSeçici_Baştan_Scroll(null, null);

                SağTuşMenü_Çizelge_Birbirinin_aynısı_olan_zaman_dilimlerini_atla.Checked = Yardımcıİşlemler.BilgiToplama.ZamanDilimi_BirbirininAynısıOlanlarıAtla;
                SağTuşMenü_Çizelge_Birbirinin_aynısı_olan_zaman_dilimlerini_atla.Visible = true;
            }
            catch (Exception ex)
            {
                Günlük.Ekle(ex.ToString());
                AnaEkran_Ağacı_Başlat();
            }
        }

        void ÖlüEkranlama_Çizdir(bool HatayıTut = true)
        {
            try
            {
                Ekranlama.AğaçVeÇizelge_Görsellerini_Üret(Yardımcıİşlemler.BilgiToplama.ZamanDilimi_Sayısı.ToString());

                AralıkSeçici_Baştan_Scroll(null, null);
                S.Çizdir();
            }
            catch (Exception ex) 
            {
                if (HatayıTut) Günlük.Ekle(ex.Message);
                else throw;
            }
        }
        void ÖlüEkranlamayı_Başlat() //İş dosyasının yolu -> S.BaşlangıçParametreleri[0]
        {
            try
            {
                Ölü_Ekranlama = new Ekranlama_Ölü(S.BaşlangıçParametreleri[0]);

                Ağaç.NodeMouseDoubleClick -= Ağaç_NodeMouseDoubleClick_AçılışEkranı;
                Ağaç.NodeMouseDoubleClick += Ağaç_NodeMouseDoubleClick_ÇalışmaEkranı;
                Ağaç.AfterCheck += new TreeViewEventHandler(Ağaç_AfterCheck);
                Ağaç.NodeMouseClick += new TreeNodeMouseClickEventHandler(Ağaç_NodeMouseClick);
                Ağaç.MouseEnter += new EventHandler(Ağaç_MouseEnter);

                Ekle_Önceki_Sonraki_Tümü_Tuşlarını_Göster();

                ÖlüEkranlama_Çizdir(false);

                S.AnaEkran_ÇubuktakiYazı += " >>> " + Path.GetFileName(S.BaşlangıçParametreleri[0]);
                Text = S.AnaEkran_ÇubuktakiYazı;

                SağTuşMenü_Çizelge_Ayırıcı1.Visible = true;
                SağTuşMenü_Çizelge_DeğerleriNormalleştir.Visible = true;
                SağTuşMenü_Çizelge_xEkseniTarihVeSaat.Visible = true;
                SağTuşMenü_Çizelge_YenidenHesapla.Visible = true;
            }
            catch (Exception ex)
            {
                Günlük.Ekle(ex.ToString());
                AnaEkran_Ağacı_Başlat();
            }
        }
        private void Ekle_önceki_Click(object sender, EventArgs e)
        {
            if (!Ölü_Ekranlama.Ekle_Önceki())
            {
                Ekle_önceki.Visible = false;
                Ekle_önceki_tümü.Visible = false;

                Ekle_tümü.Visible = Ekle_sonraki.Visible;
            }

            ÖlüEkranlama_Çizdir();
        }
        private void Ekle_önceki_tümü_Click(object sender, EventArgs e)
        {
            Ekle_önceki.Visible = false;
            Ekle_önceki_tümü.Visible = false;

            Ekle_tümü.Visible = Ekle_sonraki.Visible;

            int za = Environment.TickCount + 1000;
            while (Ölü_Ekranlama.Ekle_Önceki() && S.Çalışşsın)
            {
                if (za < Environment.TickCount)
                {
                    Günlük.Ekle(Ölü_Ekranlama.önceki_dosya_sırano + " adet dosya kaldı", "Bilgi");
                    Application.DoEvents();
                    za = Environment.TickCount + 1000;
                }
            }

            ÖlüEkranlama_Çizdir();
        }
        private void Ekle_sonraki_Click(object sender, EventArgs e)
        {
            if (!Ölü_Ekranlama.Ekle_Sonraki())
            {
                Ekle_sonraki.Visible = false;
                Ekle_sonraki_tümü.Visible = false;

                Ekle_tümü.Visible = Ekle_önceki.Visible;
            }

            ÖlüEkranlama_Çizdir();
        }
        private void Ekle_sonraki_tümü_Click(object sender, EventArgs e)
        {
            Ekle_sonraki.Visible = false;
            Ekle_sonraki_tümü.Visible = false;

            Ekle_tümü.Visible = Ekle_önceki.Visible;

            int za = Environment.TickCount + 1000;
            while (Ölü_Ekranlama.Ekle_Sonraki() && S.Çalışşsın)
            {
                if (za < Environment.TickCount)
                {
                    Günlük.Ekle((Ölü_Ekranlama.dosyalar.Values.Count - Ölü_Ekranlama.sonraki_dosya_sırano) + " adet dosya kaldı", "Bilgi");
                    Application.DoEvents();
                    za = Environment.TickCount + 1000;
                }
            }

            ÖlüEkranlama_Çizdir();
        }
        private void Ekle_tümü_Click(object sender, EventArgs e)
        {
            Ekle_önceki.Visible = false;
            Ekle_önceki_tümü.Visible = false;
            Ekle_sonraki.Visible = false;
            Ekle_sonraki_tümü.Visible = false;
            Ekle_tümü.Visible = false;

            int za = Environment.TickCount + 1000;
            while (Ölü_Ekranlama.Ekle_Önceki() && S.Çalışşsın) 
            { 
                if (za < Environment.TickCount)
                {
                    Günlük.Ekle(Ölü_Ekranlama.önceki_dosya_sırano + " adet dosya kaldı", "Bilgi");
                    Application.DoEvents();
                    za = Environment.TickCount + 1000;
                }
            }
            while (Ölü_Ekranlama.Ekle_Sonraki() && S.Çalışşsın)
            {
                if (za < Environment.TickCount)
                {
                    Günlük.Ekle((Ölü_Ekranlama.dosyalar.Values.Count - Ölü_Ekranlama.sonraki_dosya_sırano) + " adet dosya kaldı", "Bilgi");
                    Application.DoEvents();
                    za = Environment.TickCount + 1000;
                }
            }

            ÖlüEkranlama_Çizdir();
        }
        void Ekle_Önceki_Sonraki_Tümü_Tuşlarını_Göster()
        {
            if (Ölü_Ekranlama.önceki_dosya_sırano != -1)
            {
                Ekle_önceki.Visible = true;
                Ekle_önceki_tümü.Visible = true;
                Ekle_tümü.Visible = true;
            }
            if (Ölü_Ekranlama.sonraki_dosya_sırano > -1 && Ölü_Ekranlama.sonraki_dosya_sırano < Ölü_Ekranlama.dosyalar.Count)
            {
                Ekle_sonraki.Visible = true;
                Ekle_sonraki_tümü.Visible = true;
                Ekle_tümü.Visible = true;
            }
        }

        private void SolMenu_Ağaç_Click(object sender, EventArgs e)
        {
            if (Ayraç_Ana.Panel1Collapsed)
            {
                Ayraç_Ana.Panel1Collapsed = false;
            }
            else
            {
                if (!Ayraç_AğaçAçıklama_Günlük.Panel1Collapsed)
                {
                    Ayraç_Ana.Panel1Collapsed = true;
                    Ayraç_Ana.Panel2Collapsed = false;
                }
            }

            Ayraç_AğaçAçıklama_Günlük.Panel1Collapsed = false;
            Ayraç_AğaçAçıklama_Günlük.Panel2Collapsed = true;
        }
        private void SolMenu_Cizelge_Click(object sender, EventArgs e)
        {
            if (Ayraç_Ana.Panel2Collapsed)
            {
                Ayraç_Ana.Panel2Collapsed = false;
            }
            else
            {
                Ayraç_Ana.Panel2Collapsed = true;
                Ayraç_Ana.Panel1Collapsed = false;
            }
        }
        private void SolMenu_Gunluk_Click(object sender, EventArgs e)
        {
            if (Ayraç_Ana.Panel1Collapsed)
            {
                Ayraç_Ana.Panel1Collapsed = false;
            }
            else
            {
                if (!Ayraç_AğaçAçıklama_Günlük.Panel2Collapsed)
                {
                    Ayraç_Ana.Panel1Collapsed = true;
                    Ayraç_Ana.Panel2Collapsed = false;
                }
            }

            Ayraç_AğaçAçıklama_Günlük.Panel1Collapsed = true;
            Ayraç_AğaçAçıklama_Günlük.Panel2Collapsed = false;

            SolMenu_Gunluk.Image = Properties.Resources.M_Gunluk;
        }
        private void SolMenu_BaşlatBekletDurdur_Başlat_Click(object sender, EventArgs e)
        {
            Bağlantılar._Aç();
            Yardımcıİşlemler.BilgiToplama.BaşlatBeklet = true;
        }
        private void SolMenu_BaşlatBekletDurdur_Beklet_Click(object sender, EventArgs e)
        {
            Yardımcıİşlemler.BilgiToplama.BaşlatBeklet = false;
        }
        private void SolMenu_BaşlatBekletDurdur_Durdur_Click(object sender, EventArgs e)
        {
            Bağlantılar._Kapat();
            Yardımcıİşlemler.BilgiToplama.BaşlatBeklet = false;
        }

        int AralıkSeçici_KeyDown_Bastan_Sondan_Kaydırıcı = 0;
        private void AralıkSeçici_KeyDown(object sender, KeyEventArgs e)
        {
            e.Handled = true;

            if (e.KeyCode == Keys.Up)
            {
                if (AralıkSeçici_KeyDown_Bastan_Sondan_Kaydırıcı < 2) AralıkSeçici_KeyDown_Bastan_Sondan_Kaydırıcı++;
                else AralıkSeçici_KeyDown_Bastan_Sondan_Kaydırıcı = 0;

                AralıkSeçici_EtkinOlanıBelirginleştir();
            }
            else if (e.KeyCode == Keys.Down)
            {
                if (AralıkSeçici_KeyDown_Bastan_Sondan_Kaydırıcı > 0) AralıkSeçici_KeyDown_Bastan_Sondan_Kaydırıcı--;
                else AralıkSeçici_KeyDown_Bastan_Sondan_Kaydırıcı = 2;

                AralıkSeçici_EtkinOlanıBelirginleştir();
            }
            else
            {
                int çarpan = 1;
                if (e.Control)
                {
                    if (e.Shift) çarpan = 1000;
                    else çarpan = 10;
                }
                else if (e.Shift) çarpan = 100;

                if (e.KeyCode == Keys.Left)
                {
                    switch (AralıkSeçici_KeyDown_Bastan_Sondan_Kaydırıcı)
                    {
                        case 0:
                            if (çarpan < AralıkSeçici_Baştan.Value) AralıkSeçici_Baştan.Value -= çarpan;
                            else AralıkSeçici_Baştan.Value = 0;

                            AralıkSeçici_Baştan_Scroll(null, null);
                            break;

                        case 1:
                            if (çarpan < Kaydırıcı.Value) Kaydırıcı.Value -= çarpan;
                            else Kaydırıcı.Value = 0;

                            Kaydırıcı_Scroll(null, null);
                            break;

                        case 2:
                            if (çarpan < AralıkSeçici_Sondan.Value) AralıkSeçici_Sondan.Value -= çarpan;
                            else AralıkSeçici_Sondan.Value = 0;

                            AralıkSeçici_Sondan_Scroll(null, null);
                            break;

                        default:
                            break;
                    }
                }
                else if (e.KeyCode == Keys.Right)
                {
                    switch (AralıkSeçici_KeyDown_Bastan_Sondan_Kaydırıcı)
                    {
                        case 0:
                            if (çarpan < (AralıkSeçici_Baştan.Maximum - AralıkSeçici_Baştan.Value)) AralıkSeçici_Baştan.Value += çarpan;
                            else AralıkSeçici_Baştan.Value = AralıkSeçici_Baştan.Maximum;

                            AralıkSeçici_Baştan_Scroll(null, null);
                            break;

                        case 1:
                            if (çarpan < (Kaydırıcı.Maximum - Kaydırıcı.Value)) Kaydırıcı.Value += çarpan;
                            else Kaydırıcı.Value = Kaydırıcı.Maximum;

                            Kaydırıcı_Scroll(null, null);
                            break;

                        case 2:
                            if (çarpan < (AralıkSeçici_Sondan.Maximum - AralıkSeçici_Sondan.Value)) AralıkSeçici_Sondan.Value += çarpan;
                            else AralıkSeçici_Sondan.Value = AralıkSeçici_Sondan.Maximum;

                            AralıkSeçici_Sondan_Scroll(null, null);
                            break;

                        default:
                            break;
                    }
                }
            }  
        }
        void AralıkSeçici_EtkinOlanıBelirginleştir()
        {
            if (AralıkSeçici_KeyDown_Bastan_Sondan_Kaydırıcı == 0)
            {
                AralıkSeçici_Baştan.BackColor = Color.YellowGreen;
                AralıkSeçici_Sondan.BackColor = SystemColors.Control;
                Kaydırıcı.BackColor = SystemColors.Control;
                AralıkSeçici_Baştan.Focus();
            }
            else if (AralıkSeçici_KeyDown_Bastan_Sondan_Kaydırıcı == 1)
            {
                AralıkSeçici_Baştan.BackColor = SystemColors.Control;
                AralıkSeçici_Sondan.BackColor = SystemColors.Control;
                Kaydırıcı.BackColor = Color.YellowGreen;
                Kaydırıcı.Focus();
            }
            else
            {
                AralıkSeçici_Baştan.BackColor = SystemColors.Control;
                AralıkSeçici_Sondan.BackColor = Color.YellowGreen;
                Kaydırıcı.BackColor = SystemColors.Control;
                AralıkSeçici_Sondan.Focus();
            }
        }
        int Kaydırıcı_ÖncekiDeğeri = 0;
        private void AralıkSeçici_Sondan_Scroll(object sender, EventArgs e)
        {
            if (AralıkSeçici_Sondan.Value <= AralıkSeçici_Baştan.Value)
            {
                if (AralıkSeçici_Baştan.Value > 0) AralıkSeçici_Baştan.Value--;
                AralıkSeçici_Sondan.Value = AralıkSeçici_Baştan.Value + 1;
            }

            AralıkSeçici_Baştan_Scroll(null, null);
        }
        private void AralıkSeçici_Baştan_Scroll(object sender, EventArgs e)
        {
            if (AralıkSeçici_Baştan.Value >= AralıkSeçici_Sondan.Value)
            {
                if (AralıkSeçici_Sondan.Value < AralıkSeçici_Sondan.Maximum) AralıkSeçici_Sondan.Value++;
                AralıkSeçici_Baştan.Value = AralıkSeçici_Sondan.Value - 1;
            }

            int kalan_baştan = AralıkSeçici_Baştan.Value;
            int kalan_sondan = (Yardımcıİşlemler.BilgiToplama.ZamanDilimi_Sayısı - 1) - AralıkSeçici_Sondan.Value;

            Kaydırıcı.Value = 0;
            Kaydırıcı.Maximum = kalan_baştan + kalan_sondan;
            Kaydırıcı.Value = kalan_baştan;

            Kaydırıcı_ÖncekiDeğeri = Kaydırıcı.Value;

            Kaydırıcı_Scroll(null, null);

            SağTuşMenü_Çizelge_Daralt_Baştanİtibaren.Text = AralıkSeçici_Baştan.Value.ToString();
            SağTuşMenü_Çizelge_Daralt_Sondanİtibaren.Text = AralıkSeçici_Sondan.Value.ToString();
        }
        public void Kaydırıcı_Scroll(object sender, EventArgs e)
        {
            int fark = Kaydırıcı.Value - Kaydırıcı_ÖncekiDeğeri;
            Kaydırıcı_ÖncekiDeğeri = Kaydırıcı.Value;

            AralıkSeçici_Baştan.Value += fark;
            AralıkSeçici_Sondan.Value += fark;

            int az = AralıkSeçici_Baştan.Value;
            int cok = AralıkSeçici_Sondan.Value;
            SağTuşMenü_Çizelge_tick = Environment.TickCount + 1000;

            int cooook = cok + 1;
            double[] dizi = new double[cooook - az];

            for (int i = 0; i < Sinyaller.Tümü.Count && S.Çalışşsın; i++)
            {
                if (Sinyaller.Tümü.ElementAt(i).Value.Görseller.Çizikler == null) continue;

                Sinyaller.Tümü.ElementAt(i).Value.Görseller.Çizikler.MinRenderIndex = az;
                Sinyaller.Tümü.ElementAt(i).Value.Görseller.Çizikler.MaxRenderIndex = cok;

                if (dizi.Length > 0 && Ölü_Ekranlama != null)
                {
                    Array.Copy(Sinyaller.Tümü.ElementAt(i).Value.Değeri.DeğerEkseni, az, dizi, 0, dizi.Length);

                    if (SağTuşMenü_Çizelge_Normalleştirme_çarpanlar_EnBüyük != null)
                    {
                        for (int iii = az; iii < cooook; iii++)
                        {
                            dizi[iii - az] = Hesapla.Normalleştir(dizi[iii - az], 0, 1, SağTuşMenü_Çizelge_Normalleştirme_çarpanlar_EnKüçük[i], SağTuşMenü_Çizelge_Normalleştirme_çarpanlar_EnBüyük[i]);

                            if (SağTuşMenü_Çizelge_tick <= Environment.TickCount)
                            {
                                Ağaç.Nodes[0].Text = "Bekleyiniz - " + i + " / " + Sinyaller.Tümü.Count + " - " + az + " / " + cooook;
                                Ağaç.Refresh();

                                SağTuşMenü_Çizelge_tick = Environment.TickCount + 1000;
                            }
                        }
                    }

                    double byk = dizi.Max();
                    double kck = dizi.Min();
                    string İpucu = "En büyük : " + byk + Environment.NewLine +
                                                                            "En küçük : " + kck + Environment.NewLine +
                                                                            "Fark : " + (byk - kck) + Environment.NewLine +
                                                                            "Ortalama : " + dizi.Average() + Environment.NewLine +
                                                                            "Toplam : " + dizi.Sum();

                    Sinyaller.Tümü.ElementAt(i).Value.Görseller.Dal.ToolTipText = Sinyaller.Tümü.ElementAt(i).Key + Environment.NewLine + İpucu; //hemen göster
                }
            }

            string açıklama = "Toplam " + Yardımcıİşlemler.BilgiToplama.ZamanDilimi_Sayısı + " ölçümün " + az + " ile " + cok + " aralığındaki " + (cok - az + 1) + " adet ölçüm gösteriliyor" + Environment.NewLine;
            int toplam_saniye = (int)(S.Tarih.Tarihe(SağTuşMenü_Çizelge_Normalleştirme_ZamanEkseni == null ? Yardımcıİşlemler.Sinyaller.ZamanEkseni[cok] : SağTuşMenü_Çizelge_Normalleştirme_ZamanEkseni[cok]) - S.Tarih.Tarihe(SağTuşMenü_Çizelge_Normalleştirme_ZamanEkseni == null ? Yardımcıİşlemler.Sinyaller.ZamanEkseni[az] : SağTuşMenü_Çizelge_Normalleştirme_ZamanEkseni[az])).TotalSeconds;
            açıklama += "Zaman dilimi : " + ArgeMup.HazirKod.Dönüştürme.D_Süre.Yazıya.SaatDakikaSaniye(0, 0, toplam_saniye);
            İpUcu.SetToolTip(Kaydırıcı, açıklama);

            Açıklamalar.AralıkSeçicilereGöreAyarla();

            if (Ağaç.SelectedNode == null) Ağaç.SelectedNode = Ağaç.Nodes[0];
            Ağaç_NodeMouseClick(null, new TreeNodeMouseClickEventArgs(Ağaç.SelectedNode, MouseButtons.None, 0, 0, 0));
        }
        private void AralıkSeçici_Baştan_Enter(object sender, EventArgs e)
        {
            AralıkSeçici_KeyDown_Bastan_Sondan_Kaydırıcı = 0;

            AralıkSeçici_EtkinOlanıBelirginleştir();
        }
        private void Kaydırıcı_Enter(object sender, EventArgs e)
        {
            AralıkSeçici_KeyDown_Bastan_Sondan_Kaydırıcı = 1;

            AralıkSeçici_EtkinOlanıBelirginleştir();
        }
        private void AralıkSeçici_Sondan_Enter(object sender, EventArgs e)
        {
            AralıkSeçici_KeyDown_Bastan_Sondan_Kaydırıcı = 2;

            AralıkSeçici_EtkinOlanıBelirginleştir();
        }

        private void Ağaç_AfterCheck(object sender, TreeViewEventArgs e)
        {
            if (e.Node.Tag == null || e.Node.Tag.GetType() != typeof(Sinyal_))
            {
                //alt elemanları işle
                foreach (TreeNode biri in e.Node.Nodes)
                {
                    biri.Checked = e.Node.Checked;
                }

                if (e.Node.Parent == null && e.Node.Checked == false)
                {
                    //tümünü yoket
                    foreach (Sinyal_ sinyal in Sinyaller.Tümü.Values)
                    {
                        if (sinyal.Görseller.Çizikler == null) continue;

                        sinyal.Görseller.Çizikler.IsVisible = false;
                    }
                }

                string Açıklamalar_Çizelgede_Görünsün = e.Node.Tag as string;
                if (Açıklamalar_Çizelgede_Görünsün == "Açıklamalar_Çizelgede_Görünsün")
                {
                    S.Açıklamalar_Çizelgede_Görünsün = e.Node.Checked;

                    Açıklamalar.Görünsün(S.Açıklamalar_Çizelgede_Görünsün);
                }
            }
            else
            {
                //sadece ilgili olan sinyali
                (e.Node.Tag as Sinyal_).Görseller.Çizikler.IsVisible = e.Node.Checked;
            }
        }
        private void Ağaç_MouseEnter(object sender, EventArgs e)
        {
            if (Ağaç.Nodes[0].ImageIndex > 0)
            {
                SeçilenZamandakiDeğerler_AğaçtaGöster(AralıkSeçici_Sondan.Value);
                Ağaç.Nodes[0].ImageIndex = 0;
            }

            Text = S.AnaEkran_ÇubuktakiYazı;
        }
        private void Ağaç_NodeMouseDoubleClick_AçılışEkranı(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (Ağaç.SelectedNode == null || Ağaç.SelectedNode.Parent == null) return;

            S.BaşlangıçParametreleri = new string[] { S.Kulanıcı_Klasörü + Ağaç.SelectedNode.Text };
            CanlıEkranlama_Başlat(Ağaç.SelectedNode.Text);
        }
        private void Ağaç_NodeMouseDoubleClick_ÇalışmaEkranı(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (Ağaç.SelectedNode == null) return;

            Ağaç.BeginUpdate();
            Ağaç.SelectedNode.ExpandAll();
            Ağaç.Nodes[0].Checked = false;
            Ağaç.SelectedNode.Checked = true;
            Ağaç_NodeMouseClick(null, new TreeNodeMouseClickEventArgs(Ağaç.SelectedNode, MouseButtons.None, 0, 0, 0));
            Ağaç.EndUpdate();
        }
        private void Ağaç_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Node == null) return;

            Ağaç.SelectedNode = e.Node;
            List<Sinyal_> ilgili_sny_ler = new List<Sinyal_>();
            
            if (e.Node.Tag == null || e.Node.Tag.GetType() != typeof(Sinyal_))
            {
                //kesinlikle bir sinyal değil
                if (e.Node.Nodes != null)
                {
                    //belirleen sinyalleri topla
                    for (int i = 0; i < e.Node.Nodes.Count; i++)
                    {
                        if (e.Node.Nodes[i].Tag != null && e.Node.Nodes[i].Tag.GetType() == typeof(Sinyal_) && (e.Node.Nodes[i].Tag as Sinyal_).Görseller.Çizikler != null)
                        {
                            ilgili_sny_ler.Add(e.Node.Nodes[i].Tag as Sinyal_);
                        }
                    }
                }
            }
            else
            {
                //kesinlikle bir sinyal dalı
                Sinyal_ sny = e.Node.Tag as Sinyal_;
                if (sny.Görseller.Çizikler == null) return;

                ilgili_sny_ler.Add(sny);
            }

            if (ilgili_sny_ler.Count == 0)
            {
                //hiç sinyal yok, hepsini sola at
                foreach (Sinyal_ sny in Sinyaller.Tümü.Values)
                {
                    if (sny.Görseller.Çizikler == null) continue;

                    sny.Görseller.Çizikler.YAxisIndex = Çizelge.Plot.RightAxis.AxisIndex;
                    sny.Görseller.Çizikler.IsHighlighted = false;
                }

                Çizelge.Plot.RightAxis.Color(Color.Black);
                Çizelge.Plot.RightAxis.Label("Tümü", size: Font.Size, bold:false);
                Çizelge.Plot.RightAxis.TickLabelStyle(fontSize: Font.Size, fontBold:false);
                Çizelge.Plot.LeftAxis.IsVisible = false;
            }
            else
            {
                string DaldakiSinyaller = null;
                foreach (Sinyal_ sny in Sinyaller.Tümü.Values)
                {
                    if (sny.Görseller.Çizikler == null) continue;

                    if (ilgili_sny_ler.Contains(sny))
                    {
                        sny.Görseller.Çizikler.YAxisIndex = Çizelge.Plot.RightAxis.AxisIndex;
                        sny.Görseller.Çizikler.IsHighlighted = SağTuşMenü_Çizelge_SeçiliOlanıBelirginleştr.Checked;

                        Çizelge.Plot.MoveLast(sny.Görseller.Çizikler);
                        DaldakiSinyaller += sny.Adı.GörünenAdı + ", ";
                    }
                    else
                    {
                        //diğerlerini sağ eksene geçirerek yarı görünmez yap
                        sny.Görseller.Çizikler.YAxisIndex = Çizelge.Plot.LeftAxis.AxisIndex;
                        sny.Görseller.Çizikler.IsHighlighted = false;
                    }
                }

                if (ilgili_sny_ler.Count == 1)
                {
                    Çizelge.Plot.RightAxis.Color(ilgili_sny_ler[0].Görseller.Çizikler.Color);
                    Çizelge.Plot.RightAxis.Label(ilgili_sny_ler[0].Adı.GörünenAdı, size: Font.Size * 1.5f, bold: true);
                    Çizelge.Plot.RightAxis.TickLabelStyle(fontSize: Font.Size * 1.5f, fontBold: true);
                }
                else
                {
                    Çizelge.Plot.RightAxis.Color(Color.Black);
                    Çizelge.Plot.RightAxis.Label(DaldakiSinyaller.TrimEnd(',', ' '), size: Font.Size * 1.5f, bold: true);
                    Çizelge.Plot.RightAxis.TickLabelStyle(fontSize: Font.Size * 1.5f, fontBold: true);
                }

                Çizelge.Plot.LeftAxis.Label("Diğerleri");
                Çizelge.Plot.LeftAxis.IsVisible = true;
            }

            S.Çizdir();
        }

        private void Eposta_Click(object sender, EventArgs e)
        {
            string a = string.Format("mailto:{0}?Subject={1}&Body={2}", "argemup@yandex.com", Text + " Hk.", "Mesajınız");
            S.Çalıştır.UygulamayaİşletimSistemiKararVersin(a, KapanırkenZorlaKapat:false);
        }
        public void Menu_aA_xxx_Click(object sender, EventArgs e)
        {
            double Boyut;
            if (sender == null) Boyut = S.Ayarlar.Oku_Sayı("BuyutmeOrani", Font.Size);
            else
            {
                ToolStripMenuItem tsmi = sender as ToolStripMenuItem;
                Boyut = (tsmi.Tag as string).NoktalıSayıya();
            }
            if (Boyut == 0) Boyut = Font.Size * 1.1;

            S.Ayarlar.Yaz("BuyutmeOrani", Boyut);
            Font = new Font(Font.FontFamily, (float)Boyut);

            Boyut *= 1.25;
            Çizelge.Plot.RightAxis.Label(size: (float)Boyut);
            Çizelge.Plot.RightAxis.TickLabelStyle(fontSize: (float)Boyut);
            Çizelge.Plot.LeftAxis.Label(size: (float)Boyut);
            Çizelge.Plot.LeftAxis.TickLabelStyle(fontSize: (float)Boyut);
            Çizelge.Plot.XAxis.TickLabelStyle(fontSize: (float)Boyut);

            foreach (Sinyal_ sinyal in Sinyaller.Tümü.Values)
            {
                if (sinyal.Görseller.SeçiliOlanNokta == null) continue;

                sinyal.Görseller.SeçiliOlanNokta_Yazı.FontSize = (float)Boyut;
            }

            foreach (Açıklama_ açkl in Açıklamalar.Tümü)
            {
                açkl.Görsel.FontSize = (float)Boyut;
            }

            S.Çizdir();
        }

        private void Çizelge_MouseMove(object sender, MouseEventArgs e)
        {
            if (Yardımcıİşlemler.Sinyaller.ZamanEkseni == null || 
                Yardımcıİşlemler.Sinyaller.ZamanEkseni.Length < 2 ||
                e.Button > 0) return;

            S.Çizdir_msnBoyuncaHızlıcaÇizdirmeyeDevamEt = Environment.TickCount + S.Çizdir_msnBoyuncaHızlıcaÇizdirmeyeDevamEt_Sabiti;

            int bulundu = -1;
            for (int i = 0; i < Sinyaller.Tümü.Count; i++)
            {
                Sinyal_ sinyal = Sinyaller.Tümü.Values.ElementAt(i);

                if (sinyal.Görseller.SeçiliOlanNokta == null || !sinyal.Görseller.Çizikler.IsVisible) continue;

                if (bulundu < 0)
                {
                    bulundu = SeçilenZamandakiDeğerler_FareKonumundan_EksendekiKonumuBul();
                    if (bulundu < 0) break;
                }

                sinyal.Görseller.SeçiliOlanNokta.X = Yardımcıİşlemler.Sinyaller.ZamanEkseni[bulundu];
                sinyal.Görseller.SeçiliOlanNokta.Y = sinyal.Değeri.DeğerEkseni[bulundu];
                sinyal.Görseller.SeçiliOlanNokta.IsVisible = true;
                sinyal.Görseller.SeçiliOlanNokta.YAxisIndex = sinyal.Görseller.Çizikler.YAxisIndex;
                Çizelge.Plot.MoveLast(sinyal.Görseller.SeçiliOlanNokta);

                sinyal.Görseller.SeçiliOlanNokta_Yazı.Label = " " + sinyal.Adı.GörünenAdı + " : " + S.Sayı.Yazıya(GerçekDeğeri(sinyal, i, bulundu));
                sinyal.Görseller.SeçiliOlanNokta_Yazı.X = Yardımcıİşlemler.Sinyaller.ZamanEkseni[bulundu];
                sinyal.Görseller.SeçiliOlanNokta_Yazı.Y = sinyal.Değeri.DeğerEkseni[bulundu];
                sinyal.Görseller.SeçiliOlanNokta_Yazı.IsVisible = true;
                sinyal.Görseller.SeçiliOlanNokta_Yazı.YAxisIndex = sinyal.Görseller.Çizikler.YAxisIndex;
                sinyal.Görseller.SeçiliOlanNokta_Yazı.Alignment = (((AralıkSeçici_Sondan.Value - AralıkSeçici_Baştan.Value) / 2) + AralıkSeçici_Baştan.Value) < bulundu ? Alignment.LowerRight : Alignment.LowerLeft;
                Çizelge.Plot.MoveLast(sinyal.Görseller.SeçiliOlanNokta_Yazı);
            }
            if (bulundu < 0) return;

            SeçilenZamandakiDeğerler_AğaçtaGöster(bulundu);
            S.Çizdir(false);
        }
        private void Çizelge_MouseLeave(object sender, EventArgs e)
        {
            foreach (Sinyal_ sinyal in Sinyaller.Tümü.Values)
            {
                if (sinyal.Görseller.SeçiliOlanNokta == null) continue;

                sinyal.Görseller.SeçiliOlanNokta.IsVisible = false;
                sinyal.Görseller.SeçiliOlanNokta_Yazı.IsVisible = false;
            }

            S.Çizdir(false);
        }
        private void Çizelge_MouseClicked(object sender, MouseEventArgs e)
        {
            AralıkSeçici_EtkinOlanıBelirginleştir();
        }
        int SeçilenZamandakiDeğerler_FareKonumundan_EksendekiKonumuBul()
        {
            double Koordinat_X = Çizelge.GetMouseCoordinates().x;

            if (Çizelgeç.S.AnaEkran.SağTuşMenü_Çizelge_Normalleştirme_ZamanEkseni != null)
            {
                if (Koordinat_X >= Çizelgeç.S.AnaEkran.SağTuşMenü_Çizelge_Normalleştirme_ZamanEkseni.Length) Koordinat_X = Çizelgeç.S.AnaEkran.SağTuşMenü_Çizelge_Normalleştirme_ZamanEkseni.Length - 1;
                return (int)Koordinat_X;
            }

            return Yardımcıİşlemler.Sinyaller.ZamanEkseni_EnYakın(S.Tarih.Tarihe(Koordinat_X));
        }
        void SeçilenZamandakiDeğerler_AğaçtaGöster(int No)
        {
            Ağaç.BeginUpdate();
            Ağaç.Nodes[0].Text = S.Tarih.Yazıya(SağTuşMenü_Çizelge_Normalleştirme_ZamanEkseni == null ? Yardımcıİşlemler.Sinyaller.ZamanEkseni[No] : SağTuşMenü_Çizelge_Normalleştirme_ZamanEkseni[No], S.Tarih._Şablon_uzun_Ağaç, Thread.CurrentThread.CurrentCulture) + " - İmleç Konumu (" + No + ")";
            Ağaç.Nodes[0].ImageIndex = 10;

            Text = S.AnaEkran_ÇubuktakiYazı + " / " + Ağaç.Nodes[0].Text + " / " + Çizelge.GetMouseCoordinates().y;

            for (int i = 0; i < Sinyaller.Tümü.Count; i++)
            {
                Sinyal_ biri = Sinyaller.Tümü.Values.ElementAt(i);

                if (biri.Değeri.DeğerEkseni == null ||
                   biri.Görseller.Dal == null ||
                   !biri.Görseller.Dal.IsVisible)
                {
                    continue;
                }

                biri.Görseller.Dal.Text = biri.Adı.GörünenAdı + " : " + S.Sayı.Yazıya(GerçekDeğeri(biri, i, No));
            }
            Ağaç.EndUpdate();
        }
        double GerçekDeğeri(Sinyal_ Sinyal, int SinyalSıraNo, int DeğerSıraNo)
        {
            if (SağTuşMenü_Çizelge_Normalleştirme_çarpanlar_EnBüyük == null) return Sinyal.Değeri.DeğerEkseni[DeğerSıraNo];
            else return Hesapla.Normalleştir(Sinyal.Değeri.DeğerEkseni[DeğerSıraNo], 0, 1, SağTuşMenü_Çizelge_Normalleştirme_çarpanlar_EnKüçük[SinyalSıraNo], SağTuşMenü_Çizelge_Normalleştirme_çarpanlar_EnBüyük[SinyalSıraNo]);
        }
        string SeçilenZamandakiDeğerler_YazıHalineGetir(int No)
        {
            string çıktı = S.Tarih.Yazıya(SağTuşMenü_Çizelge_Normalleştirme_ZamanEkseni == null ? Yardımcıİşlemler.Sinyaller.ZamanEkseni[No] : SağTuşMenü_Çizelge_Normalleştirme_ZamanEkseni[No], S.Tarih._Şablon_uzun_Ağaç, Thread.CurrentThread.CurrentCulture) + " - İmleç Konumu (" + No + ")" + Environment.NewLine;

            for (int i = 0; i < Sinyaller.Tümü.Count; i++)
            {
                Sinyal_ biri = Sinyaller.Tümü.Values.ElementAt(i);

                if (biri.Değeri.DeğerEkseni == null ||
                   biri.Görseller.Dal == null ||
                   !biri.Görseller.Dal.Checked)
                {
                    continue;
                }

                if (SağTuşMenü_Çizelge_Normalleştirme_çarpanlar_EnBüyük == null) çıktı += biri.Adı.Csv + " : " + S.Sayı.Yazıya(biri.Değeri.DeğerEkseni[No]) + Environment.NewLine;
                else çıktı += biri.Adı.Csv + " : " + S.Sayı.Yazıya(Hesapla.Normalleştir(biri.Değeri.DeğerEkseni[No], 0, 1, SağTuşMenü_Çizelge_Normalleştirme_çarpanlar_EnKüçük[i], SağTuşMenü_Çizelge_Normalleştirme_çarpanlar_EnBüyük[i])) + Environment.NewLine;
            }

            return çıktı;
        }

        int SağTuşMenü_Çizelge_tick = 0;
        public double[] SağTuşMenü_Çizelge_Normalleştirme_çarpanlar_EnBüyük = null;
        double[] SağTuşMenü_Çizelge_Normalleştirme_çarpanlar_EnKüçük = null;
        public double[] SağTuşMenü_Çizelge_Normalleştirme_ZamanEkseni = null;
        private void SağTuşMenü_Çizelge_Daralt_Baştanİtibaren_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                SağTuşMenü_Çizelge_Daralt_Uygula_Click(null, null);
            }
        }
        private void SağTuşMenü_Çizelge_Daralt_Uygula_Click(object sender, EventArgs e)
        {
            if (!int.TryParse(SağTuşMenü_Çizelge_Daralt_Baştanİtibaren.Text, out int baştan))
            {
                SağTuşMenü_Çizelge_Daralt_Baştanİtibaren.Text += " bilgisi sayıya dönüştürülemedi";
                Günlük.Ekle(SağTuşMenü_Çizelge_Daralt_Baştanİtibaren.Text);
                return;
            }

            if (!int.TryParse(SağTuşMenü_Çizelge_Daralt_Sondanİtibaren.Text, out int sondan))
            {
                SağTuşMenü_Çizelge_Daralt_Sondanİtibaren.Text += " bilgisi sayıya dönüştürülemedi";
                Günlük.Ekle(SağTuşMenü_Çizelge_Daralt_Sondanİtibaren.Text);
                return;
            }

            AralıkSeçici_Baştan.Value = baştan;
            AralıkSeçici_Sondan.Value = sondan;
            AralıkSeçici_Sondan_Scroll(null, null);
        }
        private void SağTuşMenü_Çizelge_ÇizgiKalınlığı_Değer_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                SağTuşMenü_Çizelge_ÇizgiKalınlığı_TümüneUygula_Click(null, null);
            }
        }
        private void SağTuşMenü_Çizelge_ÇizgiKalınlığı_TümüneUygula_Click(object sender, EventArgs e)
        {
            SağTuşMenü_Çizelge_ÇizgiKalınlığı_İşlemi(true);
        }
        private void SağTuşMenü_Çizelge_ÇizgiKalınlığı_SadeceEtkinOlanlar_Click(object sender, EventArgs e)
        {
            SağTuşMenü_Çizelge_ÇizgiKalınlığı_İşlemi(false);
        }
        private void SağTuşMenü_Çizelge_ÇizgiKalınlığı_İşlemi(bool TümüneUygula)
        {
            if (!int.TryParse(SağTuşMenü_Çizelge_ÇizgiKalınlığı_Değer.Text, out S.Çizelge_ÇizgiKalınlığı))
            {
                SağTuşMenü_Çizelge_ÇizgiKalınlığı_Değer.Text += " bilgisi sayıya dönüştürülemedi";
                Günlük.Ekle(SağTuşMenü_Çizelge_ÇizgiKalınlığı_Değer.Text);
                return;
            }

            SağTuşMenü_Çizelge_tick = Environment.TickCount + 1000;

            for (int i = 0; i < Sinyaller.Tümü.Count; i++)
            {
                Sinyal_ biri = Sinyaller.Tümü.Values.ElementAt(i);

                if (TümüneUygula ||
                   (biri.Görseller.Dal != null && biri.Görseller.Dal.Checked) )
                {
                    biri.Görseller.Çizikler.LineWidth = S.Çizelge_ÇizgiKalınlığı;
                    biri.Görseller.Çizikler.MarkerSize = ( S.Çizelge_ÇizgiKalınlığı * (float)1.5 ) + 5;
                }

                if (SağTuşMenü_Çizelge_tick <= Environment.TickCount)
                {
                    Ağaç.Nodes[0].Text = "Bekleyiniz uygulanıyor " + i + " / " + Sinyaller.Tümü.Count;
                    Ağaç.Refresh();

                    SağTuşMenü_Çizelge_tick = Environment.TickCount + 1000;
                }
            }

            Ağaç.Nodes[0].Text = "Bekleyiniz çizdiriliyor";
            Ağaç.Refresh();

            S.Çizdir();

            Ağaç.Nodes[0].Text = "Tamamlandı";
        }
        private void SağTuşMenü_Çizelge_dışarıAktar_Click(object sender, EventArgs e)
        {
            try
            {
                DosyayaKaydetDialoğu.FileName = (Canlı_Ekranlama == null ? "" : Canlı_Ekranlama.İşAdı + "-") + S.Tarih.Yazıya(SağTuşMenü_Çizelge_Normalleştirme_ZamanEkseni == null ? Yardımcıİşlemler.Sinyaller.ZamanEkseni[AralıkSeçici_Baştan.Value] : SağTuşMenü_Çizelge_Normalleştirme_ZamanEkseni[AralıkSeçici_Baştan.Value]) + "-" + S.Tarih.Yazıya(SağTuşMenü_Çizelge_Normalleştirme_ZamanEkseni == null ? Yardımcıİşlemler.Sinyaller.ZamanEkseni[AralıkSeçici_Sondan.Value] : SağTuşMenü_Çizelge_Normalleştirme_ZamanEkseni[AralıkSeçici_Sondan.Value]) + ".csv";
                DosyayaKaydetDialoğu.FileName = DosyayaKaydetDialoğu.FileName.Replace(':', '.');
                DosyayaKaydetDialoğu.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                Günlük.Ekle(ex.ToString());
            }
        }
        private void SağTuşMenü_Çizelge_dışarıAktar_DosyayaKaydetDialoğu_FileOk(object sender, System.ComponentModel.CancelEventArgs e)
        {
            bool tekrar_normalleştir_değerler = false;
            if (SağTuşMenü_Çizelge_Normalleştirme_çarpanlar_EnBüyük != null)
            {
                tekrar_normalleştir_değerler = true;
                SağTuşMenü_Çizelge_DeğerleriNormalleştir_Click(null, null);
            }

            bool tekrar_normalleştir_ZamanEkseni = false;
            if (SağTuşMenü_Çizelge_Normalleştirme_ZamanEkseni != null)
            {
                tekrar_normalleştir_ZamanEkseni = true;
                SağTuşMenü_Çizelge_xEkseniTarihVeSaat_Click(null, null);
            }

            try
            {
                SolMenu_Kaydet_DosyaBütünlüğüKodu = "";
                if (!DosyayaKaydetDialoğu.FileName.EndsWith(".csv")) DosyayaKaydetDialoğu.FileName += ".csv";

                using (FileStream fs = File.Create(DosyayaKaydetDialoğu.FileName))
                {
                    int SinyalAdedi_toplam = 0, sinyalAdedi_seçili = 0;
                    bool SeçiliOlmayanSinyalerideDahilEt = false;
                    foreach (var biri in Sinyaller.Tümü.Values)
                    {
                        if (biri.Görseller.Dal == null || biri.Görseller.Çizikler == null || biri.Değeri.DeğerEkseni == null) continue;

                        SinyalAdedi_toplam++;

                        if (biri.Görseller.Dal.Checked) sinyalAdedi_seçili++;
                    }
                    if (sinyalAdedi_seçili < SinyalAdedi_toplam)
                    {
                        if (MessageBox.Show("Tüm sinyalleri aktarmak ister misin ?", "Seçili olmayan sinyaller var !", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == DialogResult.Yes)
                        {
                            SeçiliOlmayanSinyalerideDahilEt = true;
                        }
                    }

                    List<double[]> Liste_değerler = new List<double[]>();
                    string yazı = S.Tarih.Yazıya(DateTime.Now) + ";Başlıklar";
                    foreach (var biri in Sinyaller.Tümü.Values)
                    {
                        if (biri.Görseller.Dal == null || biri.Görseller.Çizikler == null || biri.Değeri.DeğerEkseni == null) continue;

                        if (SeçiliOlmayanSinyalerideDahilEt || biri.Görseller.Dal.Checked)
                        {
                            yazı += ";" + biri.Adı.Csv;

                            Liste_değerler.Add(biri.Değeri.DeğerEkseni);
                        }
                    }
                    yazı += Environment.NewLine;
                    SağTuşMenü_Çizelge_dışarıAktar_DosyayaKaydet_Ekle(fs, yazı);

                    double[][] Liste_sadece_gerekli_olanlar = Liste_değerler.ToArray();
                    for (int x = AralıkSeçici_Baştan.Value; x <= AralıkSeçici_Sondan.Value && S.Çalışşsın; x++)
                    {
                        yazı = S.Tarih.Yazıya(Yardımcıİşlemler.Sinyaller.ZamanEkseni[x]) + ";Sinyaller";

                        for (int y = 0; y < Liste_sadece_gerekli_olanlar.Length && S.Çalışşsın; y++)
                        {
                            yazı += ";" + S.Sayı.Yazıya(Liste_sadece_gerekli_olanlar[y][x]);
                        }

                        yazı += Environment.NewLine;
                        SağTuşMenü_Çizelge_dışarıAktar_DosyayaKaydet_Ekle(fs, yazı);
                    }

                    yazı = S.Tarih.Yazıya(DateTime.Now) + ";Bilgi;ArGeMuP Çizelgeç V" + Application.ProductVersion + Environment.NewLine;
                    SağTuşMenü_Çizelge_dışarıAktar_DosyayaKaydet_Ekle(fs, yazı);
                    yazı = S.Tarih.Yazıya(DateTime.Now) + ";Bilgi;" + S.SonDurumMesajı.Replace(Environment.NewLine, " ") + Environment.NewLine;
                    SağTuşMenü_Çizelge_dışarıAktar_DosyayaKaydet_Ekle(fs, yazı);
                    yazı = S.Tarih.Yazıya(DateTime.Now) + ";Bilgi;Daha önceden alınmış ölçümlerin (içerik eksik yada değiştirilmiş olabilir) sadece bir kısmını içerir." + Environment.NewLine;
                    SağTuşMenü_Çizelge_dışarıAktar_DosyayaKaydet_Ekle(fs, yazı);
                    yazı = S.Tarih.Yazıya(DateTime.Now) + ";Doğrulama;" + SolMenu_Kaydet_DosyaBütünlüğüKodu;
                    SağTuşMenü_Çizelge_dışarıAktar_DosyayaKaydet_Ekle(fs, yazı);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                Günlük.Ekle(ex.ToString());
            }

            if (tekrar_normalleştir_değerler)
            {
                SağTuşMenü_Çizelge_DeğerleriNormalleştir_Click(null, null);
            }

            if (tekrar_normalleştir_ZamanEkseni)
            {
                SağTuşMenü_Çizelge_xEkseniTarihVeSaat_Click(null, null);
            }
        }
        void SağTuşMenü_Çizelge_dışarıAktar_DosyayaKaydet_Ekle(FileStream fs, string yazı)
        {
            byte[] dizi = System.Text.Encoding.UTF8.GetBytes(yazı);
            fs.Write(dizi, 0, dizi.Length);

            SolMenu_Kaydet_DosyaBütünlüğüKodu = ArgeMup.HazirKod.Dönüştürme.D_GeriDönülemezKarmaşıklaştırmaMetodu.Yazıdan(SolMenu_Kaydet_DosyaBütünlüğüKodu + yazı);
        }
        private void SağTuşMenü_Çizelge_panoyaKopyala_Click(object sender, EventArgs e)
        {
            if (Yardımcıİşlemler.Sinyaller.ZamanEkseni == null || Yardımcıİşlemler.Sinyaller.ZamanEkseni.Length < 2) return;

            int bulundu = SeçilenZamandakiDeğerler_FareKonumundan_EksendekiKonumuBul();
            if (bulundu < 0) return;

            string çıktı = SeçilenZamandakiDeğerler_YazıHalineGetir(bulundu);
            Clipboard.SetText(çıktı);
        }
        private void SağTuşMenü_Çizelge_KayıtKlasörünüAç_Click(object sender, EventArgs e)
        {
            S.Çalıştır.DosyaGezginindeGöster(Yardımcıİşlemler.BilgiToplama.Kayıt_Klasörü);
        }
        public void SağTuşMenü_Çizelge_DeğerleriNormalleştir_Click(object sender, EventArgs e)
        {
            SağTuşMenü_Çizelge_tick = Environment.TickCount + 1000;

            if (SağTuşMenü_Çizelge_Normalleştirme_çarpanlar_EnBüyük == null)
            {
                Ekle_sonraki.Visible = false;
                Ekle_sonraki_tümü.Visible = false;
                Ekle_tümü.Visible = false;
                Ekle_önceki.Visible = false;
                Ekle_önceki_tümü.Visible = false;

                SağTuşMenü_Çizelge_Normalleştirme_çarpanlar_EnBüyük = new double[Sinyaller.Tümü.Count];
                SağTuşMenü_Çizelge_Normalleştirme_çarpanlar_EnKüçük = new double[Sinyaller.Tümü.Count];

                for (int i = 0; i < Sinyaller.Tümü.Count; i++)
                {
                    Sinyal_ biri = Sinyaller.Tümü.Values.ElementAt(i);

                    if (biri.Değeri.DeğerEkseni != null)
                    {
                        SağTuşMenü_Çizelge_Normalleştirme_çarpanlar_EnBüyük[i] = biri.Değeri.DeğerEkseni.Max();
                        SağTuşMenü_Çizelge_Normalleştirme_çarpanlar_EnKüçük[i] = biri.Değeri.DeğerEkseni.Min();
                        
                        for (int iii = 0; iii < Yardımcıİşlemler.BilgiToplama.ZamanDilimi_Sayısı; iii++)
                        {
                            biri.Değeri.DeğerEkseni[iii] = ArgeMup.HazirKod.Hesapla.Normalleştir(biri.Değeri.DeğerEkseni[iii], SağTuşMenü_Çizelge_Normalleştirme_çarpanlar_EnKüçük[i], SağTuşMenü_Çizelge_Normalleştirme_çarpanlar_EnBüyük[i]);

                            if (SağTuşMenü_Çizelge_tick <= Environment.TickCount)
                            {
                                Ağaç.Nodes[0].Text = "Bekleyiniz - " + i + " / " + Sinyaller.Tümü.Count + " - " + iii + " / " + Yardımcıİşlemler.BilgiToplama.ZamanDilimi_Sayısı;
                                Ağaç.Refresh();

                                SağTuşMenü_Çizelge_tick = Environment.TickCount + 1000;
                            }
                        }
                    }
                }
            }
            else
            {
                for (int i = 0; i < Sinyaller.Tümü.Count; i++)
                {
                    Sinyal_ biri = Sinyaller.Tümü.Values.ElementAt(i);

                    if (biri.Değeri.DeğerEkseni != null)
                    {
                        for (int iii = 0; iii < Yardımcıİşlemler.BilgiToplama.ZamanDilimi_Sayısı; iii++)
                        {
                            biri.Değeri.DeğerEkseni[iii] = ArgeMup.HazirKod.Hesapla.Normalleştir(biri.Değeri.DeğerEkseni[iii], 0, 1, SağTuşMenü_Çizelge_Normalleştirme_çarpanlar_EnKüçük[i], SağTuşMenü_Çizelge_Normalleştirme_çarpanlar_EnBüyük[i]);

                            if (SağTuşMenü_Çizelge_tick <= Environment.TickCount)
                            {
                                Ağaç.Nodes[0].Text = "Bekleyiniz - " + i + " / " + Sinyaller.Tümü.Count + " - " + iii + " / " + Yardımcıİşlemler.BilgiToplama.ZamanDilimi_Sayısı;
                                Ağaç.Refresh();

                                SağTuşMenü_Çizelge_tick = Environment.TickCount + 1000;
                            }
                        }
                    }
                }

                SağTuşMenü_Çizelge_Normalleştirme_çarpanlar_EnBüyük = null;
                SağTuşMenü_Çizelge_Normalleştirme_çarpanlar_EnKüçük = null;

                Ekle_Önceki_Sonraki_Tümü_Tuşlarını_Göster();
            }

            Ağaç.Nodes[0].Text = "Bekleyiniz çizdiriliyor";
            Ağaç.Refresh();

            S.Çizdir();

            Ağaç.Nodes[0].Text = "Tamamlandı";
        }
        public void SağTuşMenü_Çizelge_xEkseniTarihVeSaat_Click(object sender, EventArgs e)
        {
            SağTuşMenü_Çizelge_tick = Environment.TickCount + 1000;

            if (SağTuşMenü_Çizelge_Normalleştirme_ZamanEkseni == null)
            {
                Ekle_sonraki.Visible = false;
                Ekle_sonraki_tümü.Visible = false;
                Ekle_tümü.Visible = false;
                Ekle_önceki.Visible = false;
                Ekle_önceki_tümü.Visible = false;

                SağTuşMenü_Çizelge_Normalleştirme_ZamanEkseni = new double[Yardımcıİşlemler.Sinyaller.ZamanEkseni.Length];
                Array.Copy(Yardımcıİşlemler.Sinyaller.ZamanEkseni, SağTuşMenü_Çizelge_Normalleştirme_ZamanEkseni, Yardımcıİşlemler.Sinyaller.ZamanEkseni.Length);

                for (int i = 0; i < Yardımcıİşlemler.Sinyaller.ZamanEkseni.Length; i++)
                {
                    Yardımcıİşlemler.Sinyaller.ZamanEkseni[i] = i;

                    if (SağTuşMenü_Çizelge_tick <= Environment.TickCount)
                    {
                        Ağaç.Nodes[0].Text = "Bekleyiniz - " + i + " / " + Yardımcıİşlemler.Sinyaller.ZamanEkseni.Length;
                        Ağaç.Refresh();

                        SağTuşMenü_Çizelge_tick = Environment.TickCount + 1000;
                    }
                }

                S.Çizelge.Plot.XAxis.DateTimeFormat(false);
            }
            else
            {
                Array.Copy(SağTuşMenü_Çizelge_Normalleştirme_ZamanEkseni, Yardımcıİşlemler.Sinyaller.ZamanEkseni, Yardımcıİşlemler.Sinyaller.ZamanEkseni.Length);
                SağTuşMenü_Çizelge_Normalleştirme_ZamanEkseni = null;

                S.Çizelge.Plot.XAxis.DateTimeFormat(true);

                Ekle_Önceki_Sonraki_Tümü_Tuşlarını_Göster();
            }

            Ekranlama.AğaçDalındakiDalıBul(Ağaç.Nodes[0].Nodes, "Açıklamalar").Checked = false;
            Ağaç.Nodes[0].Text = "Bekleyiniz çizdiriliyor";
            Ağaç.Refresh();

            S.Çizdir();

            Ağaç.Nodes[0].Text = "Tamamlandı";
        }
        private void SağTuşMenü_Çizelge_YenidenHesapla_Click(object sender, EventArgs e)
        {
            if (S.YenidenHesapla == null)
            {
                S.YenidenHesapla = new YenidenHesapla(Klasör.ÜstKlasör(S.BaşlangıçParametreleri[0]));
                S.YenidenHesapla.Show();
            }
            else S.YenidenHesapla.BringToFront();
        }
        private void SağTuşMenü_Çizelge_Birbirinin_aynısı_olan_zaman_dilimlerini_atla_Click(object sender, EventArgs e)
        {
            Yardımcıİşlemler.BilgiToplama.ZamanDilimi_BirbirininAynısıOlanlarıAtla = SağTuşMenü_Çizelge_Birbirinin_aynısı_olan_zaman_dilimlerini_atla.Checked;
        }
        private void SağTuşMenü_Çizelge_MerdivenGörüntüsüŞeklinde_Click(object sender, EventArgs e)
        {
            foreach (Sinyal_ sinyal in Sinyaller.Tümü.Values)
            {
                sinyal.Görseller.Çizikler.StepDisplay = SağTuşMenü_Çizelge_MerdivenGörüntüsüŞeklinde.Checked;
            }

            S.Çizdir(false);
        }

        private void SağTuşMenü_Ağaç_ilkAçılış_işlerinBulunduğuKlasörüAç_Click(object sender, EventArgs e)
        {
            S.Çalıştır.DosyaGezginindeGöster(S.Kulanıcı_Klasörü);
        }

        bool Açıklamalar_AramaÇubuğu_Çalışıyor = false;
        bool Açıklamalar_AramaÇubuğu_KapatmaTalebi = false;
        int Açıklamalar_AramaÇubuğu_Tik = 0;
        int Açıklamalar_AramaÇubuğu_Sayac_Bulundu = 0;
        Açıklama_ Açıklamalar_Son_Kullamılan = null;
        private void Açıklamalar_AramaÇubuğu_TextChanged(object sender, EventArgs e)
        {
            Açıklamalar_AramaÇubuğu_Tik = Environment.TickCount + 100;
            if (Açıklamalar_AramaÇubuğu.Text.Length < 2)
            {
                if (Açıklamalar_AramaÇubuğu_Sayac_Bulundu != 0)
                {
                    Açıklamalar_AramaÇubuğu.BackColor = Color.Salmon;

                    for (int satır = 0; satır < Açıklamalar_Tablo.RowCount; satır++)
                    {
                        Açıklamalar_Tablo.Rows[satır].Visible = true;
                        if (Açıklamalar_AramaÇubuğu_Tik < Environment.TickCount) { Application.DoEvents(); Açıklamalar_AramaÇubuğu_Tik = Environment.TickCount + 100; }
                    }

                    Açıklamalar_AramaÇubuğu.BackColor = Color.White;
                    Açıklamalar_AramaÇubuğu_Sayac_Bulundu = 0;
                    Açıklamalar_Tablo_TarihSaat.HeaderText = "Tarih Saat";
                }

                return;
            }

            if (Açıklamalar_AramaÇubuğu_Çalışıyor) { Açıklamalar_AramaÇubuğu_KapatmaTalebi = true; return; }

            Açıklamalar_AramaÇubuğu_Sayac_Bulundu = 0;
            Açıklamalar_AramaÇubuğu_Çalışıyor = true;
            Açıklamalar_AramaÇubuğu_KapatmaTalebi = false;
            Açıklamalar_AramaÇubuğu_Tik = Environment.TickCount + 500;
            Açıklamalar_AramaÇubuğu.BackColor = Color.Salmon;
            string kıstas = "*" + Açıklamalar_AramaÇubuğu.Text + "*";

            for (int satır = 0; satır < Açıklamalar_Tablo.RowCount && !Açıklamalar_AramaÇubuğu_KapatmaTalebi; satır++)
            {
                bool bulundu = false;
                for (int sutun = 0; sutun < Açıklamalar_Tablo.Columns.Count; sutun++)
                {
                    string içerik = (string)Açıklamalar_Tablo[sutun, satır].Value;
                    if (içerik.BenzerMi(kıstas, false))
                    {
                        Açıklamalar_Tablo[sutun, satır].Style.BackColor = Color.YellowGreen;
                        bulundu = true;
                    }
                    else Açıklamalar_Tablo[sutun, satır].Style.BackColor = Color.White;
                }

                Açıklamalar_Tablo.Rows[satır].Visible = bulundu;
                if (bulundu) Açıklamalar_AramaÇubuğu_Sayac_Bulundu++;

                if (Açıklamalar_AramaÇubuğu_Tik < Environment.TickCount) { Application.DoEvents(); Açıklamalar_AramaÇubuğu_Tik = Environment.TickCount + 500; }
            }

            Açıklamalar_Tablo_TarihSaat.HeaderText = "Tarih Saat (" + Açıklamalar_AramaÇubuğu_Sayac_Bulundu + ")";
            Açıklamalar_AramaÇubuğu.BackColor = Color.White;
            Açıklamalar_AramaÇubuğu_Çalışıyor = false;
            Açıklamalar_Tablo.ClearSelection();

            if (Açıklamalar_AramaÇubuğu_KapatmaTalebi) Açıklamalar_AramaÇubuğu_TextChanged(null, null);
            Açıklamalar_AramaÇubuğu_KapatmaTalebi = false;
        }
        private void Açıklamalar_Tablo_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0) return;

            Açıklama_ açklm = Açıklamalar_Tablo.Rows[e.RowIndex].Tag as Açıklama_;

            if (Açıklamalar_Son_Kullamılan != null) Açıklamalar_Son_Kullamılan.Görsel.FontBold = false;
            Açıklamalar_Son_Kullamılan = açklm;

            S.Çizelge.Plot.MoveLast(açklm.Görsel);
            açklm.Görsel.IsVisible = true;
            açklm.Görsel.FontBold = true;

            S.Çizdir(false);
        }
        private void Açıklamalar_ÖneGetir_Click(object sender, EventArgs e)
        {
            foreach(DataGridViewRow dgr in Açıklamalar_Tablo.Rows)
            {
                if (!dgr.Visible) continue;

                Açıklama_ açklm = dgr.Tag as Açıklama_;
                S.Çizelge.Plot.MoveLast(açklm.Görsel);
                açklm.Görsel.IsVisible = true;
            }

            S.Çizdir(false);
        }

        private void SağTuşMenü_İşlem_Çalıştır_Click(object sender, EventArgs e)
        {
            if (Ağaç.SelectedNode == null ||
                Ağaç.SelectedNode.Tag == null) return;

            Yardımcıİşlemler.İşlemler.İşlem_ işl = Ağaç.SelectedNode.Tag as Yardımcıİşlemler.İşlemler.İşlem_;
            if (işl == null) return;

            try
            {
                işl.İşlem(işl.Adı, işl.Hatırlatıcı);
            }
            catch (Exception ex) { Yardımcıİşlemler.ÖnYüz.Günlük(ex, "İşlemler.Çalıştır." + işl.Adı); }
        }
    }
}
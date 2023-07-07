// Copyright ArgeMup GNU GENERAL PUBLIC LICENSE Version 3 <http://www.gnu.org/licenses/> <https://github.com/ArgeMup>

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.IO;
using System.Threading;
using System.Diagnostics;
using ArgeMup.HazirKod;

namespace Çizelgeç
{
    public partial class AnaEkran : Form
    {
        YeniYazılımKontrolü_ YeniYazılımKontrolü = new YeniYazılımKontrolü_();
        Ekranlama_Ölü Ölü_Ekranlama = null;
        Ekranlama_Canlı Canlı_Ekranlama = null;
        string SolMenu_Kaydet_DosyaBütünlüğüKodu;

        public AnaEkran()
        {
            InitializeComponent();
        }
        private void AnaEkran_Load(object sender, EventArgs e)
        {
            Text = S.AnaEkran_ÇubuktakiYazı;
            Refresh();

            S.Ayarlar = new ArgeMup.HazirKod.Ayarlar_(AyarlarDosyası:S.Kulanıcı_Klasörü + "Çizelgeç.exe.Ayarlar");
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
            S.SolMenu_BaşlatDurdur = SolMenu_BaşlatDurdur;
            S.Ayraç_Ana = Ayraç_Ana;

            Günlük_Panel.Visible = false;
            Günlük_Panel.Dock = DockStyle.Fill;
            Ağaç.Dock = DockStyle.Fill;

            Directory.CreateDirectory(S.Şablon_Klasörü);
            Directory.CreateDirectory(S.Kulanıcı_Klasörü);
            Günlük.Ekle("Başladı", "Bilgi");
        }
        private void AnaEkran_Shown(object sender, EventArgs e)
        {
            int.TryParse(S.Ayarlar.Oku("S.Çizelge_ÇizgiKalınlığı", "1"), out S.Çizelge_ÇizgiKalınlığı);
            SağTuşMenü_Çizelge_ÇizgiKalınlığı_Değer.Text = S.Çizelge_ÇizgiKalınlığı.ToString();

            Font = new Font(Font.FontFamily, Convert.ToInt32(S.Ayarlar.Oku("BuyutmeOrani", "6")));
            Ayraç_Ana.SplitterDistance = Convert.ToInt32(S.Ayarlar.Oku("Ayraç_Ana.SplitterDistance", "250"));

            AnaEkran_Ağacı_Başlat();

            File.WriteAllBytes(S.Şablon_Klasörü + "Ayarlar.json", Properties.Resources.Ayarlar);
            File.WriteAllBytes(S.Şablon_Klasörü + "Senaryo1.json", Properties.Resources.Senaryo1);

            YeniYazılımKontrolü.Başlat(new Uri("https://github.com/ArgeMup/Cizelgec/blob/main/%C3%87izelge%C3%A7/bin/Release/%C3%87izelge%C3%A7.exe?raw=true"), YeniYazılımKontrolü_GeriBildirim);

            Ağaç.NodeMouseDoubleClick += Ağaç_NodeMouseDoubleClick_AçılışEkranı;

            if (S.BaşlangıçParametreleri != null && 
                S.BaşlangıçParametreleri.Length == 1 && 
                File.Exists(S.BaşlangıçParametreleri[0]))
            {
                if (Path.GetExtension(S.BaşlangıçParametreleri[0]) == ".csv" || Path.GetExtension(S.BaşlangıçParametreleri[0]) == ".mup")
                {
                    S.Çalışşsın = true;
                    ÖlüEkranlamayı_Başlat(S.BaşlangıçParametreleri[0]);
                }
                else if (Path.GetFileName(S.BaşlangıçParametreleri[0]) == "Ayarlar.json")
                {
                    string iş = Path.GetDirectoryName(S.BaşlangıçParametreleri[0]);
                    DirectoryInfo iş_klasörü = new DirectoryInfo(iş);
                    CanlıEkranlama_Başlat(iş_klasörü.Name, iş);
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
            S.BaşlatDurdur = true;
            S.Çalışşsın = false;
            Bağlantılar.Durdur();
            
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
            if (files.Length != 1 || 
                !File.Exists(files[0]) || 
                (Path.GetExtension(files[0]) != ".csv" && Path.GetExtension(files[0]) != ".mup")) return;
            
            if (S.Ağaç.Nodes[0].Text != "Kayıtlı İşler")
            {
                //yeni uygulamada aç
                Process Uygulama = new Process();
                Uygulama.StartInfo.FileName = System.Reflection.Assembly.GetEntryAssembly().Location;
                Uygulama.StartInfo.Arguments = "\"" + files[0] + "\"";
                Uygulama.Start();
                return;
            }
            else
            {
                //bu uygulamada aç
                try 
                {
                    S.Çalışşsın = true;
                    ÖlüEkranlamayı_Başlat(files[0]);
                }
                catch (Exception ex) 
                { 
                    Günlük.Ekle(ex.ToString());
                    AnaEkran_Ağacı_Başlat();
                }
            }
        }
        void AnaEkran_Ağacı_Başlat()
        {
            S.Çalışşsın = false;
            Sinyaller.Tümü.Clear();
            Bağlantılar.Tümü.Clear();

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

        void CanlıEkranlama_Başlat(string İşAdı, string AyarlarDosyasıYolu)
        {
            try
            {
                if (!Directory.Exists(AyarlarDosyasıYolu)) throw new Exception(AyarlarDosyasıYolu + " klasörüne erişilemiyor");

                if (UygulamaOncedenCalistirildiMi_Basit.KontrolEt(AyarlarDosyasıYolu + "\\Kilit.mup"))
                {
                    throw new Exception(İşAdı + " işi başka bir uygulamada açık olduğundan işlem durduruldu");
                }

                S.AnaEkran_ÇubuktakiYazı += " >>> " + İşAdı;
                Text = S.AnaEkran_ÇubuktakiYazı;

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

                Canlı_Ekranlama = new Ekranlama_Canlı(İşAdı, AyarlarDosyasıYolu);
                if (Senaryolar.Tümü.Count > 0)
                {
                    foreach (var biri in Senaryolar.Tümü.Values)
                    {
                        biri.Dal.ContextMenuStrip = SağTuşMenü_Senaryo;
                    }
                }
                
                Ağaç.NodeMouseDoubleClick -= Ağaç_NodeMouseDoubleClick_AçılışEkranı;
                Ağaç.NodeMouseDoubleClick += Ağaç_NodeMouseDoubleClick_ÇalışmaEkranı;
                Ağaç.AfterCheck += new TreeViewEventHandler(Ağaç_AfterCheck);
                Ağaç.NodeMouseClick += new TreeNodeMouseClickEventHandler(Ağaç_NodeMouseClick);
                Ağaç.MouseEnter += new EventHandler(Ağaç_MouseEnter);

                SolMenu_BaşlatDurdur.Visible = true;
                SolMenu_BaşlatDurdur_Ayraç.Visible = true;
                if (Directory.Exists(S.Dosyalama_KayıtKlasörü)) SağTuşMenü_Çizelge_KayıtKlasörünüAç.Visible = true;

                AralıkSeçici_Baştan_Scroll(null, null);

                SağTuşMenü_Çizelge_Birbirinin_aynısı_olan_zaman_dilimlerini_atla.Checked = S.BilgiToplama_BirbirininAynısıOlanZamanDilimleriniAtla;
                SağTuşMenü_Çizelge_Birbirinin_aynısı_olan_zaman_dilimlerini_atla.Visible = true;
            }
            catch (Exception ex)
            {
                Günlük.Ekle(ex.ToString());
                AnaEkran_Ağacı_Başlat();
            }
        }

        void ÖlüEkranlama_Çizdir()
        {
            try
            {
                Ekranlama.AğaçVeÇizelge_Görsellerini_Üret(S.CanliÇizdirme_ÖlçümSayısı.ToString());
                AralıkSeçici_Baştan_Scroll(null, null);
                S.Çizdir();
            }
            catch (Exception ex) { Günlük.Ekle(ex.Message); }
        }
        void ÖlüEkranlamayı_Başlat(string Dosyayolu)
        {
            try
            {
                Ölü_Ekranlama = new Ekranlama_Ölü(Dosyayolu);

                Ağaç.NodeMouseDoubleClick -= Ağaç_NodeMouseDoubleClick_AçılışEkranı;
                Ağaç.NodeMouseDoubleClick += Ağaç_NodeMouseDoubleClick_ÇalışmaEkranı;
                Ağaç.AfterCheck += new TreeViewEventHandler(Ağaç_AfterCheck);
                Ağaç.NodeMouseClick += new TreeNodeMouseClickEventHandler(Ağaç_NodeMouseClick);
                Ağaç.MouseEnter += new EventHandler(Ağaç_MouseEnter);

                Ekle_Önceki_Sonraki_Tümü_Tuşlarını_Göster();

                ÖlüEkranlama_Çizdir();

                S.AnaEkran_ÇubuktakiYazı += " >>> " + Path.GetFileName(Dosyayolu);
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
                if (Ağaç.Visible)
                {
                    Ayraç_Ana.Panel1Collapsed = true;
                    Ayraç_Ana.Panel2Collapsed = false;
                }
            }
            
            Günlük_Panel.Visible = false;
            Ağaç.Visible = true;
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
                if (Günlük_Panel.Visible)
                {
                    Ayraç_Ana.Panel1Collapsed = true;
                    Ayraç_Ana.Panel2Collapsed = false;
                }
            }

            Günlük_Panel.Visible = true;
            Ağaç.Visible = false;

            SolMenu_Gunluk.Image = Properties.Resources.M_Gunluk;
        }
        private void SolMenu_BaşlatDurdur_Click(object sender, EventArgs e)
        {
            if (S.BaşlatDurdur)
            {
                S.BaşlatDurdur = false;
                S.SolMenu_BaşlatDurdur.Image = Properties.Resources.D_Hata;
            }
            else
            {
                S.BaşlatDurdur = true;
                S.SolMenu_BaşlatDurdur.Image = Properties.Resources.D_Tamam;
            }
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
            int kalan_sondan = (S.CanliÇizdirme_ÖlçümSayısı - 1) - AralıkSeçici_Sondan.Value;

            Kaydırıcı.Value = 0;
            Kaydırıcı.Maximum = kalan_baştan + kalan_sondan;
            Kaydırıcı.Value = kalan_baştan;

            Kaydırıcı_ÖncekiDeğeri = Kaydırıcı.Value;

            Kaydırıcı_Scroll(null, null);

            SağTuşMenü_Çizelge_Daralt_Baştanİtibaren.Text = AralıkSeçici_Baştan.Value.ToString();
            SağTuşMenü_Çizelge_Daralt_Sondanİtibaren.Text = AralıkSeçici_Sondan.Value.ToString();
        }
        private void Kaydırıcı_Scroll(object sender, EventArgs e)
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

                Sinyaller.Tümü.ElementAt(i).Value.Görseller.Çizikler.minRenderIndex = az;
                Sinyaller.Tümü.ElementAt(i).Value.Görseller.Çizikler.maxRenderIndex = cok;

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

            string açıklama = "Toplam " + S.CanliÇizdirme_ÖlçümSayısı + " ölçümün " + az + " ile " + cok + " aralığındaki " + (cok - az + 1) + " adet ölçüm gösteriliyor" + Environment.NewLine;
            int toplam_saniye = (int)(S.Tarih.Tarihe(SağTuşMenü_Çizelge_Normalleştirme_ZamanEkseni == null ? S.ZamanEkseni[cok] : SağTuşMenü_Çizelge_Normalleştirme_ZamanEkseni[cok]) - S.Tarih.Tarihe(SağTuşMenü_Çizelge_Normalleştirme_ZamanEkseni == null ? S.ZamanEkseni[az] : SağTuşMenü_Çizelge_Normalleştirme_ZamanEkseni[az])).TotalSeconds;
            açıklama += "Zaman aralığı : " + ArgeMup.HazirKod.Dönüştürme.D_Süre.Yazıya.SaatDakikaSaniye(0, 0, toplam_saniye);
            İpUcu.SetToolTip(Kaydırıcı, açıklama);

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

        bool enazbirtanekalınvar = false;
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
                    var snyler = Çizelge.plt.GetPlottables();
                    while (snyler.Count > 0 && S.Çalışşsın) snyler.RemoveAt(0);

                    //S.Çizdirme_YatayÇizgi = null;
                    //S.Çizdirme_DikeyÇizgi = null;
                    //for (int i = 0; i < S.Çizdirme_Noktacıklar.Length; i++)
                    //{
                    //    S.Çizdirme_Noktacıklar[i] = null;
                    //}

                    S.Çizdir();
                }
            }
            else
            {
                //sadece ilgili olan sinyali
                if (e.Node.Checked)
                {
                    if (!Çizelge.plt.GetPlottables().Contains(e.Node.Tag)) Çizelge.plt.Add((e.Node.Tag as Sinyal_).Görseller.Çizikler);

                    //if ((e.Node.Tag as Sinyal_).Uyarı_Yazıları != null)
                    //{
                    //    foreach (var biri in (e.Node.Tag as Sinyal_).Uyarı_Yazıları)
                    //    {
                    //        if (!Çizelge.plt.GetPlottables().Contains(biri)) Çizelge.plt.Add(biri);
                    //    }
                    //}
                }
                else
                {
                    Çizelge.plt.Remove((e.Node.Tag as Sinyal_).Görseller.Çizikler);

                    //if ((e.Node.Tag as Sinyal_).Uyarı_Yazıları != null)
                    //{
                    //    foreach (var biri in (e.Node.Tag as Sinyal_).Uyarı_Yazıları)
                    //    {
                    //        Çizelge.plt.Remove(biri);
                    //    }
                    //}
                }
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

            string yol = S.Kulanıcı_Klasörü + Ağaç.SelectedNode.Text;
            CanlıEkranlama_Başlat(Ağaç.SelectedNode.Text, yol);
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
            if (e.Node.Tag == null || e.Node.Tag.GetType() != typeof(Sinyal_))
            {
                //kesinlikle bir sinyal değil
                if (e.Node.Parent != null && e.Node.Nodes != null && e.Node.Nodes.Count > 0)
                {
                    if (SağTuşMenü_Çizelge_Y_EkseniÖlçeğiniSeçiliOlanSinyaleUyarla.Checked)
                    {
                        int DaldakiSinyalSayısı = 0;
                        double[] sınır_d = new double[e.Node.Nodes.Count], sınır_y = new double[e.Node.Nodes.Count];
                        for (int i = 0; i < e.Node.Nodes.Count; i++)
                        {
                            if (e.Node.Nodes[i].Tag != null && e.Node.Nodes[i].Tag.GetType() == typeof(Sinyal_) && (e.Node.Nodes[i].Tag as Sinyal_).Değeri.DeğerEkseni != null)
                            {
                                DaldakiSinyalSayısı++;
                                sınır_d[i] = (e.Node.Nodes[i].Tag as Sinyal_).Değeri.DeğerEkseni.Min();
                                sınır_y[i] = (e.Node.Nodes[i].Tag as Sinyal_).Değeri.DeğerEkseni.Max();
                            }  
                        }
                        if (DaldakiSinyalSayısı == 0)
                        {
                            Çizelge.plt.AxisBounds();
                            Çizelge.plt.YLabel("Tümü", true, null, null, Color.Black);
                            Çizelge.plt.Style(null, null, null, Color.Black);
                        }
                        else
                        {
                            double düşük = sınır_d.Min();
                            double yüksek = sınır_y.Max();

                            if (düşük == yüksek)
                            {
                                if (yüksek == 0)
                                {
                                    yüksek = 1;
                                    düşük = -1;
                                }

                                yüksek += yüksek * 0.1;
                                düşük -= yüksek * 0.1;
                            }
                            double oran = (yüksek - düşük) * 0.05;
                            yüksek += oran;
                            düşük -= oran;

                            Çizelge.plt.AxisBounds(double.NegativeInfinity, double.PositiveInfinity, düşük, yüksek);
                            Çizelge.plt.YLabel(e.Node.Text, true, null, null, Color.Black);
                            Çizelge.plt.Style(null, null, null, Color.Black);

                            enazbirtanekalınvar = true;
                        }  
                    }
                }
                else if (enazbirtanekalınvar)
                {
                    foreach (var biri in Sinyaller.Tümü) { if (biri.Value.Görseller.Çizikler != null) biri.Value.Görseller.Çizikler.lineWidth = S.Çizelge_ÇizgiKalınlığı; }

                    Çizelge.plt.AxisBounds();
                    Çizelge.plt.YLabel("Tümü", true, null, null, Color.Black);
                    Çizelge.plt.Style(null, null, null, Color.Black);

                    enazbirtanekalınvar = false;
                }
            }
            else
            {
                //kesinlikle bir sinyal dalı
                if ((e.Node.Tag as Sinyal_).Görseller.Çizikler == null || (e.Node.Tag as Sinyal_).Değeri.DeğerEkseni == null) return;

                if (SağTuşMenü_Çizelge_SeçiliOlanıBelirginleştr.Checked)
                {
                    foreach (var biri in Sinyaller.Tümü) { if (biri.Value.Görseller.Çizikler != null) biri.Value.Görseller.Çizikler.lineWidth = S.Çizelge_ÇizgiKalınlığı; }
                    (e.Node.Tag as Sinyal_).Görseller.Çizikler.lineWidth = 3 * S.Çizelge_ÇizgiKalınlığı;
                }

                if (SağTuşMenü_Çizelge_Y_EkseniÖlçeğiniSeçiliOlanSinyaleUyarla.Checked)
                {
                    double düşük = (e.Node.Tag as Sinyal_).Değeri.DeğerEkseni.Min();
                    double yüksek = (e.Node.Tag as Sinyal_).Değeri.DeğerEkseni.Max();
                    
                    if (düşük == yüksek)
                    {
                        if (yüksek == 0)
                        {
                            yüksek = 1;
                            düşük = -1;
                        }

                        yüksek += (yüksek + 1) * 0.1;
                        düşük -= (yüksek + 1) * 0.1;
                    }
                    double oran = (yüksek - düşük) * 0.05;
                    yüksek += oran;
                    düşük -= oran;

                    Çizelge.plt.AxisBounds(double.NegativeInfinity, double.PositiveInfinity, düşük, yüksek);
                    Çizelge.plt.YLabel((e.Node.Tag as Sinyal_).Adı.GörünenAdı, true, null, null, (e.Node.Tag as Sinyal_).Görseller.Çizikler.color);
                    Çizelge.plt.Style(null, null, null, (e.Node.Tag as Sinyal_).Görseller.Çizikler.color);
                }

                enazbirtanekalınvar = true;
            }
            
            S.Çizdir();
        }

        private void Eposta_Click(object sender, EventArgs e)
        {
            string a = string.Format("mailto:{0}?Subject={1}&Body={2}", "argemup@yandex.com", Text + " Hk.", "Mesajınız");
            Process.Start(a);
        }
        private void Menu_aA_100_Click(object sender, EventArgs e)
        {
            S.Ayarlar.Yaz("BuyutmeOrani", "6");
            Font = new System.Drawing.Font(Font.FontFamily, 6);
            
            S.Çizelge.plt.YLabel(/*fontName: Font.Name,*/ fontSize: (float)(Font.Size * 1.25));
            S.Çizelge.plt.Ticks(/*fontName: Font.Name,*/ fontSize: (float)(Font.Size * 1.25));
            S.Çizdir();
        }
        private void Menu_aA_125_Click(object sender, EventArgs e)
        {
            S.Ayarlar.Yaz("BuyutmeOrani", "8");
            Font = new System.Drawing.Font(Font.FontFamily, 8);

            S.Çizelge.plt.YLabel(/*fontName: Font.Name,*/ fontSize: (float)(Font.Size * 1.25));
            S.Çizelge.plt.Ticks(/*fontName: Font.Name,*/ fontSize: (float)(Font.Size * 1.25));
            S.Çizdir();
        }
        private void Menu_aA_150_Click(object sender, EventArgs e)
        {
            S.Ayarlar.Yaz("BuyutmeOrani", "10");
            Font = new System.Drawing.Font(Font.FontFamily, 10);

            S.Çizelge.plt.YLabel(/*fontName: Font.Name,*/ fontSize: (float)(Font.Size * 1.25));
            S.Çizelge.plt.Ticks(/*fontName: Font.Name,*/ fontSize: (float)(Font.Size * 1.25));
            S.Çizdir();
        }
        private void Menu_aA_175_Click(object sender, EventArgs e)
        {
            S.Ayarlar.Yaz("BuyutmeOrani", "12");
            Font = new System.Drawing.Font(Font.FontFamily, 12);

            S.Çizelge.plt.YLabel(/*fontName: Font.Name,*/ fontSize: (float)(Font.Size * 1.25));
            S.Çizelge.plt.Ticks(/*fontName: Font.Name,*/ fontSize: (float)(Font.Size * 1.25));
            S.Çizdir();
        }
        private void Menu_aA_200_Click(object sender, EventArgs e)
        {
            S.Ayarlar.Yaz("BuyutmeOrani", "14");
            Font = new System.Drawing.Font(Font.FontFamily, 14);

            S.Çizelge.plt.YLabel(/*fontName: Font.Name,*/ fontSize: (float)(Font.Size * 1.25));
            S.Çizelge.plt.Ticks(/*fontName: Font.Name,*/ fontSize: (float)(Font.Size * 1.25));
            S.Çizdir();
        }

        private void Çizelge_MouseMove(object sender, MouseEventArgs e)
        {
            if (S.ZamanEkseni == null || S.ZamanEkseni.Length < 2 || e.Button > 0) return;

            S.Çizdir_msnBoyuncaHızlıcaÇizdirmeyeDevamEt = Environment.TickCount + S.Çizdir_msnBoyuncaHızlıcaÇizdirmeyeDevamEt_Sabiti;

            int bulundu = SeçilenZamandakiDeğerler_FareKonumundan_EksendekiKonumuBul();
            if (bulundu < 0) return;

            SeçilenZamandakiDeğerler_AğaçtaGöster(bulundu);
        }
        private void Çizelge_MouseClicked(object sender, MouseEventArgs e)
        {
            AralıkSeçici_EtkinOlanıBelirginleştir();
        }
        int SeçilenZamandakiDeğerler_FareKonumundan_EksendekiKonumuBul()
        {
            int bulundu = -1;
            double Koordinat_X = Çizelge.GetMouseCoordinates().x;

            try
            {
                if (SağTuşMenü_Çizelge_Normalleştirme_ZamanEkseni != null)
                {
                    if (Koordinat_X >= SağTuşMenü_Çizelge_Normalleştirme_ZamanEkseni.Length) Koordinat_X = SağTuşMenü_Çizelge_Normalleştirme_ZamanEkseni.Length - 1;
                    return (int)Koordinat_X;
                }
                else
                {
                    int en_yakın_konum = -1;

                    int başla = AralıkSeçici_Baştan.Value;
                    int bitir = AralıkSeçici_Sondan.Value;
                    while (bulundu == -1 && başla != bitir && S.Çalışşsın)
                    {
                        int tahmini_konum = başla + ((bitir - başla) / 2);
                        if (tahmini_konum == başla)
                        {
                            if (en_yakın_konum == -1) en_yakın_konum = tahmini_konum; //bulunamadı, en son üye olabilir 
                            break;
                        }

                        if (Koordinat_X <= S.ZamanEkseni[tahmini_konum])
                        {
                            en_yakın_konum = tahmini_konum;
                            if (Koordinat_X >= S.ZamanEkseni[tahmini_konum])
                            {
                                bulundu = tahmini_konum;
                                break;
                            }

                            bitir = tahmini_konum;
                        }
                        else başla = tahmini_konum;
                    }

                    if (bulundu == -1)
                    {
                        if (en_yakın_konum != -1)
                        {
                            if (en_yakın_konum < 1) bulundu = 0;
                            else if (en_yakın_konum >= AralıkSeçici_Sondan.Value) bulundu = AralıkSeçici_Sondan.Value;
                            else
                            {
                                double fark_soldan = Math.Abs(S.ZamanEkseni[en_yakın_konum - 1] - Koordinat_X);
                                double fark_sağdan = Math.Abs(S.ZamanEkseni[en_yakın_konum + 1] - Koordinat_X);

                                if (fark_soldan < fark_sağdan)
                                {
                                    fark_sağdan = Math.Abs(S.ZamanEkseni[en_yakın_konum] - Koordinat_X);

                                    if (fark_soldan < fark_sağdan) bulundu = en_yakın_konum - 1;
                                    else bulundu = en_yakın_konum;
                                }
                                else
                                {
                                    fark_soldan = Math.Abs(S.ZamanEkseni[en_yakın_konum] - Koordinat_X);

                                    if (fark_soldan < fark_sağdan) bulundu = en_yakın_konum;
                                    else bulundu = en_yakın_konum + 1;
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception) { }

            return bulundu;
        }
        void SeçilenZamandakiDeğerler_AğaçtaGöster(int No)
        {
            Ağaç.BeginUpdate();
            Ağaç.Nodes[0].Text = S.Tarih.Yazıya(SağTuşMenü_Çizelge_Normalleştirme_ZamanEkseni == null ? S.ZamanEkseni[No] : SağTuşMenü_Çizelge_Normalleştirme_ZamanEkseni[No], S.Tarih._Şablon_uzun_Ağaç, Thread.CurrentThread.CurrentCulture) + " - İmleç Konumu (" + No + ")";
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

                if (SağTuşMenü_Çizelge_Normalleştirme_çarpanlar_EnBüyük == null) biri.Görseller.Dal.Text = biri.Adı.GörünenAdı + " : " + S.Sayı.Yazıya(biri.Değeri.DeğerEkseni[No]);
                else biri.Görseller.Dal.Text = biri.Adı.GörünenAdı + " : " + S.Sayı.Yazıya(ArgeMup.HazirKod.Hesapla.Normalleştir(biri.Değeri.DeğerEkseni[No], 0, 1, SağTuşMenü_Çizelge_Normalleştirme_çarpanlar_EnKüçük[i], SağTuşMenü_Çizelge_Normalleştirme_çarpanlar_EnBüyük[i]));
            }
            Ağaç.EndUpdate();
        }
        string SeçilenZamandakiDeğerler_YazıHalineGetir(int No)
        {
            string çıktı = S.Tarih.Yazıya(SağTuşMenü_Çizelge_Normalleştirme_ZamanEkseni == null ? S.ZamanEkseni[No] : SağTuşMenü_Çizelge_Normalleştirme_ZamanEkseni[No], S.Tarih._Şablon_uzun_Ağaç, Thread.CurrentThread.CurrentCulture) + " - İmleç Konumu (" + No + ")" + Environment.NewLine;

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

        private void SağTuşMenü_Senaryo_Çalıştır_Click(object sender, EventArgs e)
        {
            (Ağaç.SelectedNode.Tag as Senaryo_).Çalıştır();
        }
        private void SağTuşMenü_Senaryo_Durdur_Click(object sender, EventArgs e)
        {
            (Ağaç.SelectedNode.Tag as Senaryo_).Durdur();
        }

        int SağTuşMenü_Çizelge_tick = 0;
        double[] SağTuşMenü_Çizelge_Normalleştirme_çarpanlar_EnBüyük = null;
        double[] SağTuşMenü_Çizelge_Normalleştirme_çarpanlar_EnKüçük = null;
        double[] SağTuşMenü_Çizelge_Normalleştirme_ZamanEkseni = null;
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
            SağTuşMenü_Çizelge_SeçiliOlanıBelirginleştr.Checked = false;

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
                    biri.Görseller.Çizikler.lineWidth = S.Çizelge_ÇizgiKalınlığı;
                    biri.Görseller.Çizikler.markerSize = ( S.Çizelge_ÇizgiKalınlığı * (float)1.5 ) + 5;
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
                DosyayaKaydetDialoğu.FileName = (Canlı_Ekranlama == null ? "" : Canlı_Ekranlama.İşAdı + "-") + S.Tarih.Yazıya(SağTuşMenü_Çizelge_Normalleştirme_ZamanEkseni == null ? S.ZamanEkseni[AralıkSeçici_Baştan.Value] : SağTuşMenü_Çizelge_Normalleştirme_ZamanEkseni[AralıkSeçici_Baştan.Value]) + "-" + S.Tarih.Yazıya(SağTuşMenü_Çizelge_Normalleştirme_ZamanEkseni == null ? S.ZamanEkseni[AralıkSeçici_Sondan.Value] : SağTuşMenü_Çizelge_Normalleştirme_ZamanEkseni[AralıkSeçici_Sondan.Value]) + ".csv";
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
                        yazı = S.Tarih.Yazıya(S.ZamanEkseni[x]) + ";Sinyaller";

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
            if (S.ZamanEkseni == null || S.ZamanEkseni.Length < 2) return;

            int bulundu = SeçilenZamandakiDeğerler_FareKonumundan_EksendekiKonumuBul();
            if (bulundu < 0) return;

            string çıktı = SeçilenZamandakiDeğerler_YazıHalineGetir(bulundu);
            Clipboard.SetText(çıktı);
        }
        private void SağTuşMenü_Çizelge_KayıtKlasörünüAç_Click(object sender, EventArgs e)
        {
            Process.Start(S.Dosyalama_KayıtKlasörü);
        }
        private void SağTuşMenü_Çizelge_DeğerleriNormalleştir_Click(object sender, EventArgs e)
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
                        
                        for (int iii = 0; iii < S.CanliÇizdirme_ÖlçümSayısı; iii++)
                        {
                            biri.Değeri.DeğerEkseni[iii] = ArgeMup.HazirKod.Hesapla.Normalleştir(biri.Değeri.DeğerEkseni[iii], SağTuşMenü_Çizelge_Normalleştirme_çarpanlar_EnKüçük[i], SağTuşMenü_Çizelge_Normalleştirme_çarpanlar_EnBüyük[i]);

                            if (SağTuşMenü_Çizelge_tick <= Environment.TickCount)
                            {
                                Ağaç.Nodes[0].Text = "Bekleyiniz - " + i + " / " + Sinyaller.Tümü.Count + " - " + iii + " / " + S.CanliÇizdirme_ÖlçümSayısı;
                                Ağaç.Refresh();

                                SağTuşMenü_Çizelge_tick = Environment.TickCount + 1000;
                            }
                        }
                    }
                }

                SağTuşMenü_Çizelge_DeğerleriNormalleştir.Text = "Değerleri ilk değerlerine getir";
            }
            else
            {
                for (int i = 0; i < Sinyaller.Tümü.Count; i++)
                {
                    Sinyal_ biri = Sinyaller.Tümü.Values.ElementAt(i);

                    if (biri.Değeri.DeğerEkseni != null)
                    {
                        for (int iii = 0; iii < S.CanliÇizdirme_ÖlçümSayısı; iii++)
                        {
                            biri.Değeri.DeğerEkseni[iii] = ArgeMup.HazirKod.Hesapla.Normalleştir(biri.Değeri.DeğerEkseni[iii], 0, 1, SağTuşMenü_Çizelge_Normalleştirme_çarpanlar_EnKüçük[i], SağTuşMenü_Çizelge_Normalleştirme_çarpanlar_EnBüyük[i]);

                            if (SağTuşMenü_Çizelge_tick <= Environment.TickCount)
                            {
                                Ağaç.Nodes[0].Text = "Bekleyiniz - " + i + " / " + Sinyaller.Tümü.Count + " - " + iii + " / " + S.CanliÇizdirme_ÖlçümSayısı;
                                Ağaç.Refresh();

                                SağTuşMenü_Çizelge_tick = Environment.TickCount + 1000;
                            }
                        }
                    }
                }

                SağTuşMenü_Çizelge_Normalleştirme_çarpanlar_EnBüyük = null;
                SağTuşMenü_Çizelge_Normalleştirme_çarpanlar_EnKüçük = null;
                SağTuşMenü_Çizelge_DeğerleriNormalleştir.Text = "Değerleri normalleştir";

                Ekle_Önceki_Sonraki_Tümü_Tuşlarını_Göster();
            }

            Ağaç.Nodes[0].Text = "Bekleyiniz çizdiriliyor";
            Ağaç.Refresh();

            S.Çizdir();

            Ağaç.Nodes[0].Text = "Tamamlandı";
        }
        private void SağTuşMenü_Çizelge_xEkseniTarihVeSaat_Click(object sender, EventArgs e)
        {
            SağTuşMenü_Çizelge_tick = Environment.TickCount + 1000;

            if (SağTuşMenü_Çizelge_Normalleştirme_ZamanEkseni == null)
            {
                Ekle_sonraki.Visible = false;
                Ekle_sonraki_tümü.Visible = false;
                Ekle_tümü.Visible = false;
                Ekle_önceki.Visible = false;
                Ekle_önceki_tümü.Visible = false;

                SağTuşMenü_Çizelge_Normalleştirme_ZamanEkseni = new double[S.ZamanEkseni.Length];
                Array.Copy(S.ZamanEkseni, SağTuşMenü_Çizelge_Normalleştirme_ZamanEkseni, S.ZamanEkseni.Length);

                for (int i = 0; i < S.ZamanEkseni.Length; i++)
                {
                    S.ZamanEkseni[i] = i;

                    if (SağTuşMenü_Çizelge_tick <= Environment.TickCount)
                    {
                        Ağaç.Nodes[0].Text = "Bekleyiniz - " + i + " / " + S.ZamanEkseni.Length;
                        Ağaç.Refresh();

                        SağTuşMenü_Çizelge_tick = Environment.TickCount + 1000;
                    }
                }

                S.Çizelge.plt.Ticks(dateTimeX: false);

                SağTuşMenü_Çizelge_xEkseniTarihVeSaat.Text = "Tarih saati göster";
            }
            else
            {
                Array.Copy(SağTuşMenü_Çizelge_Normalleştirme_ZamanEkseni, S.ZamanEkseni, S.ZamanEkseni.Length);
                SağTuşMenü_Çizelge_Normalleştirme_ZamanEkseni = null;

                S.Çizelge.plt.Ticks(dateTimeX: true);

                SağTuşMenü_Çizelge_xEkseniTarihVeSaat.Text = "Dizideki sıra numarasını göster";

                Ekle_Önceki_Sonraki_Tümü_Tuşlarını_Göster();
            }

            Ağaç.Nodes[0].Text = "Bekleyiniz çizdiriliyor";
            Ağaç.Refresh();

            S.Çizdir();

            Ağaç.Nodes[0].Text = "Tamamlandı";
        }
        private void SağTuşMenü_Çizelge_YenidenHesapla_SadeceSeçiliDalaUygula_Click(object sender, EventArgs e)
        {
            SağTuşMenü_Çizelge_YenidenHesapla_İşlemi(0);
        }
        private void SağTuşMenü_Çizelge_YenidenHesapla_SadeceEtkinOlanlaraUygula_Click(object sender, EventArgs e)
        {
            SağTuşMenü_Çizelge_YenidenHesapla_İşlemi(1);
        }
        private void SağTuşMenü_Çizelge_YenidenHesapla_TününeUygula_Click(object sender, EventArgs e)
        {
            SağTuşMenü_Çizelge_YenidenHesapla_İşlemi(2);
        }
        private void SağTuşMenü_Çizelge_YenidenHesapla_İşlemi(int _0_seçilidal_1_etkinler_2_tümü)
        {
            try
            {
                Çevirici.Yazıdan_NoktalıSayıya(SağTuşMenü_Çizelge_YenidenHesapla_Değer.Text.Replace("<Sinyal>", "1"));
            }
            catch (Exception)
            {
                SağTuşMenü_Çizelge_YenidenHesapla_Değer.Text += " bilgisi sayıya dönüştürülemedi";
                Günlük.Ekle(SağTuşMenü_Çizelge_YenidenHesapla_Değer.Text);
                return;
            }

            string açıklama = "";
            int başla = 0;
            int bitir = S.CanliÇizdirme_ÖlçümSayısı - 1;
            if (_0_seçilidal_1_etkinler_2_tümü == 2)
            {
                açıklama = "Tüm sinyallere ->" + SağTuşMenü_Çizelge_YenidenHesapla_Değer.Text + "<- işlemi uygulanıyor";
            }
            else if (_0_seçilidal_1_etkinler_2_tümü == 1)
            {
                başla = AralıkSeçici_Baştan.Value;
                bitir = AralıkSeçici_Sondan.Value;

                açıklama = "Seçili sinyallerin " + başla + " ile " + bitir + " aralığındaki değerlerine ->" + SağTuşMenü_Çizelge_YenidenHesapla_Değer.Text + "<- işlemi uygulanıyor";
            }
            else if (_0_seçilidal_1_etkinler_2_tümü == 0)
            {
                başla = AralıkSeçici_Baştan.Value;
                bitir = AralıkSeçici_Sondan.Value;

                açıklama = "Seçili dalın " + başla + " ile " + bitir + " aralığındaki değerlerine ->" + SağTuşMenü_Çizelge_YenidenHesapla_Değer.Text + "<- işlemi uygulanıyor";
            }
            Günlük.Ekle(açıklama, "Bilgi");

            SağTuşMenü_Çizelge_tick = Environment.TickCount + 1000;

            bool tekrar_normalleştir = false;
            if (SağTuşMenü_Çizelge_Normalleştirme_çarpanlar_EnBüyük != null)
            {
                tekrar_normalleştir = true;
                SağTuşMenü_Çizelge_DeğerleriNormalleştir_Click(null, null);
            }

            for (int i = 0; i < Sinyaller.Tümü.Count; i++)
            {
                Sinyal_ biri = Sinyaller.Tümü.Values.ElementAt(i);
                if (biri.Görseller.Dal == null) continue;

                if ((_0_seçilidal_1_etkinler_2_tümü == 0 && Ağaç.SelectedNode == biri.Görseller.Dal) ||
                    (_0_seçilidal_1_etkinler_2_tümü == 1 && biri.Görseller.Dal.Checked) ||
                    (_0_seçilidal_1_etkinler_2_tümü == 2))
                {
                    for (int iii = başla; iii <= bitir; iii++)
                    {
                        biri.Değeri.DeğerEkseni[iii] = Çevirici.Yazıdan_NoktalıSayıya(SağTuşMenü_Çizelge_YenidenHesapla_Değer.Text.Replace("<Sinyal>", S.Sayı.Yazıya(biri.Değeri.DeğerEkseni[iii])), iii);

                        if (SağTuşMenü_Çizelge_tick <= Environment.TickCount)
                        {
                            Ağaç.Nodes[0].Text = "Bekleyiniz - " + i + " / " + Sinyaller.Tümü.Count + " - " + iii + " / " + bitir;
                            Ağaç.Refresh();

                            SağTuşMenü_Çizelge_tick = Environment.TickCount + 1000;
                        }
                    }
                }
            }

            if (tekrar_normalleştir)
            {
                SağTuşMenü_Çizelge_DeğerleriNormalleştir_Click(null, null);
            }

            Ağaç.Nodes[0].Text = "Bekleyiniz çizdiriliyor";
            Ağaç.Refresh();

            S.Çizdir();

            Ağaç.Nodes[0].Text = "Tamamlandı";
        }
        private void SağTuşMenü_Çizelge_Birbirinin_aynısı_olan_zaman_dilimlerini_atla_Click(object sender, EventArgs e)
        {
            S.BilgiToplama_BirbirininAynısıOlanZamanDilimleriniAtla = SağTuşMenü_Çizelge_Birbirinin_aynısı_olan_zaman_dilimlerini_atla.Checked;
        }

        private void SağTuşMenü_Ağaç_ilkAçılış_işlerinBulunduğuKlasörüAç_Click(object sender, EventArgs e)
        {
            Process.Start(S.Kulanıcı_Klasörü);
        }
    }
}
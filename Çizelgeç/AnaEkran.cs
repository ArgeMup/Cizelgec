// Copyright ArgeMup GNU GENERAL PUBLIC LICENSE Version 3 <http://www.gnu.org/licenses/> <https://github.com/ArgeMup>

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.IO;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using System.Diagnostics;

namespace Çizelgeç
{
    public partial class AnaEkran : Form
    {
        Ekranlama_Ölü Ölü_Ekranlama = null;
        Ekranlama_Canlı Canlı_Ekranlama = null;
        string SolMenu_Kaydet_DosyaBütünlüğüKodu;

        public AnaEkran()
        {
            InitializeComponent();
        }
        private void AnaEkran_Load(object sender, EventArgs e)
        {
            Text = "ArGeMuP Çizelgeç " + Application.ProductVersion;

            S.Ayarlar = new ArgeMup.HazirKod.Ayarlar_(out _);
            //S.PeTeİkKo = new ArgeMup.HazirKod.PencereVeTepsiIkonuKontrolu_(this, S.Ayarlar, true);
            //S.PeTeİkKo.TepsiİkonunuBaşlat();
            Opacity = 1;

            S.AnaEkran = this;
            S._Günlük_YeniMesajaGit = Günlük_YeniMesajaGit;
            S._Günlük_MetinKutusu = Günlük_MetinKutusu;
            S._Günlük_Buton = SolMenu_Gunluk;
            S.Çizelge = Çizelge;
            S.Ortala = Ortala;
            S.Güncelle = Güncelle;
            S.Ağaç = Ağaç;
            S.Kaydırıcı = Kaydırıcı;
            S.AralıkSeçici = AralıkSeçici;
            S.SolMenu_BaşlatDurdur = SolMenu_BaşlatDurdur;
            
            Günlük_Panel.Visible = false;
            Ayraç_Ağaç.Panel2Collapsed = true;
            Günlük_Panel.Dock = DockStyle.Fill;
            Ayraç_Ağaç.Dock = DockStyle.Fill;

            Directory.CreateDirectory(S.Şablon_Klasörü);
            Directory.CreateDirectory(S.Kulanıcı_Klasörü);
            Günlük.Ekle("Başladı", "Bilgi");
        }
        private void AnaEkran_Shown(object sender, EventArgs e)
        {
            Font = new Font(Font.FontFamily, Convert.ToInt32(S.Ayarlar.Oku("BuyutmeOrani", "6")));
            Ayraç_Ana.SplitterDistance = Convert.ToInt32(S.Ayarlar.Oku("Ayraç_Ana.SplitterDistance", "250"));

            AnaEkran_Ağacı_Başlat();

            File.WriteAllBytes(S.Şablon_Klasörü + "Ayarlar.json", Properties.Resources.Ayarlar);
            File.WriteAllBytes(S.Şablon_Klasörü + "Senaryo1.json", Properties.Resources.Senaryo1);
            
            if (S.BaşlangıçParametreleri != null && S.BaşlangıçParametreleri.Length == 1 && File.Exists(S.BaşlangıçParametreleri[0]) && (Path.GetExtension(S.BaşlangıçParametreleri[0]) == ".csv" || Path.GetExtension(S.BaşlangıçParametreleri[0]) == ".mup"))
            {
                try
                {
                    ÖlüEkranlamayı_Başlat(S.BaşlangıçParametreleri[0]);
                    return;
                }
                catch (Exception ex)
                { 
                    Günlük.Ekle(ex.ToString());
                    AnaEkran_Ağacı_Başlat();
                }
            }

            Ağaç.NodeMouseDoubleClick += new TreeNodeMouseClickEventHandler(Ağaç_NodeMouseDoubleClick);
        }
        private void AnaEkran_FormClosed(object sender, FormClosedEventArgs e)
        {
            S.Çalışşsın = false;
            S.Ayarlar.Yaz("Ayraç_Ana.SplitterDistance", Ayraç_Ana.SplitterDistance.ToString());
        }
        private void AnaEkran_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
                if (files.Length == 1 && File.Exists(files[0]) && (Path.GetExtension(files[0]) == ".csv" || Path.GetExtension(files[0]) == ".mup"))
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
            if (files.Length != 1 || !File.Exists(files[0]) || (Path.GetExtension(files[0]) != ".csv" && Path.GetExtension(files[0]) != ".mup")) return;
            
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
                try { ÖlüEkranlamayı_Başlat(files[0]); }
                catch (Exception ex) 
                { 
                    Günlük.Ekle(ex.ToString());
                    AnaEkran_Ağacı_Başlat();
                }
            }
        }
        void AnaEkran_Ağacı_Başlat()
        {
            Ağaç.Nodes.Clear();
            Ağaç.CheckBoxes = false;

            string[] kaynaklar = Directory.GetDirectories(S.Kulanıcı_Klasörü, "*", SearchOption.TopDirectoryOnly);
            TreeNode tn = Ağaç.Nodes.Add("Kayıtlı İşler");
            for (int i = 0; i < kaynaklar.Length; i++)
            {
                TreeNode yeni = tn.Nodes.Add(kaynaklar[i].Substring(S.Kulanıcı_Klasörü.Length));
                yeni.ToolTipText = "Başlatmak için çift tıklayın.";
            }
            tn.ExpandAll();
        }
        
        void ÖlüEkranlama_Çizdir()
        {
            try
            {
                Ekranlama.AğaçVeÇizelge_Görsellerini_Üret(S.CanliÇizdirme_ÖlçümSayısı.ToString());
                AralıkSeçici_Scroll(null, null);
                S.Çizdir();
            }
            catch (Exception ex) {  Günlük.Ekle(ex.Message); }

            S.EkranıGüncelle_me = false;
        }
        void ÖlüEkranlamayı_Başlat(string Dosyayolu)
        {
            Ölü_Ekranlama = new Ekranlama_Ölü(Dosyayolu);
            AralıkSeçici_Scroll(null, null);
            Ağaç.NodeMouseDoubleClick -= Ağaç_NodeMouseDoubleClick;
            Ağaç.AfterCheck += new TreeViewEventHandler(Ağaç_AfterCheck);
            Ağaç.NodeMouseClick += new TreeNodeMouseClickEventHandler(Ağaç_NodeMouseClick);

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

            ÖlüEkranlama_Çizdir();
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

            while (Ölü_Ekranlama.Ekle_Önceki() && S.Çalışşsın) { }

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

            while (Ölü_Ekranlama.Ekle_Sonraki() && S.Çalışşsın) { }

            ÖlüEkranlama_Çizdir();
        }
        private void Ekle_tümü_Click(object sender, EventArgs e)
        {
            Ekle_önceki.Visible = false;
            Ekle_önceki_tümü.Visible = false;
            Ekle_sonraki.Visible = false;
            Ekle_sonraki_tümü.Visible = false;
            Ekle_tümü.Visible = false;

            while (Ölü_Ekranlama.Ekle_Önceki() && S.Çalışşsın) { }
            while (Ölü_Ekranlama.Ekle_Sonraki() && S.Çalışşsın) { }

            ÖlüEkranlama_Çizdir();
        }

        private void SolMenu_Ağaç_Click(object sender, EventArgs e)
        {
            bool aç = false;

            if (Ayraç_Ana.Panel1Collapsed)
            {
                aç = true;
                Ayraç_Ana.Panel1Collapsed = false;
            }
            else if (Ayraç_Ağaç.Visible)  Ayraç_Ana.Panel1Collapsed = true;
            else aç = true;

            Günlük_Panel.Visible = false;
            Ayraç_Ağaç.Visible = aç;
        }
        private void SolMenu_Gunluk_Click(object sender, EventArgs e)
        {
            bool aç = false;

            if (Ayraç_Ana.Panel1Collapsed)
            {
                aç = true;
                Ayraç_Ana.Panel1Collapsed = false;
            }
            else if (Günlük_Panel.Visible) Ayraç_Ana.Panel1Collapsed = true;
            else aç = true;

            Ayraç_Ağaç.Visible = false;
            Günlük_Panel.Visible = aç;

            if (aç) SolMenu_Gunluk.Image = Properties.Resources.M_Gunluk;
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
        private void SolMenu_Kaydet_Click(object sender, EventArgs e)
        {
            try
            {
                int toplam = Kaydırıcı.Value + AralıkSeçici.Value;
                int başlangıç = Kaydırıcı.Value;

                DosyayaKaydetDialoğu.FileName = (Canlı_Ekranlama == null ? "" : Canlı_Ekranlama.İşAdı + "-") + S.Tarih.Yazıya(S.ZamanEkseni[başlangıç]) + "-" + S.Tarih.Yazıya(S.ZamanEkseni[toplam]) + ".csv";
                DosyayaKaydetDialoğu.FileName = DosyayaKaydetDialoğu.FileName.Replace(':', '.');
                DosyayaKaydetDialoğu.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                Günlük.Ekle(ex.ToString());
            }
        }
        private void SolMenu_DosyayaKaydetDialoğu_FileOk(object sender, System.ComponentModel.CancelEventArgs e)
        {
            try
            {
                int toplam = Kaydırıcı.Value + AralıkSeçici.Value;
                int başlangıç = Kaydırıcı.Value;

                SolMenu_Kaydet_DosyaBütünlüğüKodu = "";
                if (!DosyayaKaydetDialoğu.FileName.EndsWith(".csv")) DosyayaKaydetDialoğu.FileName += ".csv";

                using (FileStream fs = File.Create(DosyayaKaydetDialoğu.FileName))
                {
                    string yazı = S.Tarih.Yazıya(DateTime.Now) + ";Başlıklar";
                    foreach (var biri in Sinyaller.Tümü.Values)
                    {
                        if (biri.Dal == null || !biri.Dal.Checked || biri.Çizikler == null || biri.Değeri.DeğerEkseni == null) continue;

                        yazı += ";" + biri.Adı.Csv;
                    }
                    yazı += Environment.NewLine;
                    SolMenu_Kaydet_Ekle(fs, yazı);

                    for (int x = başlangıç; x < toplam && S.Çalışşsın; x++)
                    {
                        yazı = S.Tarih.Yazıya(S.ZamanEkseni[x]) + ";Sinyaller";

                        for (int y = 0; y < Sinyaller.Tümü.Count && S.Çalışşsın; y++)
                        {
                            Sinyal_ biri = Sinyaller.Tümü.Values.ElementAt(y);

                            if (biri.Dal == null || !biri.Dal.Checked || biri.Çizikler == null || biri.Değeri.DeğerEkseni == null) continue;

                            yazı += ";" + S.Sayı.Yazıya(biri.Değeri.DeğerEkseni[x]);
                        }

                        yazı += Environment.NewLine;
                        SolMenu_Kaydet_Ekle(fs, yazı);
                    }

                    yazı = S.Tarih.Yazıya(DateTime.Now) + ";Bilgi;Daha önceden alınmış ölçümlerin sadece bir kısmını içerir." + Environment.NewLine;
                    SolMenu_Kaydet_Ekle(fs, yazı);

                    yazı = S.Tarih.Yazıya(DateTime.Now) + ";Doğrulama;" + SolMenu_Kaydet_DosyaBütünlüğüKodu;
                    SolMenu_Kaydet_Ekle(fs, yazı);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                Günlük.Ekle(ex.ToString());
            }
        }
        void SolMenu_Kaydet_Ekle(FileStream fs, string yazı)
        {
            byte[] dizi = System.Text.Encoding.UTF8.GetBytes(yazı);
            fs.Write(dizi, 0, dizi.Length);

            SolMenu_Kaydet_DosyaBütünlüğüKodu = ArgeMup.HazirKod.Dönüştürme.D_GeriDönülemezKarmaşıklaştırmaMetodu.Metinden(SolMenu_Kaydet_DosyaBütünlüğüKodu + yazı);
        }

        private void AralıkSeçici_Scroll(object sender, EventArgs e)
        {
            double son_konumu = (double)Kaydırıcı.Value / (double)Kaydırıcı.Maximum;
            if (double.IsNaN(son_konumu)) son_konumu = 0;

            int azami = AralıkSeçici.Maximum - AralıkSeçici.Value;

            int aranokta = (int)((double)azami * son_konumu);
            if (aranokta < 0) aranokta = 0;

            Kaydırıcı.Value = 0;
            Kaydırıcı.Maximum = azami;
            Kaydırıcı.Value = aranokta;

            İpUcu.SetToolTip(AralıkSeçici, AralıkSeçici.Value + " adet ölçüm gösteriliyor");

            Kaydırıcı_Scroll(null, null);
        }
        private void Kaydırıcı_Scroll(object sender, ScrollEventArgs e)
        {
            int az = Kaydırıcı.Value;
            int cok = Kaydırıcı.Value + AralıkSeçici.Value;
            foreach (var biri in Sinyaller.Tümü)
            {
                if (biri.Value.Çizikler == null) continue;

                biri.Value.Çizikler.minRenderIndex = az;
                biri.Value.Çizikler.maxRenderIndex = cok;
            }

            İpUcu.SetToolTip(Kaydırıcı, az + " ile " + cok + " arasındaki ölçümler gösteriliyor");

            if (Ağaç.SelectedNode == null) Ağaç.SelectedNode = Ağaç.Nodes[0];
            Ağaç_NodeMouseClick(null, new TreeNodeMouseClickEventArgs(Ağaç.SelectedNode, MouseButtons.None, 0, 0, 0));
        }

        private void SağTuşMenü_Senaryo_Çalıştır_Click(object sender, EventArgs e)
        {
            (Ağaç.SelectedNode.Tag as Senaryo_).Çalıştır();
        }
        private void SağTuşMenü_Senaryo_Durdur_Click(object sender, EventArgs e)
        {
            (Ağaç.SelectedNode.Tag as Senaryo_).Durdur();
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

                    if (!S.EkranıGüncelle_me) S.Çizdir();
                }
            }
            else
            {
                //sadece ilgili olan sinyali
                if (e.Node.Checked)
                {
                    if (!Çizelge.plt.GetPlottables().Contains(e.Node.Tag)) Çizelge.plt.Add((e.Node.Tag as Sinyal_).Çizikler);

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
                    Çizelge.plt.Remove((e.Node.Tag as Sinyal_).Çizikler);

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
            S.Çizdirme_Koordinat_Tick = 0;
        }
        private void Ağaç_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (Ağaç.SelectedNode == null || Ağaç.SelectedNode.Parent == null) return;

            try
            {
                string yol = S.Kulanıcı_Klasörü + Ağaç.SelectedNode.Text;
                if (!Directory.Exists(yol)) throw new Exception(yol + " klasörüne erişilemiyor");

                Canlı_Ekranlama = new Ekranlama_Canlı(Ağaç.SelectedNode.Text, yol);
                if (Senaryolar.Tümü.Count > 0)
                {
                    foreach (var biri in Senaryolar.Tümü.Values)
                    {
                        biri.Dal.ContextMenuStrip = SağTuşMenü_Senaryo;
                    }
                }

                AralıkSeçici.Minimum = 2;
                AralıkSeçici.Maximum = S.CanliÇizdirme_ÖlçümSayısı - 1;
                AralıkSeçici.Value = AralıkSeçici.Maximum;
                AralıkSeçici_Scroll(null, null);

                Ağaç.NodeMouseDoubleClick -= Ağaç_NodeMouseDoubleClick;
                Ağaç.AfterCheck += new TreeViewEventHandler(Ağaç_AfterCheck);
                Ağaç.NodeMouseClick += new TreeNodeMouseClickEventHandler(Ağaç_NodeMouseClick);

                SolMenu_BaşlatDurdur.Visible = true;
                SolMenu_BaşlatDurdur_Ayraç.Visible = true;
            }
            catch (Exception ex) 
            { 
                Günlük.Ekle(ex.ToString());
                AnaEkran_Ağacı_Başlat();
            }
        }
        private void Ağaç_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (S.EkranıGüncelle_me || e.Node == null) return;

			Ağaç.SelectedNode = e.Node;
            if (e.Node.Tag == null || e.Node.Tag.GetType() != typeof(Sinyal_))
            {
                //kesinlikle bir sinyal değil
                if (e.Node.Parent != null && e.Node.Nodes != null && e.Node.Nodes.Count > 0)
                {
                    if (ÖlçeğiSeçiliOlanaGöreAyarla.Checked)
                    {
                        double[] sınır_d = new double[e.Node.Nodes.Count], sınır_y = new double[e.Node.Nodes.Count];
                        for (int i = 0; i < e.Node.Nodes.Count; i++)
                        {
                            if (e.Node.Nodes[i].Tag != null && e.Node.Nodes[i].Tag.GetType() == typeof(Sinyal_) && (e.Node.Nodes[i].Tag as Sinyal_).Değeri.DeğerEkseni != null)
                            {
                                EnDüşükEnYüksek((e.Node.Nodes[i].Tag as Sinyal_).Değeri.DeğerEkseni, out sınır_d[i], out sınır_y[i]);
                            }  
                        }
                        EnDüşükEnYüksek(sınır_d, out double düşük, out double d_y);
                        EnDüşükEnYüksek(sınır_y, out double y_d, out double yüksek);

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
                else if (enazbirtanekalınvar)
                {
                    foreach (var biri in Sinyaller.Tümü) { if (biri.Value.Çizikler != null) biri.Value.Çizikler.lineWidth = 1; }

                    Çizelge.plt.AxisBounds();
                    Çizelge.plt.YLabel("Tümü", true, null, null, Color.Black);
                    Çizelge.plt.Style(null, null, null, Color.Black);

                    S.Çizdir();
                    enazbirtanekalınvar = false;
                }
            }
            else
            {
                //kesinlikle bir sinyal dalı
                if ((e.Node.Tag as Sinyal_).Çizikler == null || (e.Node.Tag as Sinyal_).Değeri.DeğerEkseni == null) return;

                if (SeçiliOlanıBelirgenleştir.Checked)
                {
                    foreach (var biri in Sinyaller.Tümü) { if (biri.Value.Çizikler != null) biri.Value.Çizikler.lineWidth = 1; }
                    (e.Node.Tag as Sinyal_).Çizikler.lineWidth = 2;
                }

                if (ÖlçeğiSeçiliOlanaGöreAyarla.Checked)
                {
                    EnDüşükEnYüksek((e.Node.Tag as Sinyal_).Değeri.DeğerEkseni, out double düşük, out double yüksek);

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
                    Çizelge.plt.YLabel((e.Node.Tag as Sinyal_).Adı.GörünenAdı, true, null, null, (e.Node.Tag as Sinyal_).Çizikler.color);
                    Çizelge.plt.Style(null, null, null, (e.Node.Tag as Sinyal_).Çizikler.color);
                }

                enazbirtanekalınvar = true;
            }
            
            S.Çizdir();
        }
        void EnDüşükEnYüksek(double[] arr, out double Düşük, out double Yüksek, int toplam = int.MinValue)
        {
            Düşük = double.MaxValue;
            Yüksek = double.MinValue;

            int başlangıç;
            if (toplam == int.MinValue)
            {
                toplam = arr.Length;
                başlangıç = 0;
            }
            else
            {
                toplam = Kaydırıcı.Value + AralıkSeçici.Value;
                başlangıç = Kaydırıcı.Value;
            }

            for (int i = başlangıç; i < toplam; i++)
            {
                if (double.IsNaN(arr[i]) || double.IsInfinity(arr[i])) continue;

                if (arr[i] < Düşük) Düşük = arr[i];
                if (arr[i] > Yüksek) Yüksek = arr[i];
            }

            if (Düşük == double.MaxValue) Düşük = double.MinValue;
            if (Yüksek == double.MinValue) Düşük = double.MaxValue;
        }

        private void Eposta_Click(object sender, EventArgs e)
        {
            string a = string.Format("mailto:{0}?Subject={1}&Body={2}", "argemup@yandex.com", Text + " Hk.", "Mesajınız");
            System.Diagnostics.Process.Start(a);
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
            if (S.EkranıGüncelle_me || S.ZamanEkseni == null || S.ZamanEkseni.Length < 2) return;

            double Koordinat_X = Çizelge.GetMouseCoordinates().x;
            //double Koordinat_Y = Çizelge.GetMouseCoordinates().y;
            //if (S.Çizdirme_DikeyÇizgi == null) S.Çizdirme_DikeyÇizgi = Çizelge.plt.PlotVLine(Koordinat_X, color: Color.Blue, lineStyle: ScottPlot.LineStyle.Dash);
            //else S.Çizdirme_DikeyÇizgi.position = Koordinat_X;
            //if (S.Çizdirme_YatayÇizgi == null) S.Çizdirme_YatayÇizgi = Çizelge.plt.PlotHLine(Koordinat_Y, color: Color.Blue, lineStyle: ScottPlot.LineStyle.Dash);
            //else S.Çizdirme_YatayÇizgi.position = Koordinat_Y;

            int bulundu = -1;
            double pencere = (S.ZamanEkseni[S.ZamanEkseni.Length - 1] - S.ZamanEkseni[S.ZamanEkseni.Length - 2]) / 2;

            int başla = S.Kaydırıcı.Value;
            int bitir = S.Kaydırıcı.Value + S.AralıkSeçici.Value;
            while (bulundu == -1 && başla != bitir && S.Çalışşsın)
            {
                int tahmini_konum = başla + ((bitir - başla) / 2);
                if (tahmini_konum == başla) break; //bulunamadı

                if (Koordinat_X <= S.ZamanEkseni[tahmini_konum] + pencere)
                {
                    if (Koordinat_X >= S.ZamanEkseni[tahmini_konum] - pencere)
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
                //S.Çizdir(); //yatay dikey çizgiler için
                return;
            }

            S.Çizdirme_Koordinat_Tick = Environment.TickCount + 5000;

            //if (S.Çizdirme_Noktacıklar.Length < Sinyaller.Tümü.Count) Array.Resize(ref S.Çizdirme_Noktacıklar, Sinyaller.Tümü.Count);

            S.Ağaç.Nodes[0].Text = "İmleç Konumu - " + S.Tarih.Yazıya(S.ZamanEkseni[bulundu]);
            for (int i = 0; i < Sinyaller.Tümü.Count; i++)
            {
                Sinyal_ biri = Sinyaller.Tümü.Values.ElementAt(i);

                if (biri.Değeri.DeğerEkseni == null ||
                    biri.Dal == null ||
                    !biri.Dal.IsVisible)
                {
                    //if (S.Çizdirme_Noktacıklar[i] != null) S.Çizelge.plt.Remove(S.Çizdirme_Noktacıklar[i]);
                    continue;
                }
                
                biri.Dal.Text = biri.Adı.GörünenAdı + " : " + S.Sayı.Yazıya(biri.Değeri.DeğerEkseni[bulundu]);

                //if (S.Çizdirme_Noktacıklar[i] == null) S.Çizdirme_Noktacıklar[i] = Çizelge.plt.PlotPoint(S.ZamanEkseni[bulundu], biri.Değeri.DeğerEkseni[bulundu], biri.Çizikler.color, 15);
                //else
                //{
                //    S.Çizdirme_Noktacıklar[i].xs[0] = S.ZamanEkseni[bulundu];
                //    S.Çizdirme_Noktacıklar[i].ys[0] = biri.Değeri.DeğerEkseni[bulundu];
                //}
            }

            //S.Çizdir(); //yatay dikey çizgiler için
        }
    }
}
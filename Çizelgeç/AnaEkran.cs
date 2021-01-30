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

            AnaEkran_Ağacı_Başlat();

            File.WriteAllBytes(S.Şablon_Klasörü + "Ayarlar.json", Properties.Resources.Ayarlar);
            File.WriteAllBytes(S.Şablon_Klasörü + "Senaryo1.json", Properties.Resources.Senaryo1);
            
            if (S.BaşlangıçParametreleri != null && S.BaşlangıçParametreleri.Length == 1 && File.Exists(S.BaşlangıçParametreleri[0]) && (Path.GetExtension(S.BaşlangıçParametreleri[0]) == ".csv" || Path.GetExtension(S.BaşlangıçParametreleri[0]) == ".mup"))
            {
                try
                {
                    new Ekranlama_Ölü(S.BaşlangıçParametreleri[0]);
                    AralıkSeçici_Scroll(null, null);
                    Ağaç.AfterCheck += new TreeViewEventHandler(Ağaç_AfterCheck);
                    Ağaç.NodeMouseClick += new TreeNodeMouseClickEventHandler(Ağaç_NodeMouseClick);
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
                try
                {
                    new Ekranlama_Ölü(files[0]);
                    AralıkSeçici_Scroll(null, null);
                    Ağaç.NodeMouseDoubleClick -= Ağaç_NodeMouseDoubleClick;
                    Ağaç.AfterCheck += new TreeViewEventHandler(Ağaç_AfterCheck);
                    Ağaç.NodeMouseClick += new TreeNodeMouseClickEventHandler(Ağaç_NodeMouseClick);
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

        private void AralıkSeçici_Scroll(object sender, EventArgs e)
        {
            int azami = AralıkSeçici.Maximum - AralıkSeçici.Value;
            //if (Kaydırıcı.Value > azami) Kaydırıcı.Value = azami;
            Kaydırıcı.Value = 0;
            Kaydırıcı.Maximum = azami;
            Kaydırıcı.Value = azami;

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
            if (S.EkranıGüncelle_me) return;

            if (e.Node.Tag == null || e.Node.Tag.GetType() != typeof(Sinyal_))
            {
                //alt elemanları işle
                foreach (TreeNode biri in e.Node.Nodes)
                {
                    biri.Checked = e.Node.Checked;
                }

                if (e.Node.Parent == null && e.Node.Checked == false)
                {
                    foreach (var biri in Sinyaller.Tümü.Values)
                    {
                        Çizelge.plt.Remove(biri.Çizikler);
                    }
                }
            }
            else
            {
                //sadece ilgili olan sinyali
                if (e.Node.Checked)
                {
                    if (!Çizelge.plt.GetPlottables().Contains(e.Node.Tag)) Çizelge.plt.Add((e.Node.Tag as Sinyal_).Çizikler);

                    if ((e.Node.Tag as Sinyal_).Uyarı_Yazıları != null)
                    {
                        foreach (var biri in (e.Node.Tag as Sinyal_).Uyarı_Yazıları)
                        {
                            if (!Çizelge.plt.GetPlottables().Contains(biri)) Çizelge.plt.Add(biri);
                        }
                    }
                }
                else
                {
                    Çizelge.plt.Remove((e.Node.Tag as Sinyal_).Çizikler);

                    if ((e.Node.Tag as Sinyal_).Uyarı_Yazıları != null)
                    {
                        foreach (var biri in (e.Node.Tag as Sinyal_).Uyarı_Yazıları)
                        {
                            Çizelge.plt.Remove(biri);
                        }
                    }
                }
            }

            //S.Çizdir();
        }
        private void Ağaç_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (Ağaç.SelectedNode == null || Ağaç.SelectedNode.Parent == null) return;

            try
            {
                string yol = S.Kulanıcı_Klasörü + Ağaç.SelectedNode.Text;
                if (!Directory.Exists(yol)) throw new Exception(yol + " klasörüne erişilemiyor");

                new Ekranlama_Canlı(Ağaç.SelectedNode.Text, yol);
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

                Ağaç.Nodes[0].Nodes[1].Checked = false; //Değişkenleri başlanıçta kapat
            }
            catch (Exception ex) 
            { 
                Günlük.Ekle(ex.ToString());
                AnaEkran_Ağacı_Başlat();
            }
        }
        private void Ağaç_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (S.EkranıGüncelle_me) return;

            Ağaç.SelectedNode = e.Node;

            if (e.Node == null || e.Node.Tag == null || e.Node.Tag.GetType() != typeof(Sinyal_))
            {
                if (e.Node != null && e.Node.Nodes != null && e.Node.Nodes.Count > 0 && e.Node.Nodes[0].Tag != null && e.Node.Nodes[0].Tag.GetType() == typeof(Sinyal_))
                {
                    if (ÖlçeğiSeçiliOlanaGöreAyarla.Checked)
                    {
                        double[] sınır_d = new double[e.Node.Nodes.Count], sınır_y = new double[e.Node.Nodes.Count];
                        for (int i = 0; i < e.Node.Nodes.Count; i++)
                        {
                            EnDüşükEnYüksek((e.Node.Nodes[i].Tag as Sinyal_).Değeri.DeğerEkseni, out sınır_d[i], out sınır_y[i]);
                        }
                        EnDüşükEnYüksek(sınır_d, out double düşük, out double d_y);
                        EnDüşükEnYüksek(sınır_y, out double y_d, out double yüksek);

                        if (düşük == yüksek) yüksek = düşük + 1;
                        else
                        {
                            double oran = (yüksek - düşük) * 0.05;
                            yüksek += oran;
                            düşük -= oran;
                        }

                        Çizelge.plt.AxisBounds(double.NegativeInfinity, double.PositiveInfinity, düşük, yüksek);
                        Çizelge.plt.YLabel((e.Node.Nodes[0].Tag as Sinyal_).Adı.Grup, true, null, null, Color.Black);
                        Çizelge.plt.Style(null, null, null, Color.Black);

                        enazbirtanekalınvar = true;
                    }
                }
                else if (enazbirtanekalınvar)
                {
                    foreach (var biri in Sinyaller.Tümü) { biri.Value.Çizikler.lineWidth = 1; }

                    Çizelge.plt.AxisBounds();
                    Çizelge.plt.YLabel("Tümü", true, null, null, Color.Black);
                    Çizelge.plt.Style(null, null, null, Color.Black);

                    S.Çizdir();
                    enazbirtanekalınvar = false;
                }
            }
            else
            {
                if (SeçiliOlanıBelirgenleştir.Checked)
                {
                    foreach (var biri in Sinyaller.Tümü) { biri.Value.Çizikler.lineWidth = 1; }
                    (e.Node.Tag as Sinyal_).Çizikler.lineWidth = 2;
                }

                if (ÖlçeğiSeçiliOlanaGöreAyarla.Checked)
                {
                    EnDüşükEnYüksek((e.Node.Tag as Sinyal_).Değeri.DeğerEkseni, out double düşük, out double yüksek);

                    if (düşük == yüksek) yüksek = düşük + 1;
                    else
                    {
                        double oran = (yüksek - düşük) * 0.05;
                        yüksek += oran;
                        düşük -= oran;
                    }

                    Çizelge.plt.AxisBounds(double.NegativeInfinity, double.PositiveInfinity, düşük, yüksek);
                    Çizelge.plt.YLabel((e.Node.Tag as Sinyal_).Adı.Uzun, true, null, null, (e.Node.Tag as Sinyal_).Çizikler.color);
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
    }
}
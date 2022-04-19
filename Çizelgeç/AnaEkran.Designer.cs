namespace Çizelgeç
{
    partial class AnaEkran
    {
        class ÇiftTamponluTreeView : System.Windows.Forms.TreeView
        {
            private const int TVM_SETEXTENDEDSTYLE = 0x1100 + 44;
            private const int TVM_GETEXTENDEDSTYLE = 0x1100 + 45;
            private const int TVS_EX_DOUBLEBUFFER = 0x0004;
            protected override void OnHandleCreated(System.EventArgs e)
            {
                ArgeMup.HazirKod.W32_9.SendMessage(this.Handle, TVM_SETEXTENDEDSTYLE, (System.IntPtr)TVS_EX_DOUBLEBUFFER, (System.IntPtr)TVS_EX_DOUBLEBUFFER);
                base.OnHandleCreated(e);
            }
        }

        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AnaEkran));
            this.İpUcu = new System.Windows.Forms.ToolTip(this.components);
            this.Günlük_YeniMesajaGit = new System.Windows.Forms.CheckBox();
            this.Ekle_önceki = new System.Windows.Forms.Button();
            this.Ekle_önceki_tümü = new System.Windows.Forms.Button();
            this.Ekle_sonraki_tümü = new System.Windows.Forms.Button();
            this.Ekle_sonraki = new System.Windows.Forms.Button();
            this.Ekle_tümü = new System.Windows.Forms.Button();
            this.AralıkSeçici_Baştan = new System.Windows.Forms.TrackBar();
            this.Kaydırıcı = new System.Windows.Forms.TrackBar();
            this.AralıkSeçici_Sondan = new System.Windows.Forms.TrackBar();
            this.SolMenü = new System.Windows.Forms.ToolStrip();
            this.SolMenu_Ağaç = new System.Windows.Forms.ToolStripButton();
            this.Eposta = new System.Windows.Forms.ToolStripButton();
            this.SolMenu_Cizelge = new System.Windows.Forms.ToolStripButton();
            this.SolMenu_Gunluk = new System.Windows.Forms.ToolStripButton();
            this.Menu_aA = new System.Windows.Forms.ToolStripDropDownButton();
            this.Menu_aA_100 = new System.Windows.Forms.ToolStripMenuItem();
            this.Menu_aA_125 = new System.Windows.Forms.ToolStripMenuItem();
            this.Menu_aA_150 = new System.Windows.Forms.ToolStripMenuItem();
            this.Menu_aA_175 = new System.Windows.Forms.ToolStripMenuItem();
            this.Menu_aA_200 = new System.Windows.Forms.ToolStripMenuItem();
            this.SolMenu_BaşlatDurdur_Ayraç = new System.Windows.Forms.ToolStripSeparator();
            this.SolMenu_BaşlatDurdur = new System.Windows.Forms.ToolStripButton();
            this.Ayraç_Ana = new System.Windows.Forms.SplitContainer();
            this.İmleçKonumuTarihi = new System.Windows.Forms.Label();
            this.Ağaç = new Çizelgeç.AnaEkran.ÇiftTamponluTreeView();
            this.Günlük_Panel = new System.Windows.Forms.Panel();
            this.Günlük_MetinKutusu = new System.Windows.Forms.TextBox();
            this.Çizelge = new ScottPlot.FormsPlot();
            this.SağTuşMenü_Çizelge = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.SağTuşMenü_Çizelge_Etkin = new System.Windows.Forms.ToolStripMenuItem();
            this.SağTuşMenü_Çizelge_TümSinyalleriEkranaSığdır = new System.Windows.Forms.ToolStripMenuItem();
            this.SağTuşMenü_Çizelge_SeçiliOlanıBelirginleştr = new System.Windows.Forms.ToolStripMenuItem();
            this.SağTuşMenü_Çizelge_Y_EkseniÖlçeğiniSeçiliOlanSinyaleUyarla = new System.Windows.Forms.ToolStripMenuItem();
            this.SağTuşMenü_Çizelge_Birbirinin_aynısı_olan_zaman_dilimlerini_atla = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.SağTuşMenü_Çizelge_Daralt = new System.Windows.Forms.ToolStripMenuItem();
            this.SağTuşMenü_Çizelge_Daralt_Baştanİtibaren = new System.Windows.Forms.ToolStripTextBox();
            this.SağTuşMenü_Çizelge_Daralt_Sondanİtibaren = new System.Windows.Forms.ToolStripTextBox();
            this.SağTuşMenü_Çizelge_Daralt_Uygula = new System.Windows.Forms.ToolStripMenuItem();
            this.SağTuşMenü_Çizelge_ÇizgiKalınlığı = new System.Windows.Forms.ToolStripMenuItem();
            this.SağTuşMenü_Çizelge_ÇizgiKalınlığı_Değer = new System.Windows.Forms.ToolStripTextBox();
            this.SağTuşMenü_Çizelge_ÇizgiKalınlığı_TümüneUygula = new System.Windows.Forms.ToolStripMenuItem();
            this.SağTuşMenü_Çizelge_ÇizgiKalınlığı_SadeceEtkinOlanlar = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.SağTuşMenü_Çizelge_dışarıAktar = new System.Windows.Forms.ToolStripMenuItem();
            this.SağTuşMenü_Çizelge_panoyaKopyala = new System.Windows.Forms.ToolStripMenuItem();
            this.SağTuşMenü_Çizelge_KayıtKlasörünüAç = new System.Windows.Forms.ToolStripMenuItem();
            this.SağTuşMenü_Çizelge_Ayırıcı1 = new System.Windows.Forms.ToolStripSeparator();
            this.SağTuşMenü_Çizelge_DeğerleriNormalleştir = new System.Windows.Forms.ToolStripMenuItem();
            this.SağTuşMenü_Çizelge_xEkseniTarihVeSaat = new System.Windows.Forms.ToolStripMenuItem();
            this.SağTuşMenü_Çizelge_YenidenHesapla = new System.Windows.Forms.ToolStripMenuItem();
            this.SağTuşMenü_Çizelge_YenidenHesapla_Değer = new System.Windows.Forms.ToolStripTextBox();
            this.SağTuşMenü_Çizelge_YenidenHesapla_SadeceSeçiliDalaUygula = new System.Windows.Forms.ToolStripMenuItem();
            this.SağTuşMenü_Çizelge_YenidenHesapla_SadeceEtkinOlanlaraUygula = new System.Windows.Forms.ToolStripMenuItem();
            this.SağTuşMenü_Çizelge_YenidenHesapla_TününeUygula = new System.Windows.Forms.ToolStripMenuItem();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.ResimListesi = new System.Windows.Forms.ImageList(this.components);
            this.SağTuşMenü_Senaryo = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.SağTuşMenü_Senaryo_Çalıştır = new System.Windows.Forms.ToolStripMenuItem();
            this.SağTuşMenü_Senaryo_Durdur = new System.Windows.Forms.ToolStripMenuItem();
            this.DosyayaKaydetDialoğu = new System.Windows.Forms.SaveFileDialog();
            ((System.ComponentModel.ISupportInitialize)(this.AralıkSeçici_Baştan)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Kaydırıcı)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.AralıkSeçici_Sondan)).BeginInit();
            this.SolMenü.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Ayraç_Ana)).BeginInit();
            this.Ayraç_Ana.Panel1.SuspendLayout();
            this.Ayraç_Ana.Panel2.SuspendLayout();
            this.Ayraç_Ana.SuspendLayout();
            this.Günlük_Panel.SuspendLayout();
            this.SağTuşMenü_Çizelge.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.SağTuşMenü_Senaryo.SuspendLayout();
            this.SuspendLayout();
            // 
            // İpUcu
            // 
            this.İpUcu.AutomaticDelay = 0;
            this.İpUcu.AutoPopDelay = 15000;
            this.İpUcu.InitialDelay = 0;
            this.İpUcu.ReshowDelay = 0;
            this.İpUcu.UseAnimation = false;
            this.İpUcu.UseFading = false;
            // 
            // Günlük_YeniMesajaGit
            // 
            this.Günlük_YeniMesajaGit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.Günlük_YeniMesajaGit.AutoSize = true;
            this.Günlük_YeniMesajaGit.Checked = true;
            this.Günlük_YeniMesajaGit.CheckState = System.Windows.Forms.CheckState.Checked;
            this.Günlük_YeniMesajaGit.Location = new System.Drawing.Point(118, 100);
            this.Günlük_YeniMesajaGit.Name = "Günlük_YeniMesajaGit";
            this.Günlük_YeniMesajaGit.Size = new System.Drawing.Size(18, 17);
            this.Günlük_YeniMesajaGit.TabIndex = 2;
            this.İpUcu.SetToolTip(this.Günlük_YeniMesajaGit, "Yeni Mesaja Git");
            this.Günlük_YeniMesajaGit.UseVisualStyleBackColor = true;
            // 
            // Ekle_önceki
            // 
            this.Ekle_önceki.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.Ekle_önceki.Font = new System.Drawing.Font("Microsoft Sans Serif", 5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.Ekle_önceki.Location = new System.Drawing.Point(691, 118);
            this.Ekle_önceki.Name = "Ekle_önceki";
            this.Ekle_önceki.Size = new System.Drawing.Size(24, 20);
            this.Ekle_önceki.TabIndex = 4;
            this.Ekle_önceki.Text = "<";
            this.İpUcu.SetToolTip(this.Ekle_önceki, "Öncekini ekle");
            this.Ekle_önceki.UseVisualStyleBackColor = true;
            this.Ekle_önceki.Visible = false;
            this.Ekle_önceki.Click += new System.EventHandler(this.Ekle_önceki_Click);
            // 
            // Ekle_önceki_tümü
            // 
            this.Ekle_önceki_tümü.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.Ekle_önceki_tümü.Font = new System.Drawing.Font("Microsoft Sans Serif", 5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.Ekle_önceki_tümü.Location = new System.Drawing.Point(691, 135);
            this.Ekle_önceki_tümü.Name = "Ekle_önceki_tümü";
            this.Ekle_önceki_tümü.Size = new System.Drawing.Size(24, 20);
            this.Ekle_önceki_tümü.TabIndex = 5;
            this.Ekle_önceki_tümü.Text = "<<";
            this.İpUcu.SetToolTip(this.Ekle_önceki_tümü, "Tüm öncekileri ekle");
            this.Ekle_önceki_tümü.UseVisualStyleBackColor = true;
            this.Ekle_önceki_tümü.Visible = false;
            this.Ekle_önceki_tümü.Click += new System.EventHandler(this.Ekle_önceki_tümü_Click);
            // 
            // Ekle_sonraki_tümü
            // 
            this.Ekle_sonraki_tümü.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.Ekle_sonraki_tümü.Font = new System.Drawing.Font("Microsoft Sans Serif", 5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.Ekle_sonraki_tümü.Location = new System.Drawing.Point(691, 170);
            this.Ekle_sonraki_tümü.Name = "Ekle_sonraki_tümü";
            this.Ekle_sonraki_tümü.Size = new System.Drawing.Size(24, 20);
            this.Ekle_sonraki_tümü.TabIndex = 7;
            this.Ekle_sonraki_tümü.Text = ">>";
            this.İpUcu.SetToolTip(this.Ekle_sonraki_tümü, "Tüm sonrakileri ekle");
            this.Ekle_sonraki_tümü.UseVisualStyleBackColor = true;
            this.Ekle_sonraki_tümü.Visible = false;
            this.Ekle_sonraki_tümü.Click += new System.EventHandler(this.Ekle_sonraki_tümü_Click);
            // 
            // Ekle_sonraki
            // 
            this.Ekle_sonraki.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.Ekle_sonraki.Font = new System.Drawing.Font("Microsoft Sans Serif", 5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.Ekle_sonraki.Location = new System.Drawing.Point(691, 187);
            this.Ekle_sonraki.Name = "Ekle_sonraki";
            this.Ekle_sonraki.Size = new System.Drawing.Size(24, 20);
            this.Ekle_sonraki.TabIndex = 8;
            this.Ekle_sonraki.Text = ">";
            this.İpUcu.SetToolTip(this.Ekle_sonraki, "Sonrakini ekle");
            this.Ekle_sonraki.UseVisualStyleBackColor = true;
            this.Ekle_sonraki.Visible = false;
            this.Ekle_sonraki.Click += new System.EventHandler(this.Ekle_sonraki_Click);
            // 
            // Ekle_tümü
            // 
            this.Ekle_tümü.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.Ekle_tümü.Font = new System.Drawing.Font("Microsoft Sans Serif", 5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.Ekle_tümü.Location = new System.Drawing.Point(691, 153);
            this.Ekle_tümü.Name = "Ekle_tümü";
            this.Ekle_tümü.Size = new System.Drawing.Size(24, 20);
            this.Ekle_tümü.TabIndex = 6;
            this.Ekle_tümü.Text = "<>";
            this.İpUcu.SetToolTip(this.Ekle_tümü, "Tümünü ekle");
            this.Ekle_tümü.UseVisualStyleBackColor = true;
            this.Ekle_tümü.Visible = false;
            this.Ekle_tümü.Click += new System.EventHandler(this.Ekle_tümü_Click);
            // 
            // AralıkSeçici_Baştan
            // 
            this.AralıkSeçici_Baştan.BackColor = System.Drawing.SystemColors.Control;
            this.AralıkSeçici_Baştan.Dock = System.Windows.Forms.DockStyle.Top;
            this.AralıkSeçici_Baştan.LargeChange = 1;
            this.AralıkSeçici_Baştan.Location = new System.Drawing.Point(3, 3);
            this.AralıkSeçici_Baştan.Name = "AralıkSeçici_Baştan";
            this.AralıkSeçici_Baştan.Size = new System.Drawing.Size(231, 37);
            this.AralıkSeçici_Baştan.TabIndex = 1;
            this.AralıkSeçici_Baştan.TickStyle = System.Windows.Forms.TickStyle.Both;
            this.İpUcu.SetToolTip(this.AralıkSeçici_Baştan, "Baştan itibaren daralt (Shift+Ctrl+Yön tuşları)");
            this.AralıkSeçici_Baştan.Scroll += new System.EventHandler(this.AralıkSeçici_Baştan_Scroll);
            this.AralıkSeçici_Baştan.Enter += new System.EventHandler(this.AralıkSeçici_Baştan_Enter);
            this.AralıkSeçici_Baştan.KeyDown += new System.Windows.Forms.KeyEventHandler(this.AralıkSeçici_KeyDown);
            // 
            // Kaydırıcı
            // 
            this.Kaydırıcı.BackColor = System.Drawing.SystemColors.Control;
            this.Kaydırıcı.Dock = System.Windows.Forms.DockStyle.Top;
            this.Kaydırıcı.LargeChange = 1;
            this.Kaydırıcı.Location = new System.Drawing.Point(240, 3);
            this.Kaydırıcı.Name = "Kaydırıcı";
            this.Kaydırıcı.Size = new System.Drawing.Size(231, 37);
            this.Kaydırıcı.TabIndex = 2;
            this.Kaydırıcı.TickStyle = System.Windows.Forms.TickStyle.Both;
            this.İpUcu.SetToolTip(this.Kaydırıcı, "Baştan itibaren daralt");
            this.Kaydırıcı.Scroll += new System.EventHandler(this.Kaydırıcı_Scroll);
            this.Kaydırıcı.Enter += new System.EventHandler(this.Kaydırıcı_Enter);
            this.Kaydırıcı.KeyDown += new System.Windows.Forms.KeyEventHandler(this.AralıkSeçici_KeyDown);
            // 
            // AralıkSeçici_Sondan
            // 
            this.AralıkSeçici_Sondan.BackColor = System.Drawing.SystemColors.Control;
            this.AralıkSeçici_Sondan.Dock = System.Windows.Forms.DockStyle.Top;
            this.AralıkSeçici_Sondan.LargeChange = 1;
            this.AralıkSeçici_Sondan.Location = new System.Drawing.Point(477, 3);
            this.AralıkSeçici_Sondan.Name = "AralıkSeçici_Sondan";
            this.AralıkSeçici_Sondan.Size = new System.Drawing.Size(233, 37);
            this.AralıkSeçici_Sondan.TabIndex = 3;
            this.AralıkSeçici_Sondan.TickStyle = System.Windows.Forms.TickStyle.Both;
            this.İpUcu.SetToolTip(this.AralıkSeçici_Sondan, "Sondan itibaren daralt");
            this.AralıkSeçici_Sondan.Scroll += new System.EventHandler(this.AralıkSeçici_Sondan_Scroll);
            this.AralıkSeçici_Sondan.Enter += new System.EventHandler(this.AralıkSeçici_Sondan_Enter);
            this.AralıkSeçici_Sondan.KeyDown += new System.Windows.Forms.KeyEventHandler(this.AralıkSeçici_KeyDown);
            // 
            // SolMenü
            // 
            this.SolMenü.Dock = System.Windows.Forms.DockStyle.Left;
            this.SolMenü.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.SolMenü.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.SolMenü.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.SolMenu_Ağaç,
            this.Eposta,
            this.SolMenu_Cizelge,
            this.SolMenu_Gunluk,
            this.Menu_aA,
            this.SolMenu_BaşlatDurdur_Ayraç,
            this.SolMenu_BaşlatDurdur});
            this.SolMenü.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.VerticalStackWithOverflow;
            this.SolMenü.Location = new System.Drawing.Point(0, 0);
            this.SolMenü.Name = "SolMenü";
            this.SolMenü.Size = new System.Drawing.Size(32, 364);
            this.SolMenü.TabIndex = 0;
            this.SolMenü.Text = "toolStrip1";
            // 
            // SolMenu_Ağaç
            // 
            this.SolMenu_Ağaç.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.SolMenu_Ağaç.Image = global::Çizelgeç.Properties.Resources.M_Senaryo;
            this.SolMenu_Ağaç.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.SolMenu_Ağaç.Name = "SolMenu_Ağaç";
            this.SolMenu_Ağaç.Size = new System.Drawing.Size(29, 24);
            this.SolMenu_Ağaç.Text = "Görseller";
            this.SolMenu_Ağaç.Click += new System.EventHandler(this.SolMenu_Ağaç_Click);
            // 
            // Eposta
            // 
            this.Eposta.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.Eposta.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.Eposta.Image = global::Çizelgeç.Properties.Resources.logo;
            this.Eposta.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.Eposta.Name = "Eposta";
            this.Eposta.Size = new System.Drawing.Size(29, 24);
            this.Eposta.Text = "toolStripButton1";
            this.Eposta.ToolTipText = "ArgeMup@yandex.com";
            this.Eposta.Click += new System.EventHandler(this.Eposta_Click);
            // 
            // SolMenu_Cizelge
            // 
            this.SolMenu_Cizelge.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.SolMenu_Cizelge.Image = global::Çizelgeç.Properties.Resources.M_Cizelge;
            this.SolMenu_Cizelge.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.SolMenu_Cizelge.Name = "SolMenu_Cizelge";
            this.SolMenu_Cizelge.Size = new System.Drawing.Size(29, 24);
            this.SolMenu_Cizelge.Text = "Uyarılar";
            this.SolMenu_Cizelge.Click += new System.EventHandler(this.SolMenu_Cizelge_Click);
            // 
            // SolMenu_Gunluk
            // 
            this.SolMenu_Gunluk.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.SolMenu_Gunluk.Image = global::Çizelgeç.Properties.Resources.M_Gunluk;
            this.SolMenu_Gunluk.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.SolMenu_Gunluk.Name = "SolMenu_Gunluk";
            this.SolMenu_Gunluk.Size = new System.Drawing.Size(29, 24);
            this.SolMenu_Gunluk.Text = "Uyarılar";
            this.SolMenu_Gunluk.Click += new System.EventHandler(this.SolMenu_Gunluk_Click);
            // 
            // Menu_aA
            // 
            this.Menu_aA.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.Menu_aA.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.Menu_aA.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.Menu_aA_100,
            this.Menu_aA_125,
            this.Menu_aA_150,
            this.Menu_aA_175,
            this.Menu_aA_200});
            this.Menu_aA.Image = ((System.Drawing.Image)(resources.GetObject("Menu_aA.Image")));
            this.Menu_aA.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.Menu_aA.Name = "Menu_aA";
            this.Menu_aA.ShowDropDownArrow = false;
            this.Menu_aA.Size = new System.Drawing.Size(29, 24);
            this.Menu_aA.Text = "aA";
            this.Menu_aA.ToolTipText = "Karakter Büyüklüğü";
            // 
            // Menu_aA_100
            // 
            this.Menu_aA_100.Name = "Menu_aA_100";
            this.Menu_aA_100.Size = new System.Drawing.Size(132, 26);
            this.Menu_aA_100.Text = "% 100";
            this.Menu_aA_100.Click += new System.EventHandler(this.Menu_aA_100_Click);
            // 
            // Menu_aA_125
            // 
            this.Menu_aA_125.Name = "Menu_aA_125";
            this.Menu_aA_125.Size = new System.Drawing.Size(132, 26);
            this.Menu_aA_125.Text = "% 125";
            this.Menu_aA_125.Click += new System.EventHandler(this.Menu_aA_125_Click);
            // 
            // Menu_aA_150
            // 
            this.Menu_aA_150.Name = "Menu_aA_150";
            this.Menu_aA_150.Size = new System.Drawing.Size(132, 26);
            this.Menu_aA_150.Text = "% 150";
            this.Menu_aA_150.Click += new System.EventHandler(this.Menu_aA_150_Click);
            // 
            // Menu_aA_175
            // 
            this.Menu_aA_175.Name = "Menu_aA_175";
            this.Menu_aA_175.Size = new System.Drawing.Size(132, 26);
            this.Menu_aA_175.Text = "% 175";
            this.Menu_aA_175.Click += new System.EventHandler(this.Menu_aA_175_Click);
            // 
            // Menu_aA_200
            // 
            this.Menu_aA_200.Name = "Menu_aA_200";
            this.Menu_aA_200.Size = new System.Drawing.Size(132, 26);
            this.Menu_aA_200.Text = "% 200";
            this.Menu_aA_200.Click += new System.EventHandler(this.Menu_aA_200_Click);
            // 
            // SolMenu_BaşlatDurdur_Ayraç
            // 
            this.SolMenu_BaşlatDurdur_Ayraç.Name = "SolMenu_BaşlatDurdur_Ayraç";
            this.SolMenu_BaşlatDurdur_Ayraç.Size = new System.Drawing.Size(29, 6);
            this.SolMenu_BaşlatDurdur_Ayraç.Visible = false;
            // 
            // SolMenu_BaşlatDurdur
            // 
            this.SolMenu_BaşlatDurdur.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.SolMenu_BaşlatDurdur.Image = global::Çizelgeç.Properties.Resources.D_Tamam;
            this.SolMenu_BaşlatDurdur.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.SolMenu_BaşlatDurdur.Name = "SolMenu_BaşlatDurdur";
            this.SolMenu_BaşlatDurdur.Size = new System.Drawing.Size(29, 24);
            this.SolMenu_BaşlatDurdur.Text = "Başlat / Durdur";
            this.SolMenu_BaşlatDurdur.ToolTipText = "Sinyallerin sürekli olarak kaydedilmesinin istenmediği durumda istenilen zamanlar" +
    "da tüm sistemi durdurup, devam ettirilebilir";
            this.SolMenu_BaşlatDurdur.Visible = false;
            this.SolMenu_BaşlatDurdur.Click += new System.EventHandler(this.SolMenu_BaşlatDurdur_Click);
            // 
            // Ayraç_Ana
            // 
            this.Ayraç_Ana.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Ayraç_Ana.Location = new System.Drawing.Point(32, 0);
            this.Ayraç_Ana.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Ayraç_Ana.Name = "Ayraç_Ana";
            // 
            // Ayraç_Ana.Panel1
            // 
            this.Ayraç_Ana.Panel1.BackColor = System.Drawing.SystemColors.Control;
            this.Ayraç_Ana.Panel1.Controls.Add(this.İmleçKonumuTarihi);
            this.Ayraç_Ana.Panel1.Controls.Add(this.Ağaç);
            this.Ayraç_Ana.Panel1.Controls.Add(this.Günlük_Panel);
            // 
            // Ayraç_Ana.Panel2
            // 
            this.Ayraç_Ana.Panel2.BackColor = System.Drawing.SystemColors.Control;
            this.Ayraç_Ana.Panel2.Controls.Add(this.Ekle_tümü);
            this.Ayraç_Ana.Panel2.Controls.Add(this.Ekle_sonraki_tümü);
            this.Ayraç_Ana.Panel2.Controls.Add(this.Ekle_sonraki);
            this.Ayraç_Ana.Panel2.Controls.Add(this.Ekle_önceki_tümü);
            this.Ayraç_Ana.Panel2.Controls.Add(this.Ekle_önceki);
            this.Ayraç_Ana.Panel2.Controls.Add(this.Çizelge);
            this.Ayraç_Ana.Panel2.Controls.Add(this.tableLayoutPanel1);
            this.Ayraç_Ana.Size = new System.Drawing.Size(876, 364);
            this.Ayraç_Ana.SplitterDistance = 159;
            this.Ayraç_Ana.TabIndex = 1;
            this.Ayraç_Ana.TabStop = false;
            this.Ayraç_Ana.Visible = false;
            // 
            // İmleçKonumuTarihi
            // 
            this.İmleçKonumuTarihi.AutoSize = true;
            this.İmleçKonumuTarihi.BackColor = System.Drawing.SystemColors.Window;
            this.İmleçKonumuTarihi.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.İmleçKonumuTarihi.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.İmleçKonumuTarihi.ImageIndex = 0;
            this.İmleçKonumuTarihi.Location = new System.Drawing.Point(1, 0);
            this.İmleçKonumuTarihi.Name = "İmleçKonumuTarihi";
            this.İmleçKonumuTarihi.Size = new System.Drawing.Size(19, 18);
            this.İmleçKonumuTarihi.TabIndex = 14;
            this.İmleçKonumuTarihi.Text = "A";
            this.İmleçKonumuTarihi.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.İmleçKonumuTarihi.Visible = false;
            // 
            // Ağaç
            // 
            this.Ağaç.FullRowSelect = true;
            this.Ağaç.HideSelection = false;
            this.Ağaç.Location = new System.Drawing.Point(16, 11);
            this.Ağaç.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Ağaç.Name = "Ağaç";
            this.Ağaç.ShowNodeToolTips = true;
            this.Ağaç.ShowRootLines = false;
            this.Ağaç.Size = new System.Drawing.Size(139, 296);
            this.Ağaç.TabIndex = 0;
            // 
            // Günlük_Panel
            // 
            this.Günlük_Panel.Controls.Add(this.Günlük_YeniMesajaGit);
            this.Günlük_Panel.Controls.Add(this.Günlük_MetinKutusu);
            this.Günlük_Panel.Location = new System.Drawing.Point(16, 312);
            this.Günlük_Panel.Name = "Günlük_Panel";
            this.Günlük_Panel.Size = new System.Drawing.Size(139, 120);
            this.Günlük_Panel.TabIndex = 3;
            // 
            // Günlük_MetinKutusu
            // 
            this.Günlük_MetinKutusu.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Günlük_MetinKutusu.Location = new System.Drawing.Point(0, 0);
            this.Günlük_MetinKutusu.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Günlük_MetinKutusu.Multiline = true;
            this.Günlük_MetinKutusu.Name = "Günlük_MetinKutusu";
            this.Günlük_MetinKutusu.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.Günlük_MetinKutusu.Size = new System.Drawing.Size(139, 120);
            this.Günlük_MetinKutusu.TabIndex = 0;
            this.Günlük_MetinKutusu.WordWrap = false;
            // 
            // Çizelge
            // 
            this.Çizelge.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Çizelge.ContextMenuStrip = this.SağTuşMenü_Çizelge;
            this.Çizelge.Location = new System.Drawing.Point(0, 8);
            this.Çizelge.Margin = new System.Windows.Forms.Padding(5);
            this.Çizelge.Name = "Çizelge";
            this.Çizelge.Size = new System.Drawing.Size(713, 356);
            this.Çizelge.TabIndex = 0;
            this.Çizelge.TabStop = false;
            this.Çizelge.MouseClicked += new System.Windows.Forms.MouseEventHandler(this.Çizelge_MouseClicked);
            this.Çizelge.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Çizelge_MouseMove);
            // 
            // SağTuşMenü_Çizelge
            // 
            this.SağTuşMenü_Çizelge.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.SağTuşMenü_Çizelge.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.SağTuşMenü_Çizelge_Etkin,
            this.SağTuşMenü_Çizelge_TümSinyalleriEkranaSığdır,
            this.SağTuşMenü_Çizelge_SeçiliOlanıBelirginleştr,
            this.SağTuşMenü_Çizelge_Y_EkseniÖlçeğiniSeçiliOlanSinyaleUyarla,
            this.SağTuşMenü_Çizelge_Birbirinin_aynısı_olan_zaman_dilimlerini_atla,
            this.toolStripSeparator2,
            this.SağTuşMenü_Çizelge_Daralt,
            this.SağTuşMenü_Çizelge_ÇizgiKalınlığı,
            this.toolStripSeparator3,
            this.SağTuşMenü_Çizelge_dışarıAktar,
            this.SağTuşMenü_Çizelge_panoyaKopyala,
            this.SağTuşMenü_Çizelge_KayıtKlasörünüAç,
            this.SağTuşMenü_Çizelge_Ayırıcı1,
            this.SağTuşMenü_Çizelge_DeğerleriNormalleştir,
            this.SağTuşMenü_Çizelge_xEkseniTarihVeSaat,
            this.SağTuşMenü_Çizelge_YenidenHesapla});
            this.SağTuşMenü_Çizelge.Name = "SağTuşMenü_Senaryo";
            this.SağTuşMenü_Çizelge.Size = new System.Drawing.Size(352, 388);
            // 
            // SağTuşMenü_Çizelge_Etkin
            // 
            this.SağTuşMenü_Çizelge_Etkin.Checked = true;
            this.SağTuşMenü_Çizelge_Etkin.CheckOnClick = true;
            this.SağTuşMenü_Çizelge_Etkin.CheckState = System.Windows.Forms.CheckState.Checked;
            this.SağTuşMenü_Çizelge_Etkin.Name = "SağTuşMenü_Çizelge_Etkin";
            this.SağTuşMenü_Çizelge_Etkin.ShowShortcutKeys = false;
            this.SağTuşMenü_Çizelge_Etkin.Size = new System.Drawing.Size(351, 26);
            this.SağTuşMenü_Çizelge_Etkin.Text = "Etkin";
            this.SağTuşMenü_Çizelge_Etkin.ToolTipText = "EKrandaki çizelgenin taranmasının uzun süre aldığı durumlarda sadece çizelgenin g" +
    "üncellemesini durdurmak için kullanılabilir";
            // 
            // SağTuşMenü_Çizelge_TümSinyalleriEkranaSığdır
            // 
            this.SağTuşMenü_Çizelge_TümSinyalleriEkranaSığdır.Checked = true;
            this.SağTuşMenü_Çizelge_TümSinyalleriEkranaSığdır.CheckOnClick = true;
            this.SağTuşMenü_Çizelge_TümSinyalleriEkranaSığdır.CheckState = System.Windows.Forms.CheckState.Checked;
            this.SağTuşMenü_Çizelge_TümSinyalleriEkranaSığdır.Name = "SağTuşMenü_Çizelge_TümSinyalleriEkranaSığdır";
            this.SağTuşMenü_Çizelge_TümSinyalleriEkranaSığdır.ShowShortcutKeys = false;
            this.SağTuşMenü_Çizelge_TümSinyalleriEkranaSığdır.Size = new System.Drawing.Size(351, 26);
            this.SağTuşMenü_Çizelge_TümSinyalleriEkranaSığdır.Text = "Tüm sinyalleri ekrana sığdır";
            this.SağTuşMenü_Çizelge_TümSinyalleriEkranaSığdır.ToolTipText = "Ekrandaki sinyalleri yatay ve dikey eksenlerden sürekli olarak ortalayıp, ölçümle" +
    "rin ekranın görünür bölgesinde kalmasını sağlar";
            // 
            // SağTuşMenü_Çizelge_SeçiliOlanıBelirginleştr
            // 
            this.SağTuşMenü_Çizelge_SeçiliOlanıBelirginleştr.Checked = true;
            this.SağTuşMenü_Çizelge_SeçiliOlanıBelirginleştr.CheckOnClick = true;
            this.SağTuşMenü_Çizelge_SeçiliOlanıBelirginleştr.CheckState = System.Windows.Forms.CheckState.Checked;
            this.SağTuşMenü_Çizelge_SeçiliOlanıBelirginleştr.Name = "SağTuşMenü_Çizelge_SeçiliOlanıBelirginleştr";
            this.SağTuşMenü_Çizelge_SeçiliOlanıBelirginleştr.ShowShortcutKeys = false;
            this.SağTuşMenü_Çizelge_SeçiliOlanıBelirginleştr.Size = new System.Drawing.Size(351, 26);
            this.SağTuşMenü_Çizelge_SeçiliOlanıBelirginleştr.Text = "Seçili sinyali belirginleştir";
            this.SağTuşMenü_Çizelge_SeçiliOlanıBelirginleştr.ToolTipText = "Seçili olan sinyali bir miktar kalınlaştırıp, belirginleştirir";
            // 
            // SağTuşMenü_Çizelge_Y_EkseniÖlçeğiniSeçiliOlanSinyaleUyarla
            // 
            this.SağTuşMenü_Çizelge_Y_EkseniÖlçeğiniSeçiliOlanSinyaleUyarla.Checked = true;
            this.SağTuşMenü_Çizelge_Y_EkseniÖlçeğiniSeçiliOlanSinyaleUyarla.CheckOnClick = true;
            this.SağTuşMenü_Çizelge_Y_EkseniÖlçeğiniSeçiliOlanSinyaleUyarla.CheckState = System.Windows.Forms.CheckState.Checked;
            this.SağTuşMenü_Çizelge_Y_EkseniÖlçeğiniSeçiliOlanSinyaleUyarla.Name = "SağTuşMenü_Çizelge_Y_EkseniÖlçeğiniSeçiliOlanSinyaleUyarla";
            this.SağTuşMenü_Çizelge_Y_EkseniÖlçeğiniSeçiliOlanSinyaleUyarla.ShowShortcutKeys = false;
            this.SağTuşMenü_Çizelge_Y_EkseniÖlçeğiniSeçiliOlanSinyaleUyarla.Size = new System.Drawing.Size(351, 26);
            this.SağTuşMenü_Çizelge_Y_EkseniÖlçeğiniSeçiliOlanSinyaleUyarla.Text = "Y ekseni ölçeğini seçili olan sinyale uyarla";
            this.SağTuşMenü_Çizelge_Y_EkseniÖlçeğiniSeçiliOlanSinyaleUyarla.ToolTipText = "Dikey eksendeki değerlerin seçili olan sinyale göre ayarlanmasını sağlar";
            // 
            // SağTuşMenü_Çizelge_Birbirinin_aynısı_olan_zaman_dilimlerini_atla
            // 
            this.SağTuşMenü_Çizelge_Birbirinin_aynısı_olan_zaman_dilimlerini_atla.Checked = true;
            this.SağTuşMenü_Çizelge_Birbirinin_aynısı_olan_zaman_dilimlerini_atla.CheckOnClick = true;
            this.SağTuşMenü_Çizelge_Birbirinin_aynısı_olan_zaman_dilimlerini_atla.CheckState = System.Windows.Forms.CheckState.Checked;
            this.SağTuşMenü_Çizelge_Birbirinin_aynısı_olan_zaman_dilimlerini_atla.Name = "SağTuşMenü_Çizelge_Birbirinin_aynısı_olan_zaman_dilimlerini_atla";
            this.SağTuşMenü_Çizelge_Birbirinin_aynısı_olan_zaman_dilimlerini_atla.ShowShortcutKeys = false;
            this.SağTuşMenü_Çizelge_Birbirinin_aynısı_olan_zaman_dilimlerini_atla.Size = new System.Drawing.Size(351, 26);
            this.SağTuşMenü_Çizelge_Birbirinin_aynısı_olan_zaman_dilimlerini_atla.Text = "Birbirinin aynısı olan zaman dilimlerini atla";
            this.SağTuşMenü_Çizelge_Birbirinin_aynısı_olan_zaman_dilimlerini_atla.ToolTipText = "Dosyalama boyutunu azaltmak için açılabilir";
            this.SağTuşMenü_Çizelge_Birbirinin_aynısı_olan_zaman_dilimlerini_atla.Visible = false;
            this.SağTuşMenü_Çizelge_Birbirinin_aynısı_olan_zaman_dilimlerini_atla.Click += new System.EventHandler(this.SağTuşMenü_Çizelge_Birbirinin_aynısı_olan_zaman_dilimlerini_atla_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(348, 6);
            // 
            // SağTuşMenü_Çizelge_Daralt
            // 
            this.SağTuşMenü_Çizelge_Daralt.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.SağTuşMenü_Çizelge_Daralt_Baştanİtibaren,
            this.SağTuşMenü_Çizelge_Daralt_Sondanİtibaren,
            this.SağTuşMenü_Çizelge_Daralt_Uygula});
            this.SağTuşMenü_Çizelge_Daralt.Name = "SağTuşMenü_Çizelge_Daralt";
            this.SağTuşMenü_Çizelge_Daralt.Size = new System.Drawing.Size(351, 26);
            this.SağTuşMenü_Çizelge_Daralt.Text = "Daralt";
            // 
            // SağTuşMenü_Çizelge_Daralt_Baştanİtibaren
            // 
            this.SağTuşMenü_Çizelge_Daralt_Baştanİtibaren.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.SağTuşMenü_Çizelge_Daralt_Baştanİtibaren.Name = "SağTuşMenü_Çizelge_Daralt_Baştanİtibaren";
            this.SağTuşMenü_Çizelge_Daralt_Baştanİtibaren.Size = new System.Drawing.Size(224, 27);
            this.SağTuşMenü_Çizelge_Daralt_Baştanİtibaren.Text = "0";
            this.SağTuşMenü_Çizelge_Daralt_Baştanİtibaren.ToolTipText = "Baştan itibaren";
            this.SağTuşMenü_Çizelge_Daralt_Baştanİtibaren.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.SağTuşMenü_Çizelge_Daralt_Baştanİtibaren_KeyPress);
            // 
            // SağTuşMenü_Çizelge_Daralt_Sondanİtibaren
            // 
            this.SağTuşMenü_Çizelge_Daralt_Sondanİtibaren.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.SağTuşMenü_Çizelge_Daralt_Sondanİtibaren.Name = "SağTuşMenü_Çizelge_Daralt_Sondanİtibaren";
            this.SağTuşMenü_Çizelge_Daralt_Sondanİtibaren.Size = new System.Drawing.Size(298, 27);
            this.SağTuşMenü_Çizelge_Daralt_Sondanİtibaren.Text = "0";
            this.SağTuşMenü_Çizelge_Daralt_Sondanİtibaren.ToolTipText = "Sondan itibaren";
            this.SağTuşMenü_Çizelge_Daralt_Sondanİtibaren.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.SağTuşMenü_Çizelge_Daralt_Baştanİtibaren_KeyPress);
            // 
            // SağTuşMenü_Çizelge_Daralt_Uygula
            // 
            this.SağTuşMenü_Çizelge_Daralt_Uygula.Name = "SağTuşMenü_Çizelge_Daralt_Uygula";
            this.SağTuşMenü_Çizelge_Daralt_Uygula.Size = new System.Drawing.Size(372, 26);
            this.SağTuşMenü_Çizelge_Daralt_Uygula.Text = "Uygula";
            this.SağTuşMenü_Çizelge_Daralt_Uygula.Click += new System.EventHandler(this.SağTuşMenü_Çizelge_Daralt_Uygula_Click);
            // 
            // SağTuşMenü_Çizelge_ÇizgiKalınlığı
            // 
            this.SağTuşMenü_Çizelge_ÇizgiKalınlığı.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.SağTuşMenü_Çizelge_ÇizgiKalınlığı_Değer,
            this.SağTuşMenü_Çizelge_ÇizgiKalınlığı_TümüneUygula,
            this.SağTuşMenü_Çizelge_ÇizgiKalınlığı_SadeceEtkinOlanlar});
            this.SağTuşMenü_Çizelge_ÇizgiKalınlığı.Name = "SağTuşMenü_Çizelge_ÇizgiKalınlığı";
            this.SağTuşMenü_Çizelge_ÇizgiKalınlığı.ShowShortcutKeys = false;
            this.SağTuşMenü_Çizelge_ÇizgiKalınlığı.Size = new System.Drawing.Size(351, 26);
            this.SağTuşMenü_Çizelge_ÇizgiKalınlığı.Text = "Çizgi kalınlığı";
            // 
            // SağTuşMenü_Çizelge_ÇizgiKalınlığı_Değer
            // 
            this.SağTuşMenü_Çizelge_ÇizgiKalınlığı_Değer.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.SağTuşMenü_Çizelge_ÇizgiKalınlığı_Değer.Name = "SağTuşMenü_Çizelge_ÇizgiKalınlığı_Değer";
            this.SağTuşMenü_Çizelge_ÇizgiKalınlığı_Değer.Size = new System.Drawing.Size(224, 27);
            this.SağTuşMenü_Çizelge_ÇizgiKalınlığı_Değer.Text = "1";
            this.SağTuşMenü_Çizelge_ÇizgiKalınlığı_Değer.ToolTipText = "Değeri";
            this.SağTuşMenü_Çizelge_ÇizgiKalınlığı_Değer.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.SağTuşMenü_Çizelge_ÇizgiKalınlığı_Değer_KeyPress);
            // 
            // SağTuşMenü_Çizelge_ÇizgiKalınlığı_TümüneUygula
            // 
            this.SağTuşMenü_Çizelge_ÇizgiKalınlığı_TümüneUygula.Name = "SağTuşMenü_Çizelge_ÇizgiKalınlığı_TümüneUygula";
            this.SağTuşMenü_Çizelge_ÇizgiKalınlığı_TümüneUygula.Size = new System.Drawing.Size(298, 26);
            this.SağTuşMenü_Çizelge_ÇizgiKalınlığı_TümüneUygula.Text = "Tümüne uygula";
            this.SağTuşMenü_Çizelge_ÇizgiKalınlığı_TümüneUygula.Click += new System.EventHandler(this.SağTuşMenü_Çizelge_ÇizgiKalınlığı_TümüneUygula_Click);
            // 
            // SağTuşMenü_Çizelge_ÇizgiKalınlığı_SadeceEtkinOlanlar
            // 
            this.SağTuşMenü_Çizelge_ÇizgiKalınlığı_SadeceEtkinOlanlar.Enabled = false;
            this.SağTuşMenü_Çizelge_ÇizgiKalınlığı_SadeceEtkinOlanlar.Name = "SağTuşMenü_Çizelge_ÇizgiKalınlığı_SadeceEtkinOlanlar";
            this.SağTuşMenü_Çizelge_ÇizgiKalınlığı_SadeceEtkinOlanlar.Size = new System.Drawing.Size(298, 26);
            this.SağTuşMenü_Çizelge_ÇizgiKalınlığı_SadeceEtkinOlanlar.Text = "Sadece etkin olanlara uygula";
            this.SağTuşMenü_Çizelge_ÇizgiKalınlığı_SadeceEtkinOlanlar.Visible = false;
            this.SağTuşMenü_Çizelge_ÇizgiKalınlığı_SadeceEtkinOlanlar.Click += new System.EventHandler(this.SağTuşMenü_Çizelge_ÇizgiKalınlığı_SadeceEtkinOlanlar_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(348, 6);
            // 
            // SağTuşMenü_Çizelge_dışarıAktar
            // 
            this.SağTuşMenü_Çizelge_dışarıAktar.Name = "SağTuşMenü_Çizelge_dışarıAktar";
            this.SağTuşMenü_Çizelge_dışarıAktar.Size = new System.Drawing.Size(351, 26);
            this.SağTuşMenü_Çizelge_dışarıAktar.Text = "Dışarı aktar";
            this.SağTuşMenü_Çizelge_dışarıAktar.Click += new System.EventHandler(this.SağTuşMenü_Çizelge_dışarıAktar_Click);
            // 
            // SağTuşMenü_Çizelge_panoyaKopyala
            // 
            this.SağTuşMenü_Çizelge_panoyaKopyala.Name = "SağTuşMenü_Çizelge_panoyaKopyala";
            this.SağTuşMenü_Çizelge_panoyaKopyala.Size = new System.Drawing.Size(351, 26);
            this.SağTuşMenü_Çizelge_panoyaKopyala.Text = "Panoya kopyala";
            this.SağTuşMenü_Çizelge_panoyaKopyala.Click += new System.EventHandler(this.SağTuşMenü_Çizelge_panoyaKopyala_Click);
            // 
            // SağTuşMenü_Çizelge_KayıtKlasörünüAç
            // 
            this.SağTuşMenü_Çizelge_KayıtKlasörünüAç.Name = "SağTuşMenü_Çizelge_KayıtKlasörünüAç";
            this.SağTuşMenü_Çizelge_KayıtKlasörünüAç.Size = new System.Drawing.Size(351, 26);
            this.SağTuşMenü_Çizelge_KayıtKlasörünüAç.Text = "Kayıt klasörünü aç";
            this.SağTuşMenü_Çizelge_KayıtKlasörünüAç.Visible = false;
            this.SağTuşMenü_Çizelge_KayıtKlasörünüAç.Click += new System.EventHandler(this.SağTuşMenü_Çizelge_KayıtKlasörünüAç_Click);
            // 
            // SağTuşMenü_Çizelge_Ayırıcı1
            // 
            this.SağTuşMenü_Çizelge_Ayırıcı1.Name = "SağTuşMenü_Çizelge_Ayırıcı1";
            this.SağTuşMenü_Çizelge_Ayırıcı1.Size = new System.Drawing.Size(348, 6);
            this.SağTuşMenü_Çizelge_Ayırıcı1.Visible = false;
            // 
            // SağTuşMenü_Çizelge_DeğerleriNormalleştir
            // 
            this.SağTuşMenü_Çizelge_DeğerleriNormalleştir.Name = "SağTuşMenü_Çizelge_DeğerleriNormalleştir";
            this.SağTuşMenü_Çizelge_DeğerleriNormalleştir.Size = new System.Drawing.Size(351, 26);
            this.SağTuşMenü_Çizelge_DeğerleriNormalleştir.Text = "Değerleri normalleştir";
            this.SağTuşMenü_Çizelge_DeğerleriNormalleştir.Visible = false;
            this.SağTuşMenü_Çizelge_DeğerleriNormalleştir.Click += new System.EventHandler(this.SağTuşMenü_Çizelge_DeğerleriNormalleştir_Click);
            // 
            // SağTuşMenü_Çizelge_xEkseniTarihVeSaat
            // 
            this.SağTuşMenü_Çizelge_xEkseniTarihVeSaat.Name = "SağTuşMenü_Çizelge_xEkseniTarihVeSaat";
            this.SağTuşMenü_Çizelge_xEkseniTarihVeSaat.Size = new System.Drawing.Size(351, 26);
            this.SağTuşMenü_Çizelge_xEkseniTarihVeSaat.Text = "Dizideki sıra numarasını göster";
            this.SağTuşMenü_Çizelge_xEkseniTarihVeSaat.Visible = false;
            this.SağTuşMenü_Çizelge_xEkseniTarihVeSaat.Click += new System.EventHandler(this.SağTuşMenü_Çizelge_xEkseniTarihVeSaat_Click);
            // 
            // SağTuşMenü_Çizelge_YenidenHesapla
            // 
            this.SağTuşMenü_Çizelge_YenidenHesapla.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.SağTuşMenü_Çizelge_YenidenHesapla_Değer,
            this.SağTuşMenü_Çizelge_YenidenHesapla_SadeceSeçiliDalaUygula,
            this.SağTuşMenü_Çizelge_YenidenHesapla_SadeceEtkinOlanlaraUygula,
            this.SağTuşMenü_Çizelge_YenidenHesapla_TününeUygula});
            this.SağTuşMenü_Çizelge_YenidenHesapla.Name = "SağTuşMenü_Çizelge_YenidenHesapla";
            this.SağTuşMenü_Çizelge_YenidenHesapla.Size = new System.Drawing.Size(351, 26);
            this.SağTuşMenü_Çizelge_YenidenHesapla.Text = "Yeniden hesapla";
            this.SağTuşMenü_Çizelge_YenidenHesapla.Visible = false;
            // 
            // SağTuşMenü_Çizelge_YenidenHesapla_Değer
            // 
            this.SağTuşMenü_Çizelge_YenidenHesapla_Değer.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.SağTuşMenü_Çizelge_YenidenHesapla_Değer.Name = "SağTuşMenü_Çizelge_YenidenHesapla_Değer";
            this.SağTuşMenü_Çizelge_YenidenHesapla_Değer.Size = new System.Drawing.Size(282, 27);
            this.SağTuşMenü_Çizelge_YenidenHesapla_Değer.Text = "<Sinyal>*2";
            this.SağTuşMenü_Çizelge_YenidenHesapla_Değer.ToolTipText = "İşlem satırı";
            // 
            // SağTuşMenü_Çizelge_YenidenHesapla_SadeceSeçiliDalaUygula
            // 
            this.SağTuşMenü_Çizelge_YenidenHesapla_SadeceSeçiliDalaUygula.Name = "SağTuşMenü_Çizelge_YenidenHesapla_SadeceSeçiliDalaUygula";
            this.SağTuşMenü_Çizelge_YenidenHesapla_SadeceSeçiliDalaUygula.Size = new System.Drawing.Size(356, 26);
            this.SağTuşMenü_Çizelge_YenidenHesapla_SadeceSeçiliDalaUygula.Text = "Sadece seçili dala uygula";
            this.SağTuşMenü_Çizelge_YenidenHesapla_SadeceSeçiliDalaUygula.Click += new System.EventHandler(this.SağTuşMenü_Çizelge_YenidenHesapla_SadeceSeçiliDalaUygula_Click);
            // 
            // SağTuşMenü_Çizelge_YenidenHesapla_SadeceEtkinOlanlaraUygula
            // 
            this.SağTuşMenü_Çizelge_YenidenHesapla_SadeceEtkinOlanlaraUygula.Name = "SağTuşMenü_Çizelge_YenidenHesapla_SadeceEtkinOlanlaraUygula";
            this.SağTuşMenü_Çizelge_YenidenHesapla_SadeceEtkinOlanlaraUygula.Size = new System.Drawing.Size(356, 26);
            this.SağTuşMenü_Çizelge_YenidenHesapla_SadeceEtkinOlanlaraUygula.Text = "Sadece etkin olanlara uygula";
            this.SağTuşMenü_Çizelge_YenidenHesapla_SadeceEtkinOlanlaraUygula.Click += new System.EventHandler(this.SağTuşMenü_Çizelge_YenidenHesapla_SadeceEtkinOlanlaraUygula_Click);
            // 
            // SağTuşMenü_Çizelge_YenidenHesapla_TününeUygula
            // 
            this.SağTuşMenü_Çizelge_YenidenHesapla_TününeUygula.Name = "SağTuşMenü_Çizelge_YenidenHesapla_TününeUygula";
            this.SağTuşMenü_Çizelge_YenidenHesapla_TününeUygula.Size = new System.Drawing.Size(356, 26);
            this.SağTuşMenü_Çizelge_YenidenHesapla_TününeUygula.Text = "Tümüne uygula";
            this.SağTuşMenü_Çizelge_YenidenHesapla_TününeUygula.Click += new System.EventHandler(this.SağTuşMenü_Çizelge_YenidenHesapla_TününeUygula_Click);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel1.ColumnCount = 3;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel1.Controls.Add(this.AralıkSeçici_Sondan, 2, 0);
            this.tableLayoutPanel1.Controls.Add(this.AralıkSeçici_Baştan, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.Kaydırıcı, 1, 0);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, -23);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(713, 43);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // ResimListesi
            // 
            this.ResimListesi.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit;
            this.ResimListesi.ImageSize = new System.Drawing.Size(16, 16);
            this.ResimListesi.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // SağTuşMenü_Senaryo
            // 
            this.SağTuşMenü_Senaryo.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.SağTuşMenü_Senaryo.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.SağTuşMenü_Senaryo_Çalıştır,
            this.SağTuşMenü_Senaryo_Durdur});
            this.SağTuşMenü_Senaryo.Name = "SağTuşMenü_Senaryo";
            this.SağTuşMenü_Senaryo.Size = new System.Drawing.Size(125, 52);
            // 
            // SağTuşMenü_Senaryo_Çalıştır
            // 
            this.SağTuşMenü_Senaryo_Çalıştır.Name = "SağTuşMenü_Senaryo_Çalıştır";
            this.SağTuşMenü_Senaryo_Çalıştır.Size = new System.Drawing.Size(124, 24);
            this.SağTuşMenü_Senaryo_Çalıştır.Text = "Çalıştır";
            this.SağTuşMenü_Senaryo_Çalıştır.Click += new System.EventHandler(this.SağTuşMenü_Senaryo_Çalıştır_Click);
            // 
            // SağTuşMenü_Senaryo_Durdur
            // 
            this.SağTuşMenü_Senaryo_Durdur.Name = "SağTuşMenü_Senaryo_Durdur";
            this.SağTuşMenü_Senaryo_Durdur.Size = new System.Drawing.Size(124, 24);
            this.SağTuşMenü_Senaryo_Durdur.Text = "Durdur";
            this.SağTuşMenü_Senaryo_Durdur.Click += new System.EventHandler(this.SağTuşMenü_Senaryo_Durdur_Click);
            // 
            // DosyayaKaydetDialoğu
            // 
            this.DosyayaKaydetDialoğu.DefaultExt = "csv";
            this.DosyayaKaydetDialoğu.Filter = "Çizelgeç CSV|*.csv";
            this.DosyayaKaydetDialoğu.FilterIndex = 0;
            this.DosyayaKaydetDialoğu.Title = "ArgemuP Çizelgeç Belirli Zaman Aralığındaki Sinyalleri Dışarı Aktarma Yardımcısı";
            this.DosyayaKaydetDialoğu.FileOk += new System.ComponentModel.CancelEventHandler(this.SağTuşMenü_Çizelge_dışarıAktar_DosyayaKaydetDialoğu_FileOk);
            // 
            // AnaEkran
            // 
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::Çizelgeç.Properties.Resources.logo;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.ClientSize = new System.Drawing.Size(908, 364);
            this.Controls.Add(this.Ayraç_Ana);
            this.Controls.Add(this.SolMenü);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "AnaEkran";
            this.Text = "Form1";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.AnaEkran_FormClosed);
            this.Load += new System.EventHandler(this.AnaEkran_Load);
            this.Shown += new System.EventHandler(this.AnaEkran_Shown);
            this.DragDrop += new System.Windows.Forms.DragEventHandler(this.AnaEkran_DragDrop);
            this.DragEnter += new System.Windows.Forms.DragEventHandler(this.AnaEkran_DragEnter);
            ((System.ComponentModel.ISupportInitialize)(this.AralıkSeçici_Baştan)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Kaydırıcı)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.AralıkSeçici_Sondan)).EndInit();
            this.SolMenü.ResumeLayout(false);
            this.SolMenü.PerformLayout();
            this.Ayraç_Ana.Panel1.ResumeLayout(false);
            this.Ayraç_Ana.Panel1.PerformLayout();
            this.Ayraç_Ana.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.Ayraç_Ana)).EndInit();
            this.Ayraç_Ana.ResumeLayout(false);
            this.Günlük_Panel.ResumeLayout(false);
            this.Günlük_Panel.PerformLayout();
            this.SağTuşMenü_Çizelge.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.SağTuşMenü_Senaryo.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.ToolTip İpUcu;
        private System.Windows.Forms.ToolStrip SolMenü;
        private System.Windows.Forms.ToolStripButton SolMenu_Ağaç;
        private System.Windows.Forms.SplitContainer Ayraç_Ana;
        private ScottPlot.FormsPlot Çizelge;
        private System.Windows.Forms.TextBox Günlük_MetinKutusu;
        private System.Windows.Forms.ToolStripButton SolMenu_Gunluk;
        private System.Windows.Forms.ToolStripButton Eposta;
        private System.Windows.Forms.ToolStripDropDownButton Menu_aA;
        private System.Windows.Forms.ToolStripMenuItem Menu_aA_100;
        private System.Windows.Forms.ToolStripMenuItem Menu_aA_125;
        private System.Windows.Forms.ToolStripMenuItem Menu_aA_150;
        private System.Windows.Forms.ToolStripMenuItem Menu_aA_175;
        private System.Windows.Forms.ToolStripMenuItem Menu_aA_200;
        private System.Windows.Forms.TrackBar AralıkSeçici_Baştan;
        private System.Windows.Forms.CheckBox Günlük_YeniMesajaGit;
        private System.Windows.Forms.Panel Günlük_Panel;
        private System.Windows.Forms.ContextMenuStrip SağTuşMenü_Senaryo;
        private System.Windows.Forms.ToolStripMenuItem SağTuşMenü_Senaryo_Çalıştır;
        private System.Windows.Forms.ToolStripMenuItem SağTuşMenü_Senaryo_Durdur;
        private System.Windows.Forms.Button Ekle_sonraki_tümü;
        private System.Windows.Forms.Button Ekle_sonraki;
        private System.Windows.Forms.Button Ekle_önceki_tümü;
        private System.Windows.Forms.Button Ekle_önceki;
        private System.Windows.Forms.Button Ekle_tümü;
        private System.Windows.Forms.SaveFileDialog DosyayaKaydetDialoğu;
        private System.Windows.Forms.ToolStripButton SolMenu_BaşlatDurdur;
        private System.Windows.Forms.ToolStripSeparator SolMenu_BaşlatDurdur_Ayraç;
        private System.Windows.Forms.ContextMenuStrip SağTuşMenü_Çizelge;
        private System.Windows.Forms.ToolStripMenuItem SağTuşMenü_Çizelge_Etkin;
        private System.Windows.Forms.ToolStripMenuItem SağTuşMenü_Çizelge_TümSinyalleriEkranaSığdır;
        private System.Windows.Forms.ToolStripMenuItem SağTuşMenü_Çizelge_SeçiliOlanıBelirginleştr;
        private System.Windows.Forms.ToolStripMenuItem SağTuşMenü_Çizelge_Y_EkseniÖlçeğiniSeçiliOlanSinyaleUyarla;
        private System.Windows.Forms.ToolStripMenuItem SağTuşMenü_Çizelge_ÇizgiKalınlığı;
        private System.Windows.Forms.ToolStripTextBox SağTuşMenü_Çizelge_ÇizgiKalınlığı_Değer;
        private System.Windows.Forms.ToolStripMenuItem SağTuşMenü_Çizelge_ÇizgiKalınlığı_TümüneUygula;
        private System.Windows.Forms.ToolStripMenuItem SağTuşMenü_Çizelge_DeğerleriNormalleştir;
        private System.Windows.Forms.ToolStripMenuItem SağTuşMenü_Çizelge_ÇizgiKalınlığı_SadeceEtkinOlanlar;
        private System.Windows.Forms.ToolStripMenuItem SağTuşMenü_Çizelge_YenidenHesapla;
        private System.Windows.Forms.ToolStripMenuItem SağTuşMenü_Çizelge_YenidenHesapla_TününeUygula;
        private System.Windows.Forms.ToolStripMenuItem SağTuşMenü_Çizelge_YenidenHesapla_SadeceEtkinOlanlaraUygula;
        private System.Windows.Forms.ToolStripTextBox SağTuşMenü_Çizelge_YenidenHesapla_Değer;
        private System.Windows.Forms.ToolStripSeparator SağTuşMenü_Çizelge_Ayırıcı1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem SağTuşMenü_Çizelge_Daralt;
        private System.Windows.Forms.ToolStripMenuItem SağTuşMenü_Çizelge_Daralt_Uygula;
        private System.Windows.Forms.ToolStripTextBox SağTuşMenü_Çizelge_Daralt_Baştanİtibaren;
        private System.Windows.Forms.ToolStripTextBox SağTuşMenü_Çizelge_Daralt_Sondanİtibaren;
        private ÇiftTamponluTreeView Ağaç;
        private System.Windows.Forms.ToolStripMenuItem SağTuşMenü_Çizelge_YenidenHesapla_SadeceSeçiliDalaUygula;
        private System.Windows.Forms.ToolStripMenuItem SağTuşMenü_Çizelge_KayıtKlasörünüAç;
        private System.Windows.Forms.ImageList ResimListesi;
        private System.Windows.Forms.Label İmleçKonumuTarihi;
        private System.Windows.Forms.ToolStripButton SolMenu_Cizelge;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripMenuItem SağTuşMenü_Çizelge_dışarıAktar;
        private System.Windows.Forms.ToolStripMenuItem SağTuşMenü_Çizelge_panoyaKopyala;
        private System.Windows.Forms.TrackBar Kaydırıcı;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.TrackBar AralıkSeçici_Sondan;
        private System.Windows.Forms.ToolStripMenuItem SağTuşMenü_Çizelge_xEkseniTarihVeSaat;
        private System.Windows.Forms.ToolStripMenuItem SağTuşMenü_Çizelge_Birbirinin_aynısı_olan_zaman_dilimlerini_atla;
    }
}


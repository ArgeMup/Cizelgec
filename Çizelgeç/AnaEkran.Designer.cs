namespace Çizelgeç
{
    partial class AnaEkran
    {
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
            this.Ortala = new System.Windows.Forms.CheckBox();
            this.SeçiliOlanıBelirgenleştir = new System.Windows.Forms.CheckBox();
            this.ÖlçeğiSeçiliOlanaGöreAyarla = new System.Windows.Forms.CheckBox();
            this.AralıkSeçici = new System.Windows.Forms.TrackBar();
            this.Güncelle = new System.Windows.Forms.CheckBox();
            this.Günlük_YeniMesajaGit = new System.Windows.Forms.CheckBox();
            this.Ekle_önceki = new System.Windows.Forms.Button();
            this.Ekle_önceki_tümü = new System.Windows.Forms.Button();
            this.Ekle_sonraki_tümü = new System.Windows.Forms.Button();
            this.Ekle_sonraki = new System.Windows.Forms.Button();
            this.Ekle_tümü = new System.Windows.Forms.Button();
            this.SolMenü = new System.Windows.Forms.ToolStrip();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.SolMenu_BaşlatDurdur_Ayraç = new System.Windows.Forms.ToolStripSeparator();
            this.Ayraç_Ana = new System.Windows.Forms.SplitContainer();
            this.Günlük_Panel = new System.Windows.Forms.Panel();
            this.Günlük_MetinKutusu = new System.Windows.Forms.TextBox();
            this.Ayraç_Ağaç = new System.Windows.Forms.SplitContainer();
            this.Ağaç = new System.Windows.Forms.TreeView();
            this.Kaydırıcı = new System.Windows.Forms.HScrollBar();
            this.Çizelge = new ScottPlot.FormsPlot();
            this.SağTuşMenü_Senaryo = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.SağTuşMenü_Senaryo_Çalıştır = new System.Windows.Forms.ToolStripMenuItem();
            this.SağTuşMenü_Senaryo_Durdur = new System.Windows.Forms.ToolStripMenuItem();
            this.DosyayaKaydetDialoğu = new System.Windows.Forms.SaveFileDialog();
            this.SolMenu_Ağaç = new System.Windows.Forms.ToolStripButton();
            this.Eposta = new System.Windows.Forms.ToolStripButton();
            this.SolMenu_Gunluk = new System.Windows.Forms.ToolStripButton();
            this.Menu_aA = new System.Windows.Forms.ToolStripDropDownButton();
            this.Menu_aA_100 = new System.Windows.Forms.ToolStripMenuItem();
            this.Menu_aA_125 = new System.Windows.Forms.ToolStripMenuItem();
            this.Menu_aA_150 = new System.Windows.Forms.ToolStripMenuItem();
            this.Menu_aA_175 = new System.Windows.Forms.ToolStripMenuItem();
            this.Menu_aA_200 = new System.Windows.Forms.ToolStripMenuItem();
            this.SolMenu_Kaydet = new System.Windows.Forms.ToolStripButton();
            this.SolMenu_BaşlatDurdur = new System.Windows.Forms.ToolStripButton();
            ((System.ComponentModel.ISupportInitialize)(this.AralıkSeçici)).BeginInit();
            this.SolMenü.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Ayraç_Ana)).BeginInit();
            this.Ayraç_Ana.Panel1.SuspendLayout();
            this.Ayraç_Ana.Panel2.SuspendLayout();
            this.Ayraç_Ana.SuspendLayout();
            this.Günlük_Panel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Ayraç_Ağaç)).BeginInit();
            this.Ayraç_Ağaç.Panel1.SuspendLayout();
            this.Ayraç_Ağaç.SuspendLayout();
            this.SağTuşMenü_Senaryo.SuspendLayout();
            this.SuspendLayout();
            // 
            // Ortala
            // 
            this.Ortala.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.Ortala.AutoSize = true;
            this.Ortala.BackColor = System.Drawing.SystemColors.Control;
            this.Ortala.Checked = true;
            this.Ortala.CheckState = System.Windows.Forms.CheckState.Checked;
            this.Ortala.Location = new System.Drawing.Point(5, 424);
            this.Ortala.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Ortala.Name = "Ortala";
            this.Ortala.Size = new System.Drawing.Size(54, 21);
            this.Ortala.TabIndex = 1;
            this.Ortala.Text = "Ort.";
            this.İpUcu.SetToolTip(this.Ortala, "Ekrandaki sinyalleri yatay ve dikey eksenlerden sürekli olarak ortalayıp, ölçümle" +
        "rin ekranın görünür bölgesinde kalmasını sağlar");
            this.Ortala.UseVisualStyleBackColor = false;
            // 
            // SeçiliOlanıBelirgenleştir
            // 
            this.SeçiliOlanıBelirgenleştir.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.SeçiliOlanıBelirgenleştir.AutoSize = true;
            this.SeçiliOlanıBelirgenleştir.BackColor = System.Drawing.SystemColors.Control;
            this.SeçiliOlanıBelirgenleştir.Checked = true;
            this.SeçiliOlanıBelirgenleştir.CheckState = System.Windows.Forms.CheckState.Checked;
            this.SeçiliOlanıBelirgenleştir.Location = new System.Drawing.Point(5, 445);
            this.SeçiliOlanıBelirgenleştir.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.SeçiliOlanıBelirgenleştir.Name = "SeçiliOlanıBelirgenleştir";
            this.SeçiliOlanıBelirgenleştir.Size = new System.Drawing.Size(54, 21);
            this.SeçiliOlanıBelirgenleştir.TabIndex = 2;
            this.SeçiliOlanıBelirgenleştir.Text = "Bel.";
            this.İpUcu.SetToolTip(this.SeçiliOlanıBelirgenleştir, "Seçili olan sinyali bir miktar kalınlaştırıp, belirginleştirir");
            this.SeçiliOlanıBelirgenleştir.UseVisualStyleBackColor = false;
            // 
            // ÖlçeğiSeçiliOlanaGöreAyarla
            // 
            this.ÖlçeğiSeçiliOlanaGöreAyarla.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.ÖlçeğiSeçiliOlanaGöreAyarla.AutoSize = true;
            this.ÖlçeğiSeçiliOlanaGöreAyarla.BackColor = System.Drawing.SystemColors.Control;
            this.ÖlçeğiSeçiliOlanaGöreAyarla.Checked = true;
            this.ÖlçeğiSeçiliOlanaGöreAyarla.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ÖlçeğiSeçiliOlanaGöreAyarla.Location = new System.Drawing.Point(5, 466);
            this.ÖlçeğiSeçiliOlanaGöreAyarla.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.ÖlçeğiSeçiliOlanaGöreAyarla.Name = "ÖlçeğiSeçiliOlanaGöreAyarla";
            this.ÖlçeğiSeçiliOlanaGöreAyarla.Size = new System.Drawing.Size(55, 21);
            this.ÖlçeğiSeçiliOlanaGöreAyarla.TabIndex = 3;
            this.ÖlçeğiSeçiliOlanaGöreAyarla.Text = "Ölç.";
            this.İpUcu.SetToolTip(this.ÖlçeğiSeçiliOlanaGöreAyarla, "Dikey eksendeki değerlerin seçili olan sinyale göre ayarlanmasını sağlar");
            this.ÖlçeğiSeçiliOlanaGöreAyarla.UseVisualStyleBackColor = false;
            // 
            // AralıkSeçici
            // 
            this.AralıkSeçici.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.AralıkSeçici.Location = new System.Drawing.Point(3, -9);
            this.AralıkSeçici.Name = "AralıkSeçici";
            this.AralıkSeçici.Size = new System.Drawing.Size(539, 56);
            this.AralıkSeçici.TabIndex = 4;
            this.İpUcu.SetToolTip(this.AralıkSeçici, "Sona doğru daralt");
            this.AralıkSeçici.Scroll += new System.EventHandler(this.AralıkSeçici_Scroll);
            // 
            // Güncelle
            // 
            this.Güncelle.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.Güncelle.AutoSize = true;
            this.Güncelle.BackColor = System.Drawing.SystemColors.Control;
            this.Güncelle.Checked = true;
            this.Güncelle.CheckState = System.Windows.Forms.CheckState.Checked;
            this.Güncelle.Location = new System.Drawing.Point(5, 402);
            this.Güncelle.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Güncelle.Name = "Güncelle";
            this.Güncelle.Size = new System.Drawing.Size(61, 21);
            this.Güncelle.TabIndex = 5;
            this.Güncelle.Text = "Gün.";
            this.İpUcu.SetToolTip(this.Güncelle, "EKrandaki çizelgenin taranmasının uzun süre aldığı durumlarda sadece çizelgenin g" +
        "üncellemesini durdurmak için kullanılabilir");
            this.Güncelle.UseVisualStyleBackColor = false;
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
            this.Ekle_önceki.Location = new System.Drawing.Point(521, 198);
            this.Ekle_önceki.Name = "Ekle_önceki";
            this.Ekle_önceki.Size = new System.Drawing.Size(24, 20);
            this.Ekle_önceki.TabIndex = 7;
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
            this.Ekle_önceki_tümü.Location = new System.Drawing.Point(521, 215);
            this.Ekle_önceki_tümü.Name = "Ekle_önceki_tümü";
            this.Ekle_önceki_tümü.Size = new System.Drawing.Size(24, 20);
            this.Ekle_önceki_tümü.TabIndex = 8;
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
            this.Ekle_sonraki_tümü.Location = new System.Drawing.Point(521, 250);
            this.Ekle_sonraki_tümü.Name = "Ekle_sonraki_tümü";
            this.Ekle_sonraki_tümü.Size = new System.Drawing.Size(24, 20);
            this.Ekle_sonraki_tümü.TabIndex = 10;
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
            this.Ekle_sonraki.Location = new System.Drawing.Point(521, 267);
            this.Ekle_sonraki.Name = "Ekle_sonraki";
            this.Ekle_sonraki.Size = new System.Drawing.Size(24, 20);
            this.Ekle_sonraki.TabIndex = 9;
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
            this.Ekle_tümü.Location = new System.Drawing.Point(521, 233);
            this.Ekle_tümü.Name = "Ekle_tümü";
            this.Ekle_tümü.Size = new System.Drawing.Size(24, 20);
            this.Ekle_tümü.TabIndex = 11;
            this.Ekle_tümü.Text = "<>";
            this.İpUcu.SetToolTip(this.Ekle_tümü, "Tümünü ekle");
            this.Ekle_tümü.UseVisualStyleBackColor = true;
            this.Ekle_tümü.Visible = false;
            this.Ekle_tümü.Click += new System.EventHandler(this.Ekle_tümü_Click);
            // 
            // SolMenü
            // 
            this.SolMenü.Dock = System.Windows.Forms.DockStyle.Left;
            this.SolMenü.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.SolMenü.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.SolMenü.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.SolMenu_Ağaç,
            this.Eposta,
            this.SolMenu_Gunluk,
            this.Menu_aA,
            this.toolStripSeparator1,
            this.SolMenu_Kaydet,
            this.SolMenu_BaşlatDurdur_Ayraç,
            this.SolMenu_BaşlatDurdur});
            this.SolMenü.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.VerticalStackWithOverflow;
            this.SolMenü.Location = new System.Drawing.Point(0, 0);
            this.SolMenü.Name = "SolMenü";
            this.SolMenü.Size = new System.Drawing.Size(32, 507);
            this.SolMenü.TabIndex = 0;
            this.SolMenü.Text = "toolStrip1";
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(29, 6);
            // 
            // SolMenu_BaşlatDurdur_Ayraç
            // 
            this.SolMenu_BaşlatDurdur_Ayraç.Name = "SolMenu_BaşlatDurdur_Ayraç";
            this.SolMenu_BaşlatDurdur_Ayraç.Size = new System.Drawing.Size(29, 6);
            this.SolMenu_BaşlatDurdur_Ayraç.Visible = false;
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
            this.Ayraç_Ana.Panel1.BackColor = System.Drawing.Color.Black;
            this.Ayraç_Ana.Panel1.Controls.Add(this.Günlük_Panel);
            this.Ayraç_Ana.Panel1.Controls.Add(this.Ayraç_Ağaç);
            // 
            // Ayraç_Ana.Panel2
            // 
            this.Ayraç_Ana.Panel2.BackColor = System.Drawing.SystemColors.Control;
            this.Ayraç_Ana.Panel2.Controls.Add(this.Ekle_tümü);
            this.Ayraç_Ana.Panel2.Controls.Add(this.Ekle_sonraki_tümü);
            this.Ayraç_Ana.Panel2.Controls.Add(this.Ekle_sonraki);
            this.Ayraç_Ana.Panel2.Controls.Add(this.Ekle_önceki_tümü);
            this.Ayraç_Ana.Panel2.Controls.Add(this.Ekle_önceki);
            this.Ayraç_Ana.Panel2.Controls.Add(this.Kaydırıcı);
            this.Ayraç_Ana.Panel2.Controls.Add(this.Güncelle);
            this.Ayraç_Ana.Panel2.Controls.Add(this.ÖlçeğiSeçiliOlanaGöreAyarla);
            this.Ayraç_Ana.Panel2.Controls.Add(this.SeçiliOlanıBelirgenleştir);
            this.Ayraç_Ana.Panel2.Controls.Add(this.Ortala);
            this.Ayraç_Ana.Panel2.Controls.Add(this.Çizelge);
            this.Ayraç_Ana.Panel2.Controls.Add(this.AralıkSeçici);
            this.Ayraç_Ana.Size = new System.Drawing.Size(829, 507);
            this.Ayraç_Ana.SplitterDistance = 280;
            this.Ayraç_Ana.TabIndex = 1;
            // 
            // Günlük_Panel
            // 
            this.Günlük_Panel.Controls.Add(this.Günlük_YeniMesajaGit);
            this.Günlük_Panel.Controls.Add(this.Günlük_MetinKutusu);
            this.Günlük_Panel.Location = new System.Drawing.Point(19, 366);
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
            // Ayraç_Ağaç
            // 
            this.Ayraç_Ağaç.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.Ayraç_Ağaç.Location = new System.Drawing.Point(19, 14);
            this.Ayraç_Ağaç.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Ayraç_Ağaç.Name = "Ayraç_Ağaç";
            this.Ayraç_Ağaç.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // Ayraç_Ağaç.Panel1
            // 
            this.Ayraç_Ağaç.Panel1.Controls.Add(this.Ağaç);
            // 
            // Ayraç_Ağaç.Panel2
            // 
            this.Ayraç_Ağaç.Panel2.BackColor = System.Drawing.SystemColors.Control;
            this.Ayraç_Ağaç.Panel2MinSize = 50;
            this.Ayraç_Ağaç.Size = new System.Drawing.Size(139, 347);
            this.Ayraç_Ağaç.SplitterDistance = 246;
            this.Ayraç_Ağaç.SplitterWidth = 1;
            this.Ayraç_Ağaç.TabIndex = 1;
            // 
            // Ağaç
            // 
            this.Ağaç.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Ağaç.Location = new System.Drawing.Point(0, 0);
            this.Ağaç.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Ağaç.Name = "Ağaç";
            this.Ağaç.ShowNodeToolTips = true;
            this.Ağaç.Size = new System.Drawing.Size(139, 246);
            this.Ağaç.TabIndex = 0;
            // 
            // Kaydırıcı
            // 
            this.Kaydırıcı.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Kaydırıcı.Location = new System.Drawing.Point(3, 491);
            this.Kaydırıcı.Name = "Kaydırıcı";
            this.Kaydırıcı.Size = new System.Drawing.Size(542, 15);
            this.Kaydırıcı.TabIndex = 6;
            this.Kaydırıcı.TabStop = true;
            this.Kaydırıcı.Scroll += new System.Windows.Forms.ScrollEventHandler(this.Kaydırıcı_Scroll);
            // 
            // Çizelge
            // 
            this.Çizelge.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Çizelge.Location = new System.Drawing.Point(3, 5);
            this.Çizelge.Margin = new System.Windows.Forms.Padding(5);
            this.Çizelge.Name = "Çizelge";
            this.Çizelge.Size = new System.Drawing.Size(539, 491);
            this.Çizelge.TabIndex = 0;
            this.Çizelge.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Çizelge_MouseMove);
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
            this.DosyayaKaydetDialoğu.FileOk += new System.ComponentModel.CancelEventHandler(this.SolMenu_DosyayaKaydetDialoğu_FileOk);
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
            // SolMenu_Kaydet
            // 
            this.SolMenu_Kaydet.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.SolMenu_Kaydet.Image = global::Çizelgeç.Properties.Resources.D_Yeni;
            this.SolMenu_Kaydet.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.SolMenu_Kaydet.Name = "SolMenu_Kaydet";
            this.SolMenu_Kaydet.Size = new System.Drawing.Size(29, 24);
            this.SolMenu_Kaydet.Text = "Kaydet";
            this.SolMenu_Kaydet.ToolTipText = "Ekranda görünen sinyalleri dosyaya kaydet";
            this.SolMenu_Kaydet.Click += new System.EventHandler(this.SolMenu_Kaydet_Click);
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
            // AnaEkran
            // 
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(861, 507);
            this.Controls.Add(this.Ayraç_Ana);
            this.Controls.Add(this.SolMenü);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "AnaEkran";
            this.Opacity = 0D;
            this.Text = "Form1";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.AnaEkran_FormClosed);
            this.Load += new System.EventHandler(this.AnaEkran_Load);
            this.Shown += new System.EventHandler(this.AnaEkran_Shown);
            this.DragDrop += new System.Windows.Forms.DragEventHandler(this.AnaEkran_DragDrop);
            this.DragEnter += new System.Windows.Forms.DragEventHandler(this.AnaEkran_DragEnter);
            ((System.ComponentModel.ISupportInitialize)(this.AralıkSeçici)).EndInit();
            this.SolMenü.ResumeLayout(false);
            this.SolMenü.PerformLayout();
            this.Ayraç_Ana.Panel1.ResumeLayout(false);
            this.Ayraç_Ana.Panel2.ResumeLayout(false);
            this.Ayraç_Ana.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Ayraç_Ana)).EndInit();
            this.Ayraç_Ana.ResumeLayout(false);
            this.Günlük_Panel.ResumeLayout(false);
            this.Günlük_Panel.PerformLayout();
            this.Ayraç_Ağaç.Panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.Ayraç_Ağaç)).EndInit();
            this.Ayraç_Ağaç.ResumeLayout(false);
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
        private System.Windows.Forms.TreeView Ağaç;
        private System.Windows.Forms.SplitContainer Ayraç_Ağaç;
        private System.Windows.Forms.ToolStripButton SolMenu_Gunluk;
        private System.Windows.Forms.ToolStripButton Eposta;
        private System.Windows.Forms.ToolStripDropDownButton Menu_aA;
        private System.Windows.Forms.ToolStripMenuItem Menu_aA_100;
        private System.Windows.Forms.ToolStripMenuItem Menu_aA_125;
        private System.Windows.Forms.ToolStripMenuItem Menu_aA_150;
        private System.Windows.Forms.ToolStripMenuItem Menu_aA_175;
        private System.Windows.Forms.ToolStripMenuItem Menu_aA_200;
        private System.Windows.Forms.CheckBox Ortala;
        private System.Windows.Forms.CheckBox SeçiliOlanıBelirgenleştir;
        private System.Windows.Forms.CheckBox ÖlçeğiSeçiliOlanaGöreAyarla;
        private System.Windows.Forms.TrackBar AralıkSeçici;
        private System.Windows.Forms.CheckBox Güncelle;
        private System.Windows.Forms.HScrollBar Kaydırıcı;
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
        private System.Windows.Forms.ToolStripButton SolMenu_Kaydet;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripSeparator SolMenu_BaşlatDurdur_Ayraç;
    }
}


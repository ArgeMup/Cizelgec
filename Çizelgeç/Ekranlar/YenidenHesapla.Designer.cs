namespace Çizelgeç
{
    partial class YenidenHesapla
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
            this.HerZamanÜstte = new System.Windows.Forms.CheckBox();
            this.Çalıştır = new System.Windows.Forms.Button();
            this.İlerlemeÇubuğu = new System.Windows.Forms.ProgressBar();
            this.KullanılanDosya = new System.Windows.Forms.ComboBox();
            this.Notlar = new System.Windows.Forms.TextBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.Aç = new System.Windows.Forms.Button();
            this.KullanılanDosyanınİşlemleri = new System.Windows.Forms.ComboBox();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // HerZamanÜstte
            // 
            this.HerZamanÜstte.AutoSize = true;
            this.HerZamanÜstte.Checked = true;
            this.HerZamanÜstte.CheckState = System.Windows.Forms.CheckState.Checked;
            this.HerZamanÜstte.Dock = System.Windows.Forms.DockStyle.Right;
            this.HerZamanÜstte.Location = new System.Drawing.Point(290, 0);
            this.HerZamanÜstte.Name = "HerZamanÜstte";
            this.HerZamanÜstte.Size = new System.Drawing.Size(125, 32);
            this.HerZamanÜstte.TabIndex = 2;
            this.HerZamanÜstte.Text = "Her zaman üstte";
            this.HerZamanÜstte.UseVisualStyleBackColor = true;
            this.HerZamanÜstte.CheckedChanged += new System.EventHandler(this.HerZamanÜstte_CheckedChanged);
            // 
            // Çalıştır
            // 
            this.Çalıştır.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Çalıştır.Enabled = false;
            this.Çalıştır.Location = new System.Drawing.Point(0, 0);
            this.Çalıştır.Name = "Çalıştır";
            this.Çalıştır.Size = new System.Drawing.Size(233, 32);
            this.Çalıştır.TabIndex = 3;
            this.Çalıştır.Text = "Çalıştır";
            this.Çalıştır.UseVisualStyleBackColor = true;
            this.Çalıştır.Click += new System.EventHandler(this.Çalıştır_Click);
            // 
            // İlerlemeÇubuğu
            // 
            this.İlerlemeÇubuğu.Dock = System.Windows.Forms.DockStyle.Top;
            this.İlerlemeÇubuğu.Location = new System.Drawing.Point(0, 80);
            this.İlerlemeÇubuğu.Name = "İlerlemeÇubuğu";
            this.İlerlemeÇubuğu.Size = new System.Drawing.Size(415, 23);
            this.İlerlemeÇubuğu.TabIndex = 4;
            // 
            // KullanılanDosya
            // 
            this.KullanılanDosya.Dock = System.Windows.Forms.DockStyle.Top;
            this.KullanılanDosya.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.KullanılanDosya.FormattingEnabled = true;
            this.KullanılanDosya.Items.AddRange(new object[] {
            "Yeni C# kod dosyası oluştur"});
            this.KullanılanDosya.Location = new System.Drawing.Point(0, 0);
            this.KullanılanDosya.Name = "KullanılanDosya";
            this.KullanılanDosya.Size = new System.Drawing.Size(415, 24);
            this.KullanılanDosya.TabIndex = 7;
            this.KullanılanDosya.SelectedIndexChanged += new System.EventHandler(this.KullanılanDosya_SelectedIndexChanged);
            // 
            // Notlar
            // 
            this.Notlar.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Notlar.Location = new System.Drawing.Point(0, 103);
            this.Notlar.Multiline = true;
            this.Notlar.Name = "Notlar";
            this.Notlar.ReadOnly = true;
            this.Notlar.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.Notlar.Size = new System.Drawing.Size(415, 119);
            this.Notlar.TabIndex = 8;
            this.Notlar.WordWrap = false;
            this.Notlar.DoubleClick += new System.EventHandler(this.Notlar_DoubleClick);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.Çalıştır);
            this.panel1.Controls.Add(this.Aç);
            this.panel1.Controls.Add(this.HerZamanÜstte);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 48);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(415, 32);
            this.panel1.TabIndex = 9;
            // 
            // Aç
            // 
            this.Aç.Dock = System.Windows.Forms.DockStyle.Right;
            this.Aç.Enabled = false;
            this.Aç.Location = new System.Drawing.Point(233, 0);
            this.Aç.Name = "Aç";
            this.Aç.Size = new System.Drawing.Size(57, 32);
            this.Aç.TabIndex = 4;
            this.Aç.Text = "Aç";
            this.Aç.UseVisualStyleBackColor = true;
            this.Aç.Click += new System.EventHandler(this.Aç_Click);
            // 
            // KullanılanDosyanınİşlemleri
            // 
            this.KullanılanDosyanınİşlemleri.Dock = System.Windows.Forms.DockStyle.Top;
            this.KullanılanDosyanınİşlemleri.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.KullanılanDosyanınİşlemleri.FormattingEnabled = true;
            this.KullanılanDosyanınİşlemleri.Location = new System.Drawing.Point(0, 24);
            this.KullanılanDosyanınİşlemleri.Name = "KullanılanDosyanınİşlemleri";
            this.KullanılanDosyanınİşlemleri.Size = new System.Drawing.Size(415, 24);
            this.KullanılanDosyanınİşlemleri.TabIndex = 10;
            // 
            // YenidenHesapla
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(415, 222);
            this.Controls.Add(this.Notlar);
            this.Controls.Add(this.İlerlemeÇubuğu);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.KullanılanDosyanınİşlemleri);
            this.Controls.Add(this.KullanılanDosya);
            this.Name = "YenidenHesapla";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Yeniden Hesapla";
            this.TopMost = true;
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.YenidenHesapla_FormClosed);
            this.Shown += new System.EventHandler(this.YenidenHesapla_Shown);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.CheckBox HerZamanÜstte;
        private System.Windows.Forms.Button Çalıştır;
        private System.Windows.Forms.ComboBox KullanılanDosya;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.ComboBox KullanılanDosyanınİşlemleri;
        public System.Windows.Forms.ProgressBar İlerlemeÇubuğu;
        public System.Windows.Forms.TextBox Notlar;
        private System.Windows.Forms.Button Aç;
    }
}
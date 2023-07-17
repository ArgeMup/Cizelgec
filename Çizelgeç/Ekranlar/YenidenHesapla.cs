using ArgeMup.HazirKod;
using ArgeMup.HazirKod.Ekİşlemler;
using System;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace Çizelgeç
{
    public partial class YenidenHesapla : Form
    {
        string KökKlasör;
        KodKümesi_Dll_ dll = null;
        public YenidenHesapla(string SinyallerDosyasıKlasörü)
        {
            InitializeComponent();
            Icon = Properties.Resources.user;

            KökKlasör = SinyallerDosyasıKlasörü;
            if (Directory.Exists(KökKlasör))
            {
                string[] cs_ler = Directory.GetFiles(KökKlasör, "*.cs", SearchOption.AllDirectories);
                foreach (string cs in cs_ler)
                {
                    KullanılanDosya.Items.Add(cs.Substring(KökKlasör.Length + 1));
                }
            }

            Font = S.AnaEkran.Font;
        }
        private void YenidenHesapla_FormClosed(object sender, FormClosedEventArgs e)
        {
            S.YenidenHesapla = null;
        }

        private void HerZamanÜstte_CheckedChanged(object sender, System.EventArgs e)
        {
            TopMost = HerZamanÜstte.Checked;
        }
        private void YenidenHesapla_Shown(object sender, System.EventArgs e)
        {
            KullanılanDosya.DroppedDown = true;
        }
        private void Notlar_DoubleClick(object sender, EventArgs e)
        {
            Notlar.Text = string.Empty;
        }
        private void Aç_Click(object sender, EventArgs e)
        {
            string d = KökKlasör + "\\" + KullanılanDosya.Text;
            if (File.Exists(d)) S.Çalıştır.UygulamayaİşletimSistemiKararVersin(KökKlasör + "\\" + KullanılanDosya.Text);
        }

        private void KullanılanDosya_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            Çalıştır.Enabled = false;
            KullanılanDosyanınİşlemleri.Items.Clear();
            Aç.Enabled = false;
            if (KullanılanDosya.SelectedIndex < 0) return;

            if (KullanılanDosya.SelectedIndex == 0)
            {
                //Yeni dosya oluştur
                string dsy_adı = "KaynakKod_1.cs";
                int syc = 2;
                while (File.Exists(KökKlasör + "\\" + dsy_adı))
                {
                    dsy_adı = "KaynakKod_" + syc++ + ".cs";
                }

                KullanılanDosya.Items.Add(dsy_adı);
                KullanılanDosya.SelectedIndex = KullanılanDosya.Items.Count - 1;
            }
            else
            {
                Aç.Enabled = true;
                KullanılanDosyanınİşlemleri.Items.Clear();

                //mevcut dosyalardan biri
                KaynakKodİçeriğiniGüncelle(KökKlasör + "\\" + KullanılanDosya.Text);

                //derleme
                Notlar.AppendText("Derleniyor" + Environment.NewLine);
                try
                {
                    dll = new KodKümesi_Dll_(KodKümesi_Dll_.Derle(new string[] { KökKlasör + "\\" + KullanılanDosya.Text }));
                    foreach (string alanadı in dll.Listele_AlanAdıVeSınıf())
                    {
                        foreach (string işlem in dll.Listele_İşlem(alanadı))
                        {
                            KullanılanDosyanınİşlemleri.Items.Add(alanadı + " " + işlem.Split('|')[1]);
                        }
                    }
                    Notlar.AppendText("Derleme Başarılı" + Environment.NewLine);
                    KullanılanDosyanınİşlemleri.DroppedDown = true;
                    Çalıştır.Enabled = KullanılanDosyanınİşlemleri.Items.Count > 0;
                }
                catch (Exception ex)
                {
                    while (ex != null)
                    {
                        Notlar.AppendText(ex.Message + Environment.NewLine);
                        ex = ex.InnerException;
                    }

                    string dhd = Kendi.Klasörü + "\\Derleme Hataları.txt";
                    if (File.Exists(dhd)) Notlar.AppendText(dhd.DosyaYolu_Oku_Yazı() + Environment.NewLine);
                }
            }
        }
        void KaynakKodİçeriğiniGüncelle(string DosyaYolu)
        {
            string Başlangıç = "/* ArGeMuP " + Kendi.DosyaAdı + " BU KISMI DEĞİŞTİRMEYİNİZ - BAŞLANGIÇ */";
            string Bitiş = "/* ArGeMuP " + Kendi.DosyaAdı + " BU KISMI DEĞİŞTİRMEYİNİZ - BİTİŞ */";
            string içerik = File.Exists(DosyaYolu) ? DosyaYolu.DosyaYolu_Oku_Yazı() : "";
            
            //eski bilgileri sil
            int knm_başla = içerik.IndexOf(Başlangıç);
            if (knm_başla >= 0)
            {
                int knm_bitir = içerik.IndexOf(Bitiş, knm_başla);
                if (knm_bitir >= 0)
                {
                    içerik = içerik.Remove(knm_başla, knm_bitir - knm_başla + Bitiş.Length);
                }
            }

            //Başlığı ekle
            string güncel = "Ölü Ekranlama : " + DateTime.Now.Yazıya() + Environment.NewLine;

            //sinyallerin isim ve numaralarını ekle
            güncel += "Sinyalin sıra numarası ve benzersiz adı (Ağaç içindeki yeri)" + Environment.NewLine;
            for (int i = 0; i < Sinyaller.Tümü.Count; i++)
            {
                Sinyal_ sny = Sinyaller.Tümü.ElementAt(i).Value;
                güncel += i + " " + Sinyaller.Tümü.ElementAt(i).Key + " (" + sny.Adı.Csv + ")" + Environment.NewLine;
            }

            içerik += Properties.Resources.ÖrnekKaynakKod_ÖlüEkranlama.Replace("??? [[[ Detaylar ]]] %%%", güncel);

            //kaydet
            içerik.Dosyaİçeriği_Yaz(DosyaYolu);
        }

        private void Çalıştır_Click(object sender, EventArgs e)
        {
            S.Çizelge.Invoke(new Action(() =>
            {
                try
                {
                    bool Normalleşme_değer = Normalleştirme_DeğerEkseni_YapılmışMı;
                    if (Normalleşme_değer) Normalleştirme_DeğerEkseni_Tersle();
                    bool Normalleşme_zaman = Normalleştirme_ZamanEkseni_YapılmışMı;
                    if (Normalleşme_zaman) Normalleştirme_ZamanEkseni_Tersle();

                    string[] dizi = KullanılanDosyanınİşlemleri.Text.Split(' ');
                    dll.Çağır(dizi[0], dizi[1]);

                    if (Normalleşme_zaman) Normalleştirme_ZamanEkseni_Tersle();
                    if (Normalleşme_değer) Normalleştirme_DeğerEkseni_Tersle();
                    Yardımcıİşlemler.ÖnYüz.Güncelle();
                }
                catch (Exception ex)
                {
                    while (ex != null)
                    {
                        Notlar.AppendText(ex.ToString() + Environment.NewLine);
                        ex = ex.InnerException;
                    }
                }
            }));
        }
        bool Normalleştirme_DeğerEkseni_YapılmışMı
        {
            get
            {
                return Çizelgeç.S.AnaEkran.SağTuşMenü_Çizelge_Normalleştirme_çarpanlar_EnBüyük != null;
            }
        }
        void Normalleştirme_DeğerEkseni_Tersle()
        {
            Çizelgeç.S.AnaEkran.SağTuşMenü_Çizelge_DeğerleriNormalleştir_Click(null, null);
        }
        bool Normalleştirme_ZamanEkseni_YapılmışMı
        {
            get
            {
                return Çizelgeç.S.AnaEkran.SağTuşMenü_Çizelge_Normalleştirme_ZamanEkseni != null;
            }
        }
        void Normalleştirme_ZamanEkseni_Tersle()
        {
            Çizelgeç.S.AnaEkran.SağTuşMenü_Çizelge_xEkseniTarihVeSaat_Click(null, null);
        }
    }
}

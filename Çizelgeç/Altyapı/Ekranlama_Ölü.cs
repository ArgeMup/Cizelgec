using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Çizelgeç
{
    public class Ekranlama_Ölü
    {
        public Ekranlama_Ölü(string DosyaYolu)
        {
            if (!File.Exists(DosyaYolu)) return;

            S.Ağaç.Nodes.Clear();
            S.Ağaç.CheckBoxes = false;
            S.Ağaç.Nodes.Add("Bekleyiniz");
            Application.DoEvents();

            S.Çizelge.Reset();
            S.Çizelge.plt.Clear();
            S.Çizelge.plt.SetCulture(System.Globalization.CultureInfo.CreateSpecificCulture("tr"));
            S.Çizelge.plt.YLabel("Tümü", /*fontName: S.AnaEkran.Font.Name,*/ fontSize: (float)(S.AnaEkran.Font.Size * 1.25));
            S.Çizelge.plt.Ticks(dateTimeX: true, /*fontName: S.AnaEkran.Font.Name,*/ fontSize: (float)(S.AnaEkran.Font.Size * 1.25)/*, useMultiplierNotation:true*/);
            S.Çizelge.Configure(enableRightClickMenu: false, enableDoubleClickBenchmark: true);

            if (DosyaYolu.ToLower().EndsWith("csv")) Csv(DosyaYolu);
            else Mup(DosyaYolu);

            S.AralıkSeçici.Minimum = 2;
            S.AralıkSeçici.Maximum = S.CanliÇizdirme_ÖlçümSayısı - 1;
            S.AralıkSeçici.Value = S.AralıkSeçici.Maximum;
        }
        public void Csv(string CsvDosyasıYolu)
        {
            string Başlıklar = "";
            string Doğrulama = "";
            string HesaplananDoğrulama = "";
            int ÖlçümSayısı = 0;
            string SonÖlçüm = "";

            #region İçeriğin Sayılması
            using (StreamReader sr = new StreamReader(CsvDosyasıYolu))
            {
                bool hesabadahilet = true;
                while (sr.Peek() >= 0 && S.Çalışşsın)
                {
                    string okunan = sr.ReadLine();
                    if (okunan.Contains(";Sinyaller;")) { SonÖlçüm = okunan; ÖlçümSayısı++; }
                    else if (okunan.Contains(";Doğrulama;")) { Doğrulama = okunan; hesabadahilet = false; }
                    else if (okunan.Contains(";Başlıklar;")) Başlıklar = okunan;

                    if (hesabadahilet) HesaplananDoğrulama = ArgeMup.HazirKod.Dönüştürme.D_GeriDönülemezKarmaşıklaştırmaMetodu.Metinden(HesaplananDoğrulama + okunan + Environment.NewLine);
                }
            }

            if (string.IsNullOrEmpty(Doğrulama) || HesaplananDoğrulama != Doğrulama.Split(';')[2]) Günlük.Ekle("Dosya bütünlüğü doğrulanamadı, içerik eksik yada değiştirilmiş olabilir.");
            S.CanliÇizdirme_ÖlçümSayısı = ÖlçümSayısı;
            S.ZamanEkseni = new double[S.CanliÇizdirme_ÖlçümSayısı];
            #endregion

            TreeNode t = new TreeNode(Path.GetFileNameWithoutExtension(CsvDosyasıYolu));
            t.Checked = true;

            if (string.IsNullOrEmpty(Başlıklar))
            {
                //başlık satırı yok, bir tane ekle
                int sinyal_adet = SonÖlçüm.Split(';').Length - 2;
                Başlıklar = "t;t";
                for (int i = 0; i < sinyal_adet; i++) Başlıklar += ";s" + (i + 1);
            }

            #region Sinyaller
            TreeNode s = new TreeNode("Sinyaller");
            string[] _sinyaller = Başlıklar.Split(';');
            for (int i = 2; i < _sinyaller.Length; i++)
            {
                string grup = "", dal = "", uzun = "";
                if (_sinyaller[i].Contains("|"))
                {
                    string[] _a = _sinyaller[i].Split('|');
                    grup = _a[0] + " " + _a[1];
                    dal = _a[2];
                }
                else dal = _sinyaller[i];
                uzun = _sinyaller[i].Replace('|', ' ').Trim('<', '>', ' ');

                Sinyal_ sinyal = Sinyaller.Ekle("<" + uzun + ">");
                sinyal.Adı.Grup = grup;
                sinyal.Adı.Dal = dal;
                sinyal.Adı.Uzun = uzun;
                sinyal.Adı.Csv = _sinyaller[i];

                if (!string.IsNullOrEmpty(sinyal.Adı.Grup))
                {
                    TreeNode[] dizi = s.Nodes.Find(sinyal.Adı.Grup, false);
                    if (dizi != null && dizi.Length > 0)
                    {
                        TreeNode y = dizi[0].Nodes.Add(sinyal.Adı.Dal);
                        y.Tag = sinyal;
                        y.Checked = true;
                        sinyal.Dal = y;
                    }
                    else
                    {
                        TreeNode alt = s.Nodes.Add(sinyal.Adı.Grup, sinyal.Adı.Grup);
                        //alt.Tag = sinyal.Ölçüm;
                        alt.Checked = true;
                        TreeNode y = alt.Nodes.Add(sinyal.Adı.Dal);
                        y.Tag = sinyal;
                        y.Checked = true;
                        sinyal.Dal = y;
                    }
                }
                else
                {
                    TreeNode y = s.Nodes.Add(sinyal.Adı.Uzun);
                    y.Tag = sinyal;
                    y.Checked = true;
                    sinyal.Dal = y;
                }

                sinyal.Değeri.DeğerEkseni = new double[ÖlçümSayısı];

                sinyal.Çizikler = S.Çizelge.plt.PlotSignalXY(S.ZamanEkseni, sinyal.Değeri.DeğerEkseni);
                sinyal.Dal.ForeColor = sinyal.Çizikler.color;
                //sinyal.Uyarı_Yazıları = new List<ScottPlot.PlottableText>();
            }
            s.Checked = true;
            t.Nodes.Add(s);

            //değişkenler grubunun dışarı atılması
            TreeNode[] dizii = s.Nodes.Find("Değişkenler", false);
            if (dizii != null && dizii.Length > 0)
            {
                s.Nodes.Remove(dizii[0]);
                t.Nodes.Add(dizii[0]);
            }
            #endregion

            //#region Uyarılar
            //TreeNode u = new TreeNode("Uyarılar");
            //u.Checked = false;
            //t.Nodes.Add(u);
            //#endregion

            S.Ağaç.Nodes.Clear();
            S.Ağaç.CheckBoxes = true;
            S.Ağaç.Nodes.Add(t);
            S.Ağaç.ExpandAll();

            #region Tamponların doldurulması
            int işlenen = 0;
            int tik = Environment.TickCount;
            using (StreamReader sr = new StreamReader(CsvDosyasıYolu))
            {
                while (sr.Peek() >= 0 && S.Çalışşsın)
                {
                    if (Environment.TickCount - tik > 1000)
                    {
                        t.Text = Path.GetFileNameWithoutExtension(CsvDosyasıYolu) + " - %" + (işlenen * 100) / ÖlçümSayısı;
                        tik = Environment.TickCount;
                        Application.DoEvents();
                    }

                    string okunan = sr.ReadLine();
                    try
                    {
                        string[] bir_satırdakiler = okunan.Split(';');
                        double zaman = S.Tarih.Sayıya(bir_satırdakiler[0]);

                        if (okunan.Contains(";Sinyaller;"))
                        {
                            S.ZamanEkseni[işlenen] = zaman;

                            for (int i = 2; i < bir_satırdakiler.Length; i++)
                            {
                                Sinyaller.Tümü.Values.ElementAt(i - 2).Değeri.DeğerEkseni[işlenen] = S.Sayı.Yazıdan(bir_satırdakiler[i]);
                            }

                            işlenen++;
                        }
                        else if (okunan.Contains(";Uyarı;"))
                        {
                            double y = S.Sayı.Yazıdan(bir_satırdakiler[3]);
                            string adı = bir_satırdakiler[2].Replace('|', ' ').Trim('<', '>', ' ');
                            Sinyal_ sinyal = Sinyaller.Bul("<" + adı + ">");

                            //sinyal.Uyarı_Yazıları.Add(S.Çizelge.plt.PlotText(bir_satırdakiler[4], zaman, y, color: sinyal.Çizikler.color, rotation: 270));
                            Günlük.Ekle(Environment.NewLine + bir_satırdakiler[4] + " " + bir_satırdakiler[2] + " " + bir_satırdakiler[3] + " " + bir_satırdakiler[0]);
                        }
                    }
                    catch (Exception ex) { Günlük.Ekle("Problemli satır -> " + okunan + " -> " + ex.ToString()); }
                }
            }
            #endregion

            #region Çizelge Görsellerini Oluştur
            t.Text = "Grafik hazırlanıyor";
            S.Çizdir();
            #endregion

            t.Text = Path.GetFileNameWithoutExtension(CsvDosyasıYolu) + " - " + ÖlçümSayısı;
        }
        public void Mup(string MupDosyasıYolu)
        {
            int ÖlçümSayısı = 1;
            S.ZamanEkseni = new double[S.CanliÇizdirme_ÖlçümSayısı];
            int tik = Environment.TickCount;

            TreeNode t = new TreeNode(Path.GetFileNameWithoutExtension(MupDosyasıYolu));
            t.Checked = true;

            #region ayarlar dosyası varmı kontrolü
            string kla = Path.GetDirectoryName(MupDosyasıYolu) + "\\Ayarlar.json";
            if (File.Exists(kla)) json_Ayıkla.Ayarlar(kla);
            Bağlantılar.Tümü.Clear();
            foreach (var biri in Sinyaller.Tümü.Values)
            {
                biri.Değeri.DeğerEkseni = new double[S.CanliÇizdirme_ÖlçümSayısı];
            }
            #endregion

            bool Enazbirtanedüzgüntarihbulundu = false;
            double bir_zamanlar = S.Tarih.Sayıya(new DateTime(2000, 1, 1));
            using (StreamReader sr = new StreamReader(MupDosyasıYolu))
            {
                while (sr.Peek() >= 0 && S.Çalışşsın)
                {
                    if (Environment.TickCount - tik > 1000)
                    {
                        t.Text = Path.GetFileNameWithoutExtension(MupDosyasıYolu) + " - %" + (ÖlçümSayısı * 100) / S.CanliÇizdirme_ÖlçümSayısı;
                        tik = Environment.TickCount;
                        Application.DoEvents();
                    }

                    string okunan = sr.ReadLine();
                    try
                    {
                        int başlangıç = okunan.IndexOf(S.BilgiToplama_SinyallerCümleBaşlangıcı + S.BilgiToplama_KelimeAyracı);
                        if (başlangıç >= 0)
                        {
                            if (ÖlçümSayısı >= S.CanliÇizdirme_ÖlçümSayısı)
                            {
                                S.CanliÇizdirme_ÖlçümSayısı += 10000;
                                foreach (var biri in Sinyaller.Tümü.Values)
                                {
                                    Array.Resize(ref biri.Değeri.DeğerEkseni, S.CanliÇizdirme_ÖlçümSayısı);
                                }

                                Array.Resize(ref S.ZamanEkseni, S.CanliÇizdirme_ÖlçümSayısı);
                            }

                            foreach (var biri in Sinyaller.Tümü.Values)
                            {
                                biri.Değeri.DeğerEkseni[ÖlçümSayısı] = biri.Değeri.DeğerEkseni[ÖlçümSayısı - 1];
                            }

                            string gelen = okunan.Substring(başlangıç).Trim(' ', S.BilgiToplama_KelimeAyracı);
                            string[] dizi = gelen.Split(S.BilgiToplama_KelimeAyracı);
                            for (int i = 2; i < dizi.Length; i++)
                            {
                                string sinyal_yazı = "<" + dizi[1] + "[" + (i - 2).ToString() + "]>";
                                Sinyal_ sinyal = Sinyaller.Ekle(sinyal_yazı);
                                if (sinyal.Değeri.DeğerEkseni == null) sinyal.Değeri.DeğerEkseni = new double[S.CanliÇizdirme_ÖlçümSayısı];
                                sinyal.Değeri.DeğerEkseni[ÖlçümSayısı] = S.Sayı.Yazıdan(dizi[i]);
                            }

                            //tarihi okumaya calış
                            if (!S.Tarih.Sayıya(okunan.Split(S.BilgiToplama_KelimeAyracı)[0], out S.ZamanEkseni[ÖlçümSayısı])) S.ZamanEkseni[ÖlçümSayısı] = ÖlçümSayısı;
                            else
                            {
                                if (S.ZamanEkseni[ÖlçümSayısı] > bir_zamanlar) Enazbirtanedüzgüntarihbulundu = true;
                            }

                            ÖlçümSayısı++;
                        }
                    }
                    catch (Exception ex) { Günlük.Ekle("Problemli satır -> " + okunan + " -> " + ex.ToString()); }
                }
            }

            if (Sinyaller.Tümü.Count == 0)
            {
                throw new Exception("Hiç bilgi alınamadı. Cümle başlangıcı ve kelime ayracı uygun olmayabilir. Kaynak dosyanın olduğu klasörün içerisine kaynak dosya için düzenlenmiş bir Ayarlar.json dosyası kopyalayın");
            }

            #region daraltma
            S.CanliÇizdirme_ÖlçümSayısı = ÖlçümSayısı - 1;
            foreach (var biri in Sinyaller.Tümü.Values)
            {
                Array.Resize(ref biri.Değeri.DeğerEkseni, S.CanliÇizdirme_ÖlçümSayısı);
            }
            Array.Resize(ref S.ZamanEkseni, S.CanliÇizdirme_ÖlçümSayısı);
            #endregion

            #region Tarih Saatte olabilecek Eksiklikleri Giderme
            if (Enazbirtanedüzgüntarihbulundu)
            {
                DateTime ilk_zaman = DateTime.MinValue;
                double son_zaman = 0;
                for (int i = S.CanliÇizdirme_ÖlçümSayısı - 1; i >= 0; i--)
                {
                    if (S.ZamanEkseni[i] >= bir_zamanlar) { son_zaman = S.ZamanEkseni[i]; continue; }

                    ilk_zaman = S.Tarih.Tarihe(son_zaman) - TimeSpan.FromMilliseconds(S.ZamanEkseni[i] + 1000 /*son ölçümden 1sn önceye*/);
                    break;
                }

                if (ilk_zaman != DateTime.MinValue)
                {
                    for (int i = 0; i < S.CanliÇizdirme_ÖlçümSayısı; i++)
                    {
                        if (S.ZamanEkseni[i] >= bir_zamanlar) break;

                        S.ZamanEkseni[i] = S.Tarih.Sayıya(ilk_zaman + TimeSpan.FromMilliseconds(S.ZamanEkseni[i]));
                    }
                }
            }
            else
            {
                S.Çizelge.plt.Ticks(dateTimeX: false);
            }
            #endregion  

            #region Sinyaller
            TreeNode s = new TreeNode("Sinyaller");
            foreach (var sinyal in Sinyaller.Tümü.Values)
            {
                if (!string.IsNullOrEmpty(sinyal.Adı.Grup))
                {
                    TreeNode[] dizi = s.Nodes.Find(sinyal.Adı.Grup, false);
                    if (dizi != null && dizi.Length > 0)
                    {
                        TreeNode y = dizi[0].Nodes.Add(sinyal.Adı.Dal);
                        y.Tag = sinyal;
                        y.Checked = true;
                        sinyal.Dal = y;
                    }
                    else
                    {
                        TreeNode alt = s.Nodes.Add(sinyal.Adı.Grup, sinyal.Adı.Grup);
                        //alt.Tag = sinyal.Ölçüm;
                        alt.Checked = true;
                        TreeNode y = alt.Nodes.Add(sinyal.Adı.Dal);
                        y.Tag = sinyal;
                        y.Checked = true;
                        sinyal.Dal = y;
                    }
                }
                else
                {
                    TreeNode y = s.Nodes.Add(sinyal.Adı.Uzun);
                    y.Tag = sinyal;
                    y.Checked = true;
                    sinyal.Dal = y;
                }

                sinyal.Çizikler = S.Çizelge.plt.PlotSignalXY(S.ZamanEkseni, sinyal.Değeri.DeğerEkseni);
                sinyal.Dal.ForeColor = sinyal.Çizikler.color;
            }
            s.Checked = true;
            t.Nodes.Add(s);
            #endregion

            S.Ağaç.Nodes.Clear();
            S.Ağaç.CheckBoxes = true;
            S.Ağaç.Nodes.Add(t);
            S.Ağaç.ExpandAll();

            #region Çizelge Görsellerini Oluştur
            t.Text = "Grafik hazırlanıyor";
            S.Çizdir();
            #endregion

            t.Text = Path.GetFileNameWithoutExtension(MupDosyasıYolu) + " - " + ÖlçümSayısı;
        }
    }
}

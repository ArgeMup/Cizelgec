// Copyright ArgeMup GNU GENERAL PUBLIC LICENSE Version 3 <http://www.gnu.org/licenses/> <https://github.com/ArgeMup>

using ArgeMup.HazirKod;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Threading;
using System.Windows.Forms;

namespace Çizelgeç
{
    class S
    {
        #region Bilgi Toplama / Dosyalama
        public static string AnaKlasör = Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location).Trim('\\') + "\\";
        public static string Şablon_Klasörü = AnaKlasör + "Sablon\\";
        public static string Kulanıcı_Klasörü = AnaKlasör + Environment.MachineName + "_" + Environment.UserName + "\\";

        public static string DosyaKlasorIcindeKullanilamayacakKarakterler = new string(Path.GetInvalidFileNameChars()) + new string(Path.GetInvalidPathChars()); 
        public static string DosyaKlasörAdınıDüzelt(string Girdi)
        {
            foreach (char c in DosyaKlasorIcindeKullanilamayacakKarakterler)
            {
                Girdi = Girdi.Replace(c.ToString(), ""); 
            }
            return Girdi;
        }
        public class Tarih
        {
            public const string _Şablon_kısa = "dd.MM.yyyy HH:mm:ss";
            public const string _Şablon_uzun = "dd.MM.yyyy HH:mm:ss.fff";
            public const string _Şablon_uzun_Ağaç = "HH:mm:ss.fff ddd dd.MM.yyyy";
            public const string _Şablon_dosyaadı = "dd_MM_yyyy_HH_mm_ss";
            public const string _Şablon_uzun_TarihSaat = "dd.MM.yyyy * HH:mm:ss.fff zzz";
            public static string Yazıya(DateTime Girdi, string Şablon = _Şablon_uzun, CultureInfo Kültür = null)
            {
                return Girdi.ToString(Şablon, Kültür == null ? CultureInfo.InvariantCulture : Kültür);
            }
            public static string Yazıya(double Girdi, string Şablon = _Şablon_uzun, CultureInfo Kültür = null)
            {
                return Yazıya(Tarihe(Girdi), Şablon, Kültür);
            }
            public static string Yazıya_TarihSaat(DateTime Girdi, CultureInfo Kültür = null)
            {
                return Yazıya(Girdi, _Şablon_uzun_TarihSaat, Kültür).Replace("*", ((int)Girdi.DayOfWeek).ToString());
            }
            public static DateTime Tarihe(double Girdi)
            {
                return DateTime.FromOADate(Girdi);
            }
            public static double Sayıya(DateTime Girdi)
            {
                return Girdi.ToOADate();
            }
            public static double Sayıya(string Girdi)
            {
                if (Sayıya(Girdi, out double Çıktı)) return Çıktı;

                throw new Exception(Girdi + " tarihe dönüştürülemiyor");
            }
            public static bool Sayıya(string Girdi, out double Çıktı)
            {
                if (Girdi.Length >= _Şablon_uzun.Length)
                {
                    if (DateTime.TryParseExact(Girdi.Substring(0, _Şablon_uzun.Length), _Şablon_uzun, System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.AssumeLocal, out DateTime yeni))
                    {
                        Çıktı = yeni.ToOADate();
                        return true;
                    }
                }

                if (Girdi.Length >= _Şablon_kısa.Length)
                {
                    if (DateTime.TryParseExact(Girdi.Substring(0, _Şablon_kısa.Length), _Şablon_kısa, System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.AssumeLocal, out DateTime yeni))
                    {
                        Çıktı = yeni.ToOADate();
                        return true;
                    }
                }

                if (Girdi.Length >= _Şablon_dosyaadı.Length)
                {
                    if (DateTime.TryParseExact(Girdi.Substring(0, _Şablon_dosyaadı.Length), _Şablon_dosyaadı, System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.AssumeLocal, out DateTime yeni))
                    {
                        Çıktı = yeni.ToOADate();
                        return true;
                    }
                }

                if (Sayı.Yazıdan(Girdi, out Çıktı))
                {
                    return true;
                }

                return false;
            }
        };
        public class Sayı
        {
            public static char ondalık_ayraç = Convert.ToChar(System.Globalization.CultureInfo.InvariantCulture.NumberFormat.NumberDecimalSeparator);
            public static double Yazıdan(string Girdi)
            {
                if (Yazıdan(Girdi, out double Çıktı)) return Çıktı;

                throw new Exception(Girdi + " sayıya dönüştürülemiyor");
            }
            public static bool Yazıdan(string Girdi, out double Çıktı)
            {
                if (Girdi == "1") { Çıktı = 1; return true; }
                else if (Girdi == "0") { Çıktı = 0;  return true; }

                Girdi = Girdi.Replace('.', ondalık_ayraç).Replace(',', ondalık_ayraç);

                if (double.TryParse(Girdi, NumberStyles.Float, CultureInfo.InvariantCulture, out Çıktı))
                {
                    return true;
                }
                
                //geçersiz karakterleri sil
                string yeni = "";
                bool Enazbirkarakterbulundu = false;
                foreach (char krt in Girdi)
                {
                    if (krt == ondalık_ayraç || krt == '+' || krt == '-' || (krt >= '0' && krt <= '9'))
                    {
                        yeni += krt;
                        Enazbirkarakterbulundu = true;
                    }
                    else if (Enazbirkarakterbulundu) break;
                }

                //tekrar dene
                if (double.TryParse(yeni, NumberStyles.Float, CultureInfo.InvariantCulture, out Çıktı))
                {
                    return true;
                }

                return false;
            }
            public static string Yazıya(double Girdi)
            {
                return Girdi.ToString(CultureInfo.InvariantCulture);
            }
        };

        public static string BilgiToplama_Kıstas = "1";
        public static string BilgiToplama_ZamanAralığı_Sn = "15";
        public static bool BilgiToplama_BirbirininAynısıOlanZamanDilimleriniAtla = true;

        public static string Dosyalama_AzamiDosyaBoyutu_Bayt = "1000000";
        public static string Dosyalama_KayıtKlasörü = Kulanıcı_Klasörü;

        public static string MupDosyasındanOkuma_CümleBaşlangıcı = ">Sinyaller";
        public static char MupDosyasındanOkuma_KelimeAyracı = ';';

        public static double[] ZamanEkseni;
        public static bool BaşlatDurdur = true;
        #endregion

        #region Günlük
        //////////////////////////////////////////////////////////////////////////
        // Günlük
        //////////////////////////////////////////////////////////////////////////
        static public ToolStripButton _Günlük_Buton;
        static public TextBox _Günlük_MetinKutusu;
        #endregion

        #region Diğer
        public static string AnaEkran_ÇubuktakiYazı = "ArGeMuP Çizelgeç V" + ArgeMup.HazirKod.Kendi.Sürümü_Dosya;
        public static bool Çalışşsın = false;
        public static int Çizelge_ÇizgiKalınlığı = 1;
        public static int CanliÇizdirme_ÖlçümSayısı = 10000;
        public static CheckBox _Günlük_YeniMesajaGit;
        public static ToolStripMenuItem SağTuşMenü_Çizelge_Etkin, SağTuşMenü_Çizelge_TümSinyalleriEkranaSığdır;
        public static TreeView Ağaç;
        public static string SonDurumMesajı = "";
        public static ScottPlot.FormsPlot Çizelge;
        public static ArgeMup.HazirKod.Ayarlar_ Ayarlar;
        public static AnaEkran AnaEkran;
        public static YenidenHesapla YenidenHesapla;
        public static TrackBar AralıkSeçici_Baştan, AralıkSeçici_Sondan;
        public static ToolStripButton SolMenu_BaşlatDurdur;
        public static SplitContainer Ayraç_Ana;
        public static string[] BaşlangıçParametreleri = null;
        public static Çalıştır_ Çalıştır = new Çalıştır_();
        #endregion

        #region Çizdirme
        public static ArgeMup.HazirKod.Ortalama_ Çizdir_Ortalama = new ArgeMup.HazirKod.Ortalama_(15);
        public const int Çizdir_msnBoyuncaHızlıcaÇizdirmeyeDevamEt_Sabiti = 5 * 60 * 1000;
        public static int Çizdir_msnBoyuncaHızlıcaÇizdirmeyeDevamEt = 0;
        //public static ScottPlot.PlottableVLine Çizdirme_DikeyÇizgi = null;
        //public static ScottPlot.PlottableHLine Çizdirme_YatayÇizgi = null;
        //public static ScottPlot.PlottableScatter[] Çizdirme_Noktacıklar = new ScottPlot.PlottableScatter[0];
        public static void Çizdir()
        {
            if (SağTuşMenü_Çizelge_Etkin.Checked && AnaEkran.WindowState != FormWindowState.Minimized && !Ayraç_Ana.Panel2Collapsed)
            {
                int simdi = Environment.TickCount;

                if (SağTuşMenü_Çizelge_TümSinyalleriEkranaSığdır.Checked) Çizelge.plt.AxisAuto();
                Çizelge.Render(true);

                Çizdir_Ortalama.Güncelle(Environment.TickCount - simdi);
            }
        }
        #endregion
    }

    #region Günlük
    public class Günlük_
    {
        public string mesaj, Tür;
        public DateTime Zaman;
    }
    public class Günlük
    {
        /// <param name="Tür">HATA veya Bilgi</param>
        public static void Ekle(string Mesaj, string Tür = "HATA")
        {
            if (Görev_Nesnesi == null)
            {
                Görev_Nesnesi = new Thread(() => Görev_İşlemi());
                Görev_Nesnesi.Start();
            }

            Günlük_ yeni = new Günlük_();
            yeni.mesaj = Mesaj;
            yeni.Tür = Tür;
            yeni.Zaman = DateTime.Now;

            Mtx.WaitOne();
            Tümü.Add(yeni);
            Mtx.ReleaseMutex();
        }

        static Mutex Mtx = new Mutex();
        static public List<Günlük_> Tümü = new List<Günlük_>();

        static Thread Görev_Nesnesi = null;
        static void Görev_İşlemi()
        {
            DateTime EkSüre = DateTime.MinValue;

            while (true)
            {
                if (!S.Çalışşsın)
                {
                    if (EkSüre == DateTime.MinValue) EkSüre = DateTime.Now + TimeSpan.FromSeconds(5);
                    else if (EkSüre < DateTime.Now)
                    {
                        Görev_Nesnesi = null;
                        return;
                    }
                }
                else EkSüre = DateTime.MinValue;

                try
                {
                    if (Tümü.Count == 0)
                    {
                        Thread.Sleep(1500);
                        continue;
                    }
                    else if (S.Çalışşsın) Thread.Sleep(1);

                    Mtx.WaitOne();
                    Günlük_ yeni = Tümü[0];
                    Tümü.RemoveAt(0);
                    Mtx.ReleaseMutex();

                    string yazı = yeni.mesaj.Trim(' ', '\r', '\n');
                    string dosya_adı = "Gunluk." + S.Tarih.Yazıya(DateTime.Now, ArgeMup.HazirKod.Dönüştürme.D_TarihSaat.Şablon_Tarih) + ".csv";
                    File.AppendAllText(S.Kulanıcı_Klasörü + dosya_adı, yeni.Tür + ";" + S.Tarih.Yazıya(yeni.Zaman) + ";" + yazı.Replace(Environment.NewLine, ";") + Environment.NewLine);

                    yazı = Environment.NewLine +
                           Environment.NewLine + yeni.Tür + " " + S.Tarih.Yazıya(yeni.Zaman) +
                           Environment.NewLine + yazı;

                    S._Günlük_MetinKutusu.Invoke((MethodInvoker)delegate ()
                    {
                        if (S._Günlük_MetinKutusu.Text.Length > 1000000) S._Günlük_MetinKutusu.Text = S._Günlük_MetinKutusu.Text.Remove(0, 100000);

                        S._Günlük_MetinKutusu.Text += yazı;

                        if (S._Günlük_YeniMesajaGit.Checked)
                        {
                            S._Günlük_MetinKutusu.SelectionStart = S._Günlük_MetinKutusu.Text.Length;
                            S._Günlük_MetinKutusu.ScrollToCaret();
                        }

                        if (yeni.Tür == "HATA") S._Günlük_Buton.Image = Properties.Resources.M_Gunluk_Yeni;
                    });
                }
                catch (Exception ex) { if (S.Çalışşsın) MessageBox.Show("İstenmeyen Durum " + ex); }
            }
        }
    }
    #endregion
}

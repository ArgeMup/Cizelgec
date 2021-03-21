using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Çizelgeç
{
    class S
    {
        #region Bilgi Toplama / Dosyalama
        public static string AnaKlasör = Directory.GetCurrentDirectory().Trim('\\') + "\\";
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
            public const string _Şablon_dosyaadı = "dd_MM_yyyy_HH_mm_ss";
            public static string Yazıya(DateTime Girdi, string Şablon = _Şablon_uzun)
            {
                return Girdi.ToString(Şablon, System.Globalization.CultureInfo.InvariantCulture);
            }
            public static string Yazıya(double Girdi, string Şablon = _Şablon_uzun)
            {
                return Tarihe(Girdi).ToString(Şablon, System.Globalization.CultureInfo.InvariantCulture);
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

                if (double.TryParse(Girdi, System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture, out Çıktı))
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
                if (double.TryParse(yeni, System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture, out Çıktı))
                {
                    return true;
                }

                return false;
            }
            public static string Yazıya(double Girdi)
            {
                return Girdi.ToString(System.Globalization.CultureInfo.InvariantCulture);
            }
        };

        public static string BilgiToplama_Kıstas = "1";
        public static string BilgiToplama_ZamanAralığı_Sn = "15";

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
        public static bool Çalışşsın = true;
        public static bool EkranıGüncelle_me = false;
        public static int CanliÇizdirme_ÖlçümSayısı = 10000;
        public static CheckBox Ortala, Güncelle, _Günlük_YeniMesajaGit;
        public static TreeView Ağaç;
        public static ScottPlot.FormsPlot Çizelge;
        public static ArgeMup.HazirKod.Ayarlar_ Ayarlar;
        //public static ArgeMup.HazirKod.PencereVeTepsiIkonuKontrolu_ PeTeİkKo;
        public static AnaEkran AnaEkran;
        public static HScrollBar Kaydırıcı;
        public static TrackBar AralıkSeçici;
        public static ToolStripButton SolMenu_BaşlatDurdur;
        public static string[] BaşlangıçParametreleri = null;
        #endregion

        #region Çizdirme
        //public static ScottPlot.PlottableVLine Çizdirme_DikeyÇizgi = null;
        //public static ScottPlot.PlottableHLine Çizdirme_YatayÇizgi = null;
        //public static ScottPlot.PlottableScatter[] Çizdirme_Noktacıklar = new ScottPlot.PlottableScatter[0];
        public static int Çizdirme_Koordinat_Tick = 0;
        public static void Çizdir()
        {
            if (S.Güncelle.Checked && AnaEkran.WindowState != FormWindowState.Minimized)
            {
                if (Ortala.Checked) Çizelge.plt.AxisAuto();
                Çizelge.Render(true);
            }
        }
        #endregion
    }
}

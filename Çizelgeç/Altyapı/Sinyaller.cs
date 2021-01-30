using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Çizelgeç
{
    #region Sinyaller
    public class Ölçüm_
    {
        public string Adı;
        public string Birimi;
        public string İşlem;
    };
   
    public class Uyarı_
    {
        public string Kıstas;
        public string[] Uyarı;
    };

    public class Değeri_
    {
        public bool Kaydedilsin = true;
        public double SonDeğeri = 0;
        public DateTime SonDeğerinAlındığıAn = DateTime.Now;
        public double[] DeğerEkseni = null;
        public bool ZamanAşımıOldu = false;
    }

    public class Adı_
    {
        public string Grup;
        public string Dal;
        public string Uzun;
        public string Csv;
    }

    public enum Tür_ { Boşta, Sinyal, Değişken };

    public class Sinyal_
    {
        public Tür_ Tür = Tür_.Boşta;
        public Adı_ Adı = new Adı_(); 
        public Değeri_ Değeri = new Değeri_();
        public Uyarı_ Uyarı = null;
        public Ölçüm_ Ölçüm = null;

        public TreeNode Dal = null;
        public ScottPlot.PlottableSignalXY Çizikler = null;
        public List<ScottPlot.PlottableText> Uyarı_Yazıları = null;

        public void Güncelle_Ölçüm(Ölçüm_ ÖlçümNesnesi = null, string SinyalAdı = "")
        {
            if (SinyalAdı.Contains('[') && SinyalAdı.Contains(']')) Tür = Tür_.Sinyal;
            else
            {
                Tür = Tür_.Değişken;
                Değeri.Kaydedilsin = false;
            }

            if (ÖlçümNesnesi == null)
            {
                Adı.Grup = "";
                Adı.Dal = SinyalAdı;
                Adı.Uzun = SinyalAdı;
                Adı.Csv = (Tür == Tür_.Değişken ? "Değişkenler||" : "") + SinyalAdı;
            }
            else
            {
                Ölçüm = ÖlçümNesnesi;

                Adı.Grup = Ölçüm.Adı + " " + Ölçüm.Birimi;
                
                Adı.Dal = SinyalAdı.Trim('<','>', ' ');
                int k = Adı.Dal.IndexOf('[');
                if (k >= 0) Adı.Dal = Adı.Dal.Remove(k);

                Adı.Uzun = Ölçüm.Adı + " " + Adı.Dal;
                Adı.Csv = Ölçüm.Adı + "|" + Ölçüm.Birimi + "|" + Adı.Dal;
            }
        }
        public void Güncelle_SonDeğer(double Girdi)
        {
            Güncelle_SonDeğer(S.Sayı.Yazıya(Girdi));
        }
        public void Güncelle_SonDeğer(string Girdi)
        {
            double değer;
            if (Ölçüm != null)
            {
                string işlem = Ölçüm.İşlem.Replace("<Sinyal>", Girdi);
                değer = Çevirici.Yazıdan_NoktalıSayıya(işlem);
            }
            else değer = S.Sayı.Yazıdan(Girdi);

            Değeri.SonDeğeri = değer;
            Değeri.SonDeğerinAlındığıAn = DateTime.Now;
            Değeri.ZamanAşımıOldu = false;
        }
        public double Güncelle_Dizi()
        {
            if (Tür == Tür_.Sinyal) 
            {
                TimeSpan fark = DateTime.Now - Değeri.SonDeğerinAlındığıAn;
                if (fark.TotalSeconds > Çevirici.Yazıdan_NoktalıSayıya(S.BilgiToplama_ZamanAşımı_Sn)) Değeri.ZamanAşımıOldu = true;
            }

            if (Değeri.DeğerEkseni == null) Değeri.DeğerEkseni = Enumerable.Repeat(Değeri.SonDeğeri, S.CanliÇizdirme_ÖlçümSayısı).ToArray();

            Array.Copy(Değeri.DeğerEkseni, 1, Değeri.DeğerEkseni, 0, Değeri.DeğerEkseni.Length - 1);
            Değeri.DeğerEkseni[Değeri.DeğerEkseni.Length - 1] = Değeri.SonDeğeri;

            return Değeri.SonDeğeri;
        }
    };
 
    public class Sinyaller
    {
        static Mutex Mtx = new Mutex();
        public static Dictionary<string, Sinyal_> Tümü = new Dictionary<string, Sinyal_>();
        public static bool UygunMu(string Adı)
        {
            return Adı.StartsWith("<") &&
                    Adı.EndsWith(">") &&
                    Adı.Length > 2;
        }
        public static bool MevcutMu(string Adı)
        {
            return Tümü.ContainsKey(Adı);
        }
        public static Sinyal_ Ekle(string Adı)
        {
            if (!UygunMu(Adı)) throw new Exception(Adı + " sinyal adı olarak uygun değil");

            Adı = Adı.Trim(' ');
            if (MevcutMu(Adı)) return Bul(Adı);

            Sinyal_ yeni = new Sinyal_();
            yeni.Güncelle_Ölçüm(null, Adı);

            Mtx.WaitOne();
            Tümü[Adı] = yeni;
            Mtx.ReleaseMutex();

            return yeni;
        }
        public static Sinyal_ Bul(string Adı)
        {
            Adı = Adı.Trim(' ');

            //Mtx.WaitOne();
            Sinyal_ girdi;
            bool sonuç = Tümü.TryGetValue(Adı, out girdi);
            //Mtx.ReleaseMutex();

            if (sonuç) return girdi;

            throw new Exception(Adı + " isimli sinyal listede bulunamadı");
        }
        public static void Yaz(string Adı, double Değeri)
        {
            Ekle(Adı).Güncelle_SonDeğer(Değeri);
        }
        public static double Oku(string Adı)
        {
            return Bul(Adı).Değeri.SonDeğeri;
        }
    }
    #endregion

    #region Bağlantılar
    public class Bağlantı_
    {
        public string Adı = "";
        public string Yöntem = "";
        public string P1 = "";
        public string P2 = "";
        public bool Kaydedilsin = true;
        public List<string> SonGelenBilgiler = new List<string>();
        public TreeNode Dal = null;
        public void Başlat()
        {
            if (Yöntem == "Komut Satırı")
            {
                new Thread(() => Görev_İşlemi_KomutSatırı(this)).Start();
            }
            else if (Yöntem == "Uart")
            {
                new Thread(() => Görev_İşlemi_Uart(this)).Start();
            }
        }
        public void Gönder(string Girdi)
        {
            if (Yöntem == "Komut Satırı")
            {
                if (Uygulama != null && !Uygulama.HasExited)
                {
                    Uygulama.StandardInput.WriteLine(Girdi);
                }
            }
            else if (Yöntem == "Uart")
            {
                if (Uart != null && Uart.IsOpen)
                {
                    Uart.WriteLine(Girdi);
                }
            }

            Kaydet(Girdi, "Giden");
        }

        #region Komut Satırı
        Process Uygulama = null;
        static void Görev_İşlemi_KomutSatırı(Bağlantı_ Bağlantı)
        {
            while (S.Çalışşsın)
            {
                try
                {
                    if (Bağlantı.Uygulama == null || Bağlantı.Uygulama.HasExited)
                    {
                        if (!File.Exists(Bağlantı.P1)) throw new Exception(Bağlantı.P1 + " bulunamadı");

                        Bağlantı.Uygulama = new Process();
                        Bağlantı.Uygulama.StartInfo.FileName = Bağlantı.P1;
                        Bağlantı.Uygulama.StartInfo.Arguments = Bağlantı.P2;

                        Bağlantı.Uygulama.StartInfo.CreateNoWindow = true;
                        Bağlantı.Uygulama.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                        Bağlantı.Uygulama.StartInfo.UseShellExecute = false;

                        Bağlantı.Uygulama.StartInfo.RedirectStandardError = true;
                        Bağlantı.Uygulama.StartInfo.RedirectStandardOutput = true;
                        Bağlantı.Uygulama.StartInfo.RedirectStandardInput= true;
                        Bağlantı.Uygulama.ErrorDataReceived += Bağlantı.Uygulama_OutputDataReceived;
                        // Bağlantı.Uygulama.Exited += Uygulama_Exited;
                        Bağlantı.Uygulama.OutputDataReceived += Bağlantı.Uygulama_OutputDataReceived;
                        Bağlantı.Uygulama.EnableRaisingEvents = true;

                        Bağlantı.Uygulama.Start();
                        Bağlantı.Uygulama.BeginOutputReadLine();
                        Bağlantı.Uygulama.BeginErrorReadLine();
                    }

                    int sn = (int)Çevirici.Yazıdan_NoktalıSayıya(S.BilgiToplama_ZamanAşımı_Sn) + 1;
                    while (S.Çalışşsın && (sn-- > 0)) Thread.Sleep(1000);
                }
                catch (Exception ex) { Günlük.Ekle(ex.ToString()); Bağlantı.Kaydet(ex.Message, "Uyarı"); Thread.Sleep(1000); }
            }

            try { Bağlantı.Uygulama.Kill(); }
            catch (Exception) { }
        }
        private void Uygulama_OutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            try
            {         
                if (!string.IsNullOrEmpty(e.Data)) GelenBilgiler.Ekle(Adı, e.Data);

                Kaydet(e.Data);
            }
            catch (Exception ex) { Günlük.Ekle(ex.ToString()); Kaydet(ex.Message, "Uyarı"); }
        }
        #endregion

        #region Uart
        SerialPort Uart = null;
        static void Görev_İşlemi_Uart(Bağlantı_ Bağlantı)
        {
            while (S.Çalışşsın)
            {
                try
                {
                    if (Bağlantı.Uart == null || !Bağlantı.Uart.IsOpen)
                    {
                        Bağlantı.Uart = new SerialPort(Bağlantı.P1, Convert.ToInt32(Bağlantı.P2), Parity.None, 8, StopBits.One);
                        Bağlantı.Uart.ReadTimeout = (int)Çevirici.Yazıdan_NoktalıSayıya(S.BilgiToplama_ZamanAşımı_Sn) * 1000;
                        Bağlantı.Uart.Open();
                    }

                    int sayac = 0;
                    while (S.Çalışşsın && Bağlantı.Uart.IsOpen)
                    {
                        string gelen = Bağlantı.Uart.ReadLine();
                        if (!string.IsNullOrEmpty(gelen)) GelenBilgiler.Ekle(Bağlantı.Adı, gelen);

                        Bağlantı.Kaydet(gelen);
                        if (sayac++ > 10) { sayac = 0; Thread.Sleep(1); } //cpu yüzdesini düşürmek için
                    }
                }
                catch (TimeoutException) { Thread.Sleep(1); } //cpu yüzdesini düşürmek için
                catch (Exception ex) { Günlük.Ekle(ex.ToString()); Bağlantı.Kaydet(ex.Message, "Uyarı"); Thread.Sleep(1000); }
            }

            try
            {
                Bağlantı.Uart.Close();
                Thread.Sleep(1000);
                Bağlantı.Uart.Dispose();
            }
            catch (Exception) { }
        }
        #endregion

        #region Dosyaya Kaydedici
        string Kaydet_DosyaAdı = "";
        int Kaydet_DosyaBoyutu = 0;
        int Kaydet_HedefDosyaBoyutu = 1000000;
        Mutex mtx_kaydet = new Mutex();
        /// <param name="Tür">Gelen veya Giden veya Uyarı</param>
        void Kaydet(string Girdi, string Tür = "Gelen")
        {
            Girdi = Girdi.Trim('\r', '\n');

            if (SonGelenBilgiler.Count < 10) SonGelenBilgiler.Add(Tür + S.BilgiToplama_KelimeAyracı + Girdi);

            if (Kaydedilsin && !string.IsNullOrEmpty(S.Dosyalama_KayıtKlasörü))
            {
                mtx_kaydet.WaitOne();

                if (Kaydet_DosyaAdı == "")
                {
                    Kaydet_DosyaAdı = S.Dosyalama_KayıtKlasörü + S.DosyaKlasörAdınıDüzelt(Adı);
                    Directory.CreateDirectory(Kaydet_DosyaAdı);
                    Kaydet_DosyaAdı += "\\" + S.Tarih.Yazıya(DateTime.Now, "dd_MM_yyyy_HH_mm_ss") + ".mup";
                    Kaydet_DosyaBoyutu = 0;
                    Kaydet_HedefDosyaBoyutu = (int)Çevirici.Yazıdan_NoktalıSayıya(S.Dosyalama_AzamiDosyaBoyutu_Bayt);
                }

                string yazı = S.Tarih.Yazıya(DateTime.Now) + S.BilgiToplama_KelimeAyracı + Tür + S.BilgiToplama_KelimeAyracı + Girdi + Environment.NewLine;
                File.AppendAllText(Kaydet_DosyaAdı, yazı);

                Kaydet_DosyaBoyutu += yazı.Length;
                if (Kaydet_DosyaBoyutu > Kaydet_HedefDosyaBoyutu) Kaydet_DosyaAdı = "";

                mtx_kaydet.ReleaseMutex();
            }
        }
        #endregion
    };
    public class Bağlantılar
    {
        //static Mutex Mtx = new Mutex();
        public static Dictionary<string, Bağlantı_> Tümü = new Dictionary<string, Bağlantı_>();
        public static bool UygunMu(string Adı)
        {
            return Adı.Trim().Length > 1;
        }
        public static bool MevcutMu(string Adı)
        {
            return Tümü.ContainsKey(Adı);
        }
        public static Bağlantı_ Ekle(string Adı)
        {
            if (!UygunMu(Adı)) throw new Exception(Adı + " bağlantı adı olarak uygun değil");

            Adı = Adı.Trim(' ');
            if (MevcutMu(Adı)) return Bul(Adı);

            Bağlantı_ yeni = new Bağlantı_();
            yeni.Adı = Adı;

            //Mtx.WaitOne();
            Tümü[Adı] = yeni;
            //Mtx.ReleaseMutex();

            return yeni;
        }
        public static Bağlantı_ Bul(string Adı)
        {
            Adı = Adı.Trim(' ');

            //Mtx.WaitOne();
            Bağlantı_ girdi;
            bool sonuç = Tümü.TryGetValue(Adı, out girdi);
            //Mtx.ReleaseMutex();

            if (sonuç) return girdi;

            throw new Exception(Adı + " isimli bağlantı listede bulunamadı");
        }
        public static void Gönder(string Adı, string Mesaj)
        {
            Bul(Adı).Gönder(Mesaj);
        }
    }
    #endregion

    #region Gelen Bilgi
    public class GelenBilgi_
    {
        public string BağlantıAdı;
        public string Bilgi;
    }
    public class GelenBilgiler
    {
        public static void Ekle(string BağlantıAdı, string Bilgi)
        {
            if (Görev_Nesnesi == null)
            {
                Görev_Nesnesi = new Thread(() => Görev_İşlemi());
                Görev_Nesnesi.Start();
            }

            if (string.IsNullOrEmpty(BağlantıAdı) || string.IsNullOrEmpty(Bilgi)) return;

            GelenBilgi_ yeni = new GelenBilgi_();
            yeni.BağlantıAdı = BağlantıAdı;
            yeni.Bilgi = Bilgi;

            Mtx.WaitOne();
            Tümü.Add(yeni);
            Mtx.ReleaseMutex();
        }

        static Mutex Mtx = new Mutex();
        static public List<GelenBilgi_> Tümü = new List<GelenBilgi_>();

        static Thread Görev_Nesnesi = null;
        static void Görev_İşlemi()
        {
            while (S.Çalışşsın)
            {
                try
                {
                    if (Tümü.Count == 0) { Thread.Sleep(1000); continue; }
                    else if (Tümü.Count < 100) Thread.Sleep(10);
                    else Thread.Sleep(1);

                    Mtx.WaitOne();
                    GelenBilgi_ sıradaki = Tümü[0];
                    Tümü.RemoveAt(0);
                    Mtx.ReleaseMutex();

                    int başlangıç = sıradaki.Bilgi.IndexOf(S.BilgiToplama_SinyallerCümleBaşlangıcı + S.BilgiToplama_KelimeAyracı);
                    if (başlangıç >= 0)
                    {
                        string gelen = sıradaki.Bilgi.Substring(başlangıç).Trim(' ', S.BilgiToplama_KelimeAyracı);
                        string[] dizi = gelen.Split(S.BilgiToplama_KelimeAyracı);
                        for (int i = 2; i < dizi.Length; i++)
                        {
                            string sinyal_yazı = "<" + dizi[1] + "[" + (i - 2).ToString() + "]>";
                            Sinyaller.Ekle(sinyal_yazı).Güncelle_SonDeğer(dizi[i]);
                        }
                    }
                }
                catch (Exception ex) { Günlük.Ekle(ex.ToString()); Thread.Sleep(1000); }
            }
        }
    }
    #endregion

    #region Kaydedici
    public class Kayıt_
    {
        public string mesaj;
        public double[] sinyaller;
    }
    public class Kaydedici
    {
        public static void Ekle(double[] Sinyaller, string Mesaj = "")
        {
            if (Görev_Nesnesi == null)
            {
                if (string.IsNullOrEmpty(S.Dosyalama_KayıtKlasörü)) return;

                Görev_Nesnesi = new Thread(() => Görev_İşlemi());
                Görev_Nesnesi.Start();
            }

            if (Sinyaller.Length < 1) return;

            Kayıt_ yeni = new Kayıt_();
            yeni.sinyaller = Sinyaller;
            yeni.mesaj = Mesaj;

            Mtx.WaitOne();
            Tümü.Add(yeni);
            Mtx.ReleaseMutex();
        }

        static Mutex Mtx = new Mutex();
        static public List<Kayıt_> Tümü = new List<Kayıt_>();

        static string DosyaBütünlüğüKodu = "";
        static Thread Görev_Nesnesi = null;
        static void Görev_İşlemi()
        {
            DateTime EnSonKayıtAnı = DateTime.Now;
            string DosyaYolu = "";
            int BaşlıkSayısı = 0;

            while (S.Çalışşsın)
            {
                try
                {
                    if (Tümü.Count == 0)
                    {
                        //kayıt yok
                        EnSonKayıtAnı = DateTime.Now;
                        Thread.Sleep(1500);
                        continue;
                    }
                    else if ((DateTime.Now - EnSonKayıtAnı).TotalSeconds < 60 && Tümü.Count < 350)
                    {
                        //şimdilik bekle
                        Thread.Sleep(1500);
                        continue;
                    }

                    if (!File.Exists(DosyaYolu))
                    {
                        DosyaYolu = S.Dosyalama_KayıtKlasörü + S.Tarih.Yazıya(DateTime.Now, "dd_MM_yyyy_HH_mm_ss") + ".csv";
                        BaşlıkSayısı = 0;
                        DosyaBütünlüğüKodu = "";
                    }

                    int işlenen = 0;
                    using (FileStream fs = File.OpenWrite(DosyaYolu))
                    {
                        fs.Position = fs.Length;

                        if (BaşlıkSayısı != Sinyaller.Tümü.Count)
                        {
                            string yazı = S.Tarih.Yazıya(DateTime.Now) + ";Başlıklar";
                            foreach (var biri in Sinyaller.Tümü)
                            {
                                if (biri.Value.Değeri.Kaydedilsin)
                                {
                                    yazı += ";" + biri.Value.Adı.Csv;
                                }
                            }
                            yazı += Environment.NewLine;
                            Ekle(fs, yazı);

                            BaşlıkSayısı = Sinyaller.Tümü.Count;
                        }

                        while (işlenen < Tümü.Count && S.Çalışşsın)
                        {
                            Kayıt_ sıradaki = Tümü[işlenen];
                            string yazı = S.Tarih.Yazıya(sıradaki.sinyaller[sıradaki.sinyaller.Length - 1]); // son eleman tarihsaat

                            if (string.IsNullOrEmpty(sıradaki.mesaj))
                            {
                                //sinyaller 
                                yazı += ";Sinyaller";
                                for (int i = 0; i < sıradaki.sinyaller.Length - 1; i++)
                                {
                                    if (Sinyaller.Tümü.ElementAt(i).Value.Değeri.Kaydedilsin)
                                    {
                                        yazı += ";" + S.Sayı.Yazıya(sıradaki.sinyaller[i]);
                                    }
                                }
                            }
                            else
                            {
                                //uyarı
                                yazı += ";Uyarı;" + sıradaki.mesaj;
                            }
                            yazı += Environment.NewLine;
                            Ekle(fs, yazı);
                            
                            işlenen++;
                            Thread.Sleep(1);
                        }

                        double hedefdosyaboyutu = Çevirici.Yazıdan_NoktalıSayıya(S.Dosyalama_AzamiDosyaBoyutu_Bayt);
                        if (fs.Length > hedefdosyaboyutu || !S.Çalışşsın)
                        {
                            if (S.Çalışşsın)
                            {
                                DosyaYolu = "";

                                string yazı = S.Tarih.Yazıya(DateTime.Now) + ";Doğrulama;" + DosyaBütünlüğüKodu;
                                Ekle(fs, yazı);
                            }
                        }
                    }

                    if (işlenen > 0)
                    {
                        Mtx.WaitOne();
                        Tümü.RemoveRange(0, işlenen);
                        Mtx.ReleaseMutex();
                    }
                }
                catch (Exception ex) { Günlük.Ekle(ex.ToString()); Thread.Sleep(1000); }
            }

            if (Tümü.Count > 0)
            {
                try
                {
                    if (!File.Exists(DosyaYolu))
                    {
                        DosyaYolu = S.Dosyalama_KayıtKlasörü + S.Tarih.Yazıya(DateTime.Now, "dd_MM_yyyy_HH_mm_ss") + ".csv";
                        BaşlıkSayısı = 0;
                        DosyaBütünlüğüKodu = "";
                    }

                    int işlenen = 0;
                    string yazı = "";
                    using (FileStream fs = File.OpenWrite(DosyaYolu))
                    {
                        fs.Position = fs.Length;

                        if (BaşlıkSayısı != Sinyaller.Tümü.Count)
                        {
                            yazı = S.Tarih.Yazıya(DateTime.Now) + ";Başlıklar";
                            foreach (var biri in Sinyaller.Tümü)
                            {
                                if (biri.Value.Değeri.Kaydedilsin)
                                {
                                    yazı += ";" + biri.Value.Adı.Csv;
                                }
                            }
                            yazı += Environment.NewLine;
                            Ekle(fs, yazı);
                        }

                        while (işlenen < Tümü.Count)
                        {
                            Kayıt_ sıradaki = Tümü[işlenen];
                            yazı = S.Tarih.Yazıya(sıradaki.sinyaller[sıradaki.sinyaller.Length - 1]); // son eleman tarihsaat

                            if (string.IsNullOrEmpty(sıradaki.mesaj))
                            {
                                //sinyaller 
                                yazı += ";Sinyaller";
                                for (int i = 0; i < sıradaki.sinyaller.Length - 1; i++)
                                {
                                    if (Sinyaller.Tümü.ElementAt(i).Value.Değeri.Kaydedilsin)
                                    {
                                        yazı += ";" + S.Sayı.Yazıya(sıradaki.sinyaller[i]);
                                    }
                                }
                            }
                            else
                            {
                                //uyarı
                                yazı += ";Uyarı;" + sıradaki.mesaj;
                            }
                            yazı += Environment.NewLine;
                            Ekle(fs, yazı);

                            işlenen++;
                        }

                        yazı = S.Tarih.Yazıya(DateTime.Now) + ";Doğrulama;" + DosyaBütünlüğüKodu;
                        Ekle(fs, yazı);
                    }
                }
                catch (Exception) {  }
            }
        }
        static void Ekle(FileStream fs, string yazı)
        {
            byte[] dizi = Encoding.UTF8.GetBytes(yazı);
            fs.Write(dizi, 0, dizi.Length);

            DosyaBütünlüğüKodu = ArgeMup.HazirKod.Dönüştürme.D_GeriDönülemezKarmaşıklaştırmaMetodu.Metinden(DosyaBütünlüğüKodu + yazı);
        }
    }
    #endregion

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
            while (S.Çalışşsın)
            {
                try
                {
                    if (Tümü.Count == 0)
                    {
                        Thread.Sleep(1500);
                        continue;
                    }
                    else Thread.Sleep(1);

                    Mtx.WaitOne();
                    Günlük_ yeni = Tümü[0];
                    Tümü.RemoveAt(0);
                    Mtx.ReleaseMutex();

                    string yazı = yeni.mesaj.Trim(' ', '\r', '\n');
                    File.AppendAllText(S.Kulanıcı_Klasörü + "Gunluk.csv", yeni.Tür + ";" + S.Tarih.Yazıya(yeni.Zaman) + ";" + yazı.Replace(Environment.NewLine, ";") + Environment.NewLine);

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

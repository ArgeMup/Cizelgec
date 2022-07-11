// Copyright ArgeMup GNU GENERAL PUBLIC LICENSE Version 3 <http://www.gnu.org/licenses/> <https://github.com/ArgeMup>

using ArgeMup.HazirKod.DonanımHaberleşmesi;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace Çizelgeç
{
    #region Bağlantılar
    public enum Bağlantı_Türü_ { Boşta, SeriPort, KomutSatırı, Tcpİstemci, TcpSunucu, Udpİstemci, UdpSunucu };

    public class Bağlantı_
    {
        public string Adı = "";
        public Bağlantı_Türü_ Türü = Bağlantı_Türü_.Boşta;
        public string GöbekAdı = null;
        public string CümleBaşlangıcı = ">Sinyaller";
        public char KelimeAyracı = ';';
        public string P1 = "";
        public string P2 = "";
        public bool Kaydedilsin = true;
        public enum TanımlanmamışSinyalleri_ { Kullan, Kaydetme, Atla }; public TanımlanmamışSinyalleri_ TanımlanmamışSinyalleri = TanımlanmamışSinyalleri_.Kullan;
        public List<string> SonGelenBilgiler = new List<string>();
        public TreeNode Dal = null;
        
        public bool DaldakiYazıyıGüncelleVeKabart = true;
        public DateTime SonDeğerinAlındığıAn = DateTime.Now;
        public UInt64 Sayac_Güncelleme = 0;

        #region Bağlantının İçeriğinin Dosyaya Kaydedilmesi
        public string Kaydet_DosyaAdı = "";
        public int Kaydet_DosyaBoyutu = 0;
        public int Kaydet_HedefDosyaBoyutu = 5 * 1024 * 1024;
        #endregion

        public IDonanımHaberlleşmesi Haberleşme = null;

        public GelenBilgiler_ GelenBilgiler = new GelenBilgiler_();
    };
    public class Bağlantılar
    {
        //static Mutex Mtx = new Mutex();
        public static Dictionary<string, Bağlantı_> Tümü = new Dictionary<string, Bağlantı_>();
        public static bool UygunMu(string Adı)
        {
            return Adı.Trim().Length > 0;
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
        public static void Başlat(string Adı)
        {
            Bağlantı_ aranan = Bul(Adı);

            switch (aranan.Türü)
            {
                case Bağlantı_Türü_.KomutSatırı:
                    if (!File.Exists(aranan.P1))
                    {
                        string[] lst = Directory.GetFiles(S.AnaKlasör, aranan.P1, SearchOption.AllDirectories);
                        if (lst.Length == 0)
                        {
                            lst = Directory.GetFiles(S.Dosyalama_KayıtKlasörü, aranan.P1, SearchOption.AllDirectories);
                            if (lst.Length == 0) throw new Exception(aranan.P1 + " bulunamadı");
                        }

                        aranan.P1 = lst[0];
                        Günlük.Ekle(aranan.Adı + " bağlantısının P1 parametresi değiştirildi -> " + aranan.P1, "Bilgi");
                    }

                    aranan.Haberleşme = new KomutSatırıUygulaması_(aranan.P1, aranan.P2, BilgiGeldiVeyaDurumDeğişikliği, aranan);
                    break;

                case Bağlantı_Türü_.SeriPort:
                    aranan.Haberleşme = new SeriPort_(aranan.P1, int.Parse(aranan.P2), BilgiGeldiVeyaDurumDeğişikliği, aranan);
                    break;

                case Bağlantı_Türü_.Tcpİstemci:
                    aranan.Haberleşme = new Tcpİstemci_(int.Parse(aranan.P2), aranan.P1, BilgiGeldiVeyaDurumDeğişikliği, aranan);
                    break;

                case Bağlantı_Türü_.TcpSunucu:
                    aranan.Haberleşme = new TcpSunucu_(int.Parse(aranan.P1), BilgiGeldiVeyaDurumDeğişikliği, aranan, SadeceYerel:false);
                    break;

                case Bağlantı_Türü_.UdpSunucu:
                    aranan.Haberleşme = new UdpDinleyici_(int.Parse(aranan.P1), BilgiGeldiVeyaDurumDeğişikliği, aranan, SadeceYerel:false);
                    break;

                default:
                    break;
            }

            if (aranan.Haberleşme != null) aranan.GelenBilgiler.Başlat(aranan);
        }
        public static void Durdur()
        {
            foreach (var biri in Tümü.Values)
            {
                biri.Haberleşme.Durdur();
            }

            Tümü.Clear();
        }
        public static void Gönder(string Adı, string Mesaj)
        {
            Bağlantı_ kendi = Bul(Adı);
            kendi.Haberleşme.Gönder(Mesaj, kendi.Türü == Bağlantı_Türü_.Udpİstemci ? kendi.P1 + ":" + kendi.P2 : null);

            kendi.GelenBilgiler.Ekle(Mesaj, "Giden");
        }

        static void BilgiGeldiVeyaDurumDeğişikliği(string Kaynak, GeriBildirim_Türü_ Tür, object İçerik, object Hatırlatıcı)
        {
            Bağlantı_ kendi = (Bağlantı_)Hatırlatıcı;

            if (Tür == GeriBildirim_Türü_.BilgiGeldi) kendi.GelenBilgiler.Ekle((string)İçerik);
            else if (Tür == GeriBildirim_Türü_.BağlantıKurulmasıTekrarDenecek) return;
            else
            {
                Günlük.Ekle(kendi.Adı + " " + Tür.ToString() + " (Eğer sürekli olarak tekrar ediyorsa ayarlar hatalı olabilir)");
                kendi.GelenBilgiler.Ekle(kendi.Adı + " " + Kaynak + " " + Tür.ToString(), "Uyarı");
            }
        }
    }
    #endregion

    #region Bağlantılardan Gelen Bilgilerin Ayıklanması, Kaydedilmesi ve Depolanması
    public class GelenBilgi_
    {
        public string Bilgi;

        public DateTime Zaman;
        public string Tür;
    }
    public class GelenBilgiler_
    {
        Bağlantı_ Bağlantı;
        public void Başlat(Bağlantı_ Bağlantı)
        {
            this.Bağlantı = Bağlantı;

            if (string.IsNullOrEmpty(S.Dosyalama_KayıtKlasörü)) Bağlantı.Kaydedilsin = false;

            if (Bağlantı.Kaydedilsin)
            {
                Görev_Nesnesi_Kaydetme = new Thread(() => Görev_İşlemi_Kaydetme());
                Görev_Nesnesi_Kaydetme.Start();
            }

            Görev_Nesnesi_Ayıklama = new Thread(() => Görev_İşlemi_Ayıklama());
            Görev_Nesnesi_Ayıklama.Start();
        }

        /// <param name="Tür">Gelen bilgi için null olmalı</param>
        public void Ekle(string Bilgi, string Tür = null)
        {
            if (Bilgi == null) return;

            GelenBilgi_ yeni = new GelenBilgi_();
            yeni.Bilgi = Bilgi;

            if (Tür == null && Bilgi != "")
            {
                Mtx_Ayıklama.WaitOne();
                Tümü_Ayıklama.Add(yeni);
                Mtx_Ayıklama.ReleaseMutex();
            }

            if (string.IsNullOrEmpty(Tür)) Tür = "Gelen";

            if (Bağlantı.Kaydedilsin)
            {
                yeni.Tür = Tür;
                yeni.Zaman = DateTime.Now;

                Mtx_Kaydetme.WaitOne();
                Tümü_Kaydetme.Add(yeni);
                Mtx_Kaydetme.ReleaseMutex();
            }

            if (Bağlantı.SonGelenBilgiler.Count < 10) Bağlantı.SonGelenBilgiler.Add(Tür + Bağlantı.KelimeAyracı + Bilgi.Trim('\r', '\n'));

            Bağlantı.DaldakiYazıyıGüncelleVeKabart = true;
            Bağlantı.SonDeğerinAlındığıAn = DateTime.Now;
            Bağlantı.Sayac_Güncelleme++;
        }

        #region Ayıklama
        Mutex Mtx_Ayıklama = new Mutex();
        public List<GelenBilgi_> Tümü_Ayıklama = new List<GelenBilgi_>();
        Thread Görev_Nesnesi_Ayıklama = null;
        void Görev_İşlemi_Ayıklama()
        {
            while (S.Çalışşsın)
            {
                try
                {
                    if (Tümü_Ayıklama.Count == 0) { Thread.Sleep(1000); continue; }
                    else if (Tümü_Ayıklama.Count < 100) Thread.Sleep(1); //cpu yüzdesini düşürmek için

                    Mtx_Ayıklama.WaitOne();
                    GelenBilgi_ sıradaki = Tümü_Ayıklama[0];
                    if (S.BaşlatDurdur)
                    {
                        if (Tümü_Ayıklama.Count > 100000)
                        {
                            Tümü_Ayıklama.RemoveRange(0, 90000);

                            string mesaj = Bağlantı.Adı + " bağlantısı çok fazla ayıklanamamış girdiye sahip oldugundan ilk 90000 girdi silindi.";
                            Kaydedici.Ekle(new double[1] { S.Tarih.Sayıya(DateTime.Now) }, mesaj);
                            Günlük.Ekle(mesaj);
                        }
                        else Tümü_Ayıklama.RemoveAt(0);
                    }
                    else Tümü_Ayıklama.Clear();
                    Mtx_Ayıklama.ReleaseMutex();

                    int başlangıç = sıradaki.Bilgi.LastIndexOf(Bağlantı.CümleBaşlangıcı);
                    if (başlangıç >= 0)
                    {
                        string gelen = sıradaki.Bilgi.Substring(başlangıç + Bağlantı.CümleBaşlangıcı.Length).Trim(' ', Bağlantı.KelimeAyracı);
                        string[] dizi = gelen.Split(Bağlantı.KelimeAyracı);
                        for (int i = 1; i < dizi.Length; i++)
                        {
                            string sinyal_yazı;
                            if (string.IsNullOrEmpty(Bağlantı.GöbekAdı)) sinyal_yazı = "<" + dizi[0] + "[" + (i - 1).ToString() + "]>";
                            else sinyal_yazı = "<" + Bağlantı.GöbekAdı + dizi[0] + "[" + (i - 1).ToString() + "]>";

                            bool TanımlanmamışVeKaydetme = false;
                            if (Bağlantı.TanımlanmamışSinyalleri != Bağlantı_.TanımlanmamışSinyalleri_.Kullan)
                            {
                                if (!Sinyaller.MevcutMu(sinyal_yazı))
                                {
                                    if (Bağlantı.TanımlanmamışSinyalleri == Bağlantı_.TanımlanmamışSinyalleri_.Atla) continue;
                                    else TanımlanmamışVeKaydetme = true;
                                }
                            }

                            try 
                            {
                                Sinyal_ sny = Sinyaller.Ekle(sinyal_yazı);
                                sny.Güncelle_SonDeğer(dizi[i]);
                                if (TanımlanmamışVeKaydetme) sny.Değeri.Kaydedilsin = false;
                            }
                            catch (Exception) { Günlük.Ekle(Bağlantı.Adı + " bağlantısının " + (i + 1) + ". elemanı -> " + dizi[i] + "  <- sayıya dönüştürülemedi."); }
                        }
                    }
                }
                catch (Exception ex) { Günlük.Ekle(ex.ToString()); Thread.Sleep(1000); }
            }
        }
        #endregion

        #region Kaydetme
        Mutex Mtx_Kaydetme = new Mutex();
        public List<GelenBilgi_> Tümü_Kaydetme = new List<GelenBilgi_>();
        Thread Görev_Nesnesi_Kaydetme = null;
        void Görev_İşlemi_Kaydetme()
        {
            DateTime EnSonKayıtAnı = DateTime.Now;
    
            while (S.Çalışşsın)
            {
                try
                {
                    if (Tümü_Kaydetme.Count == 0)
                    {
                        //kayıt yok
                        EnSonKayıtAnı = DateTime.Now;
                        Thread.Sleep(1500);
                        continue;
                    }
                    else if ((DateTime.Now - EnSonKayıtAnı).TotalSeconds < 60 && Tümü_Kaydetme.Count < 350)
                    {
                        //şimdilik bekle
                        Thread.Sleep(1500);
                        continue;
                    }
                    EnSonKayıtAnı = DateTime.Now;

                    while (Tümü_Kaydetme.Count > 15 && S.Çalışşsın)
                    {
                        Mtx_Kaydetme.WaitOne();
                        GelenBilgi_ sıradaki = Tümü_Kaydetme[0];
                        if (S.BaşlatDurdur)
                        {
                            if (Tümü_Kaydetme.Count > 100000)
                            {
                                Tümü_Kaydetme.RemoveRange(0, 90000);

                                string mesaj = Bağlantı.Adı + " bağlantısı çok fazla kaydedilememiş girdiye sahip oldugundan ilk 90000 girdi silindi.";
                                Görev_İşlemi_DosyayaKaydet(mesaj);
                                Günlük.Ekle(mesaj);
                            }
                            else Tümü_Kaydetme.RemoveAt(0);
                        }
                        else Tümü_Kaydetme.Clear();
                        Mtx_Kaydetme.ReleaseMutex();

                        Görev_İşlemi_DosyayaKaydet(S.Tarih.Yazıya(sıradaki.Zaman) + Bağlantı.KelimeAyracı + sıradaki.Tür + Bağlantı.KelimeAyracı + sıradaki.Bilgi.Trim('\r', '\n') + Environment.NewLine);

                        Thread.Sleep(1); //cpu yüzdesini düşürmek için
                    }
                }
                catch (Exception ex) { Günlük.Ekle(ex.ToString()); Thread.Sleep(1000); }
            }

            //çıkış öncesi elde kalan bilgilerin kaydedilmesi
            try
            {
                if (Tümü_Kaydetme.Count > 10000) Tümü_Kaydetme.RemoveRange(0, Tümü_Kaydetme.Count - 10000);

                while (Tümü_Kaydetme.Count > 0)
                {
                    GelenBilgi_ sıradaki = Tümü_Kaydetme[0];
                    if (S.BaşlatDurdur) Tümü_Kaydetme.RemoveAt(0);
                    else Tümü_Kaydetme.Clear();

                    Görev_İşlemi_DosyayaKaydet(S.Tarih.Yazıya(sıradaki.Zaman) + Bağlantı.KelimeAyracı + sıradaki.Tür + Bağlantı.KelimeAyracı + sıradaki.Bilgi.Trim('\r', '\n') + Environment.NewLine);
                }
            }
            catch (Exception) { }
        }
        void Görev_İşlemi_DosyayaKaydet(string Mesaj)
        {
            if (!File.Exists(Bağlantı.Kaydet_DosyaAdı))
            {
                Bağlantı.Kaydet_DosyaAdı = S.Dosyalama_KayıtKlasörü + "Bağlantılar\\" + S.DosyaKlasörAdınıDüzelt(Bağlantı.Adı);
                Directory.CreateDirectory(Bağlantı.Kaydet_DosyaAdı);

                Bağlantı.Kaydet_DosyaAdı += "\\" + S.Tarih.Yazıya(DateTime.Now, S.Tarih._Şablon_dosyaadı) + ".mup";
                Bağlantı.Kaydet_DosyaBoyutu = 0;
                Bağlantı.Kaydet_HedefDosyaBoyutu = (int)Çevirici.Yazıdan_NoktalıSayıya(S.Dosyalama_AzamiDosyaBoyutu_Bayt);
            }

            File.AppendAllText(Bağlantı.Kaydet_DosyaAdı, Mesaj);
            Bağlantı.Kaydet_DosyaBoyutu += Mesaj.Length;
            if (Bağlantı.Kaydet_DosyaBoyutu > Bağlantı.Kaydet_HedefDosyaBoyutu) Bağlantı.Kaydet_DosyaAdı = "";
        }
        #endregion
    }
    #endregion

    #region Düzenlenmiş Sinyallerin Dosyaya Kaydedilmesi
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

            double[] kopyası = new double[Sinyaller.Length];
            Array.Copy(Sinyaller, 0, kopyası, 0, Sinyaller.Length);

            Kayıt_ yeni = new Kayıt_();
            yeni.sinyaller = kopyası;
            yeni.mesaj = Mesaj;

            Mtx.WaitOne();
            Tümü.Add(yeni);
            Mtx.ReleaseMutex();
        }

        static Mutex Mtx = new Mutex();
        static public List<Kayıt_> Tümü = new List<Kayıt_>();
        static public int YeniKayıtDosyasıSayısı = 1;

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
                    else if (S.BaşlatDurdur && 
                            (DateTime.Now - EnSonKayıtAnı).TotalSeconds < 60 && 
                            Tümü.Count < 350)
                    {
                        //şimdilik bekle
                        Thread.Sleep(1500);
                        continue;
                    }
                    EnSonKayıtAnı = DateTime.Now;

                    if (!File.Exists(DosyaYolu))
                    {
                        Directory.CreateDirectory(S.Dosyalama_KayıtKlasörü);
                        DosyaYolu = S.Dosyalama_KayıtKlasörü + S.Tarih.Yazıya(DateTime.Now, S.Tarih._Şablon_dosyaadı) + ".csv";
                        BaşlıkSayısı = 0;
                        DosyaBütünlüğüKodu = "";
                    }

                    int işlenen = 0;
                    using (FileStream fs = File.OpenWrite(DosyaYolu))
                    {
                        fs.Position = fs.Length;

                        if (BaşlıkSayısı != Sinyaller.Tümü.Count)
                        {
                            Thread.Sleep(3500); //Yeni sinyallerin tüm özelliklerinin eklenemsi adımlarının tamamlanabilmesi için

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
                                if (sıradaki.sinyaller.Length < 2)
                                {
                                    işlenen++;
                                    Thread.Sleep(1);
                                    continue;
                                }

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
                        if (fs.Length > hedefdosyaboyutu || !S.BaşlatDurdur)
                        {
                            DosyaYolu = "";

                            string yazı = S.Tarih.Yazıya(DateTime.Now) + ";Bilgi;ArGeMuP Çizelgeç V" + Application.ProductVersion + Environment.NewLine;
                            Ekle(fs, yazı);
                            yazı = S.Tarih.Yazıya(DateTime.Now) + ";Bilgi;" + S.SonDurumMesajı.Replace(Environment.NewLine, " ") + Environment.NewLine;
                            Ekle(fs, yazı);
                            yazı = S.Tarih.Yazıya(DateTime.Now) + ";Bilgi;" + (S.BaşlatDurdur ? "Azami dosya boyutuna ulaşıldığı için " : "Uygulama gecici olarak durdurulduğu için ") + YeniKayıtDosyasıSayısı++ + ". dosya olarak kaydedildi" + Environment.NewLine;
                            Ekle(fs, yazı);
                            yazı = S.Tarih.Yazıya(DateTime.Now) + ";Doğrulama;" + DosyaBütünlüğüKodu;
                            Ekle(fs, yazı);
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
                        DosyaYolu = S.Dosyalama_KayıtKlasörü + S.Tarih.Yazıya(DateTime.Now, S.Tarih._Şablon_dosyaadı) + ".csv";
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
                            Thread.Sleep(350); //Yeni sinyallerin tüm özelliklerinin eklenemsi adımlarının tamamlanabilmesi için

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
                                if (sıradaki.sinyaller.Length < 2)
                                {
                                    işlenen++;
                                    continue;
                                }

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

                        yazı = S.Tarih.Yazıya(DateTime.Now) + ";Bilgi;ArGeMuP Çizelgeç V" + Application.ProductVersion + Environment.NewLine;
                        Ekle(fs, yazı);
                        yazı = S.Tarih.Yazıya(DateTime.Now) + ";Bilgi;" + S.SonDurumMesajı.Replace(Environment.NewLine, " ") + Environment.NewLine;
                        Ekle(fs, yazı);
                        yazı = S.Tarih.Yazıya(DateTime.Now) + ";Bilgi;Uygulama kapatıldığı için " + YeniKayıtDosyasıSayısı + ". dosya olarak kaydedildi" + Environment.NewLine;
                        Ekle(fs, yazı);
                        yazı = S.Tarih.Yazıya(DateTime.Now) + ";Doğrulama;" + DosyaBütünlüğüKodu;
                        Ekle(fs, yazı);
                    }
                }
                catch (Exception) {  }
            }
            else if (File.Exists(DosyaYolu) && BaşlıkSayısı > 0 && !string.IsNullOrEmpty(DosyaBütünlüğüKodu))
            {
                string yazı = S.Tarih.Yazıya(DateTime.Now) + ";Bilgi;ArGeMuP Çizelgeç V" + Application.ProductVersion + Environment.NewLine;
                Ekle(DosyaYolu, yazı);
                yazı = S.Tarih.Yazıya(DateTime.Now) + ";Bilgi;" + S.SonDurumMesajı.Replace(Environment.NewLine, " ") + Environment.NewLine;
                Ekle(DosyaYolu, yazı);
                yazı = S.Tarih.Yazıya(DateTime.Now) + ";Bilgi;Uygulama kapatıldığı için " + YeniKayıtDosyasıSayısı + ". dosya olarak kaydedildi" + Environment.NewLine;
                Ekle(DosyaYolu, yazı);
                yazı = S.Tarih.Yazıya(DateTime.Now) + ";Doğrulama;" + DosyaBütünlüğüKodu;
                Ekle(DosyaYolu, yazı);
            }
        }
        static void Ekle(FileStream fs, string yazı)
        {
            byte[] dizi = Encoding.UTF8.GetBytes(yazı);
            fs.Write(dizi, 0, dizi.Length);

            DosyaBütünlüğüKodu = ArgeMup.HazirKod.Dönüştürme.D_GeriDönülemezKarmaşıklaştırmaMetodu.Metinden(DosyaBütünlüğüKodu + yazı);
        }
        static void Ekle(string DosyaYolu, string yazı)
        {
            File.AppendAllText(DosyaYolu, yazı);

            DosyaBütünlüğüKodu = ArgeMup.HazirKod.Dönüştürme.D_GeriDönülemezKarmaşıklaştırmaMetodu.Metinden(DosyaBütünlüğüKodu + yazı);
        }
    }
    #endregion
}

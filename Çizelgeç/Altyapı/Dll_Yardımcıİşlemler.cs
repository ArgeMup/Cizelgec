// Copyright ArgeMup GNU GENERAL PUBLIC LICENSE Version 3 <http://www.gnu.org/licenses/> <https://github.com/ArgeMup>

using ArgeMup.HazirKod.Ekİşlemler;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Yardımcıİşlemler
{
    public class ÖnYüz
    {
        public static bool CanlıEkranlama
        {
            get
            {
                return Çizelgeç.S.AnaEkran.Canlı_Ekranlama != null;
            }
        }

        public static bool ÇalışmayaDevamEt { get => Çizelgeç.S.Çalışşsın; }

        public static void Güncelle()
        {
            if (Çizelgeç.S.AnaEkran.InvokeRequired)
            {
                Çizelgeç.S.AnaEkran.Invoke(new Action(() => { _ivk_(); }));
            }
            else _ivk_();

            void _ivk_()
            {
                //güncelle
                Çizelgeç.S.Çizdir();

                //kaydırma çubuğu kaynaklı görselleri güncelle
                Çizelgeç.S.AnaEkran.Kaydırıcı_Scroll(null, null);
            }
        }
        public static void İlerlemeÇubuğu(int Güncel, int Toplam)
        {
            if (Çizelgeç.S.YenidenHesapla == null || Çizelgeç.S.YenidenHesapla.Disposing || Çizelgeç.S.YenidenHesapla.IsDisposed) return;

            if (Çizelgeç.S.AnaEkran.InvokeRequired)
            {
                Çizelgeç.S.AnaEkran.Invoke(new Action(() => { _ivk_(); }));
            }
            else _ivk_();

            void _ivk_()
            {
                ProgressBar iç = Çizelgeç.S.YenidenHesapla.İlerlemeÇubuğu;
                if (iç.Maximum != Toplam)
                {
                    iç.Value = 0;
                    iç.Maximum = Toplam;
                }

                iç.Value = Güncel;
                Application.DoEvents();
            }
        }
        public static void Günlük(string Girdi)
        {
            Çizelgeç.Günlük.Ekle(Girdi);

            if (Çizelgeç.S.YenidenHesapla == null || Çizelgeç.S.YenidenHesapla.Disposing || Çizelgeç.S.YenidenHesapla.IsDisposed) return;

            if (Çizelgeç.S.AnaEkran.InvokeRequired)
            {
                Çizelgeç.S.AnaEkran.Invoke(new Action(() => { _ivk_(); }));
            }
            else _ivk_();

            void _ivk_()
            {
                Çizelgeç.S.YenidenHesapla.Notlar.AppendText(Girdi + Environment.NewLine);
            }
        }
        public static void Günlük(Exception İstisna, string ÖnYazı = null)
        {
            string hata = null;
            while (İstisna != null)
            {
                hata += İstisna.ToString() + Environment.NewLine;
                İstisna = İstisna.InnerException;
            }

            Günlük((ÖnYazı == null ? null : ÖnYazı + Environment.NewLine) + hata);
        }
        public static void SadeceŞuSinyalleriGöster(List<Çizelgeç.Sinyal_> Sinyaller)
        {
            if (Sinyaller == null || Sinyaller.Count == 0) return;

            if (Çizelgeç.S.AnaEkran.InvokeRequired)
            {
                Çizelgeç.S.AnaEkran.Invoke(new Action(() => { _ivk_(); }));
            }
            else _ivk_();

            void _ivk_()
            {
                Çizelgeç.S.Ağaç.CollapseAll();
                foreach (TreeNode t in Çizelgeç.S.Ağaç.Nodes)
                {
                    t.Checked = false;
                }

                foreach (Çizelgeç.Sinyal_ s in Sinyaller)
                {
                    s.Görseller.Dal.Checked = true;
                    _üstDallarınıDaGöster_(s.Görseller.Dal);
                }

                Çizelgeç.S.Ağaç.SelectedNode = Çizelgeç.S.Ağaç.Nodes[0];

                void _üstDallarınıDaGöster_(TreeNode _Dal_)
                {
                    while (_Dal_ != null)
                    {
                        _Dal_.Expand();
                        _Dal_ = _Dal_.Parent;
                    }
                }
            }
        }

        public static void AçıklamaEkle(string Açıklama, Color Renk = default)
        {
            if (!CanlıEkranlama) return; //Sadece canlı ekranlamadan çağırılabilir

            DateTime t = DateTime.Now;
            if (Çizelgeç.S.AnaEkran.InvokeRequired)
            {
                Çizelgeç.S.AnaEkran.Invoke(new Action(() => { Çizelgeç.Açıklamalar.Ekle(t, Açıklama, Renk); }));
            }
            else Çizelgeç.Açıklamalar.Ekle(t, Açıklama, Renk);
        }

        public static void UygulamayıKapat(bool BeşDkİçindeBilgisayarıKapat = false)
        {
            if (BeşDkİçindeBilgisayarıKapat) 
            {
                #if DEBUG
                    MessageBox.Show("BilgisayarıKapat", "Bilgisayarı kapatma durum değişikliği");
                #else
                    Çizelgeç.S.Çalıştır.UygulamayıDoğrudanÇalıştır("shutdown.exe", new string[] { "-s", "-t", "300" }, true, false);
                #endif
            }

            Application.Exit();
        }
    };

    public class Bağlantılar
    {
        public static Çizelgeç.Bağlantı_ Ekle_KomutSatırı(string BağlantıAdı, string Uygulama, string Parametre = null)
        {
            if (!ÖnYüz.CanlıEkranlama || BağlantıAdı.BoşMu(true)) return null; //Sadece canlı ekranlamadan çağırılabilir

            Çizelgeç.Bağlantı_ yeni = Çizelgeç.Bağlantılar.Ekle(BağlantıAdı);
            yeni.Türü = Çizelgeç.Bağlantı_Türü_.KomutSatırı;
            yeni.P1 = Uygulama;
            yeni.P2 = Parametre;

            return yeni;
        }
        public static Çizelgeç.Bağlantı_ Ekle_Uart(string BağlantıAdı, int BitHızı, string ErişimNoktası = "COMx")
        {
            if (!ÖnYüz.CanlıEkranlama || BağlantıAdı.BoşMu(true)) return null; //Sadece canlı ekranlamadan çağırılabilir

            Çizelgeç.Bağlantı_ yeni = Çizelgeç.Bağlantılar.Ekle(BağlantıAdı);
            yeni.Türü = Çizelgeç.Bağlantı_Türü_.SeriPort;
            yeni.P1 = ErişimNoktası;
            yeni.P2 = BitHızı.Yazıya();

            return yeni;
        }
        public static Çizelgeç.Bağlantı_ Ekle_Tcpİstemcisi(string BağlantıAdı, string IPveyaAdresi, int ErişimNoktası)
        {
            if (!ÖnYüz.CanlıEkranlama || BağlantıAdı.BoşMu(true)) return null; //Sadece canlı ekranlamadan çağırılabilir

            Çizelgeç.Bağlantı_ yeni = Çizelgeç.Bağlantılar.Ekle(BağlantıAdı);
            yeni.Türü = Çizelgeç.Bağlantı_Türü_.Tcpİstemci;
            yeni.P1 = IPveyaAdresi;
            yeni.P2 = ErişimNoktası.Yazıya();

            return yeni;
        }
        public static Çizelgeç.Bağlantı_ Ekle_Udpİstemcisi(string BağlantıAdı, string IPveyaAdresi, int ErişimNoktası)
        {
            if (!ÖnYüz.CanlıEkranlama || BağlantıAdı.BoşMu(true)) return null; //Sadece canlı ekranlamadan çağırılabilir

            Çizelgeç.Bağlantı_ yeni = Çizelgeç.Bağlantılar.Ekle(BağlantıAdı);
            yeni.Türü = Çizelgeç.Bağlantı_Türü_.Udpİstemci;
            yeni.P1 = IPveyaAdresi;
            yeni.P2 = ErişimNoktası.Yazıya();

            return yeni;
        }
        public static Çizelgeç.Bağlantı_ Ekle_TcpSunucusu(string BağlantıAdı, int ErişimNoktası)
        {
            if (!ÖnYüz.CanlıEkranlama || BağlantıAdı.BoşMu(true)) return null; //Sadece canlı ekranlamadan çağırılabilir

            Çizelgeç.Bağlantı_ yeni = Çizelgeç.Bağlantılar.Ekle(BağlantıAdı);
            yeni.Türü = Çizelgeç.Bağlantı_Türü_.TcpSunucu;
            yeni.P1 = ErişimNoktası.Yazıya();

            return yeni;
        }
        public static Çizelgeç.Bağlantı_ Ekle_UdpSunucusu(string BağlantıAdı, int ErişimNoktası)
        {
            if (!ÖnYüz.CanlıEkranlama || BağlantıAdı.BoşMu(true)) return null; //Sadece canlı ekranlamadan çağırılabilir

            Çizelgeç.Bağlantı_ yeni = Çizelgeç.Bağlantılar.Ekle(BağlantıAdı);
            yeni.Türü = Çizelgeç.Bağlantı_Türü_.UdpSunucu;
            yeni.P1 = ErişimNoktası.Yazıya();

            return yeni;
        }

        public static List<Çizelgeç.Bağlantı_> Bul(string BağlantıAdıKıstası = "*", bool BüyükKüçükHarfDuyarlı = true, char Ayraç = '*')
        {
            List<Çizelgeç.Bağlantı_> Bulunanlar = new List<Çizelgeç.Bağlantı_>();

            foreach (Çizelgeç.Bağlantı_ Bağlantı in Çizelgeç.Bağlantılar.Tümü.Values)
            {
                if (Bağlantı.Adı.BenzerMi(BağlantıAdıKıstası, BüyükKüçükHarfDuyarlı, Ayraç)) Bulunanlar.Add(Bağlantı);
            }

            return Bulunanlar;
        }
    }

    public class Sinyaller
    {
        public enum Tanımlanmamış_Sinyali { Kullan, Kaydetme, Atla };

        public static Func<bool> GeriBildirimİşlemi_Kaydedilecek = null;

        public static double[] ZamanEkseni;
        public static int ZamanEkseni_EnYakın(DateTime TarihSaat)
        {
            double Koordinat_X = Çizelgeç.S.Tarih.Sayıya(TarihSaat);

            if (ZamanEkseni == null || ZamanEkseni.Length < 2 || Koordinat_X <= 0 || double.IsNaN(Koordinat_X) || double.IsInfinity(Koordinat_X)) return -1;

            (_, _, int bulundu) = Çizelgeç.Sinyaller.Tümü.Values.First().Görseller.Çizikler.GetPointNearestX(Koordinat_X);

            return bulundu;
        }
        public static DateTime ZamanEkseni_TarihSaateÇevir(double TarihSaat)
        {
            return Çizelgeç.S.Tarih.Tarihe(TarihSaat);
        }
        public static double ZamanEkseni_TarihSaattenÇevir(DateTime TarihSaat)
        {
            return Çizelgeç.S.Tarih.Sayıya(TarihSaat);
        }

        public static Çizelgeç.Sinyal_ Ekle(string SinyalAdı, string AğaçİçindekiDalınAdı, string GörünenAdı, bool Kaydedilsin = true)
        {
            if (SinyalAdı.BoşMu(true) || Çizelgeç.Sinyaller.MevcutMu(SinyalAdı)) return null;

            Çizelgeç.Sinyal_ sinyal = Çizelgeç.Sinyaller.Ekle(SinyalAdı);
            sinyal.Güncelle_Adı(SinyalAdı, AğaçİçindekiDalınAdı, GörünenAdı);
            sinyal.Değeri.Kaydedilsin = Kaydedilsin;

            #region Birbirinin aynı csv adına sahip sinyal varmı kontrolü
            foreach (var biri in Çizelgeç.Sinyaller.Tümü)
            {
                foreach (var diğeri in Çizelgeç.Sinyaller.Tümü)
                {
                    if (biri.Equals(diğeri)) continue;

                    if (biri.Value.Adı.Csv == diğeri.Value.Adı.Csv)
                    {
                        throw new Exception("Sinyal csv adı " + diğeri.Value.Adı.Csv + " zaten eklendi");
                    }
                }
            }
            #endregion

            if (!ÖnYüz.CanlıEkranlama) Çizelgeç.Ekranlama.SinyaliAğacaEkle(sinyal);

            return sinyal;
        }
        public static List<Çizelgeç.Sinyal_> Bul(string SinyalAdıKıstası = "*", bool BüyükKüçükHarfDuyarlı = true, char Ayraç = '*')
        {
            List<Çizelgeç.Sinyal_> Bulunanlar = new List<Çizelgeç.Sinyal_>();

            foreach (Çizelgeç.Sinyal_ Sinyal in Çizelgeç.Sinyaller.Tümü.Values)
            {
                if (Sinyal.Adı.Sinyal.BenzerMi(SinyalAdıKıstası, BüyükKüçükHarfDuyarlı, Ayraç)) Bulunanlar.Add(Sinyal);
            }

            return Bulunanlar;
        }
    };

    public class İşlemler
    {
        public class İşlem_
        {
            public string Adı;
            public Action<string, object> İşlem;
            public object Hatırlatıcı;
        }

        public static bool DeğişiklikYapıldı = false;
        public static List<İşlem_> Tümü = null;

        public static void Ekle(string İşlemAdı, Action<string, object> İşlem, object Hatırlatıcı = null)
        {
            if (İşlemAdı.BoşMu(true) || İşlem == null || !ÖnYüz.CanlıEkranlama) return;  //Sadece canlı ekranlamadan çağırılabilir

            if (Tümü == null) Tümü = new List<İşlem_>();

            Tümü.Add(new İşlem_() { Adı = İşlemAdı, İşlem = İşlem, Hatırlatıcı = Hatırlatıcı });

            DeğişiklikYapıldı = true;
        }
        public static void Sil(string İşlemAdıKıstası, bool BüyükKüçükHarfDuyarlı = true, char Ayraç = '*')
        {
            if (Tümü == null) return;

            if (İşlemAdıKıstası == null) Tümü.Clear();
            else
            {
                List<İşlem_> silinecekler = new List<İşlem_>();
                foreach (İşlem_ biri in Tümü)
                {
                    if (biri.Adı.BenzerMi(İşlemAdıKıstası, BüyükKüçükHarfDuyarlı, Ayraç)) silinecekler.Add(biri);
                }
                foreach (İşlem_ biri in silinecekler)
                {
                    Tümü.Remove(biri);
                }
            }
            
            DeğişiklikYapıldı = true;
        }
    }

    public class Görevler
    {
        protected static ArgeMup.HazirKod.ArkaPlan.Hatırlatıcı_ Tümü = null;

        public static void Ekle(string TakmaAdı, DateTime İlkTetikleyeceğiZaman, Func<string, object, int> GeriBildirim_Islemi, object Hatırlatıcı = null)
        {
            if (TakmaAdı.BoşMu(true) || GeriBildirim_Islemi == null || !ÖnYüz.CanlıEkranlama) return; //Sadece canlı ekranlamadan çağırılabilir

            if (Tümü == null) Tümü = new ArgeMup.HazirKod.ArkaPlan.Hatırlatıcı_();

            Tümü.Ekle(TakmaAdı, İlkTetikleyeceğiZaman, null, GeriBildirim_Islemi, Hatırlatıcı, true);
        }
        public static List<ArgeMup.HazirKod.ArkaPlan.Hatırlatıcı_.Durum_> Bul(bool SüresiDolanlarıDahilEt = true, bool ÇalışmayıBekleyenleriDahilEt = true, string TakmaAdıKıstası = "*", bool BüyükKüçükHarfDuyarlı = true, char Ayraç = '*')
        {
            if (Tümü == null) return null; //Sadece canlı ekranlamadan çağırılabilir

            return Tümü.Bul(SüresiDolanlarıDahilEt, ÇalışmayıBekleyenleriDahilEt, TakmaAdıKıstası, BüyükKüçükHarfDuyarlı, Ayraç);
        }
        public static void Sil(string TakmaAdıKıstası, bool BüyükKüçükHarfDuyarlı = true, char Ayraç = '*')
        {
            if (Tümü == null) return; //Sadece canlı ekranlamadan çağırılabilir

            if (TakmaAdıKıstası == null)
            {
                Tümü.AyarlarıOku(true);
                Tümü = null;
            }
            else Tümü.Sil(TakmaAdıKıstası, BüyükKüçükHarfDuyarlı, Ayraç);
        }
    }

    public class BilgiToplama
    {
        protected static bool BaşlatDurdur_ = true;
        public static bool BaşlatDurdur
        {
            get
            {
                return BaşlatDurdur_;
            }
            set
            {
                BaşlatDurdur_ = value;

                if (Çizelgeç.S.AnaEkran.InvokeRequired)
                {
                    Çizelgeç.S.AnaEkran.Invoke(new Action(() => { _ivk_(); }));
                }
                else _ivk_();

                void _ivk_()
                {
                    Çizelgeç.S.SolMenu_BaşlatDurdur.Image = BaşlatDurdur_ ? Çizelgeç.Properties.Resources.D_Tamam : Çizelgeç.Properties.Resources.D_Hata;
                }
            }
        }

        protected static bool ZamanDilimi_BirbirininAynısıOlanlarıAtla_ = true;
        public static bool ZamanDilimi_BirbirininAynısıOlanlarıAtla
        {
            get
            {
                return ZamanDilimi_BirbirininAynısıOlanlarıAtla_;
            }
            set
            {
                ZamanDilimi_BirbirininAynısıOlanlarıAtla_ = value;

                if (Çizelgeç.S.AnaEkran.InvokeRequired)
                {
                    Çizelgeç.S.AnaEkran.Invoke(new Action(() => { _ivk_(); }));
                }
                else _ivk_();

                void _ivk_()
                {
                    Çizelgeç.S.AnaEkran.SağTuşMenü_Çizelge_Birbirinin_aynısı_olan_zaman_dilimlerini_atla.Checked = ZamanDilimi_BirbirininAynısıOlanlarıAtla_;
                }
            }
        }

        public static int ZamanDilimi_msn = 500;
        public static int ZamanDilimi_Sayısı = 5000;

        public static string Kayıt_Klasörü = null;
        public static long Kayıt_AzamiDosyaBoyutu_Bayt = 5 * 1024 * 1024; // 5 MiB
    }

    public class MupDosyasındanOkuma
    {
        public static List<string[]> CümleBaşlangıcıVeKelimeAyraçları = new List<string[]>();
    }
}

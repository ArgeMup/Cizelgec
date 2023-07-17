// Copyright ArgeMup GNU GENERAL PUBLIC LICENSE Version 3 <http://www.gnu.org/licenses/> <https://github.com/ArgeMup>

using ArgeMup.HazirKod.Ekİşlemler;
using System;
using System.Windows.Forms;

namespace Yardımcıİşlemler
{
    public class ÖnYüz
    {
        internal static bool BaşlatDurdur_ = true;
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
        public static void Günlük(Exception İstisna)
        {
            string hata = null;
            while (İstisna != null)
            {
                hata += İstisna.ToString() + Environment.NewLine;
                İstisna = İstisna.InnerException;
            }

            Çizelgeç.Günlük.Ekle(hata);

            if (Çizelgeç.S.YenidenHesapla == null || Çizelgeç.S.YenidenHesapla.Disposing || Çizelgeç.S.YenidenHesapla.IsDisposed) return;

            if (Çizelgeç.S.AnaEkran.InvokeRequired)
            {
                Çizelgeç.S.AnaEkran.Invoke(new Action(() => { _ivk_(); }));
            }
            else _ivk_();

            void _ivk_()
            {
                Çizelgeç.S.YenidenHesapla.Notlar.AppendText(hata + Environment.NewLine);
            }
        }
        public static void SadeceŞuSinyalleriGöster(Çizelgeç.Sinyal_[] Sinyaller)
        {
            if (Çizelgeç.S.AnaEkran.InvokeRequired)
            {
                Çizelgeç.S.AnaEkran.Invoke(new Action(() => { _ivk_(); }));
            }
            else _ivk_();

            void _ivk_()
            {
                foreach (TreeNode t in Çizelgeç.S.Ağaç.Nodes)
                {
                    t.Checked = false;
                }

                foreach (Çizelgeç.Sinyal_ s in Sinyaller)
                {
                    s.Görseller.Dal.Checked = true;
                }

                Çizelgeç.S.Ağaç.SelectedNode = Çizelgeç.S.Ağaç.Nodes[0];
            }
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
        public enum TanımlanmamışSinyalleri { Kullan, Kaydetme, Atla };

        public static Çizelgeç.Bağlantı_ Ekle_KomutSatırı(string BağlantıAdı, string Uygulama, string Parametre = null)
        {
            if (!ÖnYüz.CanlıEkranlama) return null; //Sadece canlı ekranlamadan çağırılabilir

            Çizelgeç.Bağlantı_ yeni = Çizelgeç.Bağlantılar.Ekle(BağlantıAdı);
            yeni.Türü = Çizelgeç.Bağlantı_Türü_.KomutSatırı;
            yeni.P1 = Uygulama;
            yeni.P2 = Parametre;

            return yeni;
        }
        public static Çizelgeç.Bağlantı_ Ekle_Uart(string BağlantıAdı, int BitHızı, string ErişimNoktası = "COMx")
        {
            if (!ÖnYüz.CanlıEkranlama) return null; //Sadece canlı ekranlamadan çağırılabilir

            Çizelgeç.Bağlantı_ yeni = Çizelgeç.Bağlantılar.Ekle(BağlantıAdı);
            yeni.Türü = Çizelgeç.Bağlantı_Türü_.SeriPort;
            yeni.P1 = ErişimNoktası;
            yeni.P2 = BitHızı.Yazıya();

            return yeni;
        }
        public static Çizelgeç.Bağlantı_ Ekle_Tcpİstemcisi(string BağlantıAdı, string IPveyaAdresi, int ErişimNoktası)
        {
            if (!ÖnYüz.CanlıEkranlama) return null; //Sadece canlı ekranlamadan çağırılabilir

            Çizelgeç.Bağlantı_ yeni = Çizelgeç.Bağlantılar.Ekle(BağlantıAdı);
            yeni.Türü = Çizelgeç.Bağlantı_Türü_.Tcpİstemci;
            yeni.P1 = IPveyaAdresi;
            yeni.P2 = ErişimNoktası.Yazıya();

            return yeni;
        }
        public static Çizelgeç.Bağlantı_ Ekle_Udpİstemcisi(string BağlantıAdı, string IPveyaAdresi, int ErişimNoktası)
        {
            if (!ÖnYüz.CanlıEkranlama) return null; //Sadece canlı ekranlamadan çağırılabilir

            Çizelgeç.Bağlantı_ yeni = Çizelgeç.Bağlantılar.Ekle(BağlantıAdı);
            yeni.Türü = Çizelgeç.Bağlantı_Türü_.Udpİstemci;
            yeni.P1 = IPveyaAdresi;
            yeni.P2 = ErişimNoktası.Yazıya();

            return yeni;
        }
        public static Çizelgeç.Bağlantı_ Ekle_TcpSunucusu(string BağlantıAdı, int ErişimNoktası)
        {
            if (!ÖnYüz.CanlıEkranlama) return null; //Sadece canlı ekranlamadan çağırılabilir

            Çizelgeç.Bağlantı_ yeni = Çizelgeç.Bağlantılar.Ekle(BağlantıAdı);
            yeni.Türü = Çizelgeç.Bağlantı_Türü_.TcpSunucu;
            yeni.P1 = ErişimNoktası.Yazıya();

            return yeni;
        }
        public static Çizelgeç.Bağlantı_ Ekle_UdpSunucusu(string BağlantıAdı, int ErişimNoktası)
        {
            if (!ÖnYüz.CanlıEkranlama) return null; //Sadece canlı ekranlamadan çağırılabilir

            Çizelgeç.Bağlantı_ yeni = Çizelgeç.Bağlantılar.Ekle(BağlantıAdı);
            yeni.Türü = Çizelgeç.Bağlantı_Türü_.UdpSunucu;
            yeni.P1 = ErişimNoktası.Yazıya();

            return yeni;
        }

        public static Çizelgeç.Bağlantı_ Bul(string BağlantıAdı)
        {
            if (!Çizelgeç.Bağlantılar.MevcutMu(BağlantıAdı)) return null;

            return Çizelgeç.Bağlantılar.Bul(BağlantıAdı);
        }
    }

    public class Sinyaller
    {
        public static Func<bool, bool> GeriBildirimİşlemi_Kaydedilecek = null;

        public static double[] ZamanEkseni;
        public static int ZamanEkseni_EnYakın(DateTime TarihSaat, int BaşlangıçKonumu = 0, int BitişKonumu = -1)
        {
            double Koordinat_X = Çizelgeç.S.Tarih.Sayıya(TarihSaat);
            if (ZamanEkseni == null || ZamanEkseni.Length < 2 || Koordinat_X <= 0 || double.IsNaN(Koordinat_X) || double.IsInfinity(Koordinat_X)) return -1;

            int bulundu = -1;
            int en_yakın_konum = -1;
            if (BitişKonumu == -1) BitişKonumu = ZamanEkseni.Length - 1;

            while (bulundu == -1 && BaşlangıçKonumu != BitişKonumu && Çizelgeç.S.Çalışşsın)
            {
                int tahmini_konum = BaşlangıçKonumu + ((BitişKonumu - BaşlangıçKonumu) / 2);
                if (tahmini_konum == BaşlangıçKonumu)
                {
                    if (en_yakın_konum == -1) en_yakın_konum = tahmini_konum; //bulunamadı, en son üye olabilir 
                    break;
                }

                if (Koordinat_X <= ZamanEkseni[tahmini_konum])
                {
                    en_yakın_konum = tahmini_konum;
                    if (Koordinat_X >= ZamanEkseni[tahmini_konum])
                    {
                        bulundu = tahmini_konum;
                        break;
                    }

                    BitişKonumu = tahmini_konum;
                }
                else BaşlangıçKonumu = tahmini_konum;
            }

            if (bulundu == -1)
            {
                if (en_yakın_konum != -1)
                {
                    if (en_yakın_konum < 1) bulundu = 0;
                    else if (en_yakın_konum >= BitişKonumu) bulundu = BitişKonumu;
                    else
                    {
                        double fark_soldan = Math.Abs(ZamanEkseni[en_yakın_konum - 1] - Koordinat_X);
                        double fark_sağdan = Math.Abs(ZamanEkseni[en_yakın_konum + 1] - Koordinat_X);

                        if (fark_soldan < fark_sağdan)
                        {
                            fark_sağdan = Math.Abs(ZamanEkseni[en_yakın_konum] - Koordinat_X);

                            if (fark_soldan < fark_sağdan) bulundu = en_yakın_konum - 1;
                            else bulundu = en_yakın_konum;
                        }
                        else
                        {
                            fark_soldan = Math.Abs(ZamanEkseni[en_yakın_konum] - Koordinat_X);

                            if (fark_soldan < fark_sağdan) bulundu = en_yakın_konum;
                            else bulundu = en_yakın_konum + 1;
                        }
                    }
                }
            }

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

        public static void KaydaAçıklamaEkle(string Açıklama)
        {
            if (!ÖnYüz.CanlıEkranlama) return; //Sadece canlı ekranlamadan çağırılabilir

            Çizelgeç.Kaydedici.Ekle(DateTime.Now, null, Açıklama);
        }
   
        public static Çizelgeç.Sinyal_ Ekle(string SinyalAdı, string AğaçİçindekiDalınAdı, string GörünenAdı, bool Kaydedilsin = true)
        {
            if (Çizelgeç.Sinyaller.MevcutMu(SinyalAdı)) throw new Exception("Sinyal adı " + SinyalAdı + " zaten eklendi");

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
        public static void Düzenle(Çizelgeç.Sinyal_ Sinyal, double ZamanAşımı_Sn, Action<Çizelgeç.Sinyal_> GeriBildirimİşlemi_GüncelDeğeriGüncellendi)
        {
            Sinyal.Değeri.ZamanAşımı_Sn = ZamanAşımı_Sn;
            Sinyal.Değeri.GeriBildirimİşlemi_GüncelDeğeriGüncellendi = GeriBildirimİşlemi_GüncelDeğeriGüncellendi;
        }
        public static Çizelgeç.Sinyal_ Bul(string SinyalAdı)
        {
            if (!Çizelgeç.Sinyaller.MevcutMu(SinyalAdı)) return null;

            return Çizelgeç.Sinyaller.Bul(SinyalAdı);
        }
    };

    public class BilgiToplama
    {
        public static bool ZamanDilimi_BirbirininAynısıOlanlarıAtla = true;
        public static int ZamanDilimi_msn = 500;
        public static int ZamanDilimi_Sayısı = 5000;

        public static string Kayıt_Klasörü = null;
        public static long Kayıt_AzamiDosyaBoyutu_Bayt = 5 * 1024 * 1024; // 5 MiB
    }

    public class Görev
    {
        internal static ArgeMup.HazirKod.ArkaPlan.Hatırlatıcı_ Görevler = null;

        public static void Ekle(string TakmaAdı, DateTime İlkTetikleyeceğiZaman, Func<string, object, int> GeriBildirim_Islemi, object Hatırlatıcı = null)
        {
            if (!ÖnYüz.CanlıEkranlama) return; //Sadece canlı ekranlamadan çağırılabilir

            if (Görevler == null) Görevler = new ArgeMup.HazirKod.ArkaPlan.Hatırlatıcı_();

            Görevler.Kur(TakmaAdı, İlkTetikleyeceğiZaman, null, GeriBildirim_Islemi, Hatırlatıcı);
        }
        public static ArgeMup.HazirKod.ArkaPlan.Hatırlatıcı_.Durum_ Bul(string TakmaAdı)
        {
            if (Görevler == null) return null; //Sadece canlı ekranlamadan çağırılabilir

            return Görevler.Bul(TakmaAdı);
        }
        public static void Sil(string TakmaAdı)
        {
            if (Görevler == null) return; //Sadece canlı ekranlamadan çağırılabilir
            
            Görevler.Sil(TakmaAdı);
        }
    }

    public class MupDosyasındanOkuma
    {
        public static string CümleBaşlangıcı = ">Sinyaller";
        public static char KelimeAyracı = ';';
    }
}

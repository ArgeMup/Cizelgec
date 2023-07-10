// Copyright ArgeMup GNU GENERAL PUBLIC LICENSE Version 3 <http://www.gnu.org/licenses/> <https://github.com/ArgeMup>

using System;
using System.Windows.Forms;

namespace Yardımcıİşlemler
{
    public class ÖnYüz
    {
        public static bool ÇalışmayaDevamEt { get => Çizelgeç.S.Çalışşsın; }
        public static void Güncelle()
        {
            //güncelle
            Çizelgeç.S.Çizdir();

            //kaydırma çubuğu kaynaklı görselleri güncelle
            Çizelgeç.S.AnaEkran.Kaydırıcı_Scroll(null, null);
        }
        public static void İlerlemeÇubuğu(int Güncel, int Toplam)
        {
            if (Çizelgeç.S.YenidenHesapla == null || Çizelgeç.S.YenidenHesapla.Disposing || Çizelgeç.S.YenidenHesapla.IsDisposed) return;

            ProgressBar iç = Çizelgeç.S.YenidenHesapla.İlerlemeÇubuğu;
            if (iç.Maximum != Toplam)
            {
                iç.Value = 0;
                iç.Maximum = Toplam;
            }

            iç.Value = Güncel;
            Application.DoEvents();
        }
        public static void Günlük(string Girdi)
        {
            Çizelgeç.Günlük.Ekle(Girdi);

            if (Çizelgeç.S.YenidenHesapla == null || Çizelgeç.S.YenidenHesapla.Disposing || Çizelgeç.S.YenidenHesapla.IsDisposed) return;
            Çizelgeç.S.YenidenHesapla.Notlar.AppendText(Girdi + Environment.NewLine);
        }
        public static void SadeceŞuSinyalleriGöster(Çizelgeç.Sinyal_[] Sinyaller)
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
    };

    public class Sinyal
    {
        static bool CanlıEkranlama
        {
            get
            {
                return Çizelgeç.S.AnaEkran.Canlı_Ekranlama != null;
            }
        }

        public static double[] ZamanEkseni
        {
            get { return Çizelgeç.S.ZamanEkseni; }
        }
        public static int ZamanEkseni_EnYakın(DateTime TarihSaat, int BaşlangıçKonumu = 0, int BitişKonumu = -1)
        {
            double Koordinat_X = Çizelgeç.S.Tarih.Sayıya(TarihSaat);
            if (Çizelgeç.S.ZamanEkseni == null || Çizelgeç.S.ZamanEkseni.Length < 2 || Koordinat_X <= 0 || double.IsNaN(Koordinat_X) || double.IsInfinity(Koordinat_X)) return -1;

            int bulundu = -1;
            int en_yakın_konum = -1;
            if (BitişKonumu == -1) BitişKonumu = Çizelgeç.S.ZamanEkseni.Length - 1;

            while (bulundu == -1 && BaşlangıçKonumu != BitişKonumu && Çizelgeç.S.Çalışşsın)
            {
                int tahmini_konum = BaşlangıçKonumu + ((BitişKonumu - BaşlangıçKonumu) / 2);
                if (tahmini_konum == BaşlangıçKonumu)
                {
                    if (en_yakın_konum == -1) en_yakın_konum = tahmini_konum; //bulunamadı, en son üye olabilir 
                    break;
                }

                if (Koordinat_X <= Çizelgeç.S.ZamanEkseni[tahmini_konum])
                {
                    en_yakın_konum = tahmini_konum;
                    if (Koordinat_X >= Çizelgeç.S.ZamanEkseni[tahmini_konum])
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
                        double fark_soldan = Math.Abs(Çizelgeç.S.ZamanEkseni[en_yakın_konum - 1] - Koordinat_X);
                        double fark_sağdan = Math.Abs(Çizelgeç.S.ZamanEkseni[en_yakın_konum + 1] - Koordinat_X);

                        if (fark_soldan < fark_sağdan)
                        {
                            fark_sağdan = Math.Abs(Çizelgeç.S.ZamanEkseni[en_yakın_konum] - Koordinat_X);

                            if (fark_soldan < fark_sağdan) bulundu = en_yakın_konum - 1;
                            else bulundu = en_yakın_konum;
                        }
                        else
                        {
                            fark_soldan = Math.Abs(Çizelgeç.S.ZamanEkseni[en_yakın_konum] - Koordinat_X);

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

        public static Çizelgeç.Sinyal_ Ekle(string SinyalAdı, string AğaçİçindekiDalınAdı, string GörünenAdı)
        {
            if (Çizelgeç.Sinyaller.MevcutMu(SinyalAdı)) throw new Exception(SinyalAdı + " zaten eklendi");

            Çizelgeç.Sinyal_ sinyal = Çizelgeç.Sinyaller.Ekle(SinyalAdı);
            sinyal.Güncelle_Adı(SinyalAdı, AğaçİçindekiDalınAdı, GörünenAdı);

            if (!CanlıEkranlama) Çizelgeç.Ekranlama.SinyaliAğacaEkle(sinyal);

            return sinyal;
        }
        public static Çizelgeç.Sinyal_ Bul(string SinyalAdı)
        {
            return Çizelgeç.Sinyaller.Tümü.TryGetValue(SinyalAdı, out Çizelgeç.Sinyal_ Sinyal) ? Sinyal : null;
        }
        public static void Sil(string SinyalAdı)
        {
            if (CanlıEkranlama) return;

            Çizelgeç.Sinyal_ sinyal = Bul(SinyalAdı);
            if (sinyal != null)
            {
                sinyal.Değeri.DeğerEkseni = null;
                sinyal.Görseller.Dal.Remove();
                Çizelgeç.S.Çizelge.plt.Remove(sinyal.Görseller.Çizikler);
                Çizelgeç.Sinyaller.Tümü.Remove(SinyalAdı);
            }
        }
    };
}

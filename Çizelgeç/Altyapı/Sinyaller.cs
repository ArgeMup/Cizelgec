// Copyright ArgeMup GNU GENERAL PUBLIC LICENSE Version 3 <http://www.gnu.org/licenses/> <https://github.com/ArgeMup>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using Yardımcıİşlemler;

namespace Çizelgeç
{
    public class Görselleri_
    {
        public bool DaldakiYazıyıGüncelleVeKabart = true;
       
        public TreeNode Dal = null;
        public ScottPlot.Plottable.SignalPlotXY Çizikler = null;
        public ScottPlot.Plottable.MarkerPlot SeçiliOlanNokta = null;
        public ScottPlot.Plottable.Text SeçiliOlanNokta_Yazı = null;
    };
    public class Değeri_
    {
        public bool Kaydedilsin = true;
        public double SonDeğeri = 0;
        public DateTime SonDeğerinAlındığıAn = DateTime.Now;
        public double[] DeğerEkseni = null;
        public bool ZamanAşımı_Oldu = false;
        public double ZamanAşımı_Sn = 0;
        public UInt64 Sayac_Güncelleme = 0;

        public Func<Sinyal_, double, double> GeriBildirimİşlemi_GüncelDeğeriGüncellendi = null;
        public Action<Sinyal_, TimeSpan> GeriBildirimİşlemi_ZamanAşımı = null;
    }
    public class Adı_
    {
        public string Sinyal;       //<Sinyal[0]>
        public string Salkım;       //İklimsel|Sıcaklık
        public string GörünenAdı;   //TM1
        public string Csv;          //Salkım + | + GörünenAdı
    }
    public enum Tür_ { Boşta, Sinyal, Değişken };

    public class Sinyal_
    {
        public Tür_ Tür = Tür_.Boşta;
        public Adı_ Adı = new Adı_(); 
        public Değeri_ Değeri = new Değeri_();
        public Görselleri_ Görseller = new Görselleri_();

        public void Güncelle_Adı(string SinyalAdı, string Soyadı = "", string GörünenAdı = "")
        {
            if (SinyalAdı.Contains('[') && SinyalAdı.Contains(']')) Tür = Tür_.Sinyal;
            else
            {
                Tür = Tür_.Değişken;

                if (string.IsNullOrEmpty(Soyadı)) Soyadı = "Değişkenler";
            }

            Adı.Salkım = Soyadı;
            Adı.GörünenAdı = (string.IsNullOrEmpty(GörünenAdı) ? SinyalAdı : GörünenAdı).Replace("|", "");

            if (string.IsNullOrEmpty(Soyadı)) Adı.Csv = (Tür == Tür_.Değişken ? "Değişkenler|" : "") + Adı.GörünenAdı;
            else Adı.Csv = (Soyadı + "|" + Adı.GörünenAdı).Trim(' ','|');
        }
        public void Güncelle_SonDeğer(double Girdi)
        {
            if (Değeri.GeriBildirimİşlemi_GüncelDeğeriGüncellendi != null)
            {
                try
                {
                    Girdi = Değeri.GeriBildirimİşlemi_GüncelDeğeriGüncellendi(this, Girdi);
                }
                catch (Exception ex) { Yardımcıİşlemler.ÖnYüz.Günlük(ex, "Sinyaller.GeriBildirimİşlemi_GüncelDeğeriGüncellendi." + Adı.Sinyal); }
            }

            if (double.IsNaN(Girdi) || double.IsInfinity(Girdi))
            {
                Günlük.Ekle(Adı.Csv + " güncel değeri ( " + S.Sayı.Yazıya(Girdi) + " ) sayı değil, 0 olarak değiştirildi.");
                Girdi = 0;
            }

            if (Değeri.SonDeğeri != Girdi)
            {
                Görseller.DaldakiYazıyıGüncelleVeKabart = true;

                if (Değeri.Kaydedilsin)
                {
                    Sinyaller.EnAzBirSinyalDeğişti_KaydedilmesiGereken = true;
                }
            }

            if (Değeri.ZamanAşımı_Oldu)
            {
                Değeri.ZamanAşımı_Oldu = false;

                TimeSpan ts = DateTime.Now - Değeri.SonDeğerinAlındığıAn;
                ÖnYüz.AçıklamaEkle("Zaman aşımı bitti " + Adı.Csv + " (" + ts.ToString("d\\.hh\\:mm\\:ss\\:fff").Trim('0', '.', ':') + "msn)", System.Drawing.Color.Green);

                if (Değeri.GeriBildirimİşlemi_ZamanAşımı != null)
                {
                    try
                    {
                        Değeri.GeriBildirimİşlemi_ZamanAşımı(this, ts);
                    }
                    catch (Exception ex) { Yardımcıİşlemler.ÖnYüz.Günlük(ex, "Sinyaller.GeriBildirimİşlemi_ZamanAşımı." + Adı.Sinyal); }
                }
            }

            Değeri.SonDeğeri = Girdi;
            Değeri.SonDeğerinAlındığıAn = DateTime.Now;
            Değeri.Sayac_Güncelleme++; 
        }
        public void Güncelle_SonDeğer(string Girdi)
        {
            double değer = S.Sayı.Yazıdan(Girdi);
            Güncelle_SonDeğer(değer);
        }
        public double Güncelle_Dizi()
        {
            if (Değeri.DeğerEkseni != null)
            {
                Array.Copy(Değeri.DeğerEkseni, 1, Değeri.DeğerEkseni, 0, Değeri.DeğerEkseni.Length - 1);
                Değeri.DeğerEkseni[Değeri.DeğerEkseni.Length - 1] = Değeri.SonDeğeri;
            }

            return Değeri.SonDeğeri;
        }
        public void Güncelle_ZamanAşımıOlduMu_SadeceTekYerdenÇağırılabilir()
        {
            if (Değeri.ZamanAşımı_Sn > 0)
            {
                TimeSpan fark = DateTime.Now - Değeri.SonDeğerinAlındığıAn;
                if (fark.TotalSeconds > Değeri.ZamanAşımı_Sn)
                {
                    if (!Değeri.ZamanAşımı_Oldu)
                    {
                        Değeri.ZamanAşımı_Oldu = true;

                        ÖnYüz.AçıklamaEkle("Zaman aşımı oldu " + Adı.Csv, System.Drawing.Color.Red);

                        if (Değeri.GeriBildirimİşlemi_ZamanAşımı != null)
                        {
                            try
                            {
                                Değeri.GeriBildirimİşlemi_ZamanAşımı(this, default(TimeSpan));
                            }
                            catch (Exception ex) { Yardımcıİşlemler.ÖnYüz.Günlük(ex, "Sinyaller.GeriBildirimİşlemi_ZamanAşımı." + Adı.Sinyal); }
                        }
                    }
                }
            }
        }
        public void Sil()
        {
            if (ÖnYüz.CanlıEkranlama) return;

            if (S.AnaEkran.InvokeRequired)
            {
                S.AnaEkran.Invoke(new Action(() => { _ivk_(); }));
            }
            else _ivk_();

            void _ivk_()
            {
                Görseller.Dal?.Remove();
                if (Görseller.Çizikler != null) S.Çizelge.Plot.Remove(Görseller.Çizikler);
            }

            Sinyaller.Tümü.Remove(Adı.Sinyal);
            Değeri.DeğerEkseni = null;
        }
    };
 
    public class Sinyaller
    {
        static Mutex Mtx = new Mutex();
        public static bool EnAzBirSinyalDeğişti_KaydedilmesiGereken = true;
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
            if (MevcutMu(Adı)) return Bul(Adı);

            Sinyal_ yeni = new Sinyal_();
            yeni.Adı.Sinyal = Adı;
            yeni.Güncelle_Adı(Adı);

            Doğrudan_Ekle(Adı, yeni);
            return yeni;
        }
        public static void Doğrudan_Ekle(string Adı, Sinyal_ yeni)
        {
            Mtx.WaitOne();
            Tümü[Adı] = yeni;
            Mtx.ReleaseMutex();
        }
        public static Sinyal_ Bul(string Adı)
        {
            Sinyal_ girdi;
            bool sonuç = Tümü.TryGetValue(Adı, out girdi);

            if (sonuç) return girdi;

            throw new Exception(Adı + " isimli sinyal listede bulunamadı");
        }
    }
}

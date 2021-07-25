// Copyright ArgeMup GNU GENERAL PUBLIC LICENSE Version 3 <http://www.gnu.org/licenses/> <https://github.com/ArgeMup>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

namespace Çizelgeç
{
    public class Görselleri_
    {
        public bool DaldakiYazıyıGüncelleVeKabart = true;
       
        public TreeNode Dal = null;
        public ScottPlot.PlottableSignalXY Çizikler = null;
        //public List<ScottPlot.PlottableText> Uyarı_Yazıları = null;
    };
    public class Hesaplama_
    {
        public string İşlem;
        public string Değişken;
    };
    public class Uyarı_
    {
        public string Kıstas;
        public string[] Açıklama;
    };
    public class Değeri_
    {
        public bool Kaydedilsin = true;
        public double SonDeğeri = 0;
        public DateTime SonDeğerinAlındığıAn = DateTime.Now;
        public double[] DeğerEkseni = null;
        public bool ZamanAşımıOldu = false;
        public string Önİşlem = null;
        public string ZamanAşımı_Sn = "0";
        public UInt64 Sayac_Güncelleme = 0;
    }
    public class Adı_
    {
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
        public Hesaplama_[] Hesaplamalar = null;
        public Uyarı_[] Uyarılar = null;
        public Görselleri_ Görseller = new Görselleri_();
        Sinyal_ ZamanAşımıSinyali = null;

        public void Güncelle_Adı(string SinyalAdı, string Soyadı = "", string GörünenAdı = "")
        {
            if (SinyalAdı.Contains('[') && SinyalAdı.Contains(']')) Tür = Tür_.Sinyal;
            else
            {
                Tür = Tür_.Değişken;
                Değeri.Kaydedilsin = false;

                if (string.IsNullOrEmpty(Soyadı)) Soyadı = "Değişkenler";
            }

            Adı.Salkım = Soyadı;
            Adı.GörünenAdı = (string.IsNullOrEmpty(GörünenAdı) ? SinyalAdı : GörünenAdı).Replace("|", "");

            if (string.IsNullOrEmpty(Soyadı)) Adı.Csv = (Tür == Tür_.Değişken ? "Değişkenler|" : "") + Adı.GörünenAdı;
            else Adı.Csv = (Soyadı + "|" + Adı.GörünenAdı).Trim(' ','|');
        }
        public void Güncelle_SonDeğer(double Girdi)
        {
            if (!string.IsNullOrEmpty(Değeri.Önİşlem))
            {
                string işlem = Değeri.Önİşlem.Replace("<Sinyal>", S.Sayı.Yazıya(Girdi));
                Girdi = Çevirici.Yazıdan_NoktalıSayıya(işlem);
            }

            if (Değeri.SonDeğeri != Girdi)
            {
                Görseller.DaldakiYazıyıGüncelleVeKabart = true;
                Sinyaller.EnAzBirSinyalDeğişti = true;

                if (Değeri.Kaydedilsin)
                {
                    Sinyaller.EnAzBirSinyalDeğişti_KaydedilmesiGereken = true;
                }
            }

            Değeri.SonDeğeri = Girdi;
            Değeri.SonDeğerinAlındığıAn = DateTime.Now;
            Değeri.ZamanAşımıOldu = false;
            Değeri.Sayac_Güncelleme++;
        }
        public void Güncelle_SonDeğer(string Girdi)
        {
            double değer = S.Sayı.Yazıdan(Girdi);
            Güncelle_SonDeğer(değer);
        }
        public double Güncelle_Dizi()
        {
            if (Değeri.DeğerEkseni == null) Değeri.DeğerEkseni = Enumerable.Repeat(Değeri.SonDeğeri, S.CanliÇizdirme_ÖlçümSayısı).ToArray();

            #if MerdivenGörünümüİçin
                if (!S.BilgiToplama_BirbirininAynısıOlanZamanDilimleriniAtla)
                {                       
            #endif
                    Array.Copy(Değeri.DeğerEkseni, 1, Değeri.DeğerEkseni, 0, Değeri.DeğerEkseni.Length - 1);
                    Değeri.DeğerEkseni[Değeri.DeğerEkseni.Length - 1] = Değeri.SonDeğeri;
            #if MerdivenGörünümüİçin
                }
                else
                {
                    Array.Copy(Değeri.DeğerEkseni, 2, Değeri.DeğerEkseni, 0, Değeri.DeğerEkseni.Length - 2);
                    Değeri.DeğerEkseni[Değeri.DeğerEkseni.Length - 1] = Değeri.SonDeğeri;
                    Değeri.DeğerEkseni[Değeri.DeğerEkseni.Length - 2] = Değeri.SonDeğeri;
                }                        
            #endif

            return Değeri.SonDeğeri;
        }
        public bool Güncelle_ZamanAşımıOlduMu_SadeceTekYerdenÇağırılabilir()
        {
            if (Tür == Tür_.Sinyal && Değeri.ZamanAşımı_Sn != "0")
            {
                TimeSpan fark = DateTime.Now - Değeri.SonDeğerinAlındığıAn;
                if (fark.TotalSeconds > Çevirici.Yazıdan_NoktalıSayıya(Değeri.ZamanAşımı_Sn))
                {
                    Değeri.ZamanAşımıOldu = true;

                    if (ZamanAşımıSinyali == null)
                    {
                        string addddı = "<ZA " + Adı.Csv + ">";

                        ZamanAşımıSinyali = Sinyaller.Ekle(addddı);
                        ZamanAşımıSinyali.Güncelle_Adı(addddı, "Zaman Aşımları", Adı.Salkım + "|" + Adı.GörünenAdı);
                        ZamanAşımıSinyali.Değeri.Kaydedilsin = true;
                    }
                    else ZamanAşımıSinyali.Güncelle_SonDeğer(ZamanAşımıSinyali.Değeri.SonDeğeri + 1);
                }
            }

            return Değeri.ZamanAşımıOldu;
        }
    };
 
    public class Sinyaller
    {
        static Mutex Mtx = new Mutex();
        public static bool EnAzBirSinyalDeğişti = true;
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

            Adı = Adı.Trim(' ');
            if (MevcutMu(Adı)) return Bul(Adı);

            Sinyal_ yeni = new Sinyal_();
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
            return Ekle(Adı).Değeri.SonDeğeri;
        }
    }
}

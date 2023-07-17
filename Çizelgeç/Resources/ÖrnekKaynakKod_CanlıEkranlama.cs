//sinyal sil
//sinal geribildirim işlemi
//sinyaller geribildirim işlemi
//    kayda açıklama ekle
//    uygtulamayı kapat
//    bilgisayarı kapat
//    bağımızız görev yada benzeri

//    arka plandaki koddan varolan sinyal ve bağlantılar silinecek kapatılacak
//    birden fazla cümle başlangıcı ve ayraç
//    mümkün ise tüm işlemleri çapraz görevden çağır
//    çapraz işlemden çağırmak
//bağlantı açma 

namespace Yardımcıİşlemler
{
    public class Kontrolcü
    {
        Çizelgeç.Bağlantı_ BağlantıA;
        Çizelgeç.Sinyal_ SinyalA, SinyalB;

        public void İlkAyarlamalarıYap()
        {
            BağlantıA = Bağlantılar.Ekle_KomutSatırı("Ks1", @"D:\Mesleki\Proje\Kendim\Cdiyez\Çizelgeç\ConsoleApp1\ConsoleApp1\bin\Debug\ConsoleApp1.exe", "100 Denm Evet");
        }
    }
}


/* ArGeMuP Çizelgeç.exe BU KISMI DEĞİŞTİRMEYİNİZ - BAŞLANGIÇ */

/* ??? [[[ Detaylar ]]] %%%
*** Örnek Kaynak Kod ***
namespace Yardımcıİşlemler
{
    public class ÖrnekSınıf
    {
        Çizelgeç.Sinyal_ SinyalA, SinyalB;

        public void Örnekİşlem()
        {
            SinyalA = Sinyal.Bul("<SinyalAdı[0]>");
            SinyalB = Sinyal.Bul("<DeğişkenAdı>");

            if (SinyalA == null)
            {
                SinyalA = Sinyal.Ekle("<SinyalAdı[0]>", "Ağaç|İçindeki|Dalın|Adı", "GörünenAdı A");
                SinyalB = Sinyal.Ekle("<DeğişkenAdı>", "Ağaç|İçindeki|Dalın|Adı", "GörünenAdı B");
            }
            
            ÖnYüz.Günlük("Zaman ekseninde " + Sinyal.ZamanEkseni.Length + " adet bilgi var");
            ÖnYüz.SadeceŞuSinyalleriGöster(new Çizelgeç.Sinyal_[] { SinyalA, SinyalB });
            SinyalDetaylarınıYazdır(SinyalA);
            SinyalDetaylarınıYazdır(SinyalB);

            for (int i = 0; i < SinyalA.Değeri.DeğerEkseni.Length && ÖnYüz.ÇalışmayaDevamEt; i++)
            {
                SinyalA.Değeri.DeğerEkseni[i] = SinyalA.Değeri.DeğerEkseni[i] == 0 ? (byte)i : SinyalA.Değeri.DeğerEkseni[i] * 4;
                ÖnYüz.İlerlemeÇubuğu(i, Sinyal.ZamanEkseni.Length);
                ÖnYüz.Güncelle();
                System.Threading.Thread.Sleep(5);
            }

            for (int i = 0 ; i < Sinyal.ZamanEkseni.Length && ÖnYüz.ÇalışmayaDevamEt; i++)
            {
                ÖnYüz.İlerlemeÇubuğu(i, Sinyal.ZamanEkseni.Length);
                SinyalB.Değeri.DeğerEkseni[i] = SinyalA.Değeri.DeğerEkseni[i] * 2;
                ÖnYüz.Güncelle();
                System.Threading.Thread.Sleep(5);
            }
        }
        void SinyalDetaylarınıYazdır(Çizelgeç.Sinyal_ Sinyal)
        {
            ÖnYüz.Günlük("--------------------------------------");
            ÖnYüz.Günlük("Tür : " + Sinyal.Tür.ToString());
            ÖnYüz.Günlük("Adı.Salkım : " + Sinyal.Adı.Salkım.ToString());
            ÖnYüz.Günlük("Adı.GörünenAdı : " + Sinyal.Adı.GörünenAdı.ToString());
            ÖnYüz.Günlük("Adı.Csv : " + Sinyal.Adı.Csv.ToString());
            ÖnYüz.Günlük("Değeri.Kaydedilsin : " + Sinyal.Değeri.Kaydedilsin.ToString());
            ÖnYüz.Günlük("Değeri.SonDeğeri : " + Sinyal.Değeri.SonDeğeri.ToString());
            ÖnYüz.Günlük("Değeri.SonDeğerinAlındığıAn : " + Sinyal.Değeri.SonDeğerinAlındığıAn.ToString());
            ÖnYüz.Günlük("Değeri.DeğerEkseni.Length : " + Sinyal.Değeri.DeğerEkseni.Length.ToString());
            ÖnYüz.Günlük("Değeri.ZamanAşımıOldu : " + Sinyal.Değeri.ZamanAşımıOldu.ToString());
            ÖnYüz.Günlük("Değeri.ZamanAşımı_Sn : " + Sinyal.Değeri.ZamanAşımı_Sn.ToString());
            ÖnYüz.Günlük("Değeri.Sayac_Güncelleme : " + Sinyal.Değeri.Sayac_Güncelleme.ToString());
            ÖnYüz.Günlük("Görseller.DaldakiYazıyıGüncelleVeKabart : " + Sinyal.Görseller.DaldakiYazıyıGüncelleVeKabart.ToString());
            ÖnYüz.Günlük("Görseller.Dal.FullPath : " + Sinyal.Görseller.Dal.FullPath.ToString());
        }
    }
}
*/

/* ArGeMuP Çizelgeç.exe BU KISMI DEĞİŞTİRMEYİNİZ - BİTİŞ */
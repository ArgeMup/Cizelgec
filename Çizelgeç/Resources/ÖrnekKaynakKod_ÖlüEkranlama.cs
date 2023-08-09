using System;
using System.Drawing;
using System.Collections.Generic;
using ArgeMup.HazirKod;
using ArgeMup.HazirKod.Ekİşlemler;

namespace Yardımcıİşlemler
{
    public class Kontrolcü_YenidenHesapla
    {
        Çizelgeç.Sinyal_ Sinyal_1, Sinyal_2;

        public void Örnekİşlem()
        {
            var arama = Sinyaller.Bul("<SinyalAdı[0]>");
            Sinyal_1 = arama.Count > 0 ? arama[0] : null;
            arama = Sinyaller.Bul("<DeğişkenAdı>");
            Sinyal_2 = arama.Count > 0 ? arama[0] : null;

            if (Sinyal_1 == null)
            {
                Sinyal_1 = Sinyaller.Ekle("<SinyalAdı[0]>", "Ağaç|İçindeki|Dalın|Adı", "GörünenAdı A");
                Sinyal_2 = Sinyaller.Ekle("<DeğişkenAdı>", "Ağaç|İçindeki|Dalın|Adı", "GörünenAdı B");
            }
            
            ÖnYüz.Günlük("Zaman ekseninde " + Sinyaller.ZamanEkseni.Length + " adet bilgi var");
            ÖnYüz.SadeceŞuSinyalleriGöster(new List<Çizelgeç.Sinyal_>() { Sinyal_1, Sinyal_2 });
            SinyalDetaylarınıYazdır(Sinyal_1);
            SinyalDetaylarınıYazdır(Sinyal_2);

            for (int i = 0; i < Sinyal_1.Değeri.DeğerEkseni.Length && ÖnYüz.ÇalışmayaDevamEt; i++)
            {
                Sinyal_1.Değeri.DeğerEkseni[i] = Sinyal_1.Değeri.DeğerEkseni[i] == 0 ? (byte)i : Sinyal_1.Değeri.DeğerEkseni[i] * 4;
                ÖnYüz.İlerlemeÇubuğu(i, Sinyaller.ZamanEkseni.Length);
                ÖnYüz.Güncelle();
                System.Threading.Thread.Sleep(5);
            }

            for (int i = 0 ; i < Sinyaller.ZamanEkseni.Length && ÖnYüz.ÇalışmayaDevamEt; i++)
            {
                ÖnYüz.İlerlemeÇubuğu(i, Sinyaller.ZamanEkseni.Length);
                Sinyal_2.Değeri.DeğerEkseni[i] = Sinyal_1.Değeri.DeğerEkseni[i] * 2;
                ÖnYüz.Güncelle();
                System.Threading.Thread.Sleep(5);
            }

            ÖnYüz.İlerlemeÇubuğu(Sinyaller.ZamanEkseni.Length, Sinyaller.ZamanEkseni.Length);
        }
        void SinyalDetaylarınıYazdır(Çizelgeç.Sinyal_ Sinyal)
        {
            ÖnYüz.Günlük("--------------------------------------");
            ÖnYüz.Günlük("Tür : " + Sinyal.Tür.ToString());
            ÖnYüz.Günlük("Adı.Salkım : " + Sinyal.Adı.Salkım.ToString());
            ÖnYüz.Günlük("Adı.GörünenAdı : " + Sinyal.Adı.GörünenAdı.ToString());
            ÖnYüz.Günlük("Adı.Csv : " + Sinyal.Adı.Csv.ToString());
            ÖnYüz.Günlük("Değeri.DeğerEkseni.Length : " + Sinyal.Değeri.DeğerEkseni.Length.ToString());
        }
    }
}
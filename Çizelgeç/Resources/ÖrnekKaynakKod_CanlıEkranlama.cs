using System;
using System.Drawing;
using System.Collections.Generic;
using ArgeMup.HazirKod;
using ArgeMup.HazirKod.Ekİşlemler;

namespace Yardımcıİşlemler
{
    public class Kontrolcü
    {
        Çizelgeç.Bağlantı_ Bağlantı_1;
        Çizelgeç.Sinyal_ Sinyal_1, Sinyal_2;

        public void İlkAyarlamalarıYap()
        {
            BilgiToplama.Kayıt_Klasörü = @"C:\Çizelgeç Çıktıları";
            BilgiToplama.ZamanDilimi_msn = 50;

            Bağlantı_1 = Bağlantılar.Ekle_Uart("Bağlantı_1", 921600);
            Bağlantı_1.Ayıklayıcı_Ekle(">Sinyaller", ';');
            Bağlantı_1.Başlat();

            Sinyal_1 = Sinyaller.Ekle("<SinyalAdı[0]>", "Ağaç|İçindeki|Dalın|Adı", "GörünenAdı 1");
            Sinyal_1.Değeri.GeriBildirimİşlemi_GüncelDeğeriGüncellendi = GeriBildirimİşlemi_GüncelDeğeriGüncellendi;

            Sinyal_2 = Sinyaller.Ekle("<DeğişkenAdı>", "Ağaç|İçindeki|Dalın|Adı", "GörünenAdı 2");
            Sinyal_2.Değeri.ZamanAşımı_Sn = 5;
            Sinyal_2.Değeri.GeriBildirimİşlemi_ZamanAşımı = GeriBildirimİşlemi_ZamanAşımı;

            İşlemler.Ekle("İşlem_1", İşlem_1);

            Görevler.Ekle("Görev_Sinyal_1", DateTime.Now.AddSeconds(1), Görev_Sinyal_1);

            Sinyaller.GeriBildirimİşlemi_Kaydedilecek = GeriBildirimİşlemi_Kaydedilecek;
        }

        void İşlem_1(string İşlemAdı, object Hatırlatıcı)
        {
            ÖnYüz.AçıklamaEkle("İşlem_1 : " + Görev_Sinyal_1_Sayac, Color.Blue);
        }

        double Görev_Sinyal_1_Sayac = 0;
        int Görev_Sinyal_1(string TakmaAdı, object Hatırlatıcı)
        {
            Sinyal_1.Güncelle_SonDeğer(Görev_Sinyal_1_Sayac);
            Görev_Sinyal_1_Sayac += 1000;

            return 1000;
        }
        int Görev_Sinyal_2(string TakmaAdı, object Hatırlatıcı)
        {
            Sinyal_2.Güncelle_SonDeğer(Görev_Sinyal_1_Sayac);

            return -1;
        }

        bool GeriBildirimİşlemi_Kaydedilecek()
        {
            return true;
        }

        double GeriBildirimİşlemi_GüncelDeğeriGüncellendi(Çizelgeç.Sinyal_ Sinyal, double GüncelDeğeri)
        {
            return GüncelDeğeri / 1000;
        }

        void GeriBildirimİşlemi_ZamanAşımı(Çizelgeç.Sinyal_ Sinyal, TimeSpan GeçenSüre)
        {
            if (GeçenSüre == default(TimeSpan)) Görevler.Ekle("Görev_Sinyal_2", DateTime.Now.AddSeconds(5), Görev_Sinyal_2);
        }

    }
}
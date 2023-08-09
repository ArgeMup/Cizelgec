// Copyright ArgeMup GNU GENERAL PUBLIC LICENSE Version 3 <http://www.gnu.org/licenses/> <https://github.com/ArgeMup>

using ArgeMup.HazirKod.Ekİşlemler;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace Çizelgeç
{
    class Açıklama_
    {
        public DateTime TarihSaat;
        public ScottPlot.Plottable.Text Görsel;
        public DataGridViewRow Dal;
    }

    class Açıklamalar
    {
        public static string Vekil_AltSatıraGeç = " ?'#{n]$ ";
        public static List<Açıklama_> Tümü = new List<Açıklama_> ();
        public static void Ekle(DateTime TarihSaat,  string Açıklama, Color Renk = default)
        {
            if (Renk == default) Renk = Color.Black;
            Açıklama_ yeni = new Açıklama_() { TarihSaat = TarihSaat };
            yeni.Görsel = S.Çizelge.Plot.AddText(" " + S.Tarih.Yazıya(TarihSaat) + " " + Açıklama, S.Tarih.Sayıya(TarihSaat), 0, S.AnaEkran.Font.Size * 1.25f, Renk);
            yeni.Görsel.BorderSize = 2;
            yeni.Görsel.BorderColor =  Renk;
            yeni.Görsel.BackgroundColor = Color.White;
            yeni.Görsel.BackgroundFill = true;
            //yeni.Görsel.IsVisible = S.Açıklamalar_Çizelgede_Görünsün && TarihSaat <= S.Tarih.Tarihe(Yardımcıİşlemler.Sinyaller.ZamanEkseni[S.AralıkSeçici_Sondan.Value]);
            yeni.Görsel.IsVisible = false;
            yeni.Görsel.Rotation = 270;
            yeni.Görsel.Alignment = ScottPlot.Alignment.MiddleLeft;
            S.Çizelge.Plot.MoveLast(yeni.Görsel);

            int knm = S.AnaEkran.Açıklamalar_Tablo.Rows.Add(new object[] { S.Tarih.Yazıya(TarihSaat), Açıklama });
            yeni.Dal = S.AnaEkran.Açıklamalar_Tablo.Rows[knm];
            yeni.Dal.Tag = yeni;
            if (S.AnaEkran.Açıklamalar_YenileriTakipEt.Checked) S.AnaEkran.Açıklamalar_Tablo.FirstDisplayedScrollingRowIndex = S.AnaEkran.Açıklamalar_Tablo.RowCount - 1;
            if (S.AnaEkran.Ayraç_Ağaç_Açıklama.Panel2Collapsed) S.AnaEkran.Ayraç_Ağaç_Açıklama.Panel2Collapsed = false;

            Tümü.Add(yeni);

            if (Yardımcıİşlemler.ÖnYüz.CanlıEkranlama) Kaydedici.Ekle(TarihSaat, null, Açıklama.Replace("\r\n", Vekil_AltSatıraGeç).Replace("\r", "").Replace("\n", Vekil_AltSatıraGeç) + (Renk == Color.Black ? null : ";" + new byte[] { Renk.R, Renk.G, Renk.B }.HexYazıya()));
        }

        public static void Görünsün(bool Evet)
        {
            if (Evet) AralıkSeçicilereGöreAyarla();
            else
            {
                foreach (Açıklama_ a in Tümü)
                {
                    a.Görsel.IsVisible = false;
                }
            }
        }

        public static void AralıkSeçicilereGöreAyarla()
        {
            if (!S.Açıklamalar_Çizelgede_Görünsün) return;

            DateTime başlat = S.Tarih.Tarihe(Yardımcıİşlemler.Sinyaller.ZamanEkseni[S.AralıkSeçici_Baştan.Value]);
            DateTime bitir = S.Tarih.Tarihe(Yardımcıİşlemler.Sinyaller.ZamanEkseni[S.AralıkSeçici_Sondan.Value]);

            foreach (Açıklama_ a in Tümü)
            {
                a.Görsel.IsVisible = a.TarihSaat >= başlat && a.TarihSaat <= bitir;
            }
        }

        public static void EskiyenleriSil()
        {
            //zaman ekseninden çıkmış olanları sil
            DateTime t = S.Tarih.Tarihe(Yardımcıİşlemler.Sinyaller.ZamanEkseni[0]);
            int silinecek_adet = 0;
            foreach (Açıklama_ a in Tümü)
            {
                if (a.TarihSaat >= t) break;

                S.AnaEkran.Açıklamalar_Tablo.Rows.Remove(a.Dal);
                S.Çizelge.Plot.Remove(a.Görsel);
                silinecek_adet++;
            }
            Tümü.RemoveRange(0, silinecek_adet);

            //aralık seçici kapsamından çıkmış olanları gizle
            t = S.Tarih.Tarihe(Yardımcıİşlemler.Sinyaller.ZamanEkseni[S.AralıkSeçici_Baştan.Value]);
            foreach (Açıklama_ a in Tümü)
            {
                if (a.TarihSaat >= t) break;

                a.Görsel.IsVisible = false;
            }

            if (S.Açıklamalar_Çizelgede_Görünsün)
            {
                //aralık seçici kapsamına henüz girmiş olanları göster
                t = S.Tarih.Tarihe(Yardımcıİşlemler.Sinyaller.ZamanEkseni[S.AralıkSeçici_Sondan.Value]);
                for (int i = Tümü.Count - 1; i >= 0; i--)
                {
                    Açıklama_ a = Tümü[i];

                    if (a.TarihSaat > t) a.Görsel.IsVisible = false;
                    else if (a.Görsel.IsVisible) break;
                    else a.Görsel.IsVisible = a.TarihSaat <= t;
                }
            }
        }
    }
}

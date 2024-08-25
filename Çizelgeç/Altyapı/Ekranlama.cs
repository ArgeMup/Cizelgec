// Copyright ArgeMup GNU GENERAL PUBLIC LICENSE Version 3 <http://www.gnu.org/licenses/> <https://github.com/ArgeMup>

using ArgeMup.HazirKod.Ekİşlemler;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Çizelgeç
{
    class Ekranlama
    {
        //Çağırmadan önce sinyaller ve tamponları doldur
        static public void AğaçVeÇizelge_Görsellerini_Üret(string İşAdı)
        {
            S.Ağaç.Nodes.Clear();
            S.Ağaç.CheckBoxes = false;
            S.Ağaç.Nodes.Add("Bekleyiniz");
            Application.DoEvents();

            #region Tamponların Hazırlanması
            if (Yardımcıİşlemler.Sinyaller.ZamanEkseni == null) Yardımcıİşlemler.Sinyaller.ZamanEkseni = Enumerable.Repeat(S.Tarih.Sayıya(DateTime.Now), Yardımcıİşlemler.BilgiToplama.ZamanDilimi_Sayısı).ToArray();
            #endregion

            S.Çizelge.Plot.Clear();
            S.Çizelge.Plot.SetCulture(System.Globalization.CultureInfo.CreateSpecificCulture("tr"));
            S.Çizelge.Plot.YAxis.Label("Tümü", size:(float)(S.AnaEkran.Font.Size * 1.25));
            S.Çizelge.Plot.XAxis.TickLabelStyle(fontSize: (float)(S.AnaEkran.Font.Size * 1.25));
            S.Çizelge.Plot.RightAxis.IsVisible = true; S.Çizelge.Plot.RightAxis.Ticks(true);
            S.Çizelge.Plot.LeftAxis.IsVisible = false; S.Çizelge.Plot.LeftAxis.Ticks(true);
            S.Çizelge.Plot.XAxis.DateTimeFormat(true);
            S.Çizelge.RightClicked -= S.Çizelge.DefaultRightClickEvent;
            S.Çizelge.Configuration.DoubleClickBenchmark = true;
            S.Çizelge.Configuration.Quality = ScottPlot.Control.QualityMode.LowWhileDragging;

            TreeNode t = new TreeNode(İşAdı);
            t.Checked = true;
            t.Expand();

            TreeNode s = t.Nodes.Add("Sinyaller");
            foreach (var sinyal in Sinyaller.Tümü.Values)
            {
                SinyaliAğacaEkle(sinyal, s);
            }
            s.Checked = true;
            s.ExpandAll();

            #region değişkenler grubunun dışarı atılması
            TreeNode dal = AğaçDalındakiDalıBul(s.Nodes, "Değişkenler");
            if (dal != null)
            {
                s.Nodes.Remove(dal);
                dal.Collapse(false);
                t.Nodes.Add(dal);
            }
            #endregion

            #region Açıklamalar
            TreeNode a = t.Nodes.Add("Açıklamalar");
            a.Tag = "Açıklamalar_Çizelgede_Görünsün";
            #endregion

            if (Yardımcıİşlemler.ÖnYüz.CanlıEkranlama)
            {
                #region İşlemler
                TreeNode i = t.Nodes.Add("İşlemler");
                a.Checked = false;
                #endregion

                #region Bağlantılar
                TreeNode b = t.Nodes.Add("Bağlantılar");
                b.Collapse(false);
                #endregion
            }

            S.Ağaç.Nodes.Clear();
            S.Ağaç.CheckBoxes = true;
            if (t.Nodes.Count > 1) S.Ağaç.Nodes.Add(t);
            else
            {
                foreach (TreeNode biri in t.Nodes)
                {
                    S.Ağaç.Nodes.Add(biri);
                }
            }

            #region Açıklamaları Ekleme
            Açıklamalar.Tümü.Clear();
            if (S.AnaEkran.Ölü_Ekranlama != null && S.AnaEkran.Ölü_Ekranlama.BirikenAçıklamalar != null)
            {
                foreach (string açklm in S.AnaEkran.Ölü_Ekranlama.BirikenAçıklamalar)
                {
                    string[] bir_satırdakiler = açklm.Split(';');
                    Color Renk = default;

                    if (bir_satırdakiler.Length >= 4)
                    {
                        try
                        {
                            byte[] r_ler = bir_satırdakiler[3].BaytDizisine_HexYazıdan();
                            Renk = Color.FromArgb(r_ler[0], r_ler[1], r_ler[2]);
                        }
                        catch (Exception) { }
                    }

                    Açıklamalar.Ekle(S.Tarih.Tarihe(S.Tarih.Sayıya(bir_satırdakiler[0])), bir_satırdakiler[2].Replace(Açıklamalar.Vekil_AltSatıraGeç, Environment.NewLine), Renk);
                }
            }
            #endregion

            #region Çizelge Görsellerini Oluştur   
            t.Text = "Grafik hazırlanıyor";
            Application.DoEvents();
            S.Çizdir();
            #endregion

            t.Text = İşAdı + " - " + Yardımcıİşlemler.BilgiToplama.ZamanDilimi_Sayısı;

            S.AralıkSeçici_Baştan.Minimum = 0;
            S.AralıkSeçici_Baştan.Maximum = Yardımcıİşlemler.BilgiToplama.ZamanDilimi_Sayısı - 1;
            S.AralıkSeçici_Baştan.Value = 0;
            S.AralıkSeçici_Sondan.Minimum = 0;
            S.AralıkSeçici_Sondan.Maximum = Yardımcıİşlemler.BilgiToplama.ZamanDilimi_Sayısı - 1;
            S.AralıkSeçici_Sondan.Value = S.AralıkSeçici_Sondan.Maximum;

            S.AnaEkran.Menu_aA_xxx_Click(null, null);
        }

        static public void AğaçVeÇizelge_SonradanEkle(KeyValuePair<string, Sinyal_> Sinyal)
        {
            TreeNode bulunan = null;
            if (Sinyal.Value.Tür == Tür_.Değişken)
            {
                bulunan = AğaçDalındakiDalıBul(S.Ağaç.Nodes[0].Nodes, "Değişkenler");
                if (bulunan == null)
                {
                    bulunan = S.Ağaç.Nodes[0].Nodes.Add("Değişkenler");
                    bulunan.Checked = true;
                }
            }
            else if (Sinyal.Value.Tür == Tür_.Sinyal)
            {
                bulunan = AğaçDalındakiDalıBul(S.Ağaç.Nodes[0].Nodes, "Sinyaller");
                TreeNode dal = AğaçDalındakiDalıBul(bulunan.Nodes, "Tanımlanmamış Sinyaller");
                if (dal != null) bulunan = dal;
                else
                {
                    bulunan = bulunan.Nodes.Add("Tanımlanmamış Sinyaller");
                    bulunan.Checked = true;
                }

                Sinyal.Value.Güncelle_Adı(Sinyal.Key, "Tanımlanmamış Sinyaller");
            }

            SinyaliAğacaEkle(Sinyal.Value, bulunan);
        }

        static public TreeNode AğaçDalındakiDalıBul(TreeNodeCollection Dalları, string Aranan)
        {
            foreach (TreeNode dal in Dalları)
            {
                if (dal.Text == Aranan) return dal;
            }
            
            return null;
        }

        static public TreeNode AğacaDalEkle(string Dal, TreeNode Üstteki = null)
        {
            List<string> l = new List<string>();
            l.AddRange(Dal.Split('|'));

            if (Üstteki == null) Üstteki = AğaçDalındakiDalıBul(S.Ağaç.Nodes[0].Nodes, "Sinyaller");

            while (l.Count > 0)
            {
                TreeNode bulunan = AğaçDalındakiDalıBul(Üstteki.Nodes, l[0]);
                if (bulunan == null) Üstteki = Üstteki.Nodes.Add(l[0]);
                else Üstteki = bulunan;

                Üstteki.Checked = true;
                l.RemoveAt(0);
            }

            return Üstteki;
        }

        static public void SinyaliAğacaEkle(Sinyal_ Sinyal, TreeNode Üstteki = null)
        {
            Üstteki = AğacaDalEkle(Sinyal.Adı.Salkım + "|" + Sinyal.Adı.GörünenAdı, Üstteki);

            if (Üstteki.Tag != null) throw new Exception(Sinyal.Adı.Salkım + "|" + Sinyal.Adı.GörünenAdı + " zaten eklendi");

            Üstteki.Tag = Sinyal;
            Sinyal.Görseller.Dal = Üstteki;
            ÇizikVeTamponuHazırla(Sinyal);
        }
        static public void ÇizikVeTamponuHazırla(Sinyal_ Sinyal)
        {
            if (Sinyal.Değeri.DeğerEkseni == null) Sinyal.Değeri.DeğerEkseni = new double[Yardımcıİşlemler.BilgiToplama.ZamanDilimi_Sayısı];
            if (Sinyal.Görseller.Çizikler == null)
            {
                Sinyal.Görseller.Çizikler = S.Çizelge.Plot.AddSignalXY(Yardımcıİşlemler.Sinyaller.ZamanEkseni, Sinyal.Değeri.DeğerEkseni);
                Sinyal.Görseller.Çizikler.LineWidth = S.Çizelge_ÇizgiKalınlığı;
                Sinyal.Görseller.Çizikler.MarkerSize = (S.Çizelge_ÇizgiKalınlığı * (float)1.5) + 5;
                Sinyal.Görseller.Çizikler.MinRenderIndex = S.AralıkSeçici_Baştan.Value;
                Sinyal.Görseller.Çizikler.MaxRenderIndex = S.AralıkSeçici_Sondan.Value;
                Sinyal.Görseller.Dal.ForeColor = Sinyal.Görseller.Çizikler.Color;
                Sinyal.Görseller.Çizikler.StepDisplay = true;

                Sinyal.Görseller.SeçiliOlanNokta = S.Çizelge.Plot.AddMarker(0, 0);
                Sinyal.Görseller.SeçiliOlanNokta.MarkerSize = Sinyal.Görseller.Çizikler.MarkerSize * 1.5f;
                Sinyal.Görseller.SeçiliOlanNokta.MarkerShape = ScottPlot.MarkerShape.openCircle;
                Sinyal.Görseller.SeçiliOlanNokta.MarkerColor = Color.Black;
                Sinyal.Görseller.SeçiliOlanNokta.MarkerLineWidth = Sinyal.Görseller.Çizikler.MarkerSize / 2;
                Sinyal.Görseller.SeçiliOlanNokta.IsVisible = false;

                Sinyal.Görseller.SeçiliOlanNokta_Yazı = S.Çizelge.Plot.AddText(".", 0, 0, S.AnaEkran.Font.Size, Color.Black);
                Sinyal.Görseller.SeçiliOlanNokta_Yazı.BorderSize = 2;
                Sinyal.Görseller.SeçiliOlanNokta_Yazı.BorderColor =  Sinyal.Görseller.Çizikler.Color;
                Sinyal.Görseller.SeçiliOlanNokta_Yazı.BackgroundColor = Color.White;
                Sinyal.Görseller.SeçiliOlanNokta_Yazı.BackgroundFill = true;
                Sinyal.Görseller.SeçiliOlanNokta_Yazı.IsVisible = false;
            }
        }
    }
}

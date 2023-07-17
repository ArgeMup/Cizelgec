// Copyright ArgeMup GNU GENERAL PUBLIC LICENSE Version 3 <http://www.gnu.org/licenses/> <https://github.com/ArgeMup>

using ScottPlot;
using System;
using System.Collections.Generic;
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

            S.Çizelge.plt.Clear();
            S.Çizelge.plt.SetCulture(System.Globalization.CultureInfo.CreateSpecificCulture("tr"));
            S.Çizelge.plt.YLabel("Tümü", /*fontName: S.AnaEkran.Font.Name,*/ fontSize: (float)(S.AnaEkran.Font.Size * 1.25));
            S.Çizelge.plt.Ticks(dateTimeX: true, /*fontName: S.AnaEkran.Font.Name,*/ fontSize: (float)(S.AnaEkran.Font.Size * 1.25)/*, useMultiplierNotation:true*/);
            S.Çizelge.Configure(/*enableRightClickMenu: false,*/ enableDoubleClickBenchmark: true);
            //S.Çizelge.plt.Style(ScottPlot.Style.Blue3);

            TreeNode t = new TreeNode(İşAdı);
            t.Checked = true;
            t.Expand();

            TreeNode s = t.Nodes.Add("Sinyaller", "Sinyaller");
            foreach (var sinyal in Sinyaller.Tümü.Values)
            {
                SinyaliAğacaEkle(sinyal, s);
            }
            s.Checked = true;
            s.ExpandAll();

            #region değişkenler grubunun dışarı atılması
            TreeNode[] dizii = s.Nodes.Find("Değişkenler", false);
            if (dizii != null && dizii.Length > 0)
            {
                s.Nodes.Remove(dizii[0]);
                dizii[0].Collapse(false);
                t.Nodes.Add(dizii[0]);
            }
            #endregion

            //#region Uyarılar
            //TreeNode u = new TreeNode("Uyarılar");
            //u.Checked = false;
            //t.Nodes.Add(u);
            //#endregion

            #region Senaryolar
            //if (Senaryolar.Tümü.Count > 0)
            //{
            //    TreeNode se = t.Nodes.Add("Senaryolar", "Senaryolar");
            //    foreach (var biri in Senaryolar.Tümü.Values)
            //    {
            //        TreeNode y = se.Nodes.Add(biri.Adı);
            //        y.Tag = biri;
            //        biri.Dal = y;
            //    }
            //    se.Collapse(false);
            //}
            #endregion

            #region Bağlantılar
            TreeNode b = t.Nodes.Add("Bağlantılar", "Bağlantılar");
            b.Collapse(false);
            #endregion

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
        }

        static public void AğaçVeÇizelge_SonradanEkle(KeyValuePair<string, Sinyal_> Sinyal)
        {
            TreeNode bulunan = null;
            if (Sinyal.Value.Tür == Tür_.Değişken)
            {
                TreeNode[] dizi;

                if (Sinyal.Key.StartsWith("<ZA ")) //zaman aşımı değişkeni
                {
                    dizi = S.Ağaç.Nodes[0].Nodes.Find("Zaman Aşımları", false);
                    if (dizi != null && dizi.Length > 0) bulunan = dizi[0];
                    else
                    {
                        bulunan = S.Ağaç.Nodes[0].Nodes.Add("Zaman Aşımları", "Zaman Aşımları");
                        bulunan.Checked = true;
                    }
                }
                else //normal değişken
                {
                    dizi = S.Ağaç.Nodes[0].Nodes.Find("Değişkenler", false);
                    if (dizi != null && dizi.Length > 0) bulunan = dizi[0];
                    else
                    {
                        bulunan = S.Ağaç.Nodes[0].Nodes.Add("Değişkenler", "Değişkenler");
                        bulunan.Checked = true;
                    }
                }
            }
            else if (Sinyal.Value.Tür == Tür_.Sinyal)
            {
                bulunan = S.Ağaç.Nodes[0].Nodes.Find("Sinyaller", false)[0];
                TreeNode[] dizi = bulunan.Nodes.Find("Tanımlanmamış Sinyaller", false);
                if (dizi != null && dizi.Length > 0) bulunan = dizi[0];
                else
                {
                    bulunan = bulunan.Nodes.Add("Tanımlanmamış Sinyaller", "Tanımlanmamış Sinyaller");
                    bulunan.Checked = true;
                }

                Sinyal.Value.Güncelle_Adı(Sinyal.Key, "Tanımlanmamış Sinyaller");
            }

            SinyaliAğacaEkle(Sinyal.Value, bulunan);

            var snyler = S.Çizelge.plt.GetPlottables();
            if (Sinyaller.Tümü.Count != snyler.Count)
            {
                while (snyler.Count > 0 && S.Çalışşsın) snyler.RemoveAt(0);
            }
            foreach (Sinyal_ sny in Sinyaller.Tümü.Values)
            {
                S.Çizelge.plt.Add(sny.Görseller.Çizikler);
            }
        }

        static public void SinyaliAğacaEkle(Sinyal_ Sinyal, TreeNode Üstteki = null)
        {
            List<string> l = new List<string>();
            l.AddRange(Sinyal.Adı.Salkım.Split('|'));
            l.Add(Sinyal.Adı.GörünenAdı);

            if (Üstteki == null) Üstteki = S.Ağaç.Nodes[0];
            TreeNode EnÜstteki = Üstteki;

            while (l.Count > 0)
            {
                TreeNode[] bulunan = Üstteki.Nodes.Find(l[0], false);
                if (bulunan == null || bulunan.Length == 0) Üstteki = Üstteki.Nodes.Add(l[0], l[0]);
                else Üstteki = bulunan[0];
                
                Üstteki.Checked = true;
                l.RemoveAt(0);
            }

            if (Üstteki.Tag != null) throw new Exception(Sinyal.Adı.Salkım + "|" + Sinyal.Adı.GörünenAdı + " zaten eklendi");

            Üstteki.Tag = Sinyal;
            Sinyal.Görseller.Dal = Üstteki;
            ÇizikVeTamponuHazırla(Sinyal);
            
            EnÜstteki.ExpandAll();
        }
        static public void ÇizikVeTamponuHazırla(Sinyal_ Sinyal)
        {
            if (Sinyal.Değeri.DeğerEkseni == null) Sinyal.Değeri.DeğerEkseni = new double[Yardımcıİşlemler.BilgiToplama.ZamanDilimi_Sayısı];
            if (Sinyal.Görseller.Çizikler == null)
            {
                Sinyal.Görseller.Çizikler = S.Çizelge.plt.PlotSignalXY(Yardımcıİşlemler.Sinyaller.ZamanEkseni, Sinyal.Değeri.DeğerEkseni);
                Sinyal.Görseller.Çizikler.lineWidth = S.Çizelge_ÇizgiKalınlığı;
                Sinyal.Görseller.Çizikler.markerSize = (S.Çizelge_ÇizgiKalınlığı * (float)1.5) + 5;
                Sinyal.Görseller.Çizikler.minRenderIndex = S.AralıkSeçici_Baştan.Value;
                Sinyal.Görseller.Çizikler.maxRenderIndex = S.AralıkSeçici_Sondan.Value;
                Sinyal.Görseller.Dal.ForeColor = Sinyal.Görseller.Çizikler.color;
                //Sinyal.Uyarı_Yazıları = new List<ScottPlot.PlottableText>();
            }
        }
    }
}

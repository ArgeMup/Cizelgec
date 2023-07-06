// Copyright ArgeMup GNU GENERAL PUBLIC LICENSE Version 3 <http://www.gnu.org/licenses/> <https://github.com/ArgeMup>

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
            if (S.ZamanEkseni == null) S.ZamanEkseni = Enumerable.Repeat(S.Tarih.Sayıya(DateTime.Now), S.CanliÇizdirme_ÖlçümSayısı).ToArray();
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
                if (!string.IsNullOrEmpty(sinyal.Adı.Salkım))
                {
                    TreeNode[] dizi = s.Nodes.Find(sinyal.Adı.Salkım, true);
                    if (dizi != null && dizi.Length > 0)
                    {
                        TreeNode y = dizi[0].Nodes.Add(sinyal.Adı.GörünenAdı);
                        y.Tag = sinyal;
                        y.Checked = true;
                        sinyal.Görseller.Dal = y;
                    }
                    else
                    {
                        string[] dallar = sinyal.Adı.Salkım.Split('|');
                        int konum = 0;
                        TreeNode bulunan = s;

                        while (konum < dallar.Length)
                        {
                            TreeNode[] dal_yeni_dizi = bulunan.Nodes.Find(dallar[konum], false);
                            if (dal_yeni_dizi != null && dal_yeni_dizi.Length > 0)
                            {
                                bulunan = dal_yeni_dizi[0];
                            }
                            else
                            {
                                bulunan = bulunan.Nodes.Add(dallar[konum], dallar[konum]);
                                bulunan.Checked = true;
                            }

                            konum++;
                        }

                        TreeNode y = bulunan.Nodes.Add(sinyal.Adı.GörünenAdı);
                        y.Tag = sinyal;
                        y.Checked = true;
                        sinyal.Görseller.Dal = y;
                    }
                }
                else
                {
                    TreeNode y = s.Nodes.Add(sinyal.Adı.GörünenAdı);
                    y.Tag = sinyal;
                    y.Checked = true;
                    sinyal.Görseller.Dal = y;
                }

                #region Tamponların Hazırlanması
                if (sinyal.Değeri.DeğerEkseni == null) sinyal.Değeri.DeğerEkseni = new double[S.CanliÇizdirme_ÖlçümSayısı];
                sinyal.Görseller.Çizikler = S.Çizelge.plt.PlotSignalXY(S.ZamanEkseni, sinyal.Değeri.DeğerEkseni);
                sinyal.Görseller.Çizikler.lineWidth = S.Çizelge_ÇizgiKalınlığı;
                sinyal.Görseller.Çizikler.markerSize = (S.Çizelge_ÇizgiKalınlığı * (float)1.5) + 5;
                sinyal.Görseller.Dal.ForeColor = sinyal.Görseller.Çizikler.color;
                //sinyal.Uyarı_Yazıları = new List<ScottPlot.PlottableText>();
                #endregion
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
            if (Senaryolar.Tümü.Count > 0)
            {
                TreeNode se = t.Nodes.Add("Senaryolar", "Senaryolar");
                foreach (var biri in Senaryolar.Tümü.Values)
                {
                    TreeNode y = se.Nodes.Add(biri.Adı);
                    y.Tag = biri;
                    biri.Dal = y;
                }
                se.Collapse(false);
            }
            #endregion

            #region Bağlantılar
            if (Bağlantılar.Tümü.Count > 0)
            {
                TreeNode b = t.Nodes.Add("Bağlantılar", "Bağlantılar");;
                foreach (var biri in Bağlantılar.Tümü.Values)
                {
                    TreeNode y = b.Nodes.Add(biri.Adı);
                    y.Tag = biri;
                    biri.Dal = y;

                    y.Nodes.Add("Son alınan bilgiler");
                }
                b.Collapse(false);
            }
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

            t.Text = İşAdı + " - " + S.CanliÇizdirme_ÖlçümSayısı;

            S.AralıkSeçici_Baştan.Minimum = 0;
            S.AralıkSeçici_Baştan.Maximum = S.CanliÇizdirme_ÖlçümSayısı - 1;
            S.AralıkSeçici_Baştan.Value = 0;
            S.AralıkSeçici_Sondan.Minimum = 0;
            S.AralıkSeçici_Sondan.Maximum = S.CanliÇizdirme_ÖlçümSayısı - 1;
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

            bulunan = bulunan.Nodes.Add(Sinyal.Value.Adı.GörünenAdı);
			bulunan.Checked = true;
            bulunan.ExpandAll();
            bulunan.Tag = Sinyal.Value;
            Sinyal.Value.Görseller.Dal = bulunan;

            if (Sinyal.Value.Değeri.DeğerEkseni == null) Sinyal.Value.Değeri.DeğerEkseni = new double[S.CanliÇizdirme_ÖlçümSayısı];
            if (Sinyal.Value.Görseller.Çizikler == null)
            {
                Sinyal.Value.Görseller.Çizikler = S.Çizelge.plt.PlotSignalXY(S.ZamanEkseni, Sinyal.Value.Değeri.DeğerEkseni);
                Sinyal.Value.Görseller.Çizikler.lineWidth = S.Çizelge_ÇizgiKalınlığı;
                Sinyal.Value.Görseller.Çizikler.markerSize = (S.Çizelge_ÇizgiKalınlığı * (float)1.5) + 5;
                Sinyal.Value.Görseller.Çizikler.minRenderIndex = S.AralıkSeçici_Baştan.Value;
                Sinyal.Value.Görseller.Çizikler.maxRenderIndex = S.AralıkSeçici_Sondan.Value;
                Sinyal.Value.Görseller.Dal.ForeColor = Sinyal.Value.Görseller.Çizikler.color;
            }
        }
    }
}

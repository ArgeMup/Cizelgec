using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Çizelgeç
{
    public class Ekranlama_Canlı
    {
        UInt32 Sayac_Ölçüm = 0;
        string İşAdı = "";
        public Ekranlama_Canlı(string İşAdı, string AyarlarDosyasıYolu)
        {
            S.Ağaç.Nodes.Clear();
            S.Ağaç.CheckBoxes = false;
            S.Ağaç.Nodes.Add("Bekleyiniz");
            Application.DoEvents();

            Günlük.Ekle("Ayıklanıyor : " + AyarlarDosyasıYolu + "\\Ayarlar.json", "Bilgi");
            json_Ayıkla.Ayarlar(AyarlarDosyasıYolu + "\\Ayarlar.json");

            string[] snylar = Directory.GetFiles(AyarlarDosyasıYolu + "\\", "Senaryo*.json", SearchOption.TopDirectoryOnly);
            foreach (var sny in snylar) { Günlük.Ekle("Ayıklanıyor : " + sny, "Bilgi"); json_Ayıkla.Senaryo(sny); }

            Çevirici.Yazıdan_NoktalıSayıya(S.BilgiToplama_Kıstas);
            Çevirici.Yazıdan_NoktalıSayıya(S.BilgiToplama_ZamanAralığı_Sn);
            Çevirici.Yazıdan_NoktalıSayıya(S.Dosyalama_AzamiDosyaBoyutu_Bayt);

            if (!string.IsNullOrEmpty(S.Dosyalama_KayıtKlasörü))
            {
                S.Dosyalama_KayıtKlasörü = S.Dosyalama_KayıtKlasörü.Trim('\\', ' ') + "\\";
                Directory.CreateDirectory(S.Dosyalama_KayıtKlasörü);
                File.WriteAllText(S.Dosyalama_KayıtKlasörü + "silinecek.mup", "deneme");
                if (!File.Exists(S.Dosyalama_KayıtKlasörü + "silinecek.mup")) throw new Exception(S.Dosyalama_KayıtKlasörü + " içerisinde dosya oluşturma denemesi başarısız");
                File.Delete(S.Dosyalama_KayıtKlasörü + "silinecek.mup");
            }
            else Günlük.Ekle("Dosyalama KayıtKlasörü seçilmedi, ölçümler kaydedilmeyecek.");

            this.İşAdı = İşAdı;
            TreeNode t = new TreeNode(İşAdı);
            t.Checked = true;
            t.Expand();

            #region Sinyaller
            TreeNode s = new TreeNode("Sinyaller");
            foreach (var biri in Sinyaller.Tümü)
            {
                if (biri.Value.Tür != Tür_.Sinyal) continue;

                if (!string.IsNullOrEmpty(biri.Value.Adı.Grup))
                {
                    TreeNode[] dizi = s.Nodes.Find(biri.Value.Adı.Grup, false);
                    if (dizi != null && dizi.Length > 0)
                    {
                        TreeNode y = dizi[0].Nodes.Add(biri.Value.Adı.Dal);
                        y.Tag = biri.Value;
                        y.Checked = true;
                        biri.Value.Dal = y;
                    }
                    else
                    {
                        TreeNode alt = s.Nodes.Add(biri.Value.Adı.Grup, biri.Value.Adı.Grup);
                        //alt.Tag = biri.Value.Ölçüm;
                        alt.Checked = true;
                        TreeNode y = alt.Nodes.Add(biri.Value.Adı.Dal);
                        y.Tag = biri.Value;
                        y.Checked = true;
                        biri.Value.Dal = y;
                    }
                }
                else
                {
                    TreeNode y = s.Nodes.Add(biri.Value.Adı.Uzun);
                    y.Tag = biri.Value;
                    y.Checked = true;
                    biri.Value.Dal = y;
                }
            }
            s.Checked = true;
            s.ExpandAll();
            t.Nodes.Add(s);
            #endregion

            #region Değişkenler
            TreeNode d = new TreeNode("Değişkenler");
            foreach (var biri in Sinyaller.Tümü)
            {
                if (biri.Value.Tür != Tür_.Değişken) continue;

                TreeNode y = d.Nodes.Add(biri.Value.Adı.Uzun);
                y.Tag = biri.Value;
                y.Checked = true;
                biri.Value.Dal = y;
            }
            d.Checked = true;
            d.Collapse(false);
            t.Nodes.Add(d);
            #endregion

            #region Senaryolar
            TreeNode se = new TreeNode("Senaryolar");
            foreach (var biri in Senaryolar.Tümü)
            {
                TreeNode y = se.Nodes.Add(biri.Value.Adı);
                y.Tag = biri.Value;
                biri.Value.Dal = y;
            }
            se.ExpandAll();
            t.Nodes.Add(se);
            #endregion

            #region Bağlantılar
            TreeNode b = new TreeNode("Bağlantılar");
            foreach (var biri in Bağlantılar.Tümü.Values)
            {
                TreeNode y = b.Nodes.Add(biri.Adı);
                y.Tag = biri;
                biri.Dal = y;

                y.Nodes.Add("Son alınan ölçümler");

                y.ToolTipText = biri.Adı + Environment.NewLine + biri.Yöntem + Environment.NewLine + biri.P1 + Environment.NewLine + biri.P2;
            }
            b.Collapse(false);
            t.Nodes.Add(b);
            #endregion

            #region Tamponların Hazırlanması
            S.ZamanEkseni = Enumerable.Repeat(S.Tarih.Sayıya(DateTime.Now), S.CanliÇizdirme_ÖlçümSayısı).ToArray();
            #endregion

            #region Çizelge Görsellerini Oluştur
            S.Çizelge.Reset();
            S.Çizelge.plt.Clear();
            S.Çizelge.plt.SetCulture(System.Globalization.CultureInfo.CreateSpecificCulture("tr"));
            S.Çizelge.plt.YLabel("Tümü", /*fontName: S.AnaEkran.Font.Name,*/ fontSize: (float)(S.AnaEkran.Font.Size * 1.25));
            S.Çizelge.plt.Ticks(dateTimeX: true, /*fontName: S.AnaEkran.Font.Name,*/ fontSize: (float)(S.AnaEkran.Font.Size * 1.25)/*, useMultiplierNotation:true*/);
            S.Çizelge.Configure(enableRightClickMenu: false, enableDoubleClickBenchmark: true);
            //S.Çizelge.plt.Style(ScottPlot.Style.Blue3);
            #endregion

            foreach (var biri in Bağlantılar.Tümü)
            {
                Günlük.Ekle("Çalıştırılıyor -> " + biri.Value.Yöntem + " -> " + biri.Value.P1 + " -> " + biri.Value.P2, "Bilgi");
                biri.Value.Başlat();
            }

            S.Ağaç.Nodes.Clear();
            S.Ağaç.CheckBoxes = true;
            S.Ağaç.Nodes.Add(t);

            new Thread(() => Çalıştır_Ölçme_Değerlendirme()).Start();
            new Thread(() => Çalıştır_Ekranlama()).Start();
        }

        public void Çalıştır_Ölçme_Değerlendirme()
        {
            while (S.Çalışşsın)
            {
                try
                {
                    if (Çevirici.Yazıdan_NoktalıSayıya(S.BilgiToplama_Kıstas) > 0.0)
                    {
                        int toplam = Sinyaller.Tümü.Count;
                        double[] kayıt_dizisi = new double[toplam + 1];
                        double şimdi = S.Tarih.Sayıya(DateTime.Now);
                        kayıt_dizisi[toplam] = şimdi;

                        for (int i = 0; i < toplam; i++)
                        {
                            Sinyal_ biri = Sinyaller.Tümü.ElementAt(i).Value;

                            kayıt_dizisi[i] = biri.Güncelle_Dizi();

                            if (!biri.Değeri.Kaydedilsin) continue;

                            if (!biri.Değeri.ZamanAşımıOldu)
                            {
                                if (biri.Hesaplamalar != null)
                                {
                                    foreach (var hsp in biri.Hesaplamalar)
                                    {
                                        Sinyaller.Yaz(hsp.Değişken, Çevirici.Yazıdan_NoktalıSayıya(hsp.İşlem.Replace("<Sinyal>", S.Sayı.Yazıya(kayıt_dizisi[i]))));
                                    }
                                }

                                if (biri.Uyarılar != null)
                                {
                                    foreach (var uyr in biri.Uyarılar)
                                    {
                                        string kıstas = uyr.Kıstas.Replace("<Sinyal>", S.Sayı.Yazıya(kayıt_dizisi[i]));
                                        if (Çevirici.Yazıdan_NoktalıSayıya(kıstas) > 0.0)
                                        {
                                            string mesaj = Çevirici.Uyarıdan_Yazıya(uyr.Açıklama, kayıt_dizisi[i]);
                                            mesaj = biri.Adı.Csv + ";" + S.Sayı.Yazıya(kayıt_dizisi[i]) + ";" + mesaj;

                                            Kaydedici.Ekle(new double[1] { kayıt_dizisi[kayıt_dizisi.Length - 1] }, mesaj);
                                            Günlük.Ekle(mesaj);
                                        }
                                    }
                                }
                            }
                            else
                            {
                                string mesaj = biri.Adı.Csv + ";" + kayıt_dizisi[i].ToString() + ";Zaman Aşımı";

                                Kaydedici.Ekle(new double[1] { kayıt_dizisi[kayıt_dizisi.Length - 1] }, mesaj);
                                Günlük.Ekle(mesaj);
                            }
                        }

                        Array.Copy(S.ZamanEkseni, 1, S.ZamanEkseni, 0, S.CanliÇizdirme_ÖlçümSayısı - 1);
                        S.ZamanEkseni[S.CanliÇizdirme_ÖlçümSayısı - 1] = şimdi;
                        Kaydedici.Ekle(kayıt_dizisi);

                        int gecikme = (int)(Çevirici.Yazıdan_NoktalıSayıya(S.BilgiToplama_ZamanAralığı_Sn) * 1000);
                        if (gecikme < 100) gecikme = 100;
                        while (gecikme > 1500 && S.Çalışşsın)
                        {
                            Thread.Sleep(1500);
                            gecikme -= 1500;
                        }
                        Thread.Sleep(gecikme);

                        Sayac_Ölçüm++;
                    }
                }
                catch (Exception ex) { Günlük.Ekle(ex.ToString()); Thread.Sleep(1000); }
            }
        }
        public void Çalıştır_Ekranlama()
        {
            int son_grafik_tarama_süresi = 1000;

            while (S.Çalışşsın)
            {
                try
                {
                    Thread.Sleep(1000);

                    S.EkranıGüncelle_me = true;
                    S.Çizelge.Invoke((Action)(() =>
                    {
                        S.Ağaç.BeginUpdate();
                        S.Ağaç.Nodes[0].Text = İşAdı + " - " + Sayac_Ölçüm + " - " + S.Tarih.Yazıya(DateTime.Now);
                        S.Ağaç.Nodes[0].ToolTipText = Sinyaller.Tümü.Count + " adet sinyal" + Environment.NewLine +
                                                      GelenBilgiler.Tümü.Count + " adet ayıklanmayı bekleyen girdi" + Environment.NewLine +
                                                      Kaydedici.Tümü.Count + " adet yazılmayı bekleyen ölçüm";

                        for (int i = 0; i < Sinyaller.Tümü.Count; i++)
                        {
                            Sinyal_ biri = Sinyaller.Tümü.Values.ElementAt(i);

                            if (biri.Dal == null)
                            {
                                TreeNode y = null;
                                if (biri.Tür == Tür_.Değişken)
                                {
                                    y = S.Ağaç.Nodes[0].Nodes[1].Nodes.Add(biri.Adı.Uzun);
                                    y.Checked = false;
                                }
                                else if (biri.Tür == Tür_.Sinyal)
                                {
                                    y = S.Ağaç.Nodes[0].Nodes[0].Nodes.Add(biri.Adı.Uzun);
                                    y.Checked = true;
                                    S.Ağaç.Nodes[0].Nodes[0].ExpandAll();
                                }

                                y.Tag = biri;
                                biri.Dal = y;
                            }

                            if (biri.Çizikler == null)
                            {
                                if (biri.Değeri.DeğerEkseni != null)
                                {
                                    biri.Çizikler = S.Çizelge.plt.PlotSignalXY(S.ZamanEkseni, biri.Değeri.DeğerEkseni);
                                    biri.Çizikler.minRenderIndex = S.Kaydırıcı.Value;
                                    biri.Çizikler.maxRenderIndex = S.Kaydırıcı.Value + S.AralıkSeçici.Value;
                                    biri.Dal.ForeColor = biri.Çizikler.color;
                                }
                            }

                            biri.Dal.Text = biri.Adı.Dal + " : " + S.Sayı.Yazıya(biri.Değeri.SonDeğeri);
                            if (biri.Değeri.ZamanAşımıOldu)
                            {
                                biri.Dal.Text += " -> Zaman Aşımı -> ";

                                TimeSpan fark = DateTime.Now - biri.Değeri.SonDeğerinAlındığıAn;
                                if (fark.TotalHours > 1) biri.Dal.Text += biri.Değeri.SonDeğerinAlındığıAn.ToString();
                                else biri.Dal.Text += ArgeMup.HazirKod.Dönüştürme.D_Süre.Metne.SaatDakikaSaniye(0, 0, (int)fark.TotalSeconds);
                            }
                        }

                        foreach (var biri in Bağlantılar.Tümü.Values)
                        {
                            if (biri.Dal.Nodes[0].IsVisible)
                            {
                                int adet = biri.SonGelenBilgiler.Count;
                                for (int i = 0; i < adet; i++)
                                {
                                    biri.Dal.Nodes.Add(biri.SonGelenBilgiler[i]);
                                    if (biri.Dal.Nodes.Count > 10) biri.Dal.Nodes.RemoveAt(0);
                                }
                                biri.SonGelenBilgiler.Clear();
                            }
                        }

                        if (S.Ağaç.Nodes[0].Nodes[2].IsVisible)
                        {
                            foreach (var sny in Senaryolar.Tümü.Values)
                            {
                                sny.Dal.Text = sny.Adı + sny.Durum;
                            }
                        }

                        S.Ağaç.EndUpdate();

                        if (son_grafik_tarama_süresi >= 1000) son_grafik_tarama_süresi -= 1000;
                        else
                        {
                            son_grafik_tarama_süresi = Environment.TickCount;

                            S.Çizdir();
                            son_grafik_tarama_süresi = Environment.TickCount - son_grafik_tarama_süresi;

                            if (son_grafik_tarama_süresi > 5000) son_grafik_tarama_süresi = 5000;
                        }
                    }));
                }
                catch (Exception ex) { Günlük.Ekle(ex.ToString()); Thread.Sleep(1000); }

                S.EkranıGüncelle_me = false;
            }
        }
    }
}

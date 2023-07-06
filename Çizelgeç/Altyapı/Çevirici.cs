// Copyright ArgeMup GNU GENERAL PUBLIC LICENSE Version 3 <http://www.gnu.org/licenses/> <https://github.com/ArgeMup>

using System;
using System.Data;
using System.Linq;
using System.Text.Json;

namespace Çizelgeç
{
    class Çevirici
    {
        static public double Yazıdan_NoktalıSayıya(string Girdi, int ÖlçümNo = -1)
        {
            //<Değişken Adı> veya 1 veya <TM[0]> veya dört işlem AND OR NOT < > <= >= <> = + - * / %
            string Çıktı = Girdi;

            try
            {
                for (int başlangıç = 0; başlangıç < Girdi.Length && S.Çalışşsın; başlangıç++)
                {
                    if (Girdi[başlangıç] == '<')
                    {
                        for (int bitiş = başlangıç + 1; bitiş < Girdi.Length && S.Çalışşsın; bitiş++)
                        {
                            if (Girdi[bitiş] == '>')
                            {
                                string sinyal = Girdi.Substring(başlangıç, bitiş - başlangıç + 1);
                                if (Sinyaller.MevcutMu(sinyal))
                                {
                                    if (ÖlçümNo < 0)
                                    {
                                        Çıktı = Çıktı.Replace(sinyal, S.Sayı.Yazıya(Sinyaller.Oku(sinyal)));
                                    }
                                    else
                                    {
                                        Çıktı = Çıktı.Replace(sinyal, S.Sayı.Yazıya(Sinyaller.Bul(sinyal).Değeri.DeğerEkseni[ÖlçümNo]));
                                    }

                                    Girdi = Girdi.Replace(sinyal, "");
                                }

                                break;
                            }
                        }
                    }
                }

                Girdi = Çıktı;

                while (S.Çalışşsın)
                {
                    for (int başlangıç = 0; başlangıç < Girdi.Length && S.Çalışşsın; başlangıç++)
                    {
                        if (Girdi[başlangıç] == '(')
                        {
                            for (int bitiş = başlangıç + 1; bitiş < Girdi.Length && S.Çalışşsın; bitiş++)
                            {
                                if (Girdi[bitiş] == '(')
                                {
                                    başlangıç = bitiş;
                                    continue;
                                }

                                if (Girdi[bitiş] == ')')
                                {
                                    string işlem = Girdi.Substring(başlangıç, bitiş - başlangıç + 1);
                                    double sonuc_işlem = 0;
                                    try
                                    {
                                        işlem = işlem.Replace('.', S.Sayı.ondalık_ayraç).Replace(',', S.Sayı.ondalık_ayraç);
                                        sonuc_işlem = Convert.ToDouble(new DataTable().Compute(işlem, null), System.Globalization.CultureInfo.InvariantCulture);

                                        Girdi = Girdi.Replace(işlem, "");
                                        Çıktı = Çıktı.Replace(işlem, S.Sayı.Yazıya(sonuc_işlem));
                                    }
                                    catch (Exception) { }

                                    break;
                                }
                            }
                        }
                    }

                    if (string.Compare(Girdi, Çıktı) == 0) break;
                    Girdi = Çıktı;
                }

                Çıktı = Çıktı.Replace('.', S.Sayı.ondalık_ayraç).Replace(',', S.Sayı.ondalık_ayraç);
                return Convert.ToDouble(new DataTable().Compute(Çıktı, null), System.Globalization.CultureInfo.InvariantCulture);
            } 
            catch (Exception ex)
            {
                if (ex.Message.Contains("listede bulunamadı")) throw ex;
                else throw new Exception(Girdi + " yazısı sayıya çevirilemedi");
            }  
        }
        static private string Yazıdan_NoktalıSayıya_Parantesliİşlem(string Girdi)
        {
            int buyuktur = Girdi.LastIndexOf(')');

            for (int i = buyuktur; i >= 0; i--)
            {
                if (Girdi[i] == '(')
                {
                    string parantesli_işlem = Girdi.Substring(i, buyuktur - i + 1);
                    if (parantesli_işlem.Count(x => x == ')') > 1) parantesli_işlem = parantesli_işlem.Substring(0, parantesli_işlem.IndexOf(')') + 1);

                    string parantesli_işlem_2 = parantesli_işlem.Replace('.', S.Sayı.ondalık_ayraç).Replace(',', S.Sayı.ondalık_ayraç);
                    double par_işl_sonuç = Convert.ToDouble(new DataTable().Compute(parantesli_işlem_2, null), System.Globalization.CultureInfo.InvariantCulture);

                    Girdi = Girdi.Replace(parantesli_işlem, S.Sayı.Yazıya(par_işl_sonuç));
                    break;
                }
            }

            return Girdi;
        }

        static public string Uyarıdan_Yazıya(string[] Uyarı, double Sinyal = double.NaN)
        {
            string mesaj = "";
            foreach (var a in Uyarı)
            {
                string b = a.Replace("<Sinyal>", S.Sayı.Yazıya(Sinyal));
                b = b.Replace("<TarihSaat>", S.Tarih.Yazıya_TarihSaat(DateTime.Now));
                try
                {
                    mesaj += Yazıdan_NoktalıSayıya(b);
                }
                catch (Exception) { mesaj += b; }
            }

            return mesaj;
        }
        static public string Uyarıdan_Yazıya(JsonElement Uyarı)
        {
            if (Uyarı.ValueKind != JsonValueKind.Array) return "";

            string[] dizi = new string[Uyarı.GetArrayLength()];
            for (int x = 0; x < dizi.Length; x++) json_Ayıkla.Oku(Uyarı[x], "", out dizi[x]);

            return Uyarıdan_Yazıya(dizi);
        }
    };
}

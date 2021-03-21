using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Çizelgeç
{
    class Çevirici
    {
        static public double Yazıdan_NoktalıSayıya(string Girdi)
        {
            //<Değişken Adı> veya 1 veya <TM[0]> veya dört işlem karakterleri
            
            try
            {
                while (Girdi.Contains("<") && Girdi.Contains(">") && S.Çalışşsın)
                {
                    int buyuktur = Girdi.LastIndexOf('>');

                    for (int i = buyuktur; i >= 0; i--)
                    {
                        if (Girdi[i] == '<')
                        {
                            string değişken = Girdi.Substring(i, buyuktur - i + 1);
                            if (değişken.Count(x => x == '>') > 1) değişken = değişken.Substring(i, değişken.IndexOf('>') + 1);

                            Girdi = Girdi.Replace(değişken, S.Sayı.Yazıya(Sinyaller.Oku(değişken)));
                            break;
                        }
                    }
                }

                Girdi = Girdi.Replace('.', S.Sayı.ondalık_ayraç).Replace(',', S.Sayı.ondalık_ayraç);
                return Convert.ToDouble(new DataTable().Compute(Girdi, null), System.Globalization.CultureInfo.InvariantCulture);
            } 
            catch (Exception ex)
            {
                if (ex.Message.Contains("listede bulunamadı")) throw ex;
                else throw new Exception(Girdi + " yazısı sayıya çevirilemedi");
            }  
        }

        static public string Uyarıdan_Yazıya(string[] Uyarı, double Sinyal = double.NaN)
        {
            string mesaj = "";
            foreach (var a in Uyarı)
            {
                string b = a.Replace("<Sinyal>", S.Sayı.Yazıya(Sinyal));
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

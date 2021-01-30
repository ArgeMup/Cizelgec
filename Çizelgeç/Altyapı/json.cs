using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;

namespace Çizelgeç
{
    class json_Ayıkla
    {
        static public bool Oku(JsonElement json, string Anahtar, JsonValueKind Tür, out JsonElement Hedef)
        {
            if (json.TryGetProperty(Anahtar, out Hedef))
            {
                if (Hedef.ValueKind == Tür)
                {
                    if (Tür == JsonValueKind.Array && Hedef.GetArrayLength() == 0) return false;

                    return true;
                }
            }

            return false;
        }
        static public bool Oku(JsonElement json, string Anahtar, out string Hedef, string BulunamamasıDurumundakiDeğeri = "")
        {
            if (string.IsNullOrEmpty(Anahtar))
            {
                if (json.ValueKind == JsonValueKind.String)
                {
                    Hedef = json.GetString();
                    return true;
                }
            }
            else
            {
                if (json.TryGetProperty(Anahtar, out JsonElement j))
                {
                    if (j.ValueKind == JsonValueKind.String)
                    {
                        Hedef = j.GetString();
                        return true;
                    }
                }
            }

            Hedef = BulunamamasıDurumundakiDeğeri;
            return false;
        }
   
        static public void Ayarlar(string yol)
        {
            if (!File.Exists(yol)) throw new Exception(yol + " dosyası açılamadı");

            JsonDocument j = JsonDocument.Parse(File.ReadAllBytes(yol));

            if (Oku(j.RootElement, "Ölçümler", JsonValueKind.Array, out JsonElement j1))
            {
                for (int i = 0; i < j1.GetArrayLength(); i++)
                {
                    Ölçüm_ ölçüm = new Ölçüm_();
                    if (!Oku(j1[i], "Adı", out ölçüm.Adı)) { Günlük.Ekle("Ölçümler başlığı dizi elemanı " + (i+1) + " Adı anahtarı yok veya uygun değil", "Bilgi"); continue; }
                    if (!Oku(j1[i], "Birimi", out ölçüm.Birimi)) { Günlük.Ekle("Ölçümler başlığı dizi elemanı " + (i + 1) + " Birimi anahtarı yok veya uygun değil", "Bilgi"); continue; }
                    if (!Oku(j1[i], "İşlem", out ölçüm.İşlem)) { Günlük.Ekle("Ölçümler başlığı dizi elemanı " + (i + 1) + " İşlem anahtarı yok veya uygun değil", "Bilgi"); continue; }
                    if (!ölçüm.İşlem.Contains("<Sinyal>")) { Günlük.Ekle("Ölçümler başlığı dizi elemanı " + (i + 1) + " İşlem anahtarında <Sinyal> bilgisi yok", "Bilgi"); continue; }

                    if (Oku(j1[i], "Sinyaller", JsonValueKind.Array, out JsonElement j2))
                    {
                        for (int x = 0; x < j2.GetArrayLength(); x++)
                        {
                            if (!Oku(j2[x], "", out string adı)) { Günlük.Ekle("Ölçümler başlığı dizi elemanı " + (i + 1) + " Sinyaller anahtarı dizi elemanı " + (x+1) + "yok veya uygun değil", "Bilgi"); continue; }
                            Sinyaller.Ekle(adı).Güncelle_Ölçüm(ölçüm, adı);
                        }
                    }
                }
            }
            else Günlük.Ekle("Ölçümler başlığı yok veya uygun değil", "Bilgi");

            if (Oku(j.RootElement, "Uyarılar", JsonValueKind.Array, out j1))
            {
                for (int i = 0; i < j1.GetArrayLength(); i++)
                {
                    Uyarı_ Uyarı = new Uyarı_();
                    if (!Oku(j1[i], "Kıstas", out Uyarı.Kıstas)) { Günlük.Ekle("Uyarılar başlığı dizi elemanı " + (i + 1) + " Kıstas anahtarı yok veya uygun değil", "Bilgi"); continue; }

                    if (Oku(j1[i], "Uyarı", JsonValueKind.Array, out JsonElement j2))
                    {
                        Uyarı.Uyarı = new string[j2.GetArrayLength()];
                        for (int x = 0; x < j2.GetArrayLength(); x++) Oku(j2[x], "", out Uyarı.Uyarı[x]);

                        if (Oku(j1[i], "Sinyaller", JsonValueKind.Array, out j2))
                        {
                            for (int x = 0; x < j2.GetArrayLength(); x++)
                            {
                                if (Oku(j2[x], "", out string str)) Sinyaller.Ekle(str).Uyarı = Uyarı;
                            }
                        } 
                    } 
                }
            }
            else Günlük.Ekle("Uyarılar başlığı yok veya uygun değil", "Bilgi");

            if (Oku(j.RootElement, "Bilgi Toplama", JsonValueKind.Object, out j1))
            {
                if (!Oku(j1, "Zaman Aralığı (Saniye)", out S.BilgiToplama_ZamanAralığı_Sn, "15")) Günlük.Ekle("Bilgi Toplama başlığı Zaman Aralığı (Saniye) anahtarı yok veya uygun değil", "Bilgi");
                if (!Oku(j1, "Kıstas", out S.BilgiToplama_Kıstas, "1")) Günlük.Ekle("Bilgi Toplama başlığı Kıstas anahtarı yok veya uygun değil", "Bilgi");
                if (!Oku(j1, "Zaman Aşımı (Saniye)", out S.BilgiToplama_ZamanAşımı_Sn, "60")) Günlük.Ekle("Bilgi Toplama başlığı Zaman Aşımı (Saniye) anahtarı yok veya uygun değil", "Bilgi");
                if (!Oku(j1, "Sinyaller Cümle Başlangıcı", out S.BilgiToplama_SinyallerCümleBaşlangıcı, ">Sinyaller")) Günlük.Ekle("Bilgi Toplama başlığı Sinyaller Cümle Başlangıcı anahtarı yok veya uygun değil", "Bilgi");
                if (!Oku(j1, "Kelime Ayracı", out string qw, ";")) Günlük.Ekle("Bilgi Toplama başlığı Kelime Ayracı anahtarı yok veya uygun değil", "Bilgi");
                S.BilgiToplama_KelimeAyracı = qw.ToCharArray()[0];
            }
            else Günlük.Ekle("Bilgi Toplama başlığı yok veya uygun değil", "Bilgi");

            if (Oku(j.RootElement, "Bağlantılar", JsonValueKind.Array, out j1))
            {
                for (int i = 0; i < j1.GetArrayLength(); i++)
                {
                    if (!Oku(j1[i], "Adı", out string Adı)) { Günlük.Ekle("Bağlantılar başlığı dizi elemanı " + (i + 1) + " Adı anahtarı yok veya uygun değil", "Bilgi"); continue; }
                    if (!Oku(j1[i], "Yöntem", out string Yöntem)) { Günlük.Ekle("Bağlantılar başlığı dizi elemanı " + (i + 1) + " Yöntem anahtarı yok veya uygun değil", "Bilgi"); continue; }

                    string P1 = "", P2 = "";
                    if (Yöntem == "Komut Satırı")
                    {
                        if (!Oku(j1[i], "Uygulama", out P1)) { Günlük.Ekle("Bağlantılar başlığı dizi elemanı " + (i + 1) + " Uygulama anahtarı yok veya uygun değil", "Bilgi"); continue; }
                        if (!Oku(j1[i], "Parametre", out P2)) { Günlük.Ekle("Bağlantılar başlığı dizi elemanı " + (i + 1) + " Parametre anahtarı yok veya uygun değil", "Bilgi"); continue; }
                    }
                    else if (Yöntem == "Uart")
                    {
                        if (!Oku(j1[i], "Erişim Noktası", out P1)) { Günlük.Ekle("Bağlantılar başlığı dizi elemanı " + (i + 1) + " Erişim Noktası anahtarı yok veya uygun değil", "Bilgi"); continue; }
                        if (!Oku(j1[i], "Bit Hızı", out P2)) { Günlük.Ekle("Bağlantılar başlığı dizi elemanı " + (i + 1) + " Bit Hızı anahtarı yok veya uygun değil", "Bilgi"); continue; }
                    }
                    else if (Yöntem == "Tcp" || Yöntem == "Udp")
                    {
                        if (!Oku(j1[i], "Sunucu Erişim Noktası", out P1))
                        {
                            if (!Oku(j1[i], "İstemci IP veya Adresi", out P1)) { Günlük.Ekle("Bağlantılar başlığı dizi elemanı " + (i + 1) + " anahtarları yok veya uygun değil", "Bilgi"); continue; }
                            if (!Oku(j1[i], "İstemci Erişim Noktası", out P2)) { Günlük.Ekle("Bağlantılar başlığı dizi elemanı " + (i + 1) + " anahtarları yok veya uygun değil", "Bilgi"); continue; }
                        }
                    }

                    if (Bağlantılar.MevcutMu(Adı)) { throw new Exception(Adı + " isimli bağlantı ikinci kez eklendi."); }

                    Bağlantı_ yeni = Bağlantılar.Ekle(Adı);
                    yeni.Yöntem = Yöntem;
                    yeni.P1 = P1;
                    yeni.P2 = P2;
                }

                if (Bağlantılar.Tümü.Count == 0) throw new Exception("Hiç bağlantı eklenmedi, ölçüm alınamayacak.");
            }
            else Günlük.Ekle("Bağlantılar başlığı yok veya uygun değil", "Bilgi");

            if (Oku(j.RootElement, "Dosyalama", JsonValueKind.Object, out j1))
            {
                if (!Oku(j1, "Azami Dosya Boyutu (Bayt)", out S.Dosyalama_AzamiDosyaBoyutu_Bayt, "1000000")) Günlük.Ekle("Dosyalama başlığı Azami Dosya Boyutu (Bayt) anahtarı yok veya uygun değil", "Bilgi");
                if (!Oku(j1, "Kayıt Klasörü", out S.Dosyalama_KayıtKlasörü, S.Kulanıcı_Klasörü)) Günlük.Ekle("Dosyalama başlığı Kayıt Klasörü anahtarı yok veya uygun değil", "Bilgi");
                if (Oku(j1, "Kaydedilecek Değişkenler", JsonValueKind.Array, out JsonElement j2))
                {
                    for (int i = 0; i < j2.GetArrayLength(); i++)
                    {
                        if (Oku(j2[i], "", out string str)) Sinyaller.Ekle(str).Değeri.Kaydedilsin = true;
                    }
                }
                if (Oku(j1, "Kaydedilmeyecek Sinyaller", JsonValueKind.Array, out j2))
                {
                    for (int i = 0; i < j2.GetArrayLength(); i++)
                    {
                        if (Oku(j2[i], "", out string str)) Sinyaller.Ekle(str).Değeri.Kaydedilsin = false;
                    }
                }
                if (Oku(j1, "Kaydedilmeyecek Bağlantılar", JsonValueKind.Array, out j2))
                {
                    for (int i = 0; i < j2.GetArrayLength(); i++)
                    {
                        if (Oku(j2[i], "", out string str))
                        {
                            if (Bağlantılar.MevcutMu(str))
                            {
                                Bağlantılar.Bul(str).Kaydedilsin = false;
                            }
                        }
                    }
                }
            }
            else Günlük.Ekle("Dosyalama başlığı yok veya uygun değil", "Bilgi");
        }
        static public bool Senaryo(string yol)
        {
            if (!File.Exists(yol)) { Günlük.Ekle(yol + " dosyası açılamadı"); return false; }

            JsonDocument j = JsonDocument.Parse(File.ReadAllBytes(yol));

            Senaryo_ Senaryo = new Senaryo_();
            if (!Oku(j.RootElement, "Adı", out Senaryo.Adı)) { Günlük.Ekle("Senaryo Adı anahtarı yok veya uygun değil"); return false; }
            if (!Oku(j.RootElement, "İlk Çalıştırılacak Adım", out Senaryo.İlk_Çalıştırılacak_Adım)) { Günlük.Ekle("Senaryo İlk Çalıştırılacak Adım anahtarı yok veya uygun değil"); return false; }
            if (!Oku(j.RootElement, "Beklenmeyen Durumda Çalıştırılacak Adım", out Senaryo.Beklenmeyen_Durumda_Çalıştırılacak_Adım)) Günlük.Ekle("Senaryo Beklenmeyen Durumda Çalıştırılacak Adım anahtarı yok veya uygun değil", "Bilgi");

            if (Oku(j.RootElement, "Adımlar", JsonValueKind.Array, out JsonElement j1))
            {
                for (int a = 0; a < j1.GetArrayLength(); a++)
                {
                    Senaryo_Adım_ Adım = new Senaryo_Adım_();
                    if (!Oku(j1[a], "Etiket", out Adım.Etiket)) { Günlük.Ekle("Senaryo Adım " + (a+1) + " Etiket anahtarı yok veya uygun değil", "Bilgi"); continue; }

                    if (Oku(j1[a], "İşlevler", JsonValueKind.Array, out JsonElement j2))
                    {
                        for (int b = 0; b < j2.GetArrayLength(); b++)
                        {
                            Senaryo_Adım_İşlev_ İşlev = new Senaryo_Adım_İşlev_();

                            if (!Oku(j2[b], "Kıstas", out İşlev.Kıstas)) { İşlev.Kıstas = "1"; Günlük.Ekle("Senaryo Adım " + (a + 1) + " İşlev " + (b + 1) + " Kıstas anahtarı yok veya uygun değil, 1 olarak değiştirildi", "Bilgi"); }
                            if (!Oku(j2[b], "Cevap Evet", JsonValueKind.Object, out İşlev.Cevap_Evet)) { Günlük.Ekle("Senaryo Adım " + (a + 1) + " İşlev " + (b + 1) + " Cevap Evet anahtarı yok veya uygun değil", "Bilgi"); continue; }
                            if (!Oku(j2[b], "Cevap Hayır", JsonValueKind.Object, out İşlev.Cevap_Hayır) && İşlev.Kıstas != "1") { Günlük.Ekle("Senaryo Adım " + (a + 1) + " İşlev " + (b + 1) + " Cevap Hayır anahtarı yok veya uygun değil", "Bilgi"); continue; }
                            
                            Adım.İşlevler.Add(İşlev);
                        }

                        if (Adım.İşlevler.Count == 0) { Günlük.Ekle("Senaryo Adım " + (a+1) + " işlev başlığı yok veya uygun değil"); continue; }
                    }
                    else { Günlük.Ekle("Senaryo Adım " + (a + 1) + " İşlevler anahtarı yok veya uygun değil", "Bilgi"); continue; }

                    Senaryo.Adımlar.Add(Adım);
                }
            }
            else { Günlük.Ekle("Senaryo Adımlar başlığı yok veya uygun değil"); return false; }

            if (Senaryo.Adımlar.Count == 0) { Günlük.Ekle("Senaryo Adımlar başlığı yok veya uygun değil"); return false; }

            if (!Senaryolar.Ekle(Senaryo)) { Günlük.Ekle(Senaryo.Adı + " ikinci kez tanıtıldı"); return false; }

            return true;
        }
    }
}

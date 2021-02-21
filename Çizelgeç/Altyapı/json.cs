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

        static public void Ayarlar(string yol, bool DeğişkenleriGörmezdenGel = false, bool BağlantılarıGörmezdenGel = false)
        {
            if (!File.Exists(yol)) throw new Exception(yol + " dosyası açılamadı");

            JsonDocument j = JsonDocument.Parse(File.ReadAllBytes(yol), new JsonDocumentOptions() { AllowTrailingCommas = true });

            if (Oku(j.RootElement, "Sinyaller", JsonValueKind.Array, out JsonElement j1))
            {
                for (int i = 0; i < j1.GetArrayLength(); i++)
                {
                    if (!Oku(j1[i], "Adı", out string Adı)) { Günlük.Ekle("Sinyaller başlığı dizi elemanı " + (i + 1) + " Adı anahtarı yok veya uygun değil", "Bilgi"); continue; }
                    if (!Oku(j1[i], "Görünen Adı", out string GörünenAdı)) Günlük.Ekle("Sinyaller başlığı dizi elemanı " + (i + 1) + " Görünen Adı anahtarı yok veya uygun değil", "Bilgi");
                    if (!Oku(j1[i], "İşlem", out string İşlem, "<Sinyal>")) Günlük.Ekle("Sinyaller başlığı dizi elemanı " + (i + 1) + " İşlem anahtarı yok veya uygun değil", "Bilgi");
                    if (!Oku(j1[i], "Dosyaya Kaydet", out string DosyayaKaydet, "Evet")) Günlük.Ekle("Sinyaller başlığı dizi elemanı " + (i + 1) + " Dosyaya Kaydet anahtarı yok veya uygun değil", "Bilgi");
                    if (!Oku(j1[i], "Zaman Aşımı (Saniye)", out string ZamanAşımıSaniye, "0")) Günlük.Ekle("Sinyaller başlığı dizi elemanı " + (i + 1) + " Zaman Aşımı (Saniye) anahtarı yok veya uygun değil", "Bilgi");

                    if (string.IsNullOrEmpty(Adı)) { Günlük.Ekle("Sinyaller başlığı dizi elemanı " + (i + 1) + " Adı anahtarı yok veya uygun değil", "Bilgi"); continue; }
                    if (Sinyaller.MevcutMu(Adı)) { Günlük.Ekle("Sinyaller başlığı " + Adı + " isimli Sinyal ikinci kez eklenmek istenildi."); continue; }

                    List<Hesaplama_> Hesaplamalar = new List<Hesaplama_>();
                    List<Uyarı_> Uyarılar = new List<Uyarı_>();
                    JsonElement.ObjectEnumerator jsonSıralayıcı = new JsonElement.ObjectEnumerator();
                    jsonSıralayıcı = j1[i].EnumerateObject();
                    while (jsonSıralayıcı.MoveNext() && S.Çalışşsın)
                    {
                        bool sonuc = true;

                        if (jsonSıralayıcı.Current.Name.StartsWith("Hesapla"))
                        {
                            sonuc &= Oku(jsonSıralayıcı.Current.Value, "İşlem", out string h_İşlem);
                            sonuc &= Oku(jsonSıralayıcı.Current.Value, "Degişken", out string h_Degisken);
                            sonuc &= Sinyaller.UygunMu(h_Degisken);

                            if (sonuc)  Hesaplamalar.Add(new Hesaplama_ { İşlem = h_İşlem, Değişken = h_Degisken });
                            else Günlük.Ekle("Sinyaller başlığı dizi elemanı " + (i + 1) + " " + jsonSıralayıcı.Current.Name + " uygun değil", "Bilgi");
                        }
                        else if (jsonSıralayıcı.Current.Name.StartsWith("Uyar"))
                        {
                            sonuc &= Oku(jsonSıralayıcı.Current.Value, "Kıstas", out string h_Kıstas);
                            sonuc &= Oku(jsonSıralayıcı.Current.Value, "Açıklama", JsonValueKind.Array, out JsonElement j1_2);
                            if (!sonuc || j1_2.GetArrayLength() == 0) { Günlük.Ekle("Sinyaller başlığı dizi elemanı " + (i + 1) + " " + jsonSıralayıcı.Current.Name + " uygun değil", "Bilgi"); continue; }

                            string[] h_Açıklamalar = new string[j1_2.GetArrayLength()];
                            for (int x = 0; x < j1_2.GetArrayLength(); x++)
                            {
                                sonuc &= Oku(j1_2[x], "", out h_Açıklamalar[x], "");
                            }
                            if (!sonuc) { Günlük.Ekle("Sinyaller başlığı dizi elemanı " + (i + 1) + " " + jsonSıralayıcı.Current.Name + " uygun değil", "Bilgi"); continue; }

                            Uyarılar.Add(new Uyarı_ { Kıstas = h_Kıstas, Açıklama = h_Açıklamalar });
                        }
                    }
                    jsonSıralayıcı.Dispose();

                    Sinyal_ sinyal = Sinyaller.Ekle(Adı);
                    sinyal.Güncelle_Adı(Adı, "", "", GörünenAdı);
                    sinyal.Değeri.Önİşlem = İşlem;
                    sinyal.Değeri.Kaydedilsin = DosyayaKaydet == "Evet" ? true : false;
                    sinyal.Değeri.ZamanAşımı_Sn = ZamanAşımıSaniye;
                    sinyal.Hesaplamalar = Hesaplamalar.Count > 0 ? Hesaplamalar.ToArray() : null;
                    sinyal.Uyarılar = Uyarılar.Count > 0 ? Uyarılar.ToArray() : null;
                }
            }
            else Günlük.Ekle("Sinyaller başlığı yok veya uygun değil", "Bilgi");
           
            if (!DeğişkenleriGörmezdenGel)
            {
                if (Oku(j.RootElement, "Değişkenler", JsonValueKind.Array, out j1))
                {
                    for (int i = 0; i < j1.GetArrayLength(); i++)
                    {
                        if (!Oku(j1[i], "Adı", out string Adı)) { Günlük.Ekle("Değişkenler başlığı dizi elemanı " + (i + 1) + " Adı anahtarı yok veya uygun değil", "Bilgi"); continue; }
                        if (!Oku(j1[i], "Görünen Adı", out string GörünenAdı)) Günlük.Ekle("Değişkenler başlığı dizi elemanı " + (i + 1) + " Görünen Adı anahtarı yok veya uygun değil", "Bilgi");
                        if (!Oku(j1[i], "Dosyaya Kaydet", out string DosyayaKaydet, "Hayır")) Günlük.Ekle("Değişkenler başlığı dizi elemanı " + (i + 1) + " Dosyaya Kaydet anahtarı yok veya uygun değil", "Bilgi");

                        if (string.IsNullOrEmpty(Adı)) { Günlük.Ekle("Değişkenler başlığı dizi elemanı " + (i + 1) + " Adı anahtarı yok veya uygun değil", "Bilgi"); continue; }
                        if (Sinyaller.MevcutMu(Adı)) { Günlük.Ekle("Değişkenler başlığı " + Adı + " isimli Değişken ikinci kez eklenmek istenildi."); continue; }

                        Sinyal_ sinyal = Sinyaller.Ekle(Adı);
                        sinyal.Güncelle_Adı(Adı, "", "", GörünenAdı);
                        sinyal.Değeri.Kaydedilsin = DosyayaKaydet == "Hayır" ? false : true;
                    }
                }
                else Günlük.Ekle("Değişkenler başlığı yok veya uygun değil", "Bilgi");
            }
            
            if (Oku(j.RootElement, "Sinyal Grupları", JsonValueKind.Array, out j1))
            {
                for (int i = 0; i < j1.GetArrayLength(); i++)
                {
                    if (!Oku(j1[i], "Görünen Adı", out string GörünenAdı)) { Günlük.Ekle("Gruplar başlığı dizi elemanı " + (i + 1) + " Görünen Adı anahtarı yok veya uygun değil", "Bilgi"); continue; }
                    if (!Oku(j1[i], "Birimi", out string Birimi)) { Günlük.Ekle("Gruplar başlığı dizi elemanı " + (i + 1) + " Birimi anahtarı yok veya uygun değil", "Bilgi"); }

                    if (Oku(j1[i], "Sinyaller", JsonValueKind.Array, out JsonElement j2))
                    {
                        for (int x = 0; x < j2.GetArrayLength(); x++)
                        {
                            if (Oku(j2[x], "", out string str))
                            {
                                Sinyal_ sny = Sinyaller.Ekle(str);
                                if (sny.Tür == Tür_.Sinyal) sny.Güncelle_Adı(str, GörünenAdı, Birimi, sny.Adı.Dal);
                            } 
                        }
                    }
                }
            }
            else Günlük.Ekle("Gruplar başlığı yok veya uygun değil", "Bilgi");

            if (Oku(j.RootElement, "Bilgi Toplama", JsonValueKind.Object, out j1))
            {
                if (!Oku(j1, "Zaman Aralığı (Saniye)", out S.BilgiToplama_ZamanAralığı_Sn, "15")) Günlük.Ekle("Bilgi Toplama başlığı Zaman Aralığı (Saniye) anahtarı yok veya uygun değil", "Bilgi");
                if (!Oku(j1, "Kıstas", out S.BilgiToplama_Kıstas, "1")) Günlük.Ekle("Bilgi Toplama başlığı Kıstas anahtarı yok veya uygun değil", "Bilgi");
            }
            else Günlük.Ekle("Bilgi Toplama başlığı yok veya uygun değil", "Bilgi");

            if (Oku(j.RootElement, "Dosyalama", JsonValueKind.Object, out j1))
            {
                if (!Oku(j1, "Azami Dosya Boyutu (Bayt)", out S.Dosyalama_AzamiDosyaBoyutu_Bayt, "1000000")) Günlük.Ekle("Dosyalama başlığı Azami Dosya Boyutu (Bayt) anahtarı yok veya uygun değil", "Bilgi");
                if (!Oku(j1, "Kayıt Klasörü", out S.Dosyalama_KayıtKlasörü, S.Kulanıcı_Klasörü)) Günlük.Ekle("Dosyalama başlığı Kayıt Klasörü anahtarı yok veya uygun değil", "Bilgi");
            }
            else Günlük.Ekle("Dosyalama başlığı yok veya uygun değil", "Bilgi");

            if (Oku(j.RootElement, "Mup Dosyasından Okuma", JsonValueKind.Object, out j1))
            {
                if (!Oku(j1, "Cümle Başlangıcı", out S.MupDosyasındanOkuma_CümleBaşlangıcı, ">Sinyaller")) Günlük.Ekle("Mup Dosyasından Okuma başlığı Cümle Başlangıcı anahtarı yok veya uygun değil", "Bilgi");
                if (!Oku(j1, "Kelime Ayracı", out string KelimeAyracı, System.Globalization.CultureInfo.InvariantCulture.TextInfo.ListSeparator)) Günlük.Ekle("Mup Dosyasından Okuma başlığı Kelime Ayracı anahtarı yok veya uygun değil", "Bilgi");
                S.MupDosyasındanOkuma_KelimeAyracı = Convert.ToChar(KelimeAyracı);
            }
            else Günlük.Ekle("Mup Dosyasından Okuma başlığı yok veya uygun değil", "Bilgi");

            if (!BağlantılarıGörmezdenGel)
            {
                if (Oku(j.RootElement, "Bağlantılar", JsonValueKind.Array, out j1))
                {
                    for (int i = 0; i < j1.GetArrayLength(); i++)
                    {
                        if (!Oku(j1[i], "Adı", out string Adı)) { Günlük.Ekle("Bağlantılar başlığı dizi elemanı " + (i + 1) + " Adı anahtarı yok veya uygun değil", "Bilgi"); continue; }
                        if (!Oku(j1[i], "Yöntem", out string Yöntem)) { Günlük.Ekle("Bağlantılar başlığı dizi elemanı " + (i + 1) + " Yöntem anahtarı yok veya uygun değil", "Bilgi"); continue; }

                        if (!Oku(j1[i], "Cümle Başlangıcı", out string CümleBaşlangıcı, ">Sinyaller")) Günlük.Ekle("Bağlantılar başlığı dizi elemanı " + (i + 1) + " Cümle Başlangıcı anahtarı yok veya uygun değil", "Bilgi");
                        if (!Oku(j1[i], "Kelime Ayracı", out string KelimeAyracı, ";")) Günlük.Ekle("Bağlantılar başlığı dizi elemanı " + (i + 1) + " Kelime Ayracı anahtarı yok veya uygun değil", "Bilgi");
                        if (!Oku(j1[i], "Dosyaya Kaydet", out string DosyayaKaydet, "Evet")) Günlük.Ekle("Bağlantılar başlığı dizi elemanı " + (i + 1) + " Dosyaya Kaydet anahtarı yok veya uygun değil", "Bilgi");

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
                        yeni.CümleBaşlangıcı = CümleBaşlangıcı;
                        yeni.KelimeAyracı = Convert.ToChar(KelimeAyracı);
                        yeni.Kaydedilsin = DosyayaKaydet == "Evet" ? true : false;
                    }

                    if (Bağlantılar.Tümü.Count == 0) throw new Exception("Hiç bağlantı eklenmedi, ölçüm alınamayacak.");
                }
                else Günlük.Ekle("Bağlantılar başlığı yok veya uygun değil", "Bilgi");
            } 
        }
        static public bool Senaryo(string yol)
        {
            if (!File.Exists(yol)) { Günlük.Ekle(yol + " dosyası açılamadı"); return false; }

            JsonDocument j = JsonDocument.Parse(File.ReadAllBytes(yol), new JsonDocumentOptions() { AllowTrailingCommas = true });

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

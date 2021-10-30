// Copyright ArgeMup GNU GENERAL PUBLIC LICENSE Version 3 <http://www.gnu.org/licenses/> <https://github.com/ArgeMup>

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using System.Text.Encodings.Web;
using System.Text.Unicode;

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

        static public void Ayarlar(string yol, bool CanlıEkranlamaİçin = true)
        {
            Günlük_Çalışşsın = CanlıEkranlamaİçin;
            if (!File.Exists(yol)) throw new Exception(yol + " dosyası açılamadı");

            JsonDocument j = JsonDocument.Parse(File.ReadAllBytes(yol), new JsonDocumentOptions() { AllowTrailingCommas = true });

            Sinyaller.Tümü.Clear();
            if (Oku(j.RootElement, "Sinyaller", JsonValueKind.Array, out JsonElement j1))
            {
                for (int i = 0; i < j1.GetArrayLength(); i++)
                {
                    if (!Oku(j1[i], "Adı", out string Adı)) { GünlüğeEkle("Sinyaller başlığı dizi elemanı " + (i + 1) + " Adı anahtarı yok veya uygun değil"); continue; }
                    if (string.IsNullOrEmpty(Adı)) { GünlüğeEkle("Sinyaller başlığı dizi elemanı " + (i + 1) + " Adı anahtarı yok veya uygun değil"); return; }
                    if (!Oku(j1[i], "Görünen Adı", out string GörünenAdı)) GünlüğeEkle("Sinyaller başlığı dizi elemanı " + (i + 1) + " Görünen Adı anahtarı yok veya uygun değil", "Bilgi");
                    if (!Oku(j1[i], "Soyadı", out string Soyadı)) GünlüğeEkle("Sinyaller başlığı dizi elemanı " + (i + 1) + " Soyadı anahtarı yok veya uygun değil", "Bilgi");
                   
                    string GöbekAdı = "", İşlem = "", Kaydet = "", ZamanAşımıSaniye = "";
                    if (CanlıEkranlamaİçin)
                    {
                        if (!Oku(j1[i], "Göbek Adı", out GöbekAdı)) GünlüğeEkle("Sinyaller başlığı dizi elemanı " + (i + 1) + " Göbek Adı anahtarı yok veya uygun değil", "Bilgi");
                        if (!Oku(j1[i], "İşlem", out İşlem, "<Sinyal>")) GünlüğeEkle("Sinyaller başlığı dizi elemanı " + (i + 1) + " İşlem anahtarı yok veya uygun değil", "Bilgi");
                        if (!Oku(j1[i], "Kaydet", out Kaydet, "Evet")) GünlüğeEkle("Sinyaller başlığı dizi elemanı " + (i + 1) + " Dosyaya Kaydet anahtarı yok veya uygun değil", "Bilgi");
                        if (!Oku(j1[i], "Zaman Aşımı (Saniye)", out ZamanAşımıSaniye, "0")) GünlüğeEkle("Sinyaller başlığı dizi elemanı " + (i + 1) + " Zaman Aşımı (Saniye) anahtarı yok veya uygun değil", "Bilgi");
                    }

                    List<Hesaplama_> Hesaplamalar = new List<Hesaplama_>();
                    if (Oku(j1[i], "Hesaplamalar", JsonValueKind.Array, out JsonElement j2))
                    {
                        for (int ii = 0; ii < j2.GetArrayLength(); ii++)
                        {
                            if (!Oku(j2[ii], "İşlem", out string h_İşlem)) { GünlüğeEkle("Sinyaller başlığı dizi elemanı " + (i + 1) + " Hesaplamalar anahtarı dizi elemanı " + (ii + 1) + " İşlem anahtarı yok veya uygun değil"); continue; }
                            if (!Oku(j2[ii], "Degişken", out string h_Degişken)) { GünlüğeEkle("Sinyaller başlığı dizi elemanı " + (i + 1) + " Hesaplamalar anahtarı dizi elemanı " + (ii + 1) + " Degişken anahtarı yok veya uygun değil"); continue; }
                            if (!Sinyaller.UygunMu(h_Degişken)) { GünlüğeEkle("Sinyaller başlığı dizi elemanı " + (i + 1) + " Hesaplamalar anahtarı dizi elemanı " + (ii + 1) + " Degişken anahtarı yok veya uygun değil"); continue; }
                            Hesaplamalar.Add(new Hesaplama_ { İşlem = h_İşlem, Değişken = h_Degişken });
                        }
                    }
                    else GünlüğeEkle("Sinyaller başlığı dizi elemanı " + (i + 1) + " Hesaplamalar anahtarı yok veya uygun değil", "Bilgi");
                    Hesaplama_[] _Hesaplamalar_ = Hesaplamalar.Count > 0 ? Hesaplamalar.ToArray() : null;

                    List<Uyarı_> Uyarılar = new List<Uyarı_>();
                    if (Oku(j1[i], "Uyarılar", JsonValueKind.Array, out j2))
                    {
                        for (int ii = 0; ii < j2.GetArrayLength(); ii++)
                        {
                            string u_Kıstas = "";
                            string[] u_Açıklamalar = null;

                            if (!Oku(j2[ii], "Kıstas", out u_Kıstas)) { GünlüğeEkle("Sinyaller başlığı dizi elemanı " + (i + 1) + " Uyarılar anahtarı dizi elemanı " + (ii + 1) + " Kıstas anahtarı yok veya uygun değil"); continue; }
                            if (Oku(j2[ii], "Açıklama", JsonValueKind.Array, out JsonElement j3))
                            {
                                u_Açıklamalar = new string[j3.GetArrayLength()];
                                for (int iii = 0; iii < j3.GetArrayLength(); iii++)
                                {
                                    Oku(j3[iii], "", out u_Açıklamalar[iii], "");
                                }
                            }
                            else { GünlüğeEkle("Sinyaller başlığı dizi elemanı " + (i + 1) + " Uyarılar anahtarı dizi elemanı " + (ii + 1) + " Açıklama anahtarı yok veya uygun değil"); continue; }

                            Uyarılar.Add(new Uyarı_ { Kıstas = u_Kıstas, Açıklama = u_Açıklamalar });
                        }
                    }
                    else GünlüğeEkle("Sinyaller başlığı dizi elemanı " + (i + 1) + " Hesaplamalar anahtarı yok veya uygun değil", "Bilgi");
                    Uyarı_[] _Uyarılar_ = Uyarılar.Count > 0 ? Uyarılar.ToArray() : null;

                    string[] GöbekAdıDizisi = GöbekAdı.Split('|');
                    string[] AdıDizisi = Adı.Split('|');
                    string[] GörünenAdıDizisi = GörünenAdı.Split('|');
                    if (GöbekAdıDizisi.Length > 1)
                    {
                        //birden fazla bağlanıtdan gelen sinyallerin isimlendirilmesi
                        foreach (var _GöbekAdı_ in GöbekAdıDizisi)
                        {
                            if (AdıDizisi.Length > 1 && AdıDizisi.Length == GörünenAdıDizisi.Length)
                            {
                                //aynı baglantıdan gelen birden fazla sinyalin isimlendirilmesi 
                                for (int a = 0; a < AdıDizisi.Length; a++)
                                {
                                    Ayarlar_Ekle_Sinyal(AdıDizisi[a], _GöbekAdı_ + GörünenAdıDizisi[a], _GöbekAdı_, Soyadı, İşlem, Kaydet, ZamanAşımıSaniye, _Hesaplamalar_, _Uyarılar_);
                                }
                            }
                            else
                            {
                                //tek sinyalin isimlendirilmesi 
                                Ayarlar_Ekle_Sinyal(Adı, _GöbekAdı_ + GörünenAdı, _GöbekAdı_, Soyadı, İşlem, Kaydet, ZamanAşımıSaniye, _Hesaplamalar_, _Uyarılar_);
                            }
                        }
                    }
                    else
                    {
                        //tek baplantıdan gelen sinyallerin isimlendirilmesi

                        if (AdıDizisi.Length > 1 && AdıDizisi.Length == GörünenAdıDizisi.Length)
                        {
                            //aynı baglantıdan gelen birden fazla sinyalin isimlendirilmesi 
                            for (int a = 0; a < AdıDizisi.Length; a++)
                            {
                                Ayarlar_Ekle_Sinyal(AdıDizisi[a], GöbekAdı + GörünenAdıDizisi[a], GöbekAdı, Soyadı, İşlem, Kaydet, ZamanAşımıSaniye, _Hesaplamalar_, _Uyarılar_);
                            }
                        }
                        else
                        {
                            //tek sinyalin isimlendirilmesi 
                            Ayarlar_Ekle_Sinyal(Adı, GörünenAdı, GöbekAdı, Soyadı, İşlem, Kaydet, ZamanAşımıSaniye, _Hesaplamalar_, _Uyarılar_);
                        }
                    }
                }
            }
            else GünlüğeEkle("Sinyaller başlığı yok veya uygun değil", "Bilgi");
           
            if (CanlıEkranlamaİçin)
            {
                if (Oku(j.RootElement, "Değişkenler", JsonValueKind.Array, out j1))
                {
                    for (int i = 0; i < j1.GetArrayLength(); i++)
                    {
                        if (!Oku(j1[i], "Adı", out string Adı)) { GünlüğeEkle("Değişkenler başlığı dizi elemanı " + (i + 1) + " Adı anahtarı yok veya uygun değil"); continue; }
                        if (!Oku(j1[i], "Görünen Adı", out string GörünenAdı)) GünlüğeEkle("Değişkenler başlığı dizi elemanı " + (i + 1) + " Görünen Adı anahtarı yok veya uygun değil", "Bilgi");
                        if (!Oku(j1[i], "Kaydet", out string Kaydet, "Hayır")) GünlüğeEkle("Değişkenler başlığı dizi elemanı " + (i + 1) + " Dosyaya Kaydet anahtarı yok veya uygun değil", "Bilgi");
                        if (!Oku(j1[i], "Soyadı", out string Soyadı, "")) GünlüğeEkle("Değişkenler başlığı dizi elemanı " + (i + 1) + " Soyadı anahtarı yok veya uygun değil", "Bilgi");

                        if (string.IsNullOrEmpty(Adı)) { GünlüğeEkle("Değişkenler başlığı dizi elemanı " + (i + 1) + " Adı anahtarı yok veya uygun değil"); continue; }

                        bool _Kaydet_ = Kaydet == "Hayır" ? false : true;
                        string[] AdıDizisi = Adı.Split('|');
                        string[] GörünenAdıDizisi = GörünenAdı.Split('|');
                        if (AdıDizisi.Length > 1 && AdıDizisi.Length == GörünenAdıDizisi.Length)
                        {
                            //birden fazla değişkenin isimlendirilmesi 
                            for (int a = 0; a < AdıDizisi.Length; a++)
                            {
                                Ayarlar_Ekle_Değişken(AdıDizisi[a], GörünenAdıDizisi[a], Soyadı, _Kaydet_);
                            }
                        }
                        else
                        {
                            //tek değişkenin isimlendirilmesi 
                            Ayarlar_Ekle_Değişken(Adı, GörünenAdı, Soyadı, _Kaydet_);
                        }
                    }
                }
                else GünlüğeEkle("Değişkenler başlığı yok veya uygun değil", "Bilgi");
            }

            #region Birbirinin aynı csv adına sahip sinyal varmı kontrolü
            foreach (var biri in Sinyaller.Tümü)
            {
                foreach (var diğeri in Sinyaller.Tümü)
                {
                    if (biri.Equals(diğeri)) continue;

                    if (biri.Value.Adı.Csv == diğeri.Value.Adı.Csv)
                    {
                        diğeri.Value.Adı.Csv += " " + diğeri.Key;
                        break;
                    }
                }
            }
            #endregion

            if (Oku(j.RootElement, "Bilgi Toplama", JsonValueKind.Object, out j1))
            {
                if (!Oku(j1, "Zaman Aralığı (Saniye)", out S.BilgiToplama_ZamanAralığı_Sn, "15")) GünlüğeEkle("Bilgi Toplama başlığı Zaman Aralığı (Saniye) anahtarı yok veya uygun değil", "Bilgi");
                if (!Oku(j1, "Kıstas", out S.BilgiToplama_Kıstas, "1")) GünlüğeEkle("Bilgi Toplama başlığı Kıstas anahtarı yok veya uygun değil", "Bilgi");
                if (!Oku(j1, "Canlı Ölçüm Sayısı", out string CanlıÖlçümSayısı, "10000")) GünlüğeEkle("Bilgi Toplama başlığı Canlı Ölçüm Sayısı anahtarı yok veya uygun değil", "Bilgi");
                if (!Oku(j1, "Birbirinin Aynısı Olan Zaman Dilimlerini Atla", out string BirbirininAynısıOlanZamanDilimleriniAtla, "Evet")) GünlüğeEkle("Bilgi Toplama başlığı Birbirinin Aynısı Olan Zaman Dilimlerini Atla anahtarı yok veya uygun değil", "Bilgi");

                if (!int.TryParse(CanlıÖlçümSayısı, out S.CanliÇizdirme_ÖlçümSayısı)) S.CanliÇizdirme_ÖlçümSayısı = 10000;
                S.BilgiToplama_BirbirininAynısıOlanZamanDilimleriniAtla = BirbirininAynısıOlanZamanDilimleriniAtla == "Evet";
            }
            else GünlüğeEkle("Bilgi Toplama başlığı yok veya uygun değil", "Bilgi");

            if (Oku(j.RootElement, "Dosyalama", JsonValueKind.Object, out j1))
            {
                if (!Oku(j1, "Azami Dosya Boyutu (Bayt)", out S.Dosyalama_AzamiDosyaBoyutu_Bayt, "5242880")) GünlüğeEkle("Dosyalama başlığı Azami Dosya Boyutu (Bayt) anahtarı yok veya uygun değil", "Bilgi");
                if (!Oku(j1, "Kayıt Klasörü", out S.Dosyalama_KayıtKlasörü)) GünlüğeEkle("Dosyalama başlığı Kayıt Klasörü anahtarı yok veya uygun değil", "Bilgi");
            }
            else GünlüğeEkle("Dosyalama başlığı yok veya uygun değil", "Bilgi");

            if (Oku(j.RootElement, "Mup Dosyasından Okuma", JsonValueKind.Object, out j1))
            {
                if (!Oku(j1, "Cümle Başlangıcı", out S.MupDosyasındanOkuma_CümleBaşlangıcı, ">Sinyaller")) GünlüğeEkle("Mup Dosyasından Okuma başlığı Cümle Başlangıcı anahtarı yok veya uygun değil", "Bilgi");
                if (!Oku(j1, "Kelime Ayracı", out string KelimeAyracı, System.Globalization.CultureInfo.InvariantCulture.TextInfo.ListSeparator)) GünlüğeEkle("Mup Dosyasından Okuma başlığı Kelime Ayracı anahtarı yok veya uygun değil", "Bilgi");
                S.MupDosyasındanOkuma_KelimeAyracı = Convert.ToChar(KelimeAyracı);
            }
            else GünlüğeEkle("Mup Dosyasından Okuma başlığı yok veya uygun değil", "Bilgi");

            if (CanlıEkranlamaİçin)
            {
                Bağlantılar.Tümü.Clear();

                if (Oku(j.RootElement, "Bağlantılar", JsonValueKind.Array, out j1))
                {
                    for (int i = 0; i < j1.GetArrayLength(); i++)
                    {
                        if (!Oku(j1[i], "Adı", out string Adı)) { GünlüğeEkle("Bağlantılar başlığı dizi elemanı " + (i + 1) + " Adı anahtarı yok veya uygun değil"); continue; }
                        if (!Oku(j1[i], "Yöntem", out string Yöntem)) { GünlüğeEkle("Bağlantılar başlığı dizi elemanı " + (i + 1) + " Yöntem anahtarı yok veya uygun değil"); continue; }

                        if (!Oku(j1[i], "Göbek Adı", out string GöbekAdı)) GünlüğeEkle("Bağlantılar başlığı dizi elemanı " + (i + 1) + " Göbek Adı anahtarı yok veya uygun değil", "Bilgi");
                        if (!Oku(j1[i], "Cümle Başlangıcı", out string CümleBaşlangıcı, ">Sinyaller")) GünlüğeEkle("Bağlantılar başlığı dizi elemanı " + (i + 1) + " Cümle Başlangıcı anahtarı yok veya uygun değil", "Bilgi");
                        if (!Oku(j1[i], "Kelime Ayracı", out string KelimeAyracı, ";")) GünlüğeEkle("Bağlantılar başlığı dizi elemanı " + (i + 1) + " Kelime Ayracı anahtarı yok veya uygun değil", "Bilgi");
                        if (!Oku(j1[i], "Kaydet", out string Kaydet, "Evet")) GünlüğeEkle("Bağlantılar başlığı dizi elemanı " + (i + 1) + " Kaydet anahtarı yok veya uygun değil", "Bilgi");
                        if (!Oku(j1[i], "Tanımlanmamış Sinyalleri", out string TanımlanmamışSinyalleri, "Kullan")) GünlüğeEkle("Bağlantılar başlığı dizi elemanı " + (i + 1) + " Tanımlanmamış Sinyalleri anahtarı yok veya uygun değil", "Bilgi");

                        if (GöbekAdı.Contains("|")) { GünlüğeEkle("Bağlantılar başlığı dizi elemanı " + (i + 1) + " Göbek Adı anahtarı içinde | karakteri olmamalı", "Bilgi"); continue; }

                        string P1 = "", P2 = "";
                        Bağlantı_Türü_ Türü = Bağlantı_Türü_.Boşta;
                        if (Yöntem == "Komut Satırı")
                        {
                            if (!Oku(j1[i], "Uygulama", out P1)) { GünlüğeEkle("Bağlantılar başlığı dizi elemanı " + (i + 1) + " Uygulama anahtarı yok veya uygun değil"); continue; }
                            if (!Oku(j1[i], "Parametre", out P2)) { GünlüğeEkle("Bağlantılar başlığı dizi elemanı " + (i + 1) + " Parametre anahtarı yok veya uygun değil"); continue; }
                        
                            if (string.IsNullOrEmpty(P2)) { GünlüğeEkle("Bağlantılar başlığı dizi elemanı " + (i + 1) + " anahtarlardan biri yok veya uygun değil"); continue; }
                            
                            Türü = Bağlantı_Türü_.KomutSatırı;
                        }
                        else if (Yöntem == "Uart")
                        {
                            if (!Oku(j1[i], "Erişim Noktası", out P1)) { GünlüğeEkle("Bağlantılar başlığı dizi elemanı " + (i + 1) + " Erişim Noktası anahtarı yok veya uygun değil"); continue; }
                            if (!Oku(j1[i], "Bit Hızı", out P2)) { GünlüğeEkle("Bağlantılar başlığı dizi elemanı " + (i + 1) + " Bit Hızı anahtarı yok veya uygun değil"); continue; }

                            if (string.IsNullOrEmpty(P2)) { GünlüğeEkle("Bağlantılar başlığı dizi elemanı " + (i + 1) + " anahtarlardan biri yok veya uygun değil"); continue; }

                            Türü = Bağlantı_Türü_.SeriPort;
                        }
                        else if (Yöntem == "Tcp İstemcisi")
                        {
                            if (!Oku(j1[i], "IP veya Adresi", out P1)) { GünlüğeEkle("Bağlantılar başlığı dizi elemanı " + (i + 1) + " IP veya Adresi anahtarı yok veya uygun değil"); continue; }
                            if (!Oku(j1[i], "Erişim Noktası", out P2)) { GünlüğeEkle("Bağlantılar başlığı dizi elemanı " + (i + 1) + " Erişim Noktası anahtarı yok veya uygun değil"); continue; }

                            if (string.IsNullOrEmpty(P2)) { GünlüğeEkle("Bağlantılar başlığı dizi elemanı " + (i + 1) + " anahtarlardan biri yok veya uygun değil"); continue; }

                            Türü = Bağlantı_Türü_.Tcpİstemci;
                        }
                        else if (Yöntem == "Udp İstemcisi")
                        {
                            if (!Oku(j1[i], "IP veya Adresi", out P1)) { GünlüğeEkle("Bağlantılar başlığı dizi elemanı " + (i + 1) + " IP veya Adresi anahtarı yok veya uygun değil"); continue; }
                            if (!Oku(j1[i], "Erişim Noktası", out P2)) { GünlüğeEkle("Bağlantılar başlığı dizi elemanı " + (i + 1) + " Erişim Noktası anahtarı yok veya uygun değil"); continue; }

                            if (string.IsNullOrEmpty(P2)) { GünlüğeEkle("Bağlantılar başlığı dizi elemanı " + (i + 1) + " anahtarlardan biri yok veya uygun değil"); continue; }

                            Türü = Bağlantı_Türü_.Udpİstemci;
                        }
                        else if (Yöntem == "Tcp Sunucusu")
                        {
                            if (!Oku(j1[i], "Erişim Noktası", out P1)) { GünlüğeEkle("Bağlantılar başlığı dizi elemanı " + (i + 1) + " Erişim Noktası anahtarı yok veya uygun değil"); continue; }

                            Türü = Bağlantı_Türü_.TcpSunucu;
                        }
                        else if (Yöntem == "Udp Sunucusu")
                        {
                            if (!Oku(j1[i], "Erişim Noktası", out P1)) { GünlüğeEkle("Bağlantılar başlığı dizi elemanı " + (i + 1) + " Erişim Noktası anahtarı yok veya uygun değil"); continue; }

                            Türü = Bağlantı_Türü_.UdpSunucu;
                        }
                        else
                        {
                            GünlüğeEkle("Bağlantılar başlığı dizi elemanı " + (i + 1) + " Yöntem anahtarı yok veya uygun değil"); 
                            continue;
                        }

                        if (string.IsNullOrEmpty(P1)) { GünlüğeEkle("Bağlantılar başlığı dizi elemanı " + (i + 1) + " anahtarlardan biri yok veya uygun değil"); continue; }

                        if (Bağlantılar.MevcutMu(Adı)) { throw new Exception(Adı + " isimli bağlantı ikinci kez eklendi."); }

                        Bağlantı_ yeni = Bağlantılar.Ekle(Adı);
                        yeni.GöbekAdı = GöbekAdı;
                        yeni.Türü = Türü;
                        yeni.P1 = P1;
                        yeni.P2 = P2;
                        yeni.CümleBaşlangıcı = CümleBaşlangıcı;
                        yeni.KelimeAyracı = Convert.ToChar(KelimeAyracı);
                        yeni.Kaydedilsin = Kaydet == "Evet" ? true : false;
                        if (!Enum.TryParse(TanımlanmamışSinyalleri, out yeni.TanımlanmamışSinyalleri)) yeni.TanımlanmamışSinyalleri = Bağlantı_.TanımlanmamışSinyalleri_.Kullan;
                    } 
                }
                else GünlüğeEkle("Bağlantılar başlığı yok veya uygun değil");

                if (Bağlantılar.Tümü.Count == 0) throw new Exception("Hiç bağlantı eklenmedi, ölçüm alınamayacak.");
            } 
        }
        static public void Ayarlar_Ekle_Sinyal(string Adı, string GörünenAdı, string GöbekAdı, string Soyadı,
            string İşlem, string Kaydet, string ZamanAşımıSaniye,
            Hesaplama_[] Hesaplamalar, Uyarı_[] Uyarılar)
        {
            if (!string.IsNullOrEmpty(GöbekAdı)) Adı = "<" + GöbekAdı + Adı.Trim('<', '>') + ">";
            if (Sinyaller.MevcutMu(Adı)) { GünlüğeEkle("Sinyaller başlığı " + Adı + " isimli Sinyal ikinci kez eklenmek istenildi."); return; }

            Sinyal_ sinyal = Sinyaller.Ekle(Adı);
            sinyal.Güncelle_Adı(Adı, Soyadı, GörünenAdı);
            sinyal.Değeri.Önİşlem = İşlem == "<Sinyal>" ? null : İşlem;
            sinyal.Değeri.Kaydedilsin = Kaydet == "Evet";
            sinyal.Değeri.ZamanAşımı_Sn = ZamanAşımıSaniye;
            sinyal.Hesaplamalar = Hesaplamalar;
            sinyal.Uyarılar = Uyarılar;
        }
        static public void Ayarlar_Ekle_Değişken(string Adı, string GörünenAdı, string SoyAdı, bool Kaydet)
        {
            if (Sinyaller.MevcutMu(Adı)) { GünlüğeEkle("Değişkenler başlığı " + Adı + " isimli Değişken ikinci kez eklenmek istenildi."); return; }

            Sinyal_ sinyal = Sinyaller.Ekle(Adı);
            sinyal.Güncelle_Adı(Adı, SoyAdı, GörünenAdı);
            sinyal.Değeri.Kaydedilsin = Kaydet;
        }
        static public bool Senaryo(string yol)
        {
            Günlük_Çalışşsın = true;
            if (!File.Exists(yol)) { GünlüğeEkle(yol + " dosyası açılamadı"); return false; }

            JsonDocument j = JsonDocument.Parse(File.ReadAllBytes(yol), new JsonDocumentOptions() { AllowTrailingCommas = true });

            Senaryo_ Senaryo = new Senaryo_();
            if (!Oku(j.RootElement, "Adı", out Senaryo.Adı)) { GünlüğeEkle("Senaryo Adı anahtarı yok veya uygun değil"); return false; }
            if (!Oku(j.RootElement, "İlk Çalıştırılacak Adım", out Senaryo.İlk_Çalıştırılacak_Adım)) { GünlüğeEkle("Senaryo İlk Çalıştırılacak Adım anahtarı yok veya uygun değil"); return false; }
            if (!Oku(j.RootElement, "Beklenmeyen Durumda Çalıştırılacak Adım", out Senaryo.Beklenmeyen_Durumda_Çalıştırılacak_Adım)) GünlüğeEkle("Senaryo Beklenmeyen Durumda Çalıştırılacak Adım anahtarı yok veya uygun değil", "Bilgi");
            if (!Oku(j.RootElement, "Hemen Çalıştır", out string HemenÇalıştır, "Evet")) GünlüğeEkle("Senaryo Hemen Çalıştır anahtarı yok veya uygun değil", "Bilgi");
            Senaryo.HemenÇalıştır = HemenÇalıştır == "Evet";

            if (Oku(j.RootElement, "Adımlar", JsonValueKind.Array, out JsonElement j1))
            {
                for (int a = 0; a < j1.GetArrayLength(); a++)
                {
                    Senaryo_Adım_ Adım = new Senaryo_Adım_();
                    if (!Oku(j1[a], "Etiket", out Adım.Etiket)) { GünlüğeEkle("Senaryo Adım " + (a+1) + " Etiket anahtarı yok veya uygun değil"); continue; }

                    if (Oku(j1[a], "İşlevler", JsonValueKind.Array, out JsonElement j2))
                    {
                        for (int b = 0; b < j2.GetArrayLength(); b++)
                        {
                            Senaryo_Adım_İşlev_ İşlev = new Senaryo_Adım_İşlev_();

                            if (!Oku(j2[b], "Kıstas", out İşlev.Kıstas)) { İşlev.Kıstas = "1"; GünlüğeEkle("Senaryo Adım " + (a + 1) + " İşlev " + (b + 1) + " Kıstas anahtarı yok veya uygun değil, 1 olarak değiştirildi", "Bilgi"); }
                            if (!Oku(j2[b], "Cevap Evet", JsonValueKind.Object, out İşlev.Cevap_Evet)) { GünlüğeEkle("Senaryo Adım " + (a + 1) + " İşlev " + (b + 1) + " Cevap Evet anahtarı yok veya uygun değil", "Bilgi"); continue; }
                            if (!Oku(j2[b], "Cevap Hayır", JsonValueKind.Object, out İşlev.Cevap_Hayır) && İşlev.Kıstas != "1") { GünlüğeEkle("Senaryo Adım " + (a + 1) + " İşlev " + (b + 1) + " Cevap Hayır anahtarı yok veya uygun değil", "Bilgi"); continue; }
                            
                            Adım.İşlevler.Add(İşlev);
                        }

                        if (Adım.İşlevler.Count == 0) { GünlüğeEkle("Senaryo Adım " + (a+1) + " işlev başlığı yok veya uygun değil"); continue; }
                    }
                    else { GünlüğeEkle("Senaryo Adım " + (a + 1) + " İşlevler anahtarı yok veya uygun değil"); continue; }

                    Senaryo.Adımlar.Add(Adım);
                }
            }
            else { GünlüğeEkle("Senaryo Adımlar başlığı yok veya uygun değil"); return false; }

            if (Senaryo.Adımlar.Count == 0) { GünlüğeEkle("Senaryo Adımlar başlığı yok veya uygun değil"); return false; }

            if (!Senaryolar.Ekle(Senaryo)) { GünlüğeEkle(Senaryo.Adı + " ikinci kez tanıtıldı"); return false; }

            return true;
        }

        static bool Günlük_Çalışşsın = true;
        static void GünlüğeEkle(string Mesaj, string Tür = "HATA")
        {
            if (Günlük_Çalışşsın) Günlük.Ekle(Mesaj, Tür);
        }
    }

    class json_Üret
    {
        static public string MupDosyasıİçin_Ayarlar(string ŞablonAyarlarDosyası, string CümleBaşlangıcı, string KelimeAyracı)
        {
            string çıktı = "{";
            if (!File.Exists(ŞablonAyarlarDosyası)) return "";

            using (MemoryStream mst = new MemoryStream())
            {
                using (Utf8JsonWriter utf8Jsonyazıcı = new Utf8JsonWriter(mst, new JsonWriterOptions() { Indented = true, Encoder = JavaScriptEncoder.Create(UnicodeRanges.All) }))
                {
                    using (JsonDocument kaynak = JsonDocument.Parse(File.ReadAllBytes(ŞablonAyarlarDosyası), new JsonDocumentOptions() { AllowTrailingCommas = true }))
                    {
                        utf8Jsonyazıcı.WriteStartObject();

                        if (json_Ayıkla.Oku(kaynak.RootElement, "Sinyaller", JsonValueKind.Array, out JsonElement snyler))
                        {
                            utf8Jsonyazıcı.WritePropertyName("Sinyaller");
                            snyler.WriteTo(utf8Jsonyazıcı);
                        }

                        if (json_Ayıkla.Oku(kaynak.RootElement, "Değişkenler", JsonValueKind.Array, out JsonElement dşkler))
                        {
                            utf8Jsonyazıcı.WritePropertyName("Değişkenler");
                            dşkler.WriteTo(utf8Jsonyazıcı);
                        }

                        utf8Jsonyazıcı.WritePropertyName("Mup Dosyasından Okuma");

                        utf8Jsonyazıcı.WriteStartObject();
                        utf8Jsonyazıcı.WriteString("Cümle Başlangıcı", CümleBaşlangıcı);
                        utf8Jsonyazıcı.WriteString("Kelime Ayracı", KelimeAyracı);
                        utf8Jsonyazıcı.WriteEndObject();

                        utf8Jsonyazıcı.WriteEndObject();
                    }
                }

                çıktı = Encoding.UTF8.GetString(mst.ToArray());
            }

            return çıktı;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading;
using System.Windows.Forms;

namespace Çizelgeç
{
    public class Senaryo_Adım_İşlev_
    {
        public string Kıstas = "";
        public JsonElement Cevap_Evet;
        public JsonElement Cevap_Hayır;
    }
    public class Senaryo_Adım_
    {
        public string Etiket = "";
        public List<Senaryo_Adım_İşlev_> İşlevler = new List<Senaryo_Adım_İşlev_>();
    }
    public class Senaryo_
    {
        public string Adı = "";
        public string İlk_Çalıştırılacak_Adım = "";
        public string Beklenmeyen_Durumda_Çalıştırılacak_Adım = "";
        public List<Senaryo_Adım_> Adımlar = new List<Senaryo_Adım_>();
        
        public TreeNode Dal;
        public string Durum;

        bool Çalışsın = true;
        Thread Görev_Nesnesi = null;
        static void Görev_İşlemi(Senaryo_ Senaryo)
        {
            string SonrakiAdım = Senaryo.İlk_Çalıştırılacak_Adım;
            JsonElement.ObjectEnumerator jsonSıralayıcı = new JsonElement.ObjectEnumerator();

            YenidenDene:
            try
            {
                while (S.Çalışşsın && Senaryo.Çalışsın && !string.IsNullOrEmpty(SonrakiAdım))
                {
                    Thread.Sleep(1); //cpu yüzdesini düşürmek için

                    int a = 0;
                    for (; a < Senaryo.Adımlar.Count; a++) { if (Senaryo.Adımlar[a].Etiket == SonrakiAdım) break; }
                    if (a >= Senaryo.Adımlar.Count) throw new Exception("Adım : " + SonrakiAdım + " bulunamadı");
                    SonrakiAdım = "";

                    for (int b = 0; b < Senaryo.Adımlar[a].İşlevler.Count && S.Çalışşsın && Senaryo.Çalışsın; b++)
                    {
                        Thread.Sleep(1); //cpu yüzdesini düşürmek için

                        Senaryo.Durum = " -> Adım : " + Senaryo.Adımlar[a].Etiket + ", işlev : " + (b+1);

                        JsonElement eleman;
                        double sonuç = Çevirici.Yazıdan_NoktalıSayıya(Senaryo.Adımlar[a].İşlevler[b].Kıstas);
                        if (sonuç > 0.0) eleman = Senaryo.Adımlar[a].İşlevler[b].Cevap_Evet;
                        else eleman = Senaryo.Adımlar[a].İşlevler[b].Cevap_Hayır;

                        jsonSıralayıcı.Dispose();
                        jsonSıralayıcı = eleman.EnumerateObject();
                        while (jsonSıralayıcı.MoveNext() && S.Çalışşsın && Senaryo.Çalışsın)
                        {
                            Thread.Sleep(1); //cpu yüzdesini düşürmek için

                            if (jsonSıralayıcı.Current.Name.StartsWith("Senaryoyu Çalıştır"))
                            {
                                Senaryolar.Bul(jsonSıralayıcı.Current.Value.GetString()).Çalıştır();
                            }
                            else if (jsonSıralayıcı.Current.Name.StartsWith("Senaryoyu Durdur"))
                            {
                                Senaryolar.Bul(jsonSıralayıcı.Current.Value.GetString()).Durdur();
                            }
                            else if (jsonSıralayıcı.Current.Name.StartsWith("Etikete Git"))
                            {
                                SonrakiAdım = jsonSıralayıcı.Current.Value.GetString();
                                goto YenidenDene;
                            }
                            else if (jsonSıralayıcı.Current.Name.StartsWith("Uyar"))
                            {
                                string mesaj = "Senaryo;" + Senaryo.Adı + "|" + Senaryo.Adımlar[a].Etiket + ";" + Çevirici.Uyarıdan_Yazıya(jsonSıralayıcı.Current.Value);
                                Kaydedici.Ekle(new double[1] { S.Tarih.Sayıya(DateTime.Now) }, mesaj);
                                Günlük.Ekle(mesaj);
                            }
                            else if (jsonSıralayıcı.Current.Name.StartsWith("Bağlantı Üzerinden Gönder"))
                            {
                                bool sonuc = true;

                                sonuc &= json_Ayıkla.Oku(jsonSıralayıcı.Current.Value, "Bağlantı", out string Bağlantı);
                                sonuc &= json_Ayıkla.Oku(jsonSıralayıcı.Current.Value, "İçerik", JsonValueKind.Array, out JsonElement j);

                                string İçerik = Çevirici.Uyarıdan_Yazıya(j);

                                if (!sonuc || (string.IsNullOrEmpty(Bağlantı) && string.IsNullOrEmpty(İçerik)))
                                {
                                    throw new Exception("Adım : " + Senaryo.Adımlar[a].Etiket + ", işlev : " + (b + 1) + ", anahtar : " + jsonSıralayıcı.Current.Name + " yok veya hatalı");
                                }

                                Bağlantılar.Gönder(Bağlantı, İçerik);
                            }
                            else if (jsonSıralayıcı.Current.Name.StartsWith("Hesapla"))
                            {
                                bool sonuc = true;

                                sonuc &= json_Ayıkla.Oku(jsonSıralayıcı.Current.Value, "İşlem", out string İşlem);
                                sonuc &= json_Ayıkla.Oku(jsonSıralayıcı.Current.Value, "Degisken", out string Degisken);

                                if (!sonuc || (string.IsNullOrEmpty(İşlem) && string.IsNullOrEmpty(Degisken)))
                                {
                                    throw new Exception("Adım : " + Senaryo.Adımlar[a].Etiket + ", işlev : " + (b + 1) + ", anahtar : " + jsonSıralayıcı.Current.Name + " yok veya hatalı");
                                }

                                Sinyaller.Yaz(Degisken, Çevirici.Yazıdan_NoktalıSayıya(İşlem));
                            }
                            else if (jsonSıralayıcı.Current.Name.StartsWith("Bekle"))
                            {
                                bool sonuc = true;

                                sonuc &= json_Ayıkla.Oku(jsonSıralayıcı.Current.Value, "Kıstas", out string Kıstas);
                                sonuc &= json_Ayıkla.Oku(jsonSıralayıcı.Current.Value, "Saniye", out string Saniye);

                                if (!sonuc || (string.IsNullOrEmpty(Kıstas) && string.IsNullOrEmpty(Saniye)))
                                {
                                    throw new Exception("Adım : " + Senaryo.Adımlar[a].Etiket + ", işlev : " + (b + 1) + ", anahtar : " + jsonSıralayıcı.Current.Name + " yok veya hatalı");
                                }

                                if (!string.IsNullOrEmpty(Kıstas))
                                {
                                    while (Çevirici.Yazıdan_NoktalıSayıya(Kıstas) == 0.0 && S.Çalışşsın && Senaryo.Çalışsın) Thread.Sleep(1500);
                                }

                                if (!string.IsNullOrEmpty(Saniye))
                                {
                                    int msn = (int)(Çevirici.Yazıdan_NoktalıSayıya(Saniye) * 1000);
                                    while (msn > 1500 && S.Çalışşsın && Senaryo.Çalışsın) { Thread.Sleep(1500); msn -= 1500; }
                                    Thread.Sleep(msn);
                                }
                            }
                            else throw new Exception("Adım : " + Senaryo.Adımlar[a].Etiket + ", işlev : " + (b + 1) + ", anahtar : " + jsonSıralayıcı.Current.Name + " anlamsız");
                        }
                    }
                }
            }
            catch (Exception ex) 
            { 
                Senaryo.Durum = " -> " + ex.Message; 
                Günlük.Ekle(ex.ToString());

                if (!string.IsNullOrEmpty(Senaryo.Beklenmeyen_Durumda_Çalıştırılacak_Adım) && S.Çalışşsın && Senaryo.Çalışsın)
                {
                    Thread.Sleep(1000);

                    SonrakiAdım = Senaryo.Beklenmeyen_Durumda_Çalıştırılacak_Adım;
                    goto YenidenDene;
                }
            }

            jsonSıralayıcı.Dispose();
            Senaryo.Durum = " -> Bitti";
            Senaryo.Görev_Nesnesi = null;
        }

        public void Çalıştır()
        {
            if (Görev_Nesnesi == null)
            {
                Çalışsın = true;

                Görev_Nesnesi = new Thread(() => Görev_İşlemi(this));
                Görev_Nesnesi.Start();
            }
        }
        public void Durdur()
        {
            Çalışsın = false;
        }
    }
    public class Senaryolar
    {
        //static Mutex Mtx = new Mutex();
        public static Dictionary<string, Senaryo_> Tümü = new Dictionary<string, Senaryo_>();
        public static bool MevcutMu(string Adı)
        {
            return Tümü.ContainsKey(Adı);
        }
        public static bool Ekle(Senaryo_ Senaryo)
        {
            if (MevcutMu(Senaryo.Adı)) return false;

            //Mtx.WaitOne();
            Tümü[Senaryo.Adı] = Senaryo;
            //Mtx.ReleaseMutex();

            return true;
        }
        public static Senaryo_ Bul(string Adı)
        {
            //Mtx.WaitOne();
            Senaryo_ girdi;
            bool sonuç = Tümü.TryGetValue(Adı, out girdi);
            //Mtx.ReleaseMutex();

            if (sonuç) return girdi;

            throw new Exception(Adı + " isimli senaryo listede bulunamadı");
        }
        public static void Çalıştır(string Adı)
        {
            Bul(Adı).Çalıştır();
        }
        public static void Durdur(string Adı)
        {
            Bul(Adı).Durdur();
        }
    }
}

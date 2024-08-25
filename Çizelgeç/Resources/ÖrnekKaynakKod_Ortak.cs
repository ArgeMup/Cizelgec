/* ArGeMuP Çizelgeç.exe BU KISMI DEĞİŞTİRMEYİNİZ - BAŞLANGIÇ */

/* ??? [[[ Detaylar ]]] %%%
*** Kullanılabilir Nesneler ***
--- Sadece Canlı Ekranlama ----------------------------------------------------------
    ÖnYüz
        İşlem : ÖnYüz.AçıklamaEkle(string Açıklama, Color Renk = default);
    Bağlantılar : Bilgi kaynakları ile uygulamayı birbirine bağlamak için kullanılır
        Örnek bir bağlantı gönderisi (UTF8 destekler)
            <CümleBaşlangıcı><KelimeAyracı><Sinyal Grubunun Adı><KelimeAyracı><Gruptaki 1. Sinyalin Değeri><KelimeAyracı><Gruptaki 2. Sinyalin Değeri><KelimeAyracı>...<CRLF>
            _Gürültü_>Sinyaller;GrupAdı;3.5;5;0CRLF
            Bu durumda uygulama <GrupAdı[0]>, <GrupAdı[1]> ve <GrupAdı[2]> sinyallerini üretir. İlk değerleri sırasıyla 3,5     5     0
            Aynı şekilde bilgi gelmeye devam ettikçe çizim oluşmaya başlar.
        İşlem : Çizelgeç.Bağlantı_ Bağlantılar.Ekle_KomutSatırı(string BağlantıAdı, string Uygulama, string Parametre = null);
        İşlem : Çizelgeç.Bağlantı_ Bağlantılar.Ekle_Uart(string BağlantıAdı, int BitHızı, string ErişimNoktası = "COMx");
        İşlem : Çizelgeç.Bağlantı_ Bağlantılar.Ekle_Tcpİstemcisi(string BağlantıAdı, string IPveyaAdresi, int ErişimNoktası);
        İşlem : Çizelgeç.Bağlantı_ Bağlantılar.Ekle_Udpİstemcisi(string BağlantıAdı, string IPveyaAdresi, int ErişimNoktası);
        İşlem : Çizelgeç.Bağlantı_ Bağlantılar.Ekle_TcpSunucusu(string BağlantıAdı, int ErişimNoktası);
        İşlem : Çizelgeç.Bağlantı_ Bağlantılar.Ekle_UdpSunucusu(string BağlantıAdı, int ErişimNoktası);
        İşlem : List<Çizelgeç.Bağlantı_> Bağlantılar.Bul(string BağlantıAdıKıstası = "*", bool BüyükKüçükHarfDuyarlı = true, char Ayraç = '*');
            İşlem Çıktısı : Çizelgeç.Bağlantı_ -> Bağlantı_1
                İşlem : Bağlantı_1.Ayıklayıcı_Ekle(string CümleBaşlangıcı_Sinyaller = ">Sinyaller", char KelimeAyracı = ';', bool Kaydedilsin = true, Sinyaller.Tanımlanmamış_Sinyali TanımlanmamışSinyalleri = Sinyaller.Tanımlanmamış_Sinyali.Kullan veya Kaydetme veya Atla, string CümleBaşlangıcı_Başlıklar = ">Basliklar");
                İşlem : Bağlantı_1.Başlat();
                İşlem : Bağlantı_1.Gönder(string Mesaj); //Sonuna uygulama tarafından CRLF eklenir
                İşlem : Bağlantı_1.Gönder(byte[] Mesaj);
                İşlem : Bağlantı_1.Sil();
                Değişken : DateTime Bağlantı_1.SonDeğerinAlındığıAn;
                Değişken : UInt64 Bağlantı_1.Sayac_Güncelleme;
    İşlemler : Kullanıcının basabileceği tuşlar üretmek için kullanılabilir.
        İşlem : İşlemler.Ekle(string İşlemAdı, Action<string, object> İşlem, object Hatırlatıcı = null);
            İşlem Girdisi : Action<string, object> İşlem
                void İşlem(string İşlemAdı, object Hatırlatıcı)
                {
                }
        İşlem : İşlemler.Sil(string İşlemAdıKıstası, bool BüyükKüçükHarfDuyarlı = true, char Ayraç = '*'); // İşlemAdı == null ise tümünü siler
    Görevler : Bir senaryo oluşturabilmek için kullanılabilir
        İşlem : Görevler.Ekle(string TakmaAdı, DateTime İlkTetikleyeceğiZaman, Func<string, object, int> Görev_İslemi, object Hatırlatıcı = null);
            İşlem Girdisi : Func<string, object, int> Görev_İslemi
                int Görev_İslemi(string TakmaAdı, object Hatırlatıcı)
                {
                    Geri dönüş değeri - Büyüktür 0 ise belirtilen süre kadar msn sonra tekrar çağırılır
                                        Eşittir 0 ise kendi iç sıralamasına göre en kısa sürede tekrar çağırılır
                                        Küçüktür 0 ise görevi siler, tekrar çağırılmaz
                }
        İşlem : List<ArgeMup.HazirKod.ArkaPlan.Hatırlatıcı_.Durum_> Görevler.Bul(bool SüresiDolanlarıDahilEt = true, bool ÇalışmayıBekleyenleriDahilEt = true, string TakmaAdıKıstası = "*", bool BüyükKüçükHarfDuyarlı = true, char Ayraç = '*');
            İşlem Çıktısı : ArgeMup.HazirKod.ArkaPlan.Hatırlatıcı_.Durum_ -> Durum_1
                Değişken : string Durum_1.TakmaAdı;
                Değişken : bool Durum_1.TetiklenmesiBekleniyor;
                Değişken : DateTime Durum_1.TetikleneceğiAn;
                Değişken : object Durum_1.Hatırlatıcı;
        İşlem : Görevler.Sil(string TakmaAdıKıstası, bool BüyükKüçükHarfDuyarlı = true, char Ayraç = '*'); // TakmaAdı == null ise tümünü siler
    BilgiToplama
        Değişken : bool BilgiToplama.BaşlatBeklet
            = true;     //Bağlantılar ve bağımlılıklalarını geçici olarak durdurur. 
            = false;    //Kaldığı yerden devam eder.
        Değişken : bool BilgiToplama.ZamanDilimi_BirbirininAynısıOlanlarıAtla = true;
        Değişken : int BilgiToplama.ZamanDilimi_msn = 500; //Ölçümleri belirtilen süre de bir kaydeder
        Değişken : long BilgiToplama.Kayıt_AzamiDosyaBoyutu_Bayt = 5 * 1024 * 1024; // 5 MiB 
    BilgiToplama : Buradaki ayarlamalar ilk açılışta 1 kere yapılıp, değiştirilmemeli
        Değişken : string BilgiToplama.Kayıt_Klasörü = null; //Boş bırakılırsa kaydedilmeden sadece görseller izlenerek kullanılabilir
        Değişken : int BilgiToplama.ZamanDilimi_Sayısı = 5000;

--- Sadece Ölü Ekranlama ------------------------------------------------------------
    ÖnYüz
        İşlem : ÖnYüz.İlerlemeÇubuğu(int Güncel, int Toplam); //uzun sürebilecek işlemlerde çağırılarak kullanıcıya fikir verilebilir.
    MupDosyasındanOkuma
        Değişken : MupDosyasındanOkuma.CümleBaşlangıcıVeKelimeAyraçları.Add(new string[] { ">Sinyaller", ";", ">Basliklar" });

--- Ortak ---------------------------------------------------------------------------
    ÖnYüz
        İşlem : ÖnYüz.Güncelle();
        İşlem : ÖnYüz.Günlük(string Girdi);
        İşlem : ÖnYüz.Günlük(Exception İstisna, string ÖnYazı = null);
        İşlem : ÖnYüz.SadeceŞuSinyalleriGöster(List<Çizelgeç.Sinyal_> Sinyaller);
        İşlem : ÖnYüz.UygulamayıKapat(bool BeşDkİçindeBilgisayarıKapat = false);
        Değişken : bool ÖnYüz.ÇalışmayaDevamEt; //Uygulamanın kullanıcı tarafından kapatıldığını raporlar. Döngülere eklenmesi önemli.
        Değişken : bool ÖnYüz.CanlıEkranlama; //Şuanda geçerli çalışma durumunu belirtir.
    Sinyaller
        İşlem : int Sinyaller.ZamanEkseni_EnYakın(DateTime TarihSaat);
        İşlem : DateTime Sinyaller.ZamanEkseni_TarihSaateÇevir(double TarihSaat);
        İşlem : double Sinyaller.ZamanEkseni_TarihSaattenÇevir(DateTime TarihSaat);
        Değişken : double[] Sinyaller.ZamanEkseni;
        Geri Bildirim İşlemi : Func<bool> Sinyaller.GeriBildirimİşlemi_Kaydedilecek;
            bool GeriBildirimİşlemi_Kaydedilecek()
            {
                Geri dönüş değeri - Kaydedilmesi gereken bir grup bilgi kaydedilmek istenmez ise false döndürülmeli.
            }
        İşlem : Çizelgeç.Sinyal_ Sinyaller.Ekle(string SinyalAdı, string AğaçİçindekiDalınAdı, string GörünenAdı, bool Kaydedilsin = true);
        İşlem : List<Çizelgeç.Sinyal_> Sinyaller.Bul(string SinyalAdıKıstası = "*", bool BüyükKüçükHarfDuyarlı = true, char Ayraç = '*');
            İşlem Çıktısı : Çizelgeç.Sinyal_ -> Sinyal_1
                İşlem : Sinyal_1.Güncelle_SonDeğer(double Girdi);
                İşlem : Sinyal_1.Sil();                
                Değişken : string Sinyal_1.Adı.Sinyal;
                Değişken : string Sinyal_1.Adı.Csv;
                Değişken : double Sinyal_1.Değeri.SonDeğeri;
                Değişken : DateTime Sinyal_1.Değeri.SonDeğerinAlındığıAn;
                Değişken : double[] Sinyal_1.Değeri.DeğerEkseni;
                Değişken : double Sinyal_1.Değeri.ZamanAşımı_Sn;
                Değişken : bool Sinyal_1.Değeri.ZamanAşımı_Oldu;
                Değişken : UInt64 Sinyal_1.Değeri.Sayac_Güncelleme;
                Geri Bildirim İşlemi : Func<Sinyal_, double, double> Sinyal_1.Değeri.GeriBildirimİşlemi_GüncelDeğeriGüncellendi;
                    double GeriBildirimİşlemi_GüncelDeğeriGüncellendi(Çizelgeç.Sinyal_ Sinyal, double GüncelDeğeri)
                    {
                        //GüncelDeğeri değişkeni değerlendirilip, yeni değer geri döndürülür
                        //Örneğin güncel değeri 5000 birim olan GRAM cinsinden bir bilgiyi KG cinsine çevirerek içeri almak istiyorsak
                            
                        return GüncelDeğeri / 1000;
                    }
                Geri Bildirim İşlemi : Action<Çizelgeç.Sinyal_, TimeSpan> Sinyal_1.Değeri.GeriBildirimİşlemi_ZamanAşımı;
                    void GeriBildirimİşlemi_ZamanAşımı(Çizelgeç.Sinyal_ Sinyal, TimeSpan GeçenSüre)
                    {
                        //GeçenSüre == default ise zaman aşımı oldu, diğer durumda zaman aşımı bitti
                    }
*/

/* ArGeMuP Çizelgeç.exe BU KISMI DEĞİŞTİRMEYİNİZ - BİTİŞ */
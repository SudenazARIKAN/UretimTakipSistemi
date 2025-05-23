namespace UretimTakipSistemi.Models
{
    public class UretimOperasyon
    {
        public int KayitNo { get; set; }
        public DateTime Baslangic { get; set; }
        public DateTime Bitis { get; set; }
        public TimeSpan ToplamSure => Bitis - Baslangic;
        public string? Statu { get; set; }
        public string? DurusNedeni { get; set; }

        public UretimOperasyon()
        {
            Statu = "";       // boş string olarak varsayılan değer
            DurusNedeni = ""; // boş string olarak varsayılan değer
        }
    }
}

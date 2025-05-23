using Microsoft.AspNetCore.Mvc;
using UretimTakipSistemi.Models;

namespace UretimTakipSistemi.Controllers
{
    public class UretimController : Controller
    {
        public IActionResult Index()
        {
            // Orijinal kayıtlar (Tablo 1)
            var orijinal = new List<UretimOperasyon>
            {
                new UretimOperasyon { KayitNo = 1, Baslangic = DateTime.Parse("2020-05-23 07:30"), Bitis = DateTime.Parse("2020-05-23 08:30"), Statu = "URETIM" },
                new UretimOperasyon { KayitNo = 2, Baslangic = DateTime.Parse("2020-05-23 08:30"), Bitis = DateTime.Parse("2020-05-23 12:00"), Statu = "URETIM" },
                new UretimOperasyon { KayitNo = 3, Baslangic = DateTime.Parse("2020-05-23 12:00"), Bitis = DateTime.Parse("2020-05-23 13:00"), Statu = "URETIM" },
                new UretimOperasyon { KayitNo = 4, Baslangic = DateTime.Parse("2020-05-23 13:00"), Bitis = DateTime.Parse("2020-05-23 13:45"), Statu = "DURUS", DurusNedeni = "ARIZA" },
                new UretimOperasyon { KayitNo = 5, Baslangic = DateTime.Parse("2020-05-23 13:45"), Bitis = DateTime.Parse("2020-05-23 17:30"), Statu = "URETIM" }
            };

            var islenmis = UretimVerisiniIsle(orijinal);

            return View(islenmis);
        }

        private List<UretimOperasyon> UretimVerisiniIsle(List<UretimOperasyon> orijinalListe)
        {
            var molalar = new List<(TimeSpan Baslangic, TimeSpan Bitis, string Nedeni)>
            {
                (TimeSpan.Parse("10:00"), TimeSpan.Parse("10:15"), "Çay Molası"),
                (TimeSpan.Parse("12:00"), TimeSpan.Parse("12:30"), "Yemek Molası"),
                (TimeSpan.Parse("15:00"), TimeSpan.Parse("15:15"), "Çay Molası")
            };

            var yeniListe = new List<UretimOperasyon>();
            int yeniKayitNo = 1;

            foreach (var kayit in orijinalListe)
            {
                // Durus ise ekle, direkt geç
                if (kayit.Statu == "DURUS")
                {
                    yeniListe.Add(new UretimOperasyon
                    {
                        KayitNo = yeniKayitNo++,
                        Baslangic = kayit.Baslangic,
                        Bitis = kayit.Bitis,
                        Statu = "DURUS",
                        DurusNedeni = kayit.DurusNedeni
                    });
                    continue;
                }

                var baslangic = kayit.Baslangic;
                var bitis = kayit.Bitis;

                foreach (var mola in molalar)
                {
                    var molaBas = baslangic.Date + mola.Baslangic;
                    var molaBit = baslangic.Date + mola.Bitis;

                    if (molaBit <= baslangic || molaBas >= bitis)
                        continue;

                    if (baslangic < molaBas)
                    {
                        yeniListe.Add(new UretimOperasyon
                        {
                            KayitNo = yeniKayitNo++,
                            Baslangic = baslangic,
                            Bitis = molaBas,
                            Statu = "URETIM"
                        });
                    }

                    yeniListe.Add(new UretimOperasyon
                    {
                        KayitNo = yeniKayitNo++,
                        Baslangic = molaBas,
                        Bitis = molaBit,
                        Statu = "DURUS",
                        DurusNedeni = mola.Nedeni
                    });

                    baslangic = molaBit;
                }

                if (baslangic < bitis)
                {
                    yeniListe.Add(new UretimOperasyon
                    {
                        KayitNo = yeniKayitNo++,
                        Baslangic = baslangic,
                        Bitis = bitis,
                        Statu = "URETIM"
                    });
                }
            }

            return yeniListe;
        }
    }
}

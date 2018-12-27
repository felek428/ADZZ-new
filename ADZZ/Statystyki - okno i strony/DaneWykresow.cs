using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADZZ.Statystyki___okno_i_strony
{
    class DaneWykresow
    {
        PolaczenieBazaDataContext Polaczenie = new PolaczenieBazaDataContext();
        public int liczba { get; set; }
        public DaneWykresow()
        {

        }
        public double WydatkiZwierze(string wybranyKolczyk)
        {
            var queryWydatki = (from R in Polaczenie.Rozliczenia
                                where R.Zwierze.nr_kolczyka == wybranyKolczyk && R.Kategoria_rozliczen.czyPrzychod == 0
                                select R.kwota).ToList();
            double sumaKwota = 0;

            foreach (var item in queryWydatki)
            {
                sumaKwota += (double)item;
            }

            return sumaKwota;
        }

        public double PrzychodZwierze(string wybranyKolczyk)
        {
            var queryPrzychod = (from R in Polaczenie.Rozliczenia
                                 where R.Zwierze.nr_kolczyka == wybranyKolczyk && (R.Kategoria_rozliczen.czyPrzychod == 1 || R.ilosc_litrow != null)
                                 select new { Kwota = R.kwota, Data = R.data, Litry = R.ilosc_litrow }).ToList();
            var queryCena = (from Cena in Polaczenie.Historia_cen
                             where Cena.id_kategoria_rozliczen == 1
                             select Cena).ToList();

            double sumaKwota = 0;
            double sumaKwotaLitry = 0;


            foreach (var item in queryPrzychod)
            {
                if (item.Kwota != null)
                {
                    sumaKwota += (double)item.Kwota;
                }
                else if (item.Litry != null)
                {
                    foreach (var cena in queryCena)
                    {
                        if (item.Data >= cena.okres_od && item.Data <= cena.okres_do)
                        {
                            sumaKwotaLitry += (double)item.Litry * (double)cena.cena;
                        }
                    }
                }
            }

            return sumaKwota + sumaKwotaLitry;
        }
    }
}

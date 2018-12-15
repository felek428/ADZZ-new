using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.DataVisualization.Charting;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ADZZ.Zarządzanie_zwierzetami___okno_i_strony
{
    /// <summary>
    /// Logika interakcji dla klasy StadoInformacje.xaml
    /// </summary>
    public partial class StadoInformacje : Page
    {
        PolaczenieBazaDataContext Polaczenie = new PolaczenieBazaDataContext();
        private string wybranyKolczyk = string.Empty;
        public StadoInformacje(string kolczyk)
        {
            InitializeComponent();
            ramkaInformacji.Content = new FormularzDodaniaStada(kolczyk);
            wybranyKolczyk = kolczyk;
            WypelnienieWykres();
            
        }
        public StadoInformacje()
        {
            InitializeComponent();
        }

        private void WypelnienieWykres()
        {
            ((PieSeries)Wykres.Series[0]).ItemsSource =
                new KeyValuePair<string, double>[]{
                new KeyValuePair<string,double>("Wydatki", WydatkiStado()),
                new KeyValuePair<string,double>("Przychód", PrzychodStado())
                };
            
        }

        private double WydatkiStado()
        {
            var queryWydatki = (from R in Polaczenie.Rozliczenia
                                where R.Stado.nr_stada == wybranyKolczyk && R.Kategoria_rozliczen.czyPrzychod == 0
                                select new { Kwota = R.kwota, Data = R.data }).ToList();
            double sumaWydatki = 0;
            foreach (var item in queryWydatki)
            {
                if(item.Kwota != null)
                {
                    sumaWydatki += (double)item.Kwota;
                }
            }

            return sumaWydatki;
        }

        private double PrzychodStado()
        {
            var queryPrzychod = (from R in Polaczenie.Rozliczenia
                                 where R.Stado.nr_stada == wybranyKolczyk && R.Kategoria_rozliczen.czyPrzychod == 1
                                 select new { Kwota = R.kwota, Data = R.data }).ToList();
            double sumaPrzychod = 0;

            foreach (var item in queryPrzychod)
            {
                if(item.Kwota != null)
                {
                    sumaPrzychod += (double)item.Kwota;
                }
            }

            return sumaPrzychod;
        }
    }
}

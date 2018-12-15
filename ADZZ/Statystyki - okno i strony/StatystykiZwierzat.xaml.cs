using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
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

namespace ADZZ.Statystyki___okno_i_strony
{
    /// <summary>
    /// Logika interakcji dla klasy StatystykiZwierzat.xaml
    /// </summary>
    public partial class StatystykiZwierzat : Page
    {
        PolaczenieBazaDataContext Polaczenie = new PolaczenieBazaDataContext();
        private string TytulSerii = string.Empty;
        public StatystykiZwierzat()
        {
            InitializeComponent();
            WypelnienieCbStatystyk();
            cbRodzajStatystyk.SelectedIndex = 0;
            //LoadColumnChartData();
            //W.DataContext = new MyPointsCollection(5);

        }

       


        private void RozszerzWykres(int dlugosclisty,Chart wykres)
        {
            if (dlugosclisty > 9)
            {
                wykres.Width = 80 * dlugosclisty;
            }
            else wykres.Width = this.Width - 80;
        }
        private void WypelnienieCbStatystyk()
        {
            cbRodzajStatystyk.Items.Add("Liczba zwierzat");
            cbRodzajStatystyk.Items.Add("Liczba laktacji");
            cbRodzajStatystyk.Items.Add("Przychód i wydatki");
            cbRodzajStatystyk.Items.Add("Cena mleka");
            cbRodzajStatystyk.Items.Add("Wydatki zwierząt");
        }
        private void LoadColumnChartData()
        {



            ((ColumnSeries)WykresKolumnowy.Series[0]).ItemsSource =
                new KeyValuePair<string, int>[]{
        new KeyValuePair<string,int>("Project Manager", 12),
        new KeyValuePair<string,int>("CEO", 25),
        new KeyValuePair<string,int>("Software Engg.", 5),
        new KeyValuePair<string,int>("Team Leader", 6),
        new KeyValuePair<string,int>("Project Leader", 10),
        new KeyValuePair<string,int>("Developer", 4) };
        }
        #region Źródła danych do wykresów

        private KeyValuePair<string, int>[] LiczbaZwierzatDanejRasy()
        {
            var queryRasa = (from Rasa in Polaczenie.Rasa
                             select Rasa).ToList();
            var queryZwierze = (from Zwierze in Polaczenie.Zwierze
                                select Zwierze).ToList();



            var zestawienie = new KeyValuePair<string, int>[queryRasa.Count];
            for (int i = 0; i < queryRasa.Count; i++)
            {
                var licznikSztuk = 0;
                foreach (var zwierzak in queryZwierze)
                {
                    if (zwierzak.id_rasa == queryRasa[i].Id)
                    {
                        licznikSztuk++;
                    }
                }
                zestawienie[i] = new KeyValuePair<string, int>(queryRasa[i].nazwa, licznikSztuk);
            }
            return zestawienie;
        }

        private KeyValuePair<string, int>[] LiczbaLaktacji()
        {
            var queryRozrod = (from Rozrod in Polaczenie.Rozrod
                               select Rozrod).ToList();
            var queryZwierze = (from Zwierze in Polaczenie.Zwierze
                                select Zwierze).ToList();



            var zestawienie = new KeyValuePair<string, int>[queryZwierze.Count];
            for (int i = 0; i < queryZwierze.Count; i++)
            {
                var licznikSztuk = 0;
                foreach (var wpis in queryRozrod)
                {
                    if (wpis.id_zwierze == queryZwierze[i].Id && wpis.czyRuja == 0)
                    {
                        licznikSztuk++;
                    }
                }
                zestawienie[i] = new KeyValuePair<string, int>(queryZwierze[i].nr_kolczyka, licznikSztuk);
            }
            return zestawienie;
        }

        private KeyValuePair<string, double>[] PrzychodZwierzat()
        {
            DaneWykresow noweDane = new DaneWykresow();

            var zestawienie = new KeyValuePair<string, double>[12];
            Console.WriteLine(QueryPrzychod().Count);
            double buffor;
            var miesiace = DateTimeFormatInfo.CurrentInfo.MonthNames;


            for (int i = 0; i < 12; i++)
            {
                buffor = 0;
                foreach (var item in QueryPrzychod())
                {
                    if (i + 1 == item.data.Month && item.kwota != null)
                    {
                        buffor += (double)item.kwota;
                    }
                }


                Console.WriteLine(miesiace[i]);
                zestawienie[i] = new KeyValuePair<string, double>(miesiace[i], buffor);
            }


            return zestawienie;
        }

        private KeyValuePair<string, double>[] WydatkiZwierzat()
        {


            var zestawienie = new KeyValuePair<string, double>[12];
            var miesiace = DateTimeFormatInfo.CurrentInfo.MonthNames.ToList();
            Console.WriteLine(miesiace.Count);
            foreach (var item in miesiace)
            {
                Console.WriteLine(item);
            }
            double buffor;
            for (int i = 0; i < miesiace.Count - 1; i++)
            {
                buffor = 0;
                foreach (var item in QueryWydatki())
                {
                    if (i + 1 == item.data.Month && item.kwota != null)
                    {
                        buffor += (double)item.kwota;
                    }
                }



                zestawienie[i] = new KeyValuePair<string, double>(miesiace[i], buffor);
            }

            return zestawienie;
        }
        #endregion




        private void cbRodzajStatystyk_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            

            switch(cbRodzajStatystyk.SelectedItem)
            {
                case "Liczba zwierzat":
                    WykresKolumnowy.Visibility = Visibility.Visible;
                    WykresKolowy.Visibility = Visibility.Hidden;
                    WykresLiniowy.Visibility = Visibility.Hidden;

                    WykresKolumnowy.Series.Clear();

                    DodajSerie(typeof(ColumnSeries), "Liczba zwierząt", WykresKolumnowy);

                   
                    ((ColumnSeries)WykresKolumnowy.Series[0]).ItemsSource = LiczbaZwierzatDanejRasy();

                    
                    RozszerzWykres(LiczbaZwierzatDanejRasy().Length,WykresKolumnowy);




                    break;
                case "Liczba laktacji":
                    WykresKolowy.Visibility = Visibility.Visible;
                    WykresKolumnowy.Visibility = Visibility.Hidden;
                    WykresLiniowy.Visibility = Visibility.Hidden;
                    WykresKolowy.Series.Clear();

                    DodajSerie(typeof(PieSeries), "Liczba laktacji", WykresKolowy);
                    ((PieSeries)WykresKolowy.Series[0]).ItemsSource = LiczbaLaktacji();
                    
                    SeriaPie.Title = "Liczba laktacji";
                    break;
                case "Przychód i wydatki":

                    WykresKolumnowy.Visibility = Visibility.Visible;
                    WykresKolowy.Visibility = Visibility.Hidden;
                    WykresLiniowy.Visibility = Visibility.Hidden;
                    
                    WykresKolumnowy.Series.Clear();


                    DodajSerie(typeof(ColumnSeries),"Przychod",WykresKolumnowy);
                    DodajSerie(typeof(ColumnSeries),"Wydatki",WykresKolumnowy);

                    ((ColumnSeries)WykresKolumnowy.Series[0]).ItemsSource = PrzychodZwierzat();
                    ((ColumnSeries)WykresKolumnowy.Series[1]).ItemsSource = WydatkiZwierzat();
                    RozszerzWykres(PrzychodZwierzat().Length, WykresKolumnowy);
                    break;
                case "Cena mleka":
                    WykresKolumnowy.Visibility = Visibility.Hidden;
                    WykresKolowy.Visibility = Visibility.Hidden;
                    WykresLiniowy.Visibility = Visibility.Visible;
                    WykresLiniowy.Series.Clear();

                    DodajSerie(typeof(LineSeries), "Cena mleka", WykresLiniowy);
                    
                  //  ((LineSeries)WykresLiniowy.Series[0]).ItemsSource = 
       

                    break;
                case "Wydatki zwierząt":

                    break;
                
                default:
                    break;
            }

            

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="typ"> Typ serii tzn. LineSeries itp</param>
        /// <param name="tytulSerii">Nazwa serii wyswietlana w legendzie</param>
        /// <param name="wykres">Nazwa wykresu na który nanoszona jest seria</param>
        private void DodajSerie(Type typ, string tytulSerii, Chart wykres)
        {
            
            
            if (typ == typeof(ColumnSeries))
            {
                ColumnSeries nowaSeria = new ColumnSeries();
                nowaSeria.DependentValuePath = "Value";
                nowaSeria.IndependentValuePath = "Key";
                nowaSeria.Title = tytulSerii;
                wykres.Series.Add(nowaSeria);
            }
            else if(typ == typeof(PieSeries))
            {
                PieSeries nowaSeria = new PieSeries();
                nowaSeria.DependentValuePath = "Value";
                nowaSeria.IndependentValuePath = "Key";
                nowaSeria.Title = tytulSerii;
                wykres.Series.Add(nowaSeria);
            }
            else
            {
                LineSeries nowaSeria = new LineSeries();
                nowaSeria.DependentValuePath = "Value";
                nowaSeria.IndependentValuePath = "Key";
                nowaSeria.Title = tytulSerii;
                wykres.Series.Add(nowaSeria);
            }
            
        }

        #region Queries
        private List<Zwierze> QueryZwierze()
        {
            var query = (from Z in Polaczenie.Zwierze
                         select Z).ToList();

            return query;
        }
        private List<Stado> QueryStado()
        {
            var query = (from S in Polaczenie.Stado
                         select S).ToList();
            return query;
        }
        private List<Rozliczenia> QueryPrzychod()
        {
            var query = (from R in Polaczenie.Rozliczenia
                         where R.Kategoria_rozliczen.czyPrzychod == 1
                         select R).ToList();

            return query;
        }
        private List<Rozliczenia> QueryWydatki()
        {
            var query = (from R in Polaczenie.Rozliczenia
                         where R.Kategoria_rozliczen.czyPrzychod == 0
                         select R).ToList();
            return query;
        }
        #endregion

    }
    
}

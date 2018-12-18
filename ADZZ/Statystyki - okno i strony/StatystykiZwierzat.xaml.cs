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

       

        /// <summary>
        /// Zwiększa szerkość wykresu jeżeli jest znaczaca ilość danych na osi x
        /// </summary>
        /// <param name="dlugoscListy">Ilość danych naniesionych na oś x</param>
        /// <param name="wykres">Wykres na który nakładane są dane</param>
        /// <param name="czyDlugie">Jeżeli nazwy danych są długie, wartość true zwiększy szerokość na wykresie dla 1 kolumny</param>
        private void RozszerzWykres(int dlugoscListy, Chart wykres, bool czyDlugie)
        {
            if (dlugoscListy > 15 && czyDlugie == true)
            {
                wykres.Width = 130 * dlugoscListy;
            }
            else if (dlugoscListy > 9 )
            {
                wykres.Width = 80 * dlugoscListy;
            }
            else wykres.Width = this.Width - 80;
        }
        private void WypelnienieCbStatystyk()
        {
            cbRodzajStatystyk.Items.Add("Liczba zwierzat");
            cbRodzajStatystyk.Items.Add("Liczba laktacji");
            cbRodzajStatystyk.Items.Add("Przychód i wydatki");
            cbRodzajStatystyk.Items.Add("Cena mleka");
            cbRodzajStatystyk.Items.Add("Wydajność mleczna");
        }
        
        #region Źródła danych do wykresów

        private KeyValuePair<string, int>[] LiczbaZwierzatDanejRasy()
        {
            
            var zestawienie = new KeyValuePair<string, int>[QueryRasa().Count];
            for (int i = 0; i < QueryRasa().Count; i++)
            {
                var licznikSztuk = 0;
                foreach (var zwierzak in QueryZwierze())
                {
                    if (zwierzak.id_rasa == QueryRasa()[i].Id)
                    {
                        licznikSztuk++;
                    }
                }
                zestawienie[i] = new KeyValuePair<string, int>(QueryRasa()[i].nazwa, licznikSztuk);
            }
            return zestawienie;
        }

        private List<KeyValuePair<string, int>> LiczbaLaktacji()
        {
            
            
            var zestawienie = new List<KeyValuePair<string, int>>();
            for (int i = 0; i < QueryZwierze().Count; i++)
            {
                var licznikLaktacji = 0;
                foreach (var wpis in QueryRozrod())
                {
                    if (wpis.id_zwierze == QueryZwierze()[i].Id && wpis.czyRuja == 0)
                    {
                        licznikLaktacji++;
                    }
                }
                if(licznikLaktacji != 0) {
                    zestawienie.Add(new KeyValuePair<string, int>(QueryZwierze()[i].nr_kolczyka, licznikLaktacji));
                }
            }
            return zestawienie;
        }

        private KeyValuePair<string, double>[] PrzychodZwierzat()
        {
            DaneWykresow noweDane = new DaneWykresow();

            var zestawienie = new KeyValuePair<string, double>[12];

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



                zestawienie[i] = new KeyValuePair<string, double>(miesiace[i], buffor);
            }


            return zestawienie;
        }

        private KeyValuePair<string, double>[] WydatkiZwierzat()
        {


            var zestawienie = new KeyValuePair<string, double>[12];
            var miesiace = DateTimeFormatInfo.CurrentInfo.MonthNames.ToList();

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
        private KeyValuePair<string, double>[] CenyMleka()
        {
            var zestawienie = new KeyValuePair<string, double>[QueryCenyMleka().Count];

            for (int i = 0; i < QueryCenyMleka().Count; i++)
            {
                zestawienie[i] = new KeyValuePair<string, double>(Convert.ToDateTime(QueryCenyMleka()[i].okres_od).ToShortDateString(), (double)QueryCenyMleka()[i].cena);
            }


            return zestawienie;
        }

        private List<KeyValuePair<string, int>> WydajnoscMleczna()
        {
            var zestawienie = new List<KeyValuePair<string, int>>();
            var listaZwierze = QueryZwierze();
            var listaMleko = QueryMleko();
            int buffor;

            for(int i = 0; i < listaZwierze.Count; i++)
            {
                buffor = 0;
                foreach (var item in listaMleko)
                {
                    if(item.id_zwierze == listaZwierze[i].Id)
                    {
                        buffor += (int)item.ilosc_litrow;
                    }

                }
                if(buffor != 0)
                {
                    zestawienie.Add(new KeyValuePair<string, int>(listaZwierze[i].nr_kolczyka, buffor));
                }
            }         
            return zestawienie;
        }
        #endregion




        private void cbRodzajStatystyk_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            

            switch(cbRodzajStatystyk.SelectedItem)
            {
                case "Liczba zwierzat":
                    /*
                    WykresKolumnowy.Visibility = Visibility.Visible;
                    WykresKolowy.Visibility = Visibility.Hidden;
                    WykresLiniowy.Visibility = Visibility.Hidden;
                    */
                    svColumn.Visibility = Visibility.Visible;
                    svLine.Visibility = Visibility.Hidden;

                    WykresKolumnowy.Series.Clear();

                    DodajSerie(typeof(ColumnSeries), "Liczba zwierząt", WykresKolumnowy);

                   
                    ((ColumnSeries)WykresKolumnowy.Series[0]).ItemsSource = LiczbaZwierzatDanejRasy();

                    
                    RozszerzWykres(LiczbaZwierzatDanejRasy().Length,WykresKolumnowy,false);




                    break;
                case "Liczba laktacji":
                    /*
                    WykresKolowy.Visibility = Visibility.Hidden;
                    WykresKolumnowy.Visibility = Visibility.Visible;
                    WykresLiniowy.Visibility = Visibility.Hidden;
                    */
                    svColumn.Visibility = Visibility.Visible;
                    svLine.Visibility = Visibility.Hidden;

                    WykresKolowy.Series.Clear();

                    WykresKolumnowy.Series.Clear();
                    DodajSerie(typeof(ColumnSeries), "Liczba laktacji", WykresKolumnowy);
                    ((ColumnSeries)WykresKolumnowy.Series[0]).ItemsSource = LiczbaLaktacji();


                    RozszerzWykres(LiczbaLaktacji().Count, WykresKolumnowy,true);

                    SeriaPie.Title = "Liczba laktacji";
                    break;
                case "Przychód i wydatki":
                    /*
                    WykresKolumnowy.Visibility = Visibility.Visible;
                    WykresKolowy.Visibility = Visibility.Hidden;
                    WykresLiniowy.Visibility = Visibility.Hidden;
                    */
                    svColumn.Visibility = Visibility.Visible;
                    svLine.Visibility = Visibility.Hidden;

                    WykresKolumnowy.Series.Clear();


                    DodajSerie(typeof(ColumnSeries),"Przychod",WykresKolumnowy);
                    DodajSerie(typeof(ColumnSeries),"Wydatki",WykresKolumnowy);

                    ((ColumnSeries)WykresKolumnowy.Series[0]).ItemsSource = PrzychodZwierzat();
                    ((ColumnSeries)WykresKolumnowy.Series[1]).ItemsSource = WydatkiZwierzat();
                    RozszerzWykres(PrzychodZwierzat().Length, WykresKolumnowy,false);
                    break;
                case "Cena mleka":
                    /*
                    WykresKolumnowy.Visibility = Visibility.Hidden;
                    WykresKolowy.Visibility = Visibility.Hidden;
                    WykresLiniowy.Visibility = Visibility.Visible;
                    */
                    svColumn.Visibility = Visibility.Hidden;
                    svLine.Visibility = Visibility.Visible;
                    WykresLiniowy.Series.Clear();

                    DodajSerie(typeof(LineSeries), "Cena mleka", WykresLiniowy);

                    ((LineSeries)WykresLiniowy.Series[0]).ItemsSource = CenyMleka();
                    RozszerzWykres(CenyMleka().Length, WykresLiniowy, true);

                    break;
                case "Wydajność mleczna":
                    /*
                    WykresKolumnowy.Visibility = Visibility.Visible;
                    WykresKolowy.Visibility = Visibility.Hidden;
                    WykresLiniowy.Visibility = Visibility.Hidden;
                    */
                    svColumn.Visibility = Visibility.Visible;
                    svLine.Visibility = Visibility.Hidden;

                    WykresKolumnowy.Series.Clear();
                    DodajSerie(typeof(ColumnSeries), "Liczba litrów", WykresKolumnowy);
                    ((ColumnSeries)WykresKolumnowy.Series[0]).ItemsSource = WydajnoscMleczna();
                    RozszerzWykres(WydajnoscMleczna().Count,WykresKolumnowy,true);
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
        private List<Rasa> QueryRasa()
        {
            var queryRasa = (from Rasa in Polaczenie.Rasa
                             select Rasa).ToList();
            return queryRasa;
        }

        private List<Historia_cen> QueryCenyMleka()
        {
            var query = (from H in Polaczenie.Historia_cen
                         orderby H.okres_od ascending
                         select H).ToList();
            return query;
        }
        private List<Rozliczenia> QueryRozliczenia()
        {
            var query = (from R in Polaczenie.Rozliczenia
                         select R).ToList();
            return query;
        }
        /// <summary>
        /// Zwraca listę zwierzaków które dają mleko
        /// </summary>
        /// <returns></returns>
        private List<Rozliczenia> QueryMleko()
        {
            var query = (from R in Polaczenie.Rozliczenia
                        where R.ilosc_litrow != null
                        select R).ToList();

            return query;
        }

        private List<Rozrod> QueryRozrod()
        {
            var queryRozrod = (from Rozrod in Polaczenie.Rozrod
                               select Rozrod).ToList();

            return queryRozrod;

        }
        #endregion

    }
    
}

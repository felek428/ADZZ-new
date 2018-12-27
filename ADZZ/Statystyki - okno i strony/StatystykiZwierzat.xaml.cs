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
        DateTime okresOd;
        DateTime okresDo;
        public StatystykiZwierzat()
        {
            InitializeComponent();
            WypelnienieCbStatystyk();
            ResetujOkresOd();
            ResetujOkresDo();

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
            cbRodzajStatystyk.Items.Add("Zestawienie rozliczeń");
        }
        
        #region Źródła danych do wykresów

        private List<KeyValuePair<string, int>> LiczbaZwierzatDanejRasy(DateTime okres_od, DateTime okres_do)
        {
            var listaRasa = QueryRasa();
            var listaZwierze = QueryZwierze();
            var zestawienie = new List<KeyValuePair<string, int>>();
            for (int i = 0; i < listaRasa.Count; i++)
            {
                var licznikSztuk = 0;
                foreach (var zwierzak in listaZwierze)
                {
                    if (okres_od <= zwierzak.okres_od && (zwierzak.okres_do <= okres_do || zwierzak.okres_do == null))
                    {
                        if (zwierzak.id_rasa == listaRasa[i].Id)
                        {
                            licznikSztuk++;
                        }
                    }        
                }
                if(licznikSztuk != 0)
                {
                    zestawienie.Add(new KeyValuePair<string, int>(listaRasa[i].nazwa, licznikSztuk));
                }
            }
            return zestawienie;
        }

        private List<KeyValuePair<string, int>> LiczbaLaktacji()
        {
            var listaZwierze = QueryZwierze();
            var listaRozrod = QueryRozrod();

            var zestawienie = new List<KeyValuePair<string, int>>();
            for (int i = 0; i < listaZwierze.Count; i++)
            {
                var licznikLaktacji = 0;
                foreach (var wpis in listaRozrod)
                {
                    
                    if (wpis.id_zwierze == listaZwierze[i].Id && wpis.czyRuja == 0)
                    {
                        licznikLaktacji++;
                    }

                    
                }
                if(licznikLaktacji != 0) {
                    zestawienie.Add(new KeyValuePair<string, int>(listaZwierze[i].nr_kolczyka, licznikLaktacji));
                }
            }
            return zestawienie;
        }

        private double PrzychodZwierzat(DateTime okres_od, DateTime okres_do)
        {
            double zestawienie = 0;

            var listaPrzychod = QueryPrzychod();
            var listaCenyMleka = QueryCenyMleka();
            var miesiace = DateTimeFormatInfo.CurrentInfo.MonthNames;

            double buffor;
            double sumaKwotaLitry;


            
            
            foreach (var item in listaPrzychod)
            {
                buffor = 0;
                sumaKwotaLitry = 0;
                if (okres_od <= item.data && item.data <= okres_do )
                {
                    if (item.kwota != null)
                    {
                        buffor += (double)item.kwota;
                    }
                    else if (item.ilosc_litrow != null)
                    {
                        foreach (var cena in listaCenyMleka)
                        {
                            if (item.data >= cena.okres_od && item.data <= cena.okres_do)
                            {
                                sumaKwotaLitry += (double)item.ilosc_litrow * (double)cena.cena;
                            }
                        }
                    }
                }

            zestawienie += buffor + sumaKwotaLitry;
            }


            return zestawienie;
        }

        private double WydatkiZwierzat(DateTime okres_od, DateTime okres_do)
        {


            double zestawienie = 0;
            var listaWydatki = QueryWydatki();

            var miesiace = DateTimeFormatInfo.CurrentInfo.MonthNames.ToList();

            double buffor;
            
           
            foreach (var item in listaWydatki)
            {
                buffor = 0;
                if (okres_od <= item.data && item.data <= okres_do)
                {
                    if (item.kwota != null)
                    {
                        buffor += (double)item.kwota;
                    }
                }
                zestawienie += buffor;
            }



            
            

            return zestawienie;
        }
        private List<KeyValuePair<string, double>> CenyMleka(DateTime okres_od, DateTime okres_do)
        {
            var zestawienie = new List<KeyValuePair<string, double>>();
            var listaCenyMleka = QueryCenyMleka();
            for (int i = 0; i < listaCenyMleka.Count; i++)
            {
                if (okres_od <= listaCenyMleka[i].okres_od && (listaCenyMleka[i].okres_do <= okres_do || listaCenyMleka[i].okres_do == null))
                {
                    zestawienie.Add(new KeyValuePair<string, double>(Convert.ToDateTime(listaCenyMleka[i].okres_od).ToShortDateString(), (double)listaCenyMleka[i].cena));
                }

            }


            return zestawienie;
        }

        private List<KeyValuePair<string, int>> WydajnoscMleczna(DateTime okres_od, DateTime okres_do)
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
                    if (okres_od <= item.data && item.data <= okres_do)
                    {
                        if (item.id_zwierze == listaZwierze[i].Id)
                        {
                            buffor += (int)item.ilosc_litrow;
                        }
                    }
                }
                if(buffor != 0)
                {
                    zestawienie.Add(new KeyValuePair<string, int>(listaZwierze[i].nr_kolczyka, buffor));
                }
            }         
            return zestawienie;
        }

        private List<KeyValuePair<string, double>> ZestawienieRozliczen(DateTime okres_od, DateTime okres_do)
        {
            var zestawienie = new List<KeyValuePair<string, double>>();
            var listaKategorieRoziczen = QueryKategorieRozliczen();
            var listaRozliczenia = QueryRozliczenia();
            var listaCenyMleka = QueryCenyMleka();

            double buffor;

            foreach (var kategoria in listaKategorieRoziczen)
            {
                buffor = 0;

                foreach (var rozliczenie in listaRozliczenia)
                {
                    if (okres_od <= rozliczenie.data && rozliczenie.data <= okres_do)
                    {
                        if (kategoria.Id == rozliczenie.id_kategoria)
                        {
                            if (rozliczenie.kwota != null)
                            {
                                buffor += (double)rozliczenie.kwota;
                            }
                            else if (rozliczenie.ilosc_litrow != null)
                            {
                                foreach (var cena in listaCenyMleka)
                                {

                                    if (rozliczenie.data >= cena.okres_od && rozliczenie.data <= cena.okres_do)
                                    {
                                        buffor += (double)rozliczenie.ilosc_litrow * (double)cena.cena;
                                    }
                                }
                            }




                        }
                    }
                }




                zestawienie.Add(new KeyValuePair<string, double>(kategoria.nazwa, buffor));
            }

            return zestawienie;
        }
        #endregion
        /// <summary>
        /// Wywoluje głownego switcha który na podstawie wybranego itemu w glownym comboboxie generuje odpowiedni wykres
        /// </summary>
        private void WywolanieSwitch()
        {
            switch (cbRodzajStatystyk.SelectedItem)
            {
                case "Liczba zwierzat":
                    /*
                    WykresKolumnowy.Visibility = Visibility.Visible;
                    WykresKolowy.Visibility = Visibility.Hidden;
                    WykresLiniowy.Visibility = Visibility.Hidden;
                    */

                    WyswietlLiczbeZwierzat();


                    break;
                case "Liczba laktacji":
                    /*
                    WykresKolowy.Visibility = Visibility.Hidden;
                    WykresKolumnowy.Visibility = Visibility.Visible;
                    WykresLiniowy.Visibility = Visibility.Hidden;
                    */
                    svColumn.Visibility = Visibility.Visible;
                    svLine.Visibility = Visibility.Hidden;
                    svPie.Visibility = Visibility.Hidden;

                    WykresKolowy.Series.Clear();

                    WykresKolumnowy.Series.Clear();
                    DodajSerie(typeof(ColumnSeries), "Liczba laktacji", WykresKolumnowy);
                    ((ColumnSeries)WykresKolumnowy.Series[0]).ItemsSource = LiczbaLaktacji();


                    RozszerzWykres(LiczbaLaktacji().Count, WykresKolumnowy, true);

                    SeriaPie.Title = "Liczba laktacji";
                    break;
                case "Przychód i wydatki":
                    /*
                    WykresKolumnowy.Visibility = Visibility.Visible;
                    WykresKolowy.Visibility = Visibility.Hidden;
                    WykresLiniowy.Visibility = Visibility.Hidden;
                    */
                    svColumn.Visibility = Visibility.Hidden;
                    svLine.Visibility = Visibility.Hidden;
                    svPie.Visibility = Visibility.Visible;

                    WykresKolowy.Series.Clear();


                    DodajSerie(typeof(PieSeries), "Bilans", WykresKolowy);


                    ((PieSeries)WykresKolowy.Series[0]).ItemsSource = new KeyValuePair<string, double>[] {
                        new KeyValuePair<string, double>("Przychod",PrzychodZwierzat(okresOd,okresDo)),
                        new KeyValuePair<string, double>("Wydatki", WydatkiZwierzat(okresOd,okresDo))

                    };
                    
                    //RozszerzWykres(PrzychodZwierzat().Count, WykresKolumnowy, false);
                    break;
                case "Cena mleka":
                    /*
                    WykresKolumnowy.Visibility = Visibility.Hidden;
                    WykresKolowy.Visibility = Visibility.Hidden;
                    WykresLiniowy.Visibility = Visibility.Visible;
                    */
                    svColumn.Visibility = Visibility.Hidden;
                    svLine.Visibility = Visibility.Visible;
                    svPie.Visibility = Visibility.Hidden;

                    WykresLiniowy.Series.Clear();

                    DodajSerie(typeof(LineSeries), "Cena mleka", WykresLiniowy);

                    ((LineSeries)WykresLiniowy.Series[0]).ItemsSource = CenyMleka(okresOd,okresDo);
                    RozszerzWykres(CenyMleka(okresOd,okresDo).Count, WykresLiniowy, true);

                    break;
                case "Wydajność mleczna":
                    /*
                    WykresKolumnowy.Visibility = Visibility.Visible;
                    WykresKolowy.Visibility = Visibility.Hidden;
                    WykresLiniowy.Visibility = Visibility.Hidden;
                    */
                    svColumn.Visibility = Visibility.Visible;
                    svLine.Visibility = Visibility.Hidden;
                    svPie.Visibility = Visibility.Hidden;

                    WykresKolumnowy.Series.Clear();
                    DodajSerie(typeof(ColumnSeries), "Liczba litrów", WykresKolumnowy);
                    ((ColumnSeries)WykresKolumnowy.Series[0]).ItemsSource = WydajnoscMleczna(okresOd,okresDo);
                    RozszerzWykres(WydajnoscMleczna(okresOd,okresDo).Count, WykresKolumnowy, true);
                    break;
                case "Zestawienie rozliczeń":
                    svColumn.Visibility = Visibility.Visible;
                    svLine.Visibility = Visibility.Hidden;
                    svPie.Visibility = Visibility.Hidden;

                    WykresKolumnowy.Series.Clear();

                    DodajSerie(typeof(ColumnSeries), "Ilość pieniedzy", WykresKolumnowy);


                    ((ColumnSeries)WykresKolumnowy.Series[0]).ItemsSource = ZestawienieRozliczen(okresOd,okresDo);


                    RozszerzWykres(ZestawienieRozliczen(okresOd,okresDo).Count, WykresKolumnowy, false);
                    break;

                default:
                    break;
            }
        }


        private void cbRodzajStatystyk_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {


            WywolanieSwitch();
            

            

        }

        private void WyswietlLiczbeZwierzat()
        {
            svColumn.Visibility = Visibility.Visible;
            svLine.Visibility = Visibility.Hidden;
            svPie.Visibility = Visibility.Hidden;

            WykresKolumnowy.Series.Clear();

            DodajSerie(typeof(ColumnSeries), "Liczba zwierząt", WykresKolumnowy);


            ((ColumnSeries)WykresKolumnowy.Series[0]).ItemsSource = LiczbaZwierzatDanejRasy(okresOd, okresDo);

            
            RozszerzWykres(LiczbaZwierzatDanejRasy(okresOd, okresDo).Count, WykresKolumnowy, false);

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
                         where H.id_kategoria_rozliczen == 1
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
        private List<Kategoria_rozliczen> QueryKategorieRozliczen()
        {
            var query = (from K in Polaczenie.Kategoria_rozliczen
                        select K).ToList();

            return query;
        }

        #endregion

        private void ResetujOkresOd()
        {
            okresOd = Convert.ToDateTime(DateTime.Now.Day.ToString() + "." + DateTime.Now.Month.ToString() + "." + (DateTime.Now.Year - 100).ToString());
        }
        private void ResetujOkresDo()
        {
            okresDo = Convert.ToDateTime(DateTime.Now.Day.ToString() + "." + DateTime.Now.Month.ToString() + "." + (DateTime.Now.Year + 100).ToString());
        }
        private void dpOd_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            if(dpOd.SelectedDate != null)
            {
                okresOd = (DateTime)dpOd.SelectedDate;
            }
            else
            {
                ResetujOkresOd();
            }

            WywolanieSwitch();
        }

        private void dpDo_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dpDo.SelectedDate != null)
            {
                okresDo = (DateTime)dpDo.SelectedDate;
            }
            else
            {
                ResetujOkresDo();
            }

            WywolanieSwitch();
        }
    }
    
}

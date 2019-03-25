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
        DateTime? okresOd;
        DateTime? okresDo;
        PieSeries bufforSeria;
        int? iloscStada;
        List<Brush> listaKolorow = new List<Brush>();
        int licznikKolorow = 0;
        public StatystykiZwierzat()
        {
            InitializeComponent();
            DataContext = this;
         
            WypelnienieCbStatystyk();
            WypelnienieCbZakres();
            ResetujOkresOd();
            ResetujOkresDo();

            cbRodzajStatystyk.SelectedIndex = 0;
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
                wykres.MinWidth = 130 * dlugoscListy;
            }
            else if (dlugoscListy >= 6)
            {
               // wykres.Width = 90 * dlugoscListy;
                wykres.MinWidth = 90 * dlugoscListy;
            }
            else
            {
                wykres.Width = this.Width - 80;
            }
        }
        private void WypelnienieCbStatystyk()
        {
            
            cbRodzajStatystyk.Items.Add("Liczba zwierząt");
            cbRodzajStatystyk.Items.Add("Liczba laktacji");
            cbRodzajStatystyk.Items.Add("Przychód i wydatki");
            cbRodzajStatystyk.Items.Add("Cena mleka");
            cbRodzajStatystyk.Items.Add("Wydajność mleczna");
            cbRodzajStatystyk.Items.Add("Zestawienie rozliczeń");
            
            

        }
        private void WypelnienieCbZakres()
        {
            cbZakres.Items.Add("-------");
            cbZakres.Items.Add("Gatunek");
            cbZakres.Items.Add("Rasa");
            //cbZakres.Items.Add("Płeć");
            cbZakres.SelectedIndex = 0;
        }
        private void WypelnienieCbStado()
        {
            var stada = QueryStado();
            foreach (var item in stada)
            {
                cbStado.Items.Add(item.nr_stada);
            }
        }

        #region Źródła danych do wykresów pojedynczych zwierzat
        /// <summary>
        /// 
        /// </summary>
        /// <param name="listaZwierze">Lista zwierzat</param>
        /// <param name="id">id zakresu tzn.id gautunku, rasy itp</param>
        /// <param name="okres_od"></param>
        /// <param name="okres_do"></param>
        /// <param name="licznikSztuk">licznik sztuk ktory bedzie modyfikowany wewnatrz</param>
        /// <returns></returns>
        private int FiltrDatyLiczbaZwierzatAktywnych(List<Zwierze> listaZwierze, int id, DateTime? okres_od, DateTime? okres_do, string typTabeli)
        {
            int licznikSztuk = 0;
            foreach (var zwierzak in listaZwierze)
            {
                if ((((okres_od <= zwierzak.okres_od && (okres_do >= zwierzak.okres_od || okresDo == null)) || (okres_od >= zwierzak.okres_od & okres_od <= zwierzak.okres_do)) || okres_od == null) && ((zwierzak.okres_do == null && okres_do == null) || (zwierzak.okres_do >= okres_do && okres_do >= zwierzak.okres_od) || (zwierzak.okres_do == null)))
                {
                    if(typTabeli == "rasa")
                    {
                        if(zwierzak.id_rasa == id)
                        {
                            licznikSztuk++;
                        }
                    }
                    else if(typTabeli == "gatunek")
                    {
                        if(zwierzak.id_gatunek == id)
                        {
                            licznikSztuk++;
                        }
                    }
                    else if(typTabeli == "plec")
                    {
                        if(zwierzak.plec == id)
                        {
                            licznikSztuk++;
                        }
                    }
                    else if(typTabeli == "domyslne")
                    {
                        if(id == 0)
                        {
                            licznikSztuk++;
                        }              
                    }
                }
            }
            return licznikSztuk;
        }
        

        private List<KeyValuePair<string, int>> LiczbaZwierzatAktywnych()
        {
            var listaRasa = QueryRasa();
            var listaZwierze = QueryZwierze();
            var listaGatunki = QueryGatunekPojedyncze();

            var zestawienie = new List<KeyValuePair<string, int>>();

            var licznikSztuk = 0;
            if (cbZakres.SelectedIndex > 0 && cbZakres.SelectedItem.ToString() == "Rasa")
            {
         
                foreach (var rasa in listaRasa)
                {
                    licznikSztuk = FiltrDatyLiczbaZwierzatAktywnych(listaZwierze, rasa.Id, okresOd, okresDo,"rasa");
                    if (licznikSztuk != 0)
                    {
                        zestawienie.Add(new KeyValuePair<string, int>(rasa.nazwa, licznikSztuk));
                    }
                }
            }
            else if (cbZakres.SelectedIndex > 0 && cbZakres.SelectedItem.ToString() == "Gatunek" )
            {
                foreach (var gatunek in listaGatunki)
                {
                    licznikSztuk = FiltrDatyLiczbaZwierzatAktywnych(listaZwierze, gatunek.Id, okresOd, okresDo, "gatunek");
                    if (licznikSztuk != 0)
                    {
                        zestawienie.Add(new KeyValuePair<string, int>(gatunek.nazwa, licznikSztuk));
                    }
                }
            }
            else if (cbZakres.SelectedIndex > 0 && cbZakres.SelectedItem.ToString() == "Płeć")
            {
                for(int i = 0; i < 2; i++)
                {
                    licznikSztuk = FiltrDatyLiczbaZwierzatAktywnych(listaZwierze, i, okresOd, okresDo, "plec");
                    if (licznikSztuk != 0)
                    {
                        if(i == 0)
                        {
                            zestawienie.Add(new KeyValuePair<string, int>("Samce", licznikSztuk));
                        }
                        else if(i == 1)
                        {
                            zestawienie.Add(new KeyValuePair<string, int>("Samice", licznikSztuk));
                        }
                    }
                }
            }
            else
            {
                licznikSztuk = FiltrDatyLiczbaZwierzatAktywnych(listaZwierze, 0, okresOd, okresDo, "domyslne");
                if (licznikSztuk != 0)
                {
                    zestawienie.Add(new KeyValuePair<string, int>("", licznikSztuk));
                }
            }       
            return zestawienie;
        }

        private int FiltrDatyLiczbaZwierzatNieaktywnych(List<Zwierze> listaZwierze, int id, DateTime? okres_od, DateTime? okres_do, string typTabeli)
        {
            int licznikSztuk = 0;

            foreach (var zwierzak in listaZwierze)
            {
                if (((okres_od <= zwierzak.okres_do || (okres_od == null && zwierzak.okres_do != null)) && (((okres_do > zwierzak.okres_do) || okres_do == null)) || (zwierzak.okres_do != null && okres_od == null && okres_do == null)))
                {
                    if (typTabeli == "rasa")
                    {
                        if (zwierzak.id_rasa == id)
                        {
                            licznikSztuk++;
                        }
                    }
                    else if (typTabeli == "gatunek")
                    {
                        if (zwierzak.id_gatunek == id)
                        {
                            licznikSztuk++;
                        }
                    }
                    else if (typTabeli == "plec")
                    {
                        if (zwierzak.plec == id)
                        {
                            licznikSztuk++;
                        }
                    }
                    else if (typTabeli == "domyslne")
                    {
                        if (id == 0)
                        {
                            licznikSztuk++;
                        }

                    }

                }
            }


            return licznikSztuk;
        }

        private List<KeyValuePair<string, int>> LiczbaZwierzatNieaktywnych()
        {
            var listaRasa = QueryRasa();
            var listaZwierze = QueryZwierze();
            var listaGatunki = QueryGatunekPojedyncze();
            var zestawienie = new List<KeyValuePair<string, int>>();

            var licznikSztuk = 0;
            if (cbZakres.SelectedIndex > 0 && cbZakres.SelectedItem.ToString() == "Rasa")
            {

                foreach (var rasa in listaRasa)
                {
                    licznikSztuk = FiltrDatyLiczbaZwierzatNieaktywnych(listaZwierze, rasa.Id, okresOd, okresDo, "rasa");
                    if (licznikSztuk != 0)
                    {
                        zestawienie.Add(new KeyValuePair<string, int>(rasa.nazwa, licznikSztuk));
                    }
                }
            }
            else if (cbZakres.SelectedIndex > 0 && cbZakres.SelectedItem.ToString() == "Gatunek")
            {
                foreach (var gatunek in listaGatunki)
                {
                    licznikSztuk = FiltrDatyLiczbaZwierzatNieaktywnych(listaZwierze, gatunek.Id, okresOd, okresDo, "gatunek");
                    if (licznikSztuk != 0)
                    {
                        zestawienie.Add(new KeyValuePair<string, int>(gatunek.nazwa, licznikSztuk));
                    }
                }
            }
            else if (cbZakres.SelectedIndex > 0 && cbZakres.SelectedItem.ToString() == "Płeć")
            {
                for(int i = 0; i < 2; i++)
                {
                    licznikSztuk = FiltrDatyLiczbaZwierzatNieaktywnych(listaZwierze, i, okresOd, okresDo, "plec");
                    if (licznikSztuk != 0)
                    {
                        if (i == 0)
                        {
                            zestawienie.Add(new KeyValuePair<string, int>("Samce", licznikSztuk));
                        }
                        else if (i == 1)
                        {
                            zestawienie.Add(new KeyValuePair<string, int>("Samice", licznikSztuk));
                        }
                    }
                }
            }
            else
            {
                licznikSztuk = FiltrDatyLiczbaZwierzatNieaktywnych(listaZwierze, 0, okresOd, okresDo, "domyslne");
                if (licznikSztuk != 0)
                {
                    zestawienie.Add(new KeyValuePair<string, int>("", licznikSztuk));
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

        private double PrzychodZwierzat(DateTime? okres_od, DateTime? okres_do, List<Rozliczenia> listaPrzychod)
        {
            double zestawienie = 0;

           // var listaPrzychod = QueryPrzychod();
            var listaCenyMleka = QueryCenyMleka();
            var miesiace = DateTimeFormatInfo.CurrentInfo.MonthNames;

            double buffor;
            double sumaKwotaLitry;


            
            
            foreach (var item in listaPrzychod)
            {
                buffor = 0;
                sumaKwotaLitry = 0;
                if ((okres_od <= item.data || okres_od == null) && (item.data <= okres_do || okres_do == null))
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

        private double WydatkiZwierzat(DateTime? okres_od, DateTime? okres_do, List<Rozliczenia> listaWydatki)
        {


            double zestawienie = 0;
            //var listaWydatki = QueryWydatki();

            double buffor;
                   
            foreach (var item in listaWydatki)
            {
                buffor = 0;
                if ((okres_od <= item.data || okres_od == null) && (item.data <= okres_do || okres_do == null))
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
        private List<KeyValuePair<string, double>> CenyMleka(DateTime? okres_od, DateTime? okres_do)
        {
            var zestawienie = new List<KeyValuePair<string, double>>();
            var listaCenyMleka = QueryCenyMleka();
            for (int i = 0; i < listaCenyMleka.Count; i++)
            {
                if (((okres_od <= listaCenyMleka[i].okres_od || okres_od <= listaCenyMleka[i].okres_do) || okres_od == null) && (listaCenyMleka[i].okres_od <= okres_do || okres_do == null))
                {
                    zestawienie.Add(new KeyValuePair<string, double>(Convert.ToDateTime(listaCenyMleka[i].okres_od).ToShortDateString(), (double)listaCenyMleka[i].cena));
                }

            }


            return zestawienie;
        }

        private List<KeyValuePair<string, int>> WydajnoscMleczna(DateTime? okres_od, DateTime? okres_do)
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
                    if ((okres_od <= item.data || okres_od == null) && (item.data <= okres_do || okres_do == null))
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

        private List<KeyValuePair<string, double>> ZestawienieRozliczen(DateTime? okres_od, DateTime? okres_do)
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
                    if ((okres_od <= rozliczenie.data || okres_od == null) && (rozliczenie.data <= okres_do || okres_do == null))
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




                zestawienie.Add(new KeyValuePair<string, double>(kategoria.nazwa, Math.Round(buffor, 2)));
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
                case "Liczba zwierząt":
                    WykresKolumnowy.MinWidth = 0;
                    lbOd.Visibility = Visibility.Visible;
                    lbDo.Visibility = Visibility.Visible;
                    dpOd.Visibility = Visibility.Visible;
                    dpDo.Visibility = Visibility.Visible;
                    
                    checkbNieaktywne.Visibility = Visibility.Visible;

                    cbZakres.Visibility = Visibility.Visible;
                    lbZakres.Visibility = Visibility.Visible;
                    cbTyp.Visibility = Visibility.Visible;
                    lbTyp.Visibility = Visibility.Visible;

                   // cbStado.SelectedIndex = 0;
                    svColumn.Visibility = Visibility.Visible;

                    if (cbTyp.SelectedIndex == 0)
                    {
                        WykresKolumnowy.Width = double.NaN;
                        WykresKolumnowy.Axes.Clear();
                        WykresKolumnowy.Series.Clear();
                        var LiczbaAktywnych = LiczbaZwierzatAktywnych();
                        var LiczbaNieaktywnych = LiczbaZwierzatNieaktywnych();
                       

                        var maxAktywnych = LiczbaAktywnych.Aggregate((a, b) => a.Value < b.Value ? b : a);
                        var maxNieaktywnych = LiczbaNieaktywnych.Aggregate((a, b) => a.Value < b.Value ? b : a);
                        var minAktywnych = LiczbaAktywnych.Aggregate((a, b) => a.Value < b.Value ? a : b);
                        var minNieaktywnych = LiczbaNieaktywnych.Aggregate((a, b) => a.Value < b.Value ? b : a);
        

                        WykresKolumnowy.Axes.Add(DajOs((maxAktywnych.Value > maxNieaktywnych.Value ? maxAktywnych.Value : maxNieaktywnych.Value) + 2));
                        
                        if (LiczbaAktywnych.Count > 0)
                        {
                            DodajSerie(typeof(ColumnSeries), "Liczba aktywnych zwierzat", WykresKolumnowy);
                            ((ColumnSeries)WykresKolumnowy.Series[0]).ItemsSource = LiczbaAktywnych;

                        }
                        if (checkbNieaktywne.IsChecked == true)
                        {
                            if (LiczbaNieaktywnych.Count > 0 && LiczbaAktywnych.Count <= 0)
                            {
                                DodajSerie(typeof(ColumnSeries), "Liczba nieaktywnych zwierzat", WykresKolumnowy);
                                ((ColumnSeries)WykresKolumnowy.Series[0]).ItemsSource = LiczbaNieaktywnych;
                            }
                            else if (LiczbaNieaktywnych.Count > 0)
                            {
                                DodajSerie(typeof(ColumnSeries), "Liczba nieaktywnych zwierzat", WykresKolumnowy, true);
                                ((ColumnSeries)WykresKolumnowy.Series[1]).ItemsSource = LiczbaNieaktywnych;
                            }

                        }

                        RozszerzWykres(LiczbaAktywnych.Count, WykresKolumnowy, false);
                    }
                    else
                    {
                        WykresLiniowy.MinWidth = 0;
                        svColumn.Visibility = Visibility.Hidden;
                        svLine.Visibility = Visibility.Visible;
                        checkbNieaktywne.Visibility = Visibility.Hidden;

                        cbZakres.Visibility = Visibility.Hidden;
                        lbZakres.Visibility = Visibility.Hidden;

                        cbStado.Visibility = Visibility.Visible;
                        lbStado.Visibility = Visibility.Visible;

                        var listaStad = QueryStado();
                        WykresLiniowy.Width = double.NaN;
                        WykresLiniowy.Axes.Clear();
                        WykresLiniowy.Series.Clear();

                        var liczbaStada = LiczbaZwierzatStada(cbStado.SelectedItem.ToString(), okresOd, okresDo);
                        var maxAktywnych = liczbaStada.Aggregate((a, b) => a.Value < b.Value ? b : a);


                        DodajSerie(typeof(LineSeries), cbStado.SelectedItem.ToString(), WykresLiniowy);
                        
                        ((LineSeries)WykresLiniowy.Series[0]).ItemsSource = liczbaStada;
                        WykresLiniowy.Axes.Add(DajOs(((int)maxAktywnych.Value * 105) / 100));



                    }
                   
                    
                    break;
                case "Liczba laktacji":
                    WykresKolumnowy.MinWidth = 0;

                    svColumn.Visibility = Visibility.Visible;


                    WykresKolowy.Series.Clear();
                    WykresKolumnowy.Axes.Clear();
                    WykresKolumnowy.Series.Clear();
                    WykresKolumnowy.Width = double.NaN;
                    var laktacje = LiczbaLaktacji();
                    var maxLaktacje = laktacje.Aggregate((a, b) => a.Value < b.Value ? b : a);


                    WykresKolumnowy.Axes.Add(DajOs(maxLaktacje.Value));

                    DodajSerie(typeof(ColumnSeries), "Liczba laktacji", WykresKolumnowy);
                    ((ColumnSeries)WykresKolumnowy.Series[0]).ItemsSource = laktacje;


                    RozszerzWykres(LiczbaLaktacji().Count, WykresKolumnowy, true);

                    SeriaPie.Title = "Liczba laktacji";
                    break;
                case "Przychód i wydatki":
                    lbOd.Visibility = Visibility.Visible;
                    lbDo.Visibility = Visibility.Visible;
                    dpOd.Visibility = Visibility.Visible;
                    dpDo.Visibility = Visibility.Visible;
                    cbTyp.Visibility = Visibility.Visible;
                    lbTyp.Visibility = Visibility.Visible;
                    

                    svPie.Visibility = Visibility.Visible;
                    
                    WykresKolowy.Series.Clear();

                    if(cbTyp.SelectedIndex == 0)
                    {
                        DodajSerie(typeof(PieSeries), "Bilans", WykresKolowy);
                        ((PieSeries)WykresKolowy.Series[0]).ItemsSource = new KeyValuePair<string, double>[] {
                        new KeyValuePair<string, double>("Przychod",PrzychodZwierzat(okresOd,okresDo,QueryPrzychod())),
                        new KeyValuePair<string, double>("Wydatki", WydatkiZwierzat(okresOd,okresDo,QueryWydatki()))

                        };
                    }
                    else if(cbTyp.SelectedIndex == 1)
                    {
                        DodajSerie(typeof(PieSeries), "Bilans", WykresKolowy);
                        ((PieSeries)WykresKolowy.Series[0]).ItemsSource = new KeyValuePair<string, double>[] {
                        new KeyValuePair<string, double>("Przychod",PrzychodZwierzat(okresOd,okresDo,QueryPrzychodPojedyncze())),
                        new KeyValuePair<string, double>("Wydatki", WydatkiZwierzat(okresOd,okresDo,QueryWydatkiPojedyncze()))

                        };
                    }
                    else if(cbTyp.SelectedIndex == 2)
                    {
                        DodajSerie(typeof(PieSeries), "Bilans", WykresKolowy);
                        ((PieSeries)WykresKolowy.Series[0]).ItemsSource = new KeyValuePair<string, double>[] {
                        new KeyValuePair<string, double>("Przychod",PrzychodZwierzat(okresOd,okresDo,QueryPrzychodStado())),
                        new KeyValuePair<string, double>("Wydatki", WydatkiZwierzat(okresOd,okresDo,QueryWydatkiStado()))
                        };
                    } 
                    
                    
                    break;
                case "Cena mleka":
                    WykresLiniowy.MinWidth = 0;
                    lbOd.Visibility = Visibility.Visible;
                    lbDo.Visibility = Visibility.Visible;
                    dpOd.Visibility = Visibility.Visible;
                    dpDo.Visibility = Visibility.Visible;

                    svLine.Visibility = Visibility.Visible;
                    WykresLiniowy.Width = double.NaN;
                    WykresLiniowy.Axes.Clear();
                    WykresLiniowy.Series.Clear();

                    DodajSerie(typeof(LineSeries), "Cena mleka", WykresLiniowy);

                    ((LineSeries)WykresLiniowy.Series[0]).ItemsSource = CenyMleka(okresOd,okresDo);
                    RozszerzWykres(CenyMleka(okresOd,okresDo).Count, WykresLiniowy, true);
                    
                    break;
                case "Wydajność mleczna":
                    WykresKolumnowy.MinWidth = 0;
                    lbOd.Visibility = Visibility.Visible;
                    lbDo.Visibility = Visibility.Visible;
                    dpOd.Visibility = Visibility.Visible;
                    dpDo.Visibility = Visibility.Visible;

                    svColumn.Visibility = Visibility.Visible;
                    WykresKolumnowy.Width = double.NaN;
                    WykresKolumnowy.Axes.Clear();
                    WykresKolumnowy.Series.Clear();
                    
                    var wydajnosc = WydajnoscMleczna(okresOd, okresDo);
                    var maxWydajnosc = wydajnosc.Aggregate((a, b) => a.Value < b.Value ? b : a);


                    WykresKolumnowy.Axes.Add(DajOs(maxWydajnosc.Value));

                    DodajSerie(typeof(ColumnSeries), "Liczba litrów", WykresKolumnowy);
                    ((ColumnSeries)WykresKolumnowy.Series[0]).ItemsSource = WydajnoscMleczna(okresOd, okresDo);
                    RozszerzWykres(WydajnoscMleczna(okresOd,okresDo).Count, WykresKolumnowy, true);
                    break;
                case "Zestawienie rozliczeń":
                    WykresKolumnowy.MinWidth = 0;
                    lbOd.Visibility = Visibility.Visible;
                    lbDo.Visibility = Visibility.Visible;
                    dpOd.Visibility = Visibility.Visible;
                    dpDo.Visibility = Visibility.Visible;

                    svColumn.Visibility = Visibility.Visible;

                    var rozliczenia = ZestawienieRozliczen(okresOd, okresDo);
                    WykresKolumnowy.Width = double.NaN;
                    WykresKolumnowy.Series.Clear();
                    WykresKolumnowy.Axes.Clear();
                    var maxRozliczenie = rozliczenia.Aggregate((a, b) => a.Value < b.Value ? b : a);

                    WykresKolumnowy.Axes.Add(DajOs(maxRozliczenie.Value));
                    DodajSerie(typeof(ColumnSeries), "Ilość pieniedzy", WykresKolumnowy);


                    ((ColumnSeries)WykresKolumnowy.Series[0]).ItemsSource = ZestawienieRozliczen(okresOd,okresDo);

                    
                    RozszerzWykres(ZestawienieRozliczen(okresOd,okresDo).Count, WykresKolumnowy, false);
                    break;

                default:
                    break;
            }
        }
        

        #region Źródła danych do wykresow stad
        private List<KeyValuePair<string, int>> LiczbaZwierzatStada(string nr_kolczyka, DateTime? okres_od, DateTime? okres_do)
        {
            var zestawienie = new List<KeyValuePair<string, int>>();
            var listaStad = QueryStado();
            var listaHistoriaStad = QueryHistoriaStada();


            foreach (var historia in listaHistoriaStad)
            {
                if ((okres_od <= historia.okres_od  || okres_od == null) && (historia.okres_od <= okres_do || okres_do == null))
                {
                    if (nr_kolczyka == historia.Stado.nr_stada)
                    {
                        iloscStada = historia.ilosc;
                        zestawienie.Add(new KeyValuePair<string, int>(Convert.ToDateTime(historia.okres_od).ToShortDateString(), (int)historia.ilosc));
                    }
                }
                

            }

            



            return zestawienie;

        }


        #endregion

        private void WypelnienieCbTyp()
        {
            if(cbRodzajStatystyk.SelectedItem.ToString() == "Liczba zwierząt")
            {
                cbTyp.Items.Add("Pojedyńcze zwierzęta");
                cbTyp.Items.Add("Stado");
            }
            else if(cbRodzajStatystyk.SelectedItem.ToString() == "Przychód i wydatki")
            {

                cbTyp.Items.Add("Ogólne");
                cbTyp.Items.Add("Pojedyńcze zwierzęta");
                cbTyp.Items.Add("Stado");
            }
        }

        private void cbRodzajStatystyk_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            cbTyp.Items.Clear();
            WypelnienieCbTyp();
            cbTyp.SelectedIndex = 0;

            checkbNieaktywne.Visibility = Visibility.Hidden;
            cbZakres.Visibility = Visibility.Hidden;
            lbZakres.Visibility = Visibility.Hidden;

            lbOd.Visibility = Visibility.Hidden;
            lbDo.Visibility = Visibility.Hidden;
            dpOd.Visibility = Visibility.Hidden;
            dpDo.Visibility = Visibility.Hidden;

            svColumn.Visibility = Visibility.Hidden;
            svLine.Visibility = Visibility.Hidden;
            svPie.Visibility = Visibility.Hidden;

            lbTyp.Visibility = Visibility.Hidden;
            cbTyp.Visibility = Visibility.Hidden;

           

            cbZakres.SelectedIndex = 0;
            checkbNieaktywne.IsChecked = false;

            

            WywolanieSwitch();
            dpOd.SelectedDate = null;
            dpDo.SelectedDate = null;
            
            
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="typ"> Typ serii tzn. LineSeries itp</param>
        /// <param name="tytulSerii">Nazwa serii wyswietlana w legendzie</param>
        /// <param name="wykres">Nazwa wykresu na który nanoszona jest seria</param>
        private void DodajSerie(Type typ, string tytulSerii, Chart wykres, bool wiecejSerii = false)
        {                 
            if (typ == typeof(ColumnSeries))
            {
                ColumnSeries nowaSeria = new ColumnSeries();
                nowaSeria.DependentValuePath = "Value";
                nowaSeria.IndependentValuePath = "Key";
                nowaSeria.Title = tytulSerii;
                if (wiecejSerii == false)
                {                                 
                    Style styl = new Style();
                    styl.BasedOn = this.FindResource("StylColumnDataPoint") as Style;
                    styl.TargetType = typeof(ColumnDataPoint);
                    Style stylLegend = FindResource("LegendStyle") as Style;
                    nowaSeria.DataPointStyle = styl;
                    nowaSeria.LegendItemStyle = stylLegend;
                    wykres.Series.Add(nowaSeria);
                }
                else
                {                  
                    Style styl = new Style();
                    styl.BasedOn = this.FindResource("StylColumnDataPoint2") as Style;
                    styl.TargetType = typeof(ColumnDataPoint);
                    Style stylLegend = FindResource("LegendStyle2") as Style;
                    nowaSeria.DataPointStyle = styl;
                    nowaSeria.LegendItemStyle = stylLegend;
                    wykres.Series.Add(nowaSeria);
                }           
            }
            else if(typ == typeof(PieSeries))
            {
                PieSeries nowaSeria = new PieSeries();
                nowaSeria.DependentValuePath = "Value";
                nowaSeria.IndependentValuePath = "Key";
                nowaSeria.Title = tytulSerii;      

                bufforSeria = nowaSeria;
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

        private int RoundResult(double number)
        {

            
            if (number >= 0 && number <= 100)
            {
                number = Math.Round(number);
            }
            else if (number > 100 && number <= 1000)
            {
                number = Math.Round(number / 100) * 100;
            }
            else if (number > 1000 && number <= 10000)
            {
                number = Math.Round(number / 1000) * 1000;
            }
            else if (number > 10000 && number <= 100000)
            {
                number = Math.Round(number / 10000) * 10000;
            }

            return (int)number;
        }
        
        private LinearAxis DajOs(double MaxWartosc)
        {
            LinearAxis os = new LinearAxis();
            os.Orientation = AxisOrientation.Y;
            os.Minimum = 0;
            os.Maximum = (MaxWartosc * 105) / 100;
            if(os.Maximum <= 10)
            {
                os.Interval = (int)(RoundResult((double)os.Maximum)/ (RoundResult((double)os.Maximum)));
                
            }
            else
            {
                os.Interval = (int)(RoundResult((double)os.Maximum) / 10);
            }
            os.ShowGridLines = true;

            return os;
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

        private List<Rozliczenia> QueryPrzychodPojedyncze()
        {
            var query = (from R in Polaczenie.Rozliczenia
                         where R.Kategoria_rozliczen.czyPrzychod == 1 && R.id_zwierze != null
                         select R).ToList();

            return query;
        }
        private List<Rozliczenia> QueryPrzychodStado()
        {
            var query = (from R in Polaczenie.Rozliczenia
                         where R.Kategoria_rozliczen.czyPrzychod == 1 && R.id_stado != null
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

        private List<Rozliczenia> QueryWydatkiPojedyncze()
        {
            var query = (from R in Polaczenie.Rozliczenia
                         where R.Kategoria_rozliczen.czyPrzychod == 0 && R.id_zwierze != null
                         select R).ToList();
            return query;
        }
        private List<Rozliczenia> QueryWydatkiStado()
        {
            var query = (from R in Polaczenie.Rozliczenia
                         where R.Kategoria_rozliczen.czyPrzychod == 0 && R.id_stado != null
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
        private List<Gatunek> QueryGatunekPojedyncze()
        {
            var query = (from Gatunek in Polaczenie.Gatunek
                        where Gatunek.czyStado == 0
                        select Gatunek).ToList() ;

            return query;
        }
        /// <summary>
        /// Zwraca historie stada na podstawie podane kolczyka
        /// </summary>
        /// <param name="kolczyk">kolczyk stada</param>
        /// <returns></returns>
        private List<Historia_Stada> QueryHistoriaStada()
        {
            var query = (from H in Polaczenie.Historia_Stada 
                         orderby H.okres_od ascending
                         select H).ToList();
            return query;
        }

        #endregion
        #region Akcje kontrolek filtrów
        private void ResetujOkresOd()
        {
            //okresOd = Convert.ToDateTime(DateTime.Now.Day.ToString() + "." + DateTime.Now.Month.ToString() + "." + (DateTime.Now.Year - 100).ToString());
            okresOd = null;
        }
        private void ResetujOkresDo()
        {
            //okresDo = Convert.ToDateTime(DateTime.Now.Day.ToString() + "." + DateTime.Now.Month.ToString() + "." + (DateTime.Now.Year + 100).ToString());
            okresDo = null;
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

        private void checkbNieaktywne_Checked(object sender, RoutedEventArgs e)
        {
            WywolanieSwitch();
        }

        private void checkbNieaktywne_Unchecked(object sender, RoutedEventArgs e)
        {
            WywolanieSwitch();
        }

        private void cbZakres_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            WywolanieSwitch();
        }

        private void cbTyp_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            lbStado.Visibility = Visibility.Hidden;
            cbStado.Visibility = Visibility.Hidden;
            svLine.Visibility = Visibility.Hidden;
            if (cbStado.Items.Count == 0)
            {
                WypelnienieCbStado();
                cbStado.SelectedIndex = 0;
            }
            
            WywolanieSwitch();
        }

        private void cbStado_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            WywolanieSwitch();
        }

        #endregion

        private void Path_Loaded(object sender, RoutedEventArgs e)
        {
            if(listaKolorow.Count == 2)
            {
                listaKolorow.Clear();
                licznikKolorow = 0;
            }
            Random r = new Random();
            var kolor = new SolidColorBrush(Color.FromArgb(
                  0xFF,
                  (byte)r.Next(255),
                  (byte)r.Next(255),
                  (byte)r.Next(255)));

            (sender as Path).Fill = kolor;

            Style styl = FindResource("PieLegendStyle") as Style;

            listaKolorow.Add(kolor);
            System.Threading.Thread.Sleep(50);
        }

        private void Rectangle_Loaded(object sender, RoutedEventArgs e)
        {   
            (sender as Rectangle).Fill = listaKolorow[licznikKolorow];
            licznikKolorow++;
        }
    }
    
}

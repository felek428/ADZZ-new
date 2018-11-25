using System;
using System.Collections.Generic;
using System.ComponentModel;
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
            cbRodzajStatystyk.Items.Add("Liczba zwierzat");
            cbRodzajStatystyk.Items.Add("Liczba laktacji");
            //LoadColumnChartData();
            
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
        private KeyValuePair<string,int>[] LiczbaZwierzatDanejRasy()
        {
            var queryRasa = (from Rasa in Polaczenie.Rasa
                             select Rasa).ToList();
            var queryZwierze = (from Zwierze in Polaczenie.Zwierze
                                select Zwierze).ToList();


            
            var para = new KeyValuePair<string, int>[queryRasa.Count];
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
                para[i] = new KeyValuePair<string, int>(queryRasa[i].nazwa,licznikSztuk);
            }          
            return para;
        }

        private KeyValuePair<string, int>[] LiczbaLaktacji()
        {
            var queryRozrod = (from Rozrod in Polaczenie.Rozrod
                             select Rozrod).ToList();
            var queryZwierze = (from Zwierze in Polaczenie.Zwierze
                                select Zwierze).ToList();



            var para = new KeyValuePair<string, int>[queryZwierze.Count];
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
                para[i] = new KeyValuePair<string, int>(queryZwierze[i].nr_kolczyka, licznikSztuk);
            }
            return para;
        }

        private void WypelnienieCB()
        {

        }

        private void cbRodzajStatystyk_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            WykresKolumnowy.Visibility = Visibility.Visible;
            switch(cbRodzajStatystyk.SelectedItem)
            {
                case "Liczba zwierzat":
                    WykresKolumnowy.Visibility = Visibility.Visible;
                    WykresKolowy.Visibility = Visibility.Hidden;
                    ((ColumnSeries)WykresKolumnowy.Series[0]).ItemsSource = LiczbaZwierzatDanejRasy();

                    SeriaCol.Title = "Liczba zwierzat";

                    break;
                case "Liczba laktacji":
                    WykresKolowy.Visibility = Visibility.Visible;
                    WykresKolumnowy.Visibility = Visibility.Hidden;
                    ((PieSeries)WykresKolowy.Series[0]).ItemsSource = LiczbaLaktacji();

                    SeriaPie.Title = "Liczba laktacji";
                    break;
                
                default:
                    break;
            }

        }
    }
}

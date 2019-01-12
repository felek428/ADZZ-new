using ADZZ.Statystyki___okno_i_strony;
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
    /// Logika interakcji dla klasy ZwierzeInformacje.xaml
    /// </summary>
    public partial class ZwierzeInformacje : Page
    {
        PolaczenieBazaDataContext Polaczenie = new PolaczenieBazaDataContext();

        string wybranyKolczyk;
        private Frame ramkaAkcji;
        public ZwierzeInformacje()
        {
            InitializeComponent();
        }
        public ZwierzeInformacje(Frame ramka, string Kolczyk)
        {
            InitializeComponent();
            ramkaAkcji = ramka;
            wybranyKolczyk = Kolczyk;
            WypelnienieWykres();
            
            

            FormularzDodaniaZwierzecia nowy = new FormularzDodaniaZwierzecia(Kolczyk);

           
            ramkaInformacje.Content = nowy;
            WypelnienieListViewRuja(Kolczyk);
            WypelnienieLVListaWycielen(Kolczyk);
            
        }

        private void Przycisk()
        {
            MessageBox.Show("Ala");
        }

        private void btnPowrot_Click(object sender, RoutedEventArgs e)
        {
            //ramkaAkcji.Content = new ListaZwierzat(ramkaAkcji);
        }

        private void WypelnienieWykres()
        {
            DaneWykresow noweDane = new DaneWykresow();

            ((System.Windows.Controls.DataVisualization.Charting.PieSeries)Wykres.Series[0]).ItemsSource =
                new KeyValuePair<string, double>[]{
                new KeyValuePair<string,double>("Wydatki", noweDane.WydatkiZwierze(wybranyKolczyk)),
                new KeyValuePair<string,double>("Przychód", noweDane.PrzychodZwierze(wybranyKolczyk))
                };
        }
        
        private void WypelnienieListViewRuja(string kolczyk)
        {
            var query = from Rozrod in Polaczenie.Rozrod
                         where Rozrod.Zwierze.nr_kolczyka == kolczyk && Rozrod.czyRuja == 1
                         orderby Rozrod.Data descending
                         select new { NowaData=Rozrod.Data.ToShortDateString()};


            LVListaRuji.ItemsSource = query;
        }
        private void WypelnienieLVListaWycielen(string kolczyk)
        {
            var query = from Rozrod in Polaczenie.Rozrod
                         where Rozrod.Zwierze.nr_kolczyka == kolczyk && Rozrod.czyRuja == 0
                         orderby Rozrod.Data descending
                         select new { NowaData = Rozrod.Data.ToShortDateString() };
            LVListaWycielen.ItemsSource = query;
        }

        
    }
}

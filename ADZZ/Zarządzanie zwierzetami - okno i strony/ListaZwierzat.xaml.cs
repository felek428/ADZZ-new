using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
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
    /// Logika interakcji dla klasy ListaZwierzat.xaml
    /// </summary>
    public partial class ListaZwierzat : Page
    {
        private Frame ramkaAkcji;
        PolaczenieBazaDataContext Polaczenie = new PolaczenieBazaDataContext();
        public ListaZwierzat(Frame ramka, int wybranyIndex)
        {
            InitializeComponent();

            if(wybranyIndex == 0)
            {
                var query = (from Zwierze in Polaczenie.Zwierze
                             select new ZwierzeNiepelnyOpis { NrKolczyka = Zwierze.nr_kolczyka, NazwaRasa = Zwierze.Rasa.nazwa, NazwaGatunek = Zwierze.Gatunek.nazwa }).ToList();

                LVListaZwierzat.ItemsSource = query;
            }
            else if(wybranyIndex == 1)
            {
                var query = (from Stado in Polaczenie.Stado
                             select new ZwierzeNiepelnyOpis { NrKolczyka = Stado.nr_stada, NazwaGatunek = Stado.Gatunek.nazwa }).ToList();
                LVListaZwierzat.ItemsSource = query;
                
            }


            

            CollectionView filtrowanieKolekcji = (CollectionView)CollectionViewSource.GetDefaultView(LVListaZwierzat.ItemsSource);
            filtrowanieKolekcji.Filter = ListaZwierzatFiltr;
            ramkaAkcji = ramka;
        }

        public ListaZwierzat()
        {

        }


        private void ListViewItem_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            
            var item = sender as ListViewItem;
            
            if (item != null && item.IsSelected)
            {
                var SelectedKolczyk = (ZwierzeNiepelnyOpis)LVListaZwierzat.SelectedItem;
                
                
                ramkaAkcji.Content = new ZwierzeInformacje(ramkaAkcji,SelectedKolczyk.NrKolczyka);
            }
        }
        public class ZwierzeNiepelnyOpis
        {
            public string NrKolczyka { get; set; }
            public string NazwaRasa { get; set; }
            public string NazwaGatunek { get; set; }
            public string NrSiedzibyStada { get; set; }

           
        }

        private void TBFiltr_TextChanged(object sender, TextChangedEventArgs e)
        {
            CollectionViewSource.GetDefaultView(LVListaZwierzat.ItemsSource).Refresh();
        }

        private bool ListaZwierzatFiltr(object lista)
        {
            if (String.IsNullOrEmpty(tbFiltr.Text))
            {
                return true;
            }
            else
            {
                return ((lista as ZwierzeNiepelnyOpis).NrKolczyka.IndexOf(tbFiltr.Text,StringComparison.OrdinalIgnoreCase) >=0);
            }
        }
    }
}

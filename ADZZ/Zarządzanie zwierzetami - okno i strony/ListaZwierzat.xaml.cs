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
        private int wybranyIndex;
        private int WybranyId;
        public ListaZwierzat(Frame ramka, int wybranyIndex)
        {
            InitializeComponent();
            this.wybranyIndex = wybranyIndex;

            WypelnijListe();
            
            

            CollectionView filtrowanieKolekcji = (CollectionView)CollectionViewSource.GetDefaultView(LVListaZwierzat.ItemsSource);
            filtrowanieKolekcji.Filter = ListaZwierzatFiltr;
            ramkaAkcji = ramka;
        }

        public ListaZwierzat()
        {

        }

        private void WypelnijListe()
        {
            if (wybranyIndex == 0)
            {
                var query = (from Zwierze in Polaczenie.Zwierze
                             select new ZwierzeNiepelnyOpis { NrKolczyka = Zwierze.nr_kolczyka, NazwaRasa = Zwierze.Rasa.nazwa, NazwaGatunek = Zwierze.Gatunek.nazwa, Id = Zwierze.Id }).ToList();

                LVListaZwierzat.ItemsSource = query;
            }
            else if (wybranyIndex == 1)
            {
                var query = (from Stado in Polaczenie.Stado
                             select new ZwierzeNiepelnyOpis { NrKolczyka = Stado.nr_stada, NazwaGatunek = Stado.Gatunek.nazwa, Id = Stado.Id }).ToList();
                LVListaZwierzat.ItemsSource = query;

            }
        }

        private void ListViewItem_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            
            var item = sender as ListViewItem;

            

            if (item != null && item.IsSelected)
            {
                var SelectedKolczyk = (ZwierzeNiepelnyOpis)LVListaZwierzat.SelectedItem;

                if (wybranyIndex == 0)
                {
                    ramkaAkcji.Content = new ZwierzeInformacje(ramkaAkcji, SelectedKolczyk.NrKolczyka);
                }
                else if (wybranyIndex == 1)
                {
                    ramkaAkcji.Content = new StadoInformacje(SelectedKolczyk.NrKolczyka);

                }
                
            }
        }
        public class ZwierzeNiepelnyOpis
        {
            public string NrKolczyka { get; set; }
            public string NazwaRasa { get; set; }
            public string NazwaGatunek { get; set; }
            public string NrSiedzibyStada { get; set; }
            public int Id { get; set; }

           
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

        private void LVListaZwierzat_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(LVListaZwierzat.SelectedItem != null)
            {
                btUsun.IsEnabled = true;
                
            }
            else
            {
                btUsun.IsEnabled = false;
            }
            
           
        }
        private void ListViewItem_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            
            var item = sender as ListViewItem;
            if (item != null && item.IsSelected)
            {
                var ZaznaczonaPozycja = (ZwierzeNiepelnyOpis)LVListaZwierzat.SelectedItem;
                WybranyId = ZaznaczonaPozycja.Id;
                
            }
        }

        

        private void btUsun_Click(object sender, RoutedEventArgs e)
        {
            if(wybranyIndex == 0)
            {
                if (LVListaZwierzat.SelectedItem != null)
                {
                    Zwierze usunZwierze = Polaczenie.Zwierze.Single(x => x.Id == WybranyId);

                    Polaczenie.Zwierze.DeleteOnSubmit(usunZwierze);
                    Polaczenie.SubmitChanges();
                }
            }
            else
            {
                if (LVListaZwierzat.SelectedItem != null)
                {
                    Stado usunStado = Polaczenie.Stado.Single(x => x.Id == WybranyId);

                    Polaczenie.Stado.DeleteOnSubmit(usunStado);
                    Polaczenie.SubmitChanges();
                }
            }
            
            WypelnijListe();
        }
    }
}

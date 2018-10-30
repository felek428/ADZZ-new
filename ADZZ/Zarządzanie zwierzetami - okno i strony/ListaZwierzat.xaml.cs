﻿using System;
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
        public ListaZwierzat(Frame ramka)
        {
            InitializeComponent();
            var query = (from Zwierze in Polaczenie.Zwierze           
                         select new ZwierzeNiepelne{ NrKolczyka = Zwierze.nr_kolczyka, NazwaRasa = Zwierze.Rasa.nazwa, NazwaGatunek = Zwierze.Gatunek.nazwa }).ToList();
            /*
            var query2 = from Rozrod in Polaczenie.Rozrod
                         where Rozrod.Zwierze.nr_kolczyka == "PL111111111111" && Rozrod.czyRuja == 1
                         select new { Nowa=Rozrod.Data };*/
            LVListaZwierzat.ItemsSource = query;

            CollectionView filtrowanieKolekcji = (CollectionView)CollectionViewSource.GetDefaultView(LVListaZwierzat.ItemsSource);
            filtrowanieKolekcji.Filter = ListaZwierzatFiltr;
            Datagrid.ItemsSource = query;
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
                var SelectedKolczyk = (ZwierzeNiepelne)LVListaZwierzat.SelectedItem;
                
                
                ramkaAkcji.Content = new ZwierzeInformacje(ramkaAkcji,SelectedKolczyk.NrKolczyka);
            }
        }
        public class ZwierzeNiepelne
        {
            public string NrKolczyka { get; set; }
            public string NazwaRasa { get; set; }
            public string NazwaGatunek { get; set; }

           
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
                return ((lista as ZwierzeNiepelne).NrKolczyka.IndexOf(tbFiltr.Text,StringComparison.OrdinalIgnoreCase) >=0);
            }
        }
    }
}

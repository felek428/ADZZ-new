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
    /// Logika interakcji dla klasy FormularzDodaniaZwierzecia.xaml
    /// </summary>
    public partial class FormularzDodaniaZwierzecia : Page
    {
        PolaczenieBazaDataContext Polaczenie = new PolaczenieBazaDataContext();
        private int wybraneZwierzeId;
        
    public FormularzDodaniaZwierzecia()
        {
            InitializeComponent();
            WypelnienieCBGatunek();
            
        }
        public FormularzDodaniaZwierzecia(string nr_kolczyka, int plec, DateTime dataur, DateTime okresod, DateTime okresdo, string rasa, string gatunek)
        {
            InitializeComponent();
            //WypelnienieCBGatunek();
            btDodaj.Content = "Zmień";
            btDodaj.Click -= new RoutedEventHandler(BtDodaj_Click);
            btDodaj.Click += new RoutedEventHandler(BtDodaj_Zmien_Click);
            tbKolczyk.IsEnabled = false;
            

            var query = (from Z in Polaczenie.Zwierze
                         where Z.nr_kolczyka == nr_kolczyka.ToString()
                         select Z).ToList();
            wybraneZwierzeId = query[0].Id;
            WypelnienieCBGatunek();
            foreach (var item in query)
            { 
                tbKolczyk.Text = item.nr_kolczyka;

                if( item.plec == 1)
                {
                    femaleCheckB.IsChecked = true;
                }
                else
                {
                    maleCheckB.IsChecked = true;
                }
                DataUrDP.SelectedDate = item.data_urodzenia;
                okresOdDP.SelectedDate = item.okres_od;
                if(item.okres_do != null)
                {
                    okresDoDP.SelectedDate = item.okres_do;
                }
                GatunekCB.SelectedItem = item.Gatunek.nazwa;
                if(RasaCB.Items.Count != 0 && item.id_rasa != null)
                {
                    RasaCB.SelectedItem = item.Rasa.nazwa;
                    Console.WriteLine("Tutaj sa rasy");
                }
                else
                {
                    Console.WriteLine("Ale tu nie ma ras");
                }
            }



        }
        private void BtDodaj_Zmien_Click(object sender, RoutedEventArgs e)
        {
            

            MessageBoxResult potwierdzenie = MessageBox.Show("Czy na pewno chcesz wprowadzic podane zmiany?", 
                                                             "Potwierdzenie", 
                                                             MessageBoxButton.YesNo);
            if(potwierdzenie == MessageBoxResult.Yes)
            {
                Zwierze queryZmiana = (from Zwierze in Polaczenie.Zwierze
                                       where Zwierze.Id == wybraneZwierzeId
                                       select Zwierze).SingleOrDefault();

                WpisDoBazyZwierze(queryZmiana);

                Polaczenie.SubmitChanges();

                MessageBox.Show("Powiodło się!");
            }
            
        }
        /// <summary>
        /// Dodaje wpis do bazy
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtDodaj_Click(object sender, RoutedEventArgs e)
        {
            
           
            //if(okresOdDP.SelectedDate.Value.Date > okresDoDP.SelectedDate.Value.Date)

            //DataUrDP.SelectedDate = null;
            //okresOdDP.SelectedDate = null;
            if (tbKolczyk != null && (maleCheckB.IsChecked==true || femaleCheckB.IsChecked == true) &&  DataUrDP.SelectedDate != null && okresOdDP.SelectedDate != null)
            {
                Zwierze NowyZwierzak = new Zwierze();

                NowyZwierzak.nr_kolczyka = tbKolczyk.Text;
                WpisDoBazyZwierze(NowyZwierzak);
                
        
                Polaczenie.Zwierze.InsertOnSubmit(NowyZwierzak);
                Polaczenie.SubmitChanges();
                
                MessageBox.Show("Udalo sie!");
                
                tbKolczyk.Text = string.Empty;
                maleCheckB.IsChecked = false;
                femaleCheckB.IsChecked = false;
                DataUrDP.SelectedDate = null;
                GatunekCB.SelectedIndex = -1;
                RasaCB.SelectedIndex = -1;
                okresOdDP.SelectedDate = null;
                okresDoDP.SelectedDate = null;

            }
            else
            {
                MessageBox.Show("Uzupelnij brakujace pola");
            }
        }

        private void GatunekCB_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            RasaCB.IsEnabled = false;
            if (GatunekCB.SelectedIndex != -1)
            {
                var queryGatunek = (from Gatunek in Polaczenie.Gatunek
                                    where Gatunek.nazwa == GatunekCB.SelectedItem.ToString()
                                    select Gatunek.Id).FirstOrDefault();

                var queryRasa = from Rasa in Polaczenie.Rasa
                                where Rasa.id_gatunek == queryGatunek
                                select Rasa.nazwa;
                if (queryRasa.ToList().Count > 0)
                {
                    RasaCB.IsEnabled = true;
                    RasaCB.ItemsSource = queryRasa.ToList();
                }
                else
                {
                    RasaCB.ItemsSource = null;
                }
                
            }
            
        }

        private void femaleCheckB_Click(object sender, RoutedEventArgs e)
        {
            femaleCheckB.IsChecked = true;
            maleCheckB.IsChecked = false;
            
        }

        private void maleCheckB_Click(object sender, RoutedEventArgs e)
        {
            femaleCheckB.IsChecked = false;
            maleCheckB.IsChecked = true;
        }

        private void tbKolczyk_TextChanged(object sender, TextChangedEventArgs e)
        {
            
            Kolczyk kolczyk = new Kolczyk();
            kolczyk.walidacjaKolczyk(tbKolczyk);
            
        }

        private void RasaCB_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(RasaCB.SelectedIndex != -1)
            {
                var queryRasa = (from Rasa in Polaczenie.Rasa
                                 where Rasa.nazwa == RasaCB.SelectedItem.ToString()
                                 select Rasa.Id).FirstOrDefault();
                Console.WriteLine(queryRasa);
            }
            
        }
        private void WypelnienieCBGatunek()
        {
            foreach (Gatunek rekord in Polaczenie.Gatunek)
            {
                GatunekCB.Items.Add(rekord.nazwa);
            }
            
        }



        private void WpisDoBazyZwierze(Zwierze zwierze)
        {
            zwierze.data_urodzenia = DataUrDP.SelectedDate.Value.Date;

            if (maleCheckB.IsChecked == true)
            {
                zwierze.plec = 0;
            }
            else
            {
                zwierze.plec = 1;
            }
            zwierze.okres_od = okresOdDP.SelectedDate.Value.Date;
            if (okresDoDP.SelectedDate == null)
            {
                zwierze.okres_do = null;
            }
            else
            {
                zwierze.okres_do = okresDoDP.SelectedDate.Value.Date;
            }

            if (GatunekCB.SelectedItem == null)
            {
                zwierze.id_gatunek = null;
            }
            else
            {
                var queryGatunek = (from Gatunek in Polaczenie.Gatunek
                                    where Gatunek.nazwa == GatunekCB.SelectedItem.ToString()
                                    select Gatunek.Id).FirstOrDefault();
                zwierze.id_gatunek = queryGatunek;
            }

            if (RasaCB.SelectedItem == null)
            {
                zwierze.id_rasa = null;
            }
            else
            {
                var queryRasa = (from Rasa in Polaczenie.Rasa
                                 where Rasa.nazwa == RasaCB.SelectedItem.ToString()
                                 select Rasa.Id).FirstOrDefault();
                zwierze.id_rasa = queryRasa;
            }
        }

      
    }
}

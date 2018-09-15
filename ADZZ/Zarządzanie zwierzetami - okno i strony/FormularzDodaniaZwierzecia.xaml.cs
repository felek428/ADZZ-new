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

    public FormularzDodaniaZwierzecia()
        {
            InitializeComponent();
            
            

            foreach(Gatunek rekord in Polaczenie.Gatunek)
            {
                //listaGatunkow.Add(rekord.nazwa);
                GatunekCB.Items.Add(rekord.nazwa);
             
            }
            //GatunekCB.ItemsSource = listaGatunkow;
            


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

                NowyZwierzak.data_urodzenia = DataUrDP.SelectedDate.Value.Date;
                NowyZwierzak.nr_kolczyka = "PL"+tbKolczyk.Text;
                if(maleCheckB.IsChecked == true)
                {
                    NowyZwierzak.plec = 0;
                }
                else
                {
                    NowyZwierzak.plec = 1;
                }
                NowyZwierzak.okres_od = okresOdDP.SelectedDate.Value.Date;
                if(okresDoDP.SelectedDate == null)
                {
                    NowyZwierzak.okres_do = null;
                }
                else
                {
                    NowyZwierzak.okres_do = okresDoDP.SelectedDate.Value.Date;
                }
                if(RasaCB.SelectedItem == null)
                {
                    NowyZwierzak.id_rasa = null;
                }
                else
                {
                    var queryRasa = (from Rasa in Polaczenie.Rasa
                                     where Rasa.nazwa == RasaCB.SelectedItem.ToString()
                                     select Rasa.Id).FirstOrDefault();
                    NowyZwierzak.id_rasa = queryRasa;
                }

                
                Polaczenie.Zwierze.InsertOnSubmit(NowyZwierzak);
                Polaczenie.SubmitChanges();

                MessageBox.Show("Udalo sie!");

            }
            else
            {
                MessageBox.Show("Uzupelnij brakujace pola");
            }
        }

        private void GatunekCB_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            RasaCB.IsEnabled = false;
            
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
                MessageBox.Show("Brakuje ras? Dodaj nowe!");
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
            for (int i = 0; i < tbKolczyk.Text.Length; i++)
            {
                if (!Char.IsDigit(tbKolczyk.Text[i]))
                {

                    tbKolczyk.Text = tbKolczyk.Text.Remove(i, 1);
                    tbKolczyk.Focus();
                    tbKolczyk.Select(tbKolczyk.Text.Length, 0);
                    MessageBox.Show("Wpisany znak nie jest cyfrą!");
                    break;
                }
            }
        }

        private void RasaCB_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var queryRasa = (from Rasa in Polaczenie.Rasa
                            where Rasa.nazwa == RasaCB.SelectedItem.ToString()
                            select Rasa.Id).FirstOrDefault();
            Console.WriteLine(queryRasa);
        }
    }
}

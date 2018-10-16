using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
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

namespace ADZZ.Rozliczenia___okno_i_strony
{
    /// <summary>
    /// Logika interakcji dla klasy FormularzDodaniaRozliczenia.xaml
    /// </summary>
    public partial class FormularzDodaniaRozliczenia : Page
    {
        private int Wybor { get; set; }
        public ObservableCollection<string> listaKategorii = new ObservableCollection<string>();
        List<string> listaRodzajuRozliczen = new List<string>(); //Chwilowo, poki nie jest podłączona baza
        PolaczenieBazaDataContext Polaczenie = new PolaczenieBazaDataContext();
        private FormularzDodaniaRozliczenia actualForm;
        public FormularzDodaniaRozliczenia()
        {

            InitializeComponent();
            actualForm = this;
           // listaKategorii = new ObservableCollection<string>();
           // DataContext = this;
            StworzListe(listaRodzajuRozliczen);
            WypelnienieComboBox();                      //Wypelnianie combobox|  Sposób wypełniania bedzie inny
            //WyborRozliczenia.ItemsSource = listaKategorii;
            TypZwierzat noweTypy = new TypZwierzat();
            noweTypy.WypelnienieCBTypami(cbTypyZwierzat);
            
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            
            if(WyborRozliczenia.SelectedItem.ToString() == "Sprzedaz mleka")
            {
                lbLitry.Visibility = Visibility.Visible;
                tbLitry.Visibility = Visibility.Visible;
            }
            else
            {
                lbLitry.Visibility = Visibility.Hidden;
                tbLitry.Visibility = Visibility.Hidden;
            }
        }
        public void StworzListe(List<string> lista)
        {
            lista.Add("Przychód");                              //Stala lista przychod/wydatek, ale to bede zaciagal z bazy
            lista.Add("Wydatek");
        }
        public void WypelnienieComboBox()
        {
            listaKategorii.Clear();
            var queryKategoria = from Kategoria_rozliczen in Polaczenie.Kategoria_rozliczen
                                 select Kategoria_rozliczen.nazwa;
            
            foreach (var item in queryKategoria.ToList())
            {
                listaKategorii.Add(item);
            }
            this.WyborRozliczenia.ItemsSource = listaKategorii;
            

        }
        /// <summary>
        /// Dodaje wpis do bazy
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtDodaj_Click(object sender, RoutedEventArgs e)
        {
        

            Rozliczenia nowy = new Rozliczenia();
            if (WyborRozliczenia.SelectedItem != null && tbKwota.Text != string.Empty && DataDP.SelectedDate != null)
            {
                nowy.data = DataDP.SelectedDate.Value.Date;
                nowy.kwota = Convert.ToDouble(tbKwota.Text);
                nowy.opis = tbOpis.Text;

                var queryKategoria = (from Kategoria_rozliczen in Polaczenie.Kategoria_rozliczen
                                      where Kategoria_rozliczen.nazwa == WyborRozliczenia.SelectedItem.ToString()
                                      select Kategoria_rozliczen.Id).FirstOrDefault();

                nowy.id_kategoria = queryKategoria;

                

                if (tbKolczyk.Text != string.Empty)
                {
                    var queryKolczyk = from Zwierze in Polaczenie.Zwierze
                                       where Zwierze.nr_kolczyka == tbKolczyk.Text
                                       select Zwierze.Id;

                    nowy.id_zwierze = queryKolczyk.FirstOrDefault();
                }
                
                Polaczenie.Rozliczenia.InsertOnSubmit(nowy);
                Polaczenie.SubmitChanges();
            }
            else
            {
                MessageBox.Show("Uzupelnij pola!");
            }
            

        }

        

        private void tbKwota_TextChanged(object sender, TextChangedEventArgs e)
        {  
            string newText = string.Empty;
            bool dotExist = false;
            int miejscaDziesietne = 0;
            foreach (var znak in tbKwota.Text.ToCharArray())
            {
                if (Char.IsDigit(znak) && dotExist ==true && miejscaDziesietne < 2)
                {
                    newText += znak;
                    miejscaDziesietne++;
                }
                else if (Char.IsDigit(znak) && dotExist == false)
                {
                    newText += znak;}
                else if (znak == ',' && dotExist == false)
                {
                    newText += znak;
                    dotExist = true;
                }


            }
            tbKwota.Text = newText;
            tbKwota.Focus();
            tbKwota.Select(tbKwota.Text.Length,0);


        }
        private void usuniecieOstatniegoZnaku()
        {
            tbKwota.Text = tbKwota.Text.Remove(tbKwota.Text.Length - 1);
            tbKwota.Focus();
            tbKwota.Select(tbKwota.Text.Length, 0);
        }

        private void cbTypyZwierzat_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            switch(cbTypyZwierzat.SelectedIndex)
            {
                case 0:
                    lbNrKolczyka.Visibility = Visibility.Visible;
                    lbPL.Visibility = Visibility.Visible;
                    tbKolczyk.Visibility = Visibility.Visible;

                    lbNrStada.Visibility = Visibility.Hidden;
                    tbNrStada.Visibility = Visibility.Hidden;
                    break;
                case 1:
                    lbNrStada.Visibility = Visibility.Visible;
                    tbNrStada.Visibility = Visibility.Visible;

                    lbNrKolczyka.Visibility = Visibility.Hidden;
                    tbKolczyk.Visibility = Visibility.Hidden;
                    break;
                default:
                    break;
            }
        }

        private void PackIcon_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            FormularzDodaniaKategoriiRozliczen nowaKategoria = new FormularzDodaniaKategoriiRozliczen(actualForm);
            nowaKategoria.ShowDialog();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            listaKategorii.Add("kat");

            Kategoria_rozliczen nowakategoria = new Kategoria_rozliczen();
            nowakategoria.nazwa = "TestDodanie";
            Polaczenie.Kategoria_rozliczen.InsertOnSubmit(nowakategoria);
            Polaczenie.SubmitChanges();
            WypelnienieComboBox();

        }

        private void infoDodajKategorie_MouseEnter(object sender, MouseEventArgs e)
        {
            Cursor = Cursors.Hand;
        }

        private void infoDodajKategorie_MouseLeave(object sender, MouseEventArgs e)
        {
            Cursor = Cursors.Arrow;
        }
    }
}

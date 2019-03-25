using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Globalization;
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
        private int wybranyTyp;
        private int WybraneIdRozliczenie;
        private int WybraneIdMleko;
        
        public FormularzDodaniaRozliczenia(int wybranyTyp)
        {

            InitializeComponent();
            actualForm = this;
            this.wybranyTyp = wybranyTyp;
            // listaKategorii = new ObservableCollection<string>();
            // DataContext = this;
            StworzListe(listaRodzajuRozliczen);
            WypelnienieComboBox();                      //Wypelnianie combobox|  Sposób wypełniania bedzie inny
            //WyborRozliczenia.ItemsSource = listaKategorii;
            TypZwierzat noweTypy = new TypZwierzat();
            TypKontrolek(wybranyTyp);
            
            Kolczyk TrescCbKolczyk = new Kolczyk();
            TrescCbKolczyk.WypelnienieCbKolczykZwierze(cbKolczyk);
            TrescCbKolczyk.WypelnienieCbKolczykStado(cbNrStada);
            WypelnijCbRok();
            WypelnijListeRozliczenia();
            WypelnijListeMleko();
        }
        public FormularzDodaniaRozliczenia()
        {

        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(WyborRozliczenia.SelectedIndex != -1)
            {
                if (WyborRozliczenia.SelectedIndex == 0)
                {
                    lbLitry.Visibility = Visibility.Visible;
                    tbLitry.Visibility = Visibility.Visible;
                    tbKwota.IsEnabled = false;
                }
                else
                {
                    lbLitry.Visibility = Visibility.Hidden;
                    tbLitry.Visibility = Visibility.Hidden;
                    tbKwota.IsEnabled = true;
                }
            }
            
        }
        public void StworzListe(List<string> lista)
        {
            lista.Add("Przychód");                              //Stala lista przychod/wydatek
            lista.Add("Wydatek");
        }
        /// <summary>
        /// Wypelnienie comboboxa z kategoriami rozliczen
        /// </summary>
        public void WypelnienieComboBox()
        {
            List<string> queryKategoria = new List<string>();
            listaKategorii.Clear();
            if(wybranyTyp == 0)
            {
                queryKategoria = (from Kategoria_rozliczen in Polaczenie.Kategoria_rozliczen
                                     select Kategoria_rozliczen.nazwa).ToList();
            }
            else
            {
                queryKategoria = (from Kategoria_rozliczen in Polaczenie.Kategoria_rozliczen
                                     where Kategoria_rozliczen.Id != 1
                                     select Kategoria_rozliczen.nazwa).ToList();
            }
            
            foreach (var item in queryKategoria)
            {
                listaKategorii.Add(item);
            }
            WyborRozliczenia.ItemsSource = listaKategorii;

            for (int i = 1; i <= 12; i++)
            {
                cbMiesiac.Items.Add(i);
                if(i <= 2)
                {
                    cbPolowa.Items.Add(i);
                }
            }

        }
        /// <summary>
        /// Dodaje wpis do bazy
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtDodaj_Click(object sender, RoutedEventArgs e)
        {

      
            Rozliczenia nowy = new Rozliczenia();
            if (WyborRozliczenia.SelectedItem != null && DataDP.SelectedDate != null)
            {
                nowy.data = DataDP.SelectedDate.Value.Date;
                
                nowy.opis = tbOpis.Text;

                if(tbKwota.Text != string.Empty)
                {
                    nowy.kwota = Convert.ToDouble(tbKwota.Text);
                }

                var queryKategoria = (from Kategoria_rozliczen in Polaczenie.Kategoria_rozliczen
                                      where Kategoria_rozliczen.nazwa == WyborRozliczenia.SelectedItem.ToString()
                                      select Kategoria_rozliczen.Id).FirstOrDefault();

                nowy.id_kategoria = queryKategoria;

                

                if (cbKolczyk.SelectedItem != null)
                {
                    var queryKolczyk = from Zwierze in Polaczenie.Zwierze
                                       where Zwierze.nr_kolczyka == cbKolczyk.SelectedItem.ToString()
                                       select Zwierze.Id;

                    nowy.id_zwierze = queryKolczyk.FirstOrDefault();
                }
                else if (cbNrStada.SelectedItem != null)
                {
                    var queryNrStada = from Stado in Polaczenie.Stado
                                       where Stado.nr_stada == cbNrStada.SelectedItem.ToString()
                                       select Stado.Id;

                    nowy.id_stado = queryNrStada.FirstOrDefault();
                }else if(cbNrStada.Text.Length > 0 && (cbKolczyk.SelectedItem == null && cbNrStada.SelectedItem == null))
                {
                    MessageBox.Show("Podany identyfikator zwierząt nie istnieje!");
                    return;
                }
               
                if(tbLitry.Text != string.Empty)
                {
                    nowy.ilosc_litrow = Convert.ToInt32(tbLitry.Text);
                }
                
                Polaczenie.Rozliczenia.InsertOnSubmit(nowy);
                Polaczenie.SubmitChanges();



                MessageBox.Show("Powiodło się!");



                cbNrStada.SelectedItem = null;
                cbKolczyk.SelectedItem = null;
                WyborRozliczenia.SelectedItem = null;
                tbKwota.Text = string.Empty;
                DataDP.SelectedDate = null;
                tbOpis.Text = string.Empty;
                tbLitry.Text = string.Empty;
                tbLitry.Visibility = Visibility.Hidden;
                lbLitry.Visibility = Visibility.Hidden;

                WypelnijListeRozliczenia();
                WypelnijListeMleko();
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
            var textbox = (sender as TextBox);
            foreach (var znak in textbox.Text.ToCharArray())
            {
                if (Char.IsDigit(znak) && dotExist == true && miejscaDziesietne < 2)
                {
                    newText += znak;
                    miejscaDziesietne++;
                }
                else if (Char.IsDigit(znak) && dotExist == false)
                {
                    newText += znak;
                }
                else if (znak == ',' && dotExist == false)
                {
                    newText += znak;
                    dotExist = true;
                }
            }
            textbox.Text = newText;
            textbox.Focus();
            textbox.Select(textbox.Text.Length,0);
        }
        private void usuniecieOstatniegoZnaku()
        {
            tbKwota.Text = tbKwota.Text.Remove(tbKwota.Text.Length - 1);
            tbKwota.Focus();
            tbKwota.Select(tbKwota.Text.Length, 0);
        }
        /// <summary>
        /// Okresla jakie kontrolki maja sie wyswietlic na podstawie wybranego typu zwierzat
        /// </summary>
        /// <param name="wybranyTyp"></param>
        private void TypKontrolek(int wybranyTyp)
        {
            switch (wybranyTyp)
            {
                case 0:
                    lbNrKolczyka.Visibility = Visibility.Visible;
                    cbKolczyk.Visibility = Visibility.Visible;

                    lbNrStada.Visibility = Visibility.Hidden;
                    cbNrStada.Visibility = Visibility.Hidden;
                    break;
                case 1:
                    lbNrStada.Visibility = Visibility.Visible;
                    cbNrStada.Visibility = Visibility.Visible;

                    lbNrKolczyka.Visibility = Visibility.Hidden;
                    cbKolczyk.Visibility = Visibility.Hidden;
                    break;
                default:
                    break;
            }
        }


        private void PackIcon_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            FormularzDodaniaKategoriiRozliczen nowaKategoria = new FormularzDodaniaKategoriiRozliczen(actualForm, WyborRozliczenia);
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

        private void cbKolczyk_TextChanged(object sender, TextChangedEventArgs e)
        {
            Kolczyk kolczyk = new Kolczyk();
            

            switch (wybranyTyp)
            {
                case 0:
                    kolczyk.walidacjaKolczyk(cbKolczyk);
                    break;
                case 1:
                    //kolczyk.walidacjaKolczyk(cbNrStada);
                    break;
                default:
                    break;
            }

            
        }

        private void WypelnijCbRok()
        {
            for (int i = 0; i < 20; i++)
            {
                cbRok.Items.Add(DateTime.Now.Year - i);
            }

            cbRok.SelectedIndex = 0;
        }

        private void btDodajCene_Click(object sender, RoutedEventArgs e)
        {
            Historia_cen queryCena = null;

            if (Convert.ToInt32(cbPolowa.SelectedItem) == 1)
            {
                 queryCena = (from H in Polaczenie.Historia_cen
                                 where H.okres_od.Equals(DataPierwszaPolowa())
                                 select H).SingleOrDefault();

            }
            else if (Convert.ToInt32(cbPolowa.SelectedItem) == 2)
            {
                 queryCena = (from H in Polaczenie.Historia_cen
                                 where H.okres_od.Equals(DataDrugaPolowa())
                                 select H).SingleOrDefault();
            }
            

            if (queryCena == null)
            {
                if (cbMiesiac.SelectedItem != null && cbPolowa.SelectedItem != null && tbCenaMleka.Text != string.Empty)
                {
                    Historia_cen nowaCena = new Historia_cen();



                    if (Convert.ToInt32(cbPolowa.SelectedItem) == 1)
                    {
                        nowaCena.okres_od = DataPierwszaPolowa();
                        nowaCena.okres_do = Convert.ToDateTime("15." + cbMiesiac.SelectedItem.ToString() + "." + cbRok.SelectedItem.ToString());

                    }
                    else if (Convert.ToInt32(cbPolowa.SelectedItem) == 2)
                    {
                        nowaCena.okres_od = DataDrugaPolowa();
                        nowaCena.okres_do = Convert.ToDateTime(DateTime.DaysInMonth(Convert.ToInt32(cbRok.SelectedItem), Convert.ToInt32(cbMiesiac.SelectedItem)) + "." + cbMiesiac.SelectedItem.ToString() + "." + cbRok.SelectedItem.ToString());

                    }

                    nowaCena.id_kategoria_rozliczen = 1;
                    nowaCena.cena = Convert.ToDouble(tbCenaMleka.Text);

                    Polaczenie.Historia_cen.InsertOnSubmit(nowaCena);
                    Polaczenie.SubmitChanges();

                    MessageBox.Show("Dodano!");

                    cbMiesiac.SelectedIndex = -1;
                    cbRok.SelectedIndex = -1;
                    cbPolowa.SelectedIndex = -1;
                    tbCenaMleka.Text = string.Empty;

                }
                else
                {
                    MessageBox.Show("Uzupełnij puste pola!");
                }
                
            }
            else
            {
                if(tbCenaMleka.Text != string.Empty)
                {
                    queryCena.cena = Convert.ToDouble(tbCenaMleka.Text);
                    Polaczenie.SubmitChanges();

                    
                }
                else if(tbCenaMleka.Text == string.Empty || Convert.ToInt32(tbCenaMleka.Text) == 0)
                {
                    

                    Polaczenie.Historia_cen.DeleteOnSubmit(queryCena);
                    Polaczenie.SubmitChanges();
                }

                MessageBox.Show("Zaktualizowano!");
            }
            
        }
        private void WypelnijListeRozliczenia()
        {
            var query = (from R in Polaczenie.Rozliczenia
                         where R.id_kategoria != 1
                         orderby R.data descending
                         select new Rozliczenie { Data = R.data.ToShortDateString(), Kwota = R.kwota, Opis = R.opis, Kategoria = R.Kategoria_rozliczen.nazwa, Id = R.Id }).ToList();

            lvListaRozliczen.ItemsSource = query;
        }
        private void WypelnijListeMleko()
        {
            var query = (from R in Polaczenie.Rozliczenia
                         where R.id_kategoria == 1
                         orderby R.data descending
                         select new Rozliczenie { Data = R.data.ToShortDateString(), Litry = R.ilosc_litrow, Kategoria = R.Kategoria_rozliczen.nazwa,  Kolczyk = R.Zwierze.nr_kolczyka, Id = R.Id}).ToList();

            lvMleko.ItemsSource = query;
        }

        private void PobierzCeneMleka()
        {

            if(cbRok.SelectedItem != null && cbPolowa.SelectedItem != null && cbMiesiac.SelectedItem != null)
            {
                DateTime? Data = null;

                if (Convert.ToInt32(cbPolowa.SelectedItem) == 1)
                {
                    Data = DataPierwszaPolowa();
                   

                }
                else if (Convert.ToInt32(cbPolowa.SelectedItem) == 2)
                {
                    Data = DataDrugaPolowa();
                }

                var query = (from H in Polaczenie.Historia_cen
                            where H.okres_od.Equals(Data)
                            select H.cena).FirstOrDefault();

                tbCenaMleka.Text = query.ToString();
            }
        }

        private void cbRok_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            PobierzCeneMleka();
        }

        private void cbMiesiac_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            PobierzCeneMleka();
        }

        private void cbPolowa_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            PobierzCeneMleka();
        }

        private DateTime? DataPierwszaPolowa()
        {
            var Data = Convert.ToDateTime(Convert.ToDateTime("1." + cbMiesiac.SelectedItem.ToString() + "." + cbRok.SelectedItem.ToString()).ToShortDateString());


            return Data;
        }
        private DateTime? DataDrugaPolowa()
        {
            var Data = Convert.ToDateTime("16." + cbMiesiac.SelectedItem.ToString() + "." + cbRok.SelectedItem.ToString());


            return Data;
        }
        private void lvRozliczeniaItem_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {

            var item = sender as ListViewItem;
            if (item != null && item.IsSelected)
            {
                var ZaznaczonaPozycja = (Rozliczenie)lvListaRozliczen.SelectedItem;
                WybraneIdRozliczenie = ZaznaczonaPozycja.Id;

            }
        }
        private void lvMlekoItem_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {

            var item = sender as ListViewItem;
            if (item != null && item.IsSelected)
            {
                var ZaznaczonaPozycja = (Rozliczenie)lvMleko.SelectedItem;
                WybraneIdMleko = ZaznaczonaPozycja.Id;

            }
        }

        private void lvListaRozliczen_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lvListaRozliczen.SelectedItem != null)
            {
                btUsunRozliczenie.IsEnabled = true;

            }
            else
            {
                btUsunRozliczenie.IsEnabled = false;
            }
        }

        private void lvMleko_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lvMleko.SelectedItem != null)
            {
                btUsunMleko.IsEnabled = true;

            }
            else
            {
                btUsunMleko.IsEnabled = false;
            }
        }

        private void btUsunRozliczenie_Click(object sender, RoutedEventArgs e)
        {
            
            if (lvListaRozliczen.SelectedItem != null)
            {
                Rozliczenia usunRozliczenie = Polaczenie.Rozliczenia.Single(x => x.Id == WybraneIdRozliczenie);

                Polaczenie.Rozliczenia.DeleteOnSubmit(usunRozliczenie);
                Polaczenie.SubmitChanges();
            }
            
            WypelnijListeRozliczenia();
        }

        private void btUsunMleko_Click(object sender, RoutedEventArgs e)
        {
            
            if (lvMleko.SelectedItem != null)
            {
                Rozliczenia usunRozliczenie = Polaczenie.Rozliczenia.Single(x => x.Id == WybraneIdMleko);

                Polaczenie.Rozliczenia.DeleteOnSubmit(usunRozliczenie);
                Polaczenie.SubmitChanges();
            }
            
            WypelnijListeMleko();
        }
    }
    class Rozliczenie
    {
        public string Data { get; set; }
        public string Opis { get; set; }
        public double? Kwota { get; set; }  
        public int? Litry { get; set; }  
        public string Kategoria { get; set; }
        public string Kolczyk { get; set; }
        public int Id { get; set; }
    }
}

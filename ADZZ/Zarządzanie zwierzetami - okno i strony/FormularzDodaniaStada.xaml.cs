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
    /// Logika interakcji dla klasy FormularzDodaniaStada.xaml
    /// </summary>
    public partial class FormularzDodaniaStada : Page
    {
        PolaczenieBazaDataContext Polaczenie = new PolaczenieBazaDataContext();
        private int wybraneStadoId;
        public FormularzDodaniaStada(string kolczyk)
        {
            InitializeComponent();
            wypelnienieGatunekCb(GatunekCB);
            btDodaj.Content = "Aktualizuj";
            btDodaj.Click -= new RoutedEventHandler(btDodaj_Click);
            btDodaj.Click += new RoutedEventHandler(btAktualizuj_Click);
            tbKolczyk.IsEnabled = false;

            var queryStado = (from Stado in Polaczenie.Stado
                              where Stado.nr_stada == kolczyk
                              select Stado).FirstOrDefault();
            wybraneStadoId = queryStado.Id;

            var queryHistoria = (from Historia in Polaczenie.Historia_Stada
                                 where Historia.id_stado == queryStado.Id
                                 select Historia).ToList().Last();

            tbKolczyk.Text = kolczyk;
            tbIlosc.Text = queryHistoria.ilosc.ToString();
            GatunekCB.SelectedItem = queryStado.Gatunek.nazwa;
            okresOdDP.SelectedDate = queryHistoria.okres_od;

        }
        public FormularzDodaniaStada()
        {
            InitializeComponent();
            wypelnienieGatunekCb(GatunekCB);
        }

        private void tbKolczyk_TextChanged(object sender, TextChangedEventArgs e)
        {
            Kolczyk sprawdzenie = new Kolczyk();
            if (GatunekCB.SelectedItem.ToString().Equals("Trzoda"))
            {
                sprawdzenie.walidacjaKolczyk(tbKolczyk);
            }
            
        }

        private void wypelnienieGatunekCb(ComboBox comboBox)
        {
            foreach (var item in Polaczenie.Gatunek)
            {
                if(item.czyStado == 1)
                {
                    comboBox.Items.Add(item.nazwa);
                }
            }
        }

        private void tbIlosc_TextChanged(object sender, TextChangedEventArgs e)
        {
            var ilosc = sender as TextBox;
            for (int i = 0; i < ilosc.Text.Length; i++)
            {
                if (!Char.IsDigit(ilosc.Text[i]))
                {

                    ilosc.Text = ilosc.Text.Remove(i, 1);
                    ilosc.Focus();
                    ilosc.Select(ilosc.Text.Length, 0);

                }
            }
            
        }

        private void btAktualizuj_Click(object sender, RoutedEventArgs e)
        {
            Stado queryStado = (from Stado in Polaczenie.Stado
                               where Stado.Id == wybraneStadoId
                               select Stado).FirstOrDefault();
            WpisDoStado(queryStado);
            Polaczenie.SubmitChanges();
            Historia_Stada queryHistoria = (from Historia in Polaczenie.Historia_Stada
                                            where Historia.id_stado == wybraneStadoId
                                            select Historia).ToList().Last();
            WpisDoHistoriaStada(queryHistoria, wybraneStadoId);

        }

        private void btDodaj_Click(object sender, RoutedEventArgs e)
        {
            Stado noweStado = new Stado();
            Historia_Stada nowaHistoriaStada = new Historia_Stada();

            WpisDoBazy(noweStado, nowaHistoriaStada);

           
            
            WpisDoStado(noweStado);
            WpisDoHistoriaStada(nowaHistoriaStada, OstatniWpisStado());

        }
        /// <summary>
        /// Tworzy nowe rekordy w tabeli stado oraz tabeli Historia_stad(tworząc nową historie dla nowo dodanego stada)
        /// </summary>
        /// <param name="noweStado"></param>
        /// <param name="nowaHistoria"></param>
        private void WpisDoBazy(Stado noweStado, Historia_Stada nowaHistoria)
        {
            if (tbKolczyk.Text != string.Empty && tbIlosc.Text != string.Empty && GatunekCB.SelectedItem != null && okresOdDP.SelectedDate != null)
            {
                WpisDoStado(noweStado);


                Polaczenie.Stado.InsertOnSubmit(noweStado);

                Polaczenie.SubmitChanges();
                nowaHistoria.id_stado = OstatniWpisStado();
                nowaHistoria.okres_od = okresOdDP.SelectedDate;
                nowaHistoria.ilosc = Convert.ToInt32(tbIlosc.Text);
                Polaczenie.Historia_Stada.InsertOnSubmit(nowaHistoria);
                Polaczenie.SubmitChanges();


                tbKolczyk.Text = string.Empty;
                tbIlosc.Text = string.Empty;
                GatunekCB.SelectedIndex = -1;
                okresOdDP.SelectedDate = null;
                nrKolczykaL.Visibility = Visibility.Hidden;
                lbGwiazdaNrStada.Visibility = Visibility.Hidden;
                lbGwiazdaNazwaStada.Visibility = Visibility.Hidden;
                tbKolczyk.Visibility = Visibility.Hidden;

                MessageBox.Show("Powiodło się");

                

            }
            else
            {
                MessageBox.Show("Uzupełnij brakujące pola!");
            }
        }

        private void WpisDoStado(Stado noweStado)
        {
            if (tbIlosc.Text != string.Empty && GatunekCB.SelectedItem != null && okresOdDP.SelectedDate != null)
            {
                var queryGatunek = (from Gatunek in Polaczenie.Gatunek
                                    where Gatunek.nazwa == (string)GatunekCB.SelectedItem
                                    select Gatunek.Id).FirstOrDefault();
                noweStado.nr_stada = tbKolczyk.Text;
                noweStado.id_gatunek = queryGatunek;

            }
            
        }

        private void WpisDoHistoriaStada(Historia_Stada staraHistoria, int idStada)
        {
            if (tbKolczyk.Text != string.Empty && tbIlosc.Text != string.Empty && GatunekCB.SelectedItem != null && okresOdDP.SelectedDate != null)
            {
                if(staraHistoria.okres_od != okresOdDP.SelectedDate.Value.Date && !tbIlosc.Text.Equals(staraHistoria.ilosc))
                {
                    staraHistoria.okres_do = okresOdDP.SelectedDate;
                    Polaczenie.SubmitChanges();


                    var nowaHistoria = new Historia_Stada();

                    nowaHistoria.id_stado = idStada;
                    nowaHistoria.okres_od = okresOdDP.SelectedDate;
                    nowaHistoria.ilosc = Convert.ToInt32(tbIlosc.Text);
                    Polaczenie.Historia_Stada.InsertOnSubmit(nowaHistoria);
                    Polaczenie.SubmitChanges();
                }

                


               
               
            }
        }

        private int OstatniWpisHistorii()
        {
            var query = (from Historia in Polaczenie.Historia_Stada
                         select Historia.Id).ToList().Last();
            
            return query;
        }

        private int OstatniWpisStado()
        {
            var query = (from Stado in Polaczenie.Stado
                         select Stado.Id).ToList().Last();

            return query;
        }

        private void GatunekCB_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            tbKolczyk.Text = string.Empty;
            if(GatunekCB.SelectedItem != null)
            {
                if (GatunekCB.SelectedItem.ToString().Equals("Trzoda"))
                {
                    string labelContent = "Nr siedziby stada:";
                    nrKolczykaL.Content =  labelContent;
                    nrKolczykaL.Visibility = Visibility.Visible;
                    lbGwiazdaNrStada.Visibility = Visibility.Visible;
                    lbGwiazdaNazwaStada.Visibility = Visibility.Hidden;
                    tbKolczyk.Visibility = Visibility.Visible;
                }
                else if (GatunekCB.SelectedItem.ToString().Equals("Drób"))
                {
                    string labelContent = "Nazwa:";
                    nrKolczykaL.Content = labelContent;

                    lbGwiazdaNrStada.Visibility = Visibility.Hidden;
                    lbGwiazdaNazwaStada.Visibility = Visibility.Visible;
                    nrKolczykaL.Visibility = Visibility.Visible;
                    tbKolczyk.Visibility = Visibility.Visible;
                }
            }
            
        }

        
    }
}

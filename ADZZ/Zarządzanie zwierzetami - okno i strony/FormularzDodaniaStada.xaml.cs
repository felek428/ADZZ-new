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
        public FormularzDodaniaStada()
        {
            InitializeComponent();
            wypelnienieGatunekCb(GatunekCB);
        }

        private void tbKolczyk_TextChanged(object sender, TextChangedEventArgs e)
        {
            Kolczyk sprawdzenie = new Kolczyk();
            sprawdzenie.walidacjaKolczyk(tbKolczyk);
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

        private void btDodaj_Click(object sender, RoutedEventArgs e)
        {
            Stado noweStado = new Stado();
            Historia_Stada nowaHistoriaStada = new Historia_Stada();

            WpisDoStado(noweStado);
            WpisDoHistoriaStada(nowaHistoriaStada, OstatniWpisStado());
            
        }

        private void WpisDoBazy()
        {
            
            
            
        }


        private void WpisDoStado(Stado noweStado)
        {
            if (tbKolczyk.Text != string.Empty && tbIlosc.Text != string.Empty && GatunekCB.SelectedItem != null && okresOdDP.SelectedDate != null)
            {
                var queryGatunek = (from Gatunek in Polaczenie.Gatunek
                                    where Gatunek.nazwa == (string)GatunekCB.SelectedItem
                                    select Gatunek.Id).FirstOrDefault();
                noweStado.nr_stada = tbKolczyk.Text;
                noweStado.id_gatunek = queryGatunek;

                
                Polaczenie.Stado.InsertOnSubmit(noweStado);
                
                Polaczenie.SubmitChanges();
            }
            
        }

        private void WpisDoHistoriaStada(Historia_Stada nowaHistoria, int idStada)
        {
            if (tbKolczyk.Text != string.Empty && tbIlosc.Text != string.Empty && GatunekCB.SelectedItem != null && okresOdDP.SelectedDate != null)
            {
                nowaHistoria.id_stado = idStada;
                nowaHistoria.okres_od = okresOdDP.SelectedDate;
                nowaHistoria.ilosc = Convert.ToInt32(tbIlosc.Text);
                Polaczenie.Historia_Stada.InsertOnSubmit(nowaHistoria);
                Polaczenie.SubmitChanges();
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
    }
}

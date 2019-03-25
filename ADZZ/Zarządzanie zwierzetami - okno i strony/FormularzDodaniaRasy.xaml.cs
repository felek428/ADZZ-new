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
using System.Windows.Shapes;

namespace ADZZ.Zarządzanie_zwierzetami___okno_i_strony
{
    /// <summary>
    /// Logika interakcji dla klasy FormularzDodaniaRasy.xaml
    /// </summary>
    public partial class FormularzDodaniaRasy : Window
    {
        PolaczenieBazaDataContext Polaczenie = new PolaczenieBazaDataContext();
        private int wybranyGatunekId;
        private ComboBox cbRasa;
        public FormularzDodaniaRasy(int idGatunek, ComboBox cbRasa)
        {
            InitializeComponent();
            wybranyGatunekId = idGatunek;
            this.cbRasa = cbRasa;
            
        }
        public FormularzDodaniaRasy()
        {

        }

        private void btnDodaj_Click(object sender, RoutedEventArgs e)
        {
            var query = from Rasa in Polaczenie.Rasa
                        where Rasa.id_gatunek == wybranyGatunekId
                        select Rasa.nazwa;
            bool czyIstnieje = false;
            foreach (var item in query)
            {
                if (item.ToLower().Equals(tbNazwaRasy.Text.ToLower()))
                {
                    czyIstnieje = true;
                }
                
            }
            

            if(czyIstnieje == false)
            {
                if(tbNazwaRasy.Text != string.Empty)
                {
                    Rasa nowaRasa = new Rasa();
                    nowaRasa.nazwa = tbNazwaRasy.Text;
                    nowaRasa.id_gatunek = wybranyGatunekId;


                    Polaczenie.Rasa.InsertOnSubmit(nowaRasa);
                    Polaczenie.SubmitChanges();


                    cbRasa.IsEnabled = true;

                    cbRasa.ItemsSource = query;
                    cbRasa.SelectedItem = nowaRasa.nazwa;


                    Close();
                }
                else
                {
                    MessageBox.Show("Uzupełnij puste pole!");
                }
                
            }
            else
            {
                MessageBox.Show("Podana nazwa już istnieje!");
            }
          
        }
    }
}

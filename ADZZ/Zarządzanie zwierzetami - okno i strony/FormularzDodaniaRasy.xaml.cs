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
            Rasa nowaRasa = new Rasa();
            nowaRasa.nazwa = tbNazwaRasy.Text;
            nowaRasa.id_gatunek = wybranyGatunekId;
            
            
            Polaczenie.Rasa.InsertOnSubmit(nowaRasa);
            Polaczenie.SubmitChanges();

            
                cbRasa.IsEnabled = true;
                var query = from Rasa in Polaczenie.Rasa
                            where Rasa.id_gatunek == wybranyGatunekId
                            select Rasa.nazwa;
                cbRasa.ItemsSource = query;
                cbRasa.SelectedItem = nowaRasa.nazwa;
            

            Close();
        }
    }
}

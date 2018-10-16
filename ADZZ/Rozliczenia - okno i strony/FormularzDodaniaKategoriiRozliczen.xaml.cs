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

namespace ADZZ.Rozliczenia___okno_i_strony
{
    /// <summary>
    /// Logika interakcji dla klasy FormularzDodaniaKategoriiRozliczen.xaml
    /// </summary>
    public partial class FormularzDodaniaKategoriiRozliczen : Window
    {
        PolaczenieBazaDataContext Polaczenie = new PolaczenieBazaDataContext();
        FormularzDodaniaRozliczenia actualForm;
        public FormularzDodaniaKategoriiRozliczen( FormularzDodaniaRozliczenia formularz)
        {
            actualForm = formularz;
            InitializeComponent();
            WypelnienieCbCzyPrzychod();
        }

        private void btnDodaj_Click(object sender, RoutedEventArgs e)
        {
            Kategoria_rozliczen nowaKategoria = new Kategoria_rozliczen();
            switch (cbCzyPrzychod.SelectedItem)
            {
                case "Przychód":
                    nowaKategoria.czyPrzychod = 1;
                    break;
                case "Wydatek":
                    nowaKategoria.czyPrzychod = 0;
                    break;
            }
            nowaKategoria.nazwa = tbNazwaKategorii.Text;
            Polaczenie.Kategoria_rozliczen.InsertOnSubmit(nowaKategoria);
            Polaczenie.SubmitChanges();
            actualForm.WypelnienieComboBox();
            
            Close();
        }

        private void WypelnienieCbCzyPrzychod()
        {
            List<string> lista = new List<string>();
            lista.Add("Przychód");
            lista.Add("Wydatek");
            cbCzyPrzychod.ItemsSource = lista;
        }
    }
}

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
        ComboBox cbListaKategorii;
        public FormularzDodaniaKategoriiRozliczen( FormularzDodaniaRozliczenia formularz, ComboBox cbListaKategorii)
        {
            actualForm = formularz;
            this.cbListaKategorii = cbListaKategorii;
            InitializeComponent();
            WypelnienieCbCzyPrzychod();
        }

        private void btnDodaj_Click(object sender, RoutedEventArgs e)
        {
            var query = from KR in Polaczenie.Kategoria_rozliczen    
                        select KR.nazwa;
            bool czyIstnieje = false;
            foreach (var item in query)
            {
                if (item.ToLower().Equals(tbNazwaKategorii.Text.ToLower()))
                {
                    czyIstnieje = true;
                }

            }


            if (czyIstnieje == false)
            {
                if (tbNazwaKategorii.Text != string.Empty && cbCzyPrzychod.SelectedItem != null)
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
                    actualForm.WyborRozliczenia.SelectedItem = tbNazwaKategorii.Text;
                    cbListaKategorii.SelectedItem = tbNazwaKategorii.Text;
                    Close();
                }
                else
                {
                    MessageBox.Show("Uzupełnij puste pola!");
                }
            }
            else
            {
                MessageBox.Show("Podana nazwa już istnieje!");
            }
           
           
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

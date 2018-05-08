using System;
using System.Collections.Generic;
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

        List<string> listaRodzajuRozliczen = new List<string>(); //Chwilowo, poki nie jest podłączona baza
        
        

        public FormularzDodaniaRozliczenia()
        {
            InitializeComponent();
            StworzListe(listaRodzajuRozliczen);
            WypelnienieComboBox();
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var elementComboBox = WyborRozliczenia.SelectedItem.ToString(); //Odczytuje wybrany item w combobox 
            switch (elementComboBox)
            {
                case "Przychód":
                    Wybor = 1;
                   
                    break;
                case "Wydatek":
                    Wybor = 2;
                    
                    break;
                default:
                    break;
            }
        }
        public void StworzListe(List<string> lista)
        {
            lista.Add("Przychód");
            lista.Add("Wydatek");
        }
        public void WypelnienieComboBox()
        {
            WyborRozliczenia.ItemsSource = listaRodzajuRozliczen;
        }

        private void BtDodaj_Click(object sender, RoutedEventArgs e)
        {
            int kwota = Int32.Parse(tbKwota.Text);
            if((kwota>0 && Wybor == 1) || (kwota<0 && Wybor == 2))
            {
                MessageBoxResult result = MessageBox.Show("dodano!");
            }
            else if(kwota > 0 && Wybor == 2)
            {
                MessageBoxResult result = MessageBox.Show("Wydatek nie moze być dodatni!");

            }
            else
            {
                MessageBoxResult result = MessageBox.Show("Przychód nie może być ujemny!");
            }


        }
    }
}

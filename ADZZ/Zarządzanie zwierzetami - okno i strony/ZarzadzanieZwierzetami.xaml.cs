using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Tracing;
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
    /// Logika interakcji dla klasy ZarzadzanieZwierzetami.xaml
    /// </summary>
    public partial class ZarzadzanieZwierzetami : Window
    {
        

        public ZarzadzanieZwierzetami()
        {
            InitializeComponent();
            TypZwierzat.UstawListeTypow(WyborTypu);
            
            Debug.WriteLine("ELO");
        }
        /// <summary>
        /// Powrot do głównego okna
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnWroc_Click(object sender, RoutedEventArgs e)
        {
            TypZwierzat.wybor = 0;
            Close();
        }

        /// <summary>
        /// Ustawia wartosc zmiennej wybor w statycznym polu w klasie TypZwierzat
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void WyborTypu_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var elementComboBox = WyborTypu.SelectedItem.ToString(); //Odczytuje wybrany item w combobox 
            switch (elementComboBox)
            {
                case "Pojedyncze zwierze":
                    TypZwierzat.wybor = 1;
                    Debug.WriteLine("ELO");
                    break;
                case "Stado":
                    TypZwierzat.wybor = 2;
                    Debug.WriteLine("ELO");
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// Metoda wyswietla odpowiedni formularz w zaleznosci od wybranego typu zwierzat
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnDodaj_Click(object sender, RoutedEventArgs e)
        {
            var wybor = TypZwierzat.wybor;
            switch (wybor)
            {
                case 1:
                    RamkaForumalarzaDanych.Content = new FormularzDodaniaZwierzecia();
                    break;
                case 2:
                    RamkaForumalarzaDanych.Content = new FormularzDodaniaStada();
                    break;
                default:
                    MessageBoxResult brakWyboru = MessageBox.Show("Wybierz typ zwierząt!");
                    break;
            }
            
        }

        private void BtnEdytuj_Click(object sender, RoutedEventArgs e)
        {
            RamkaForumalarzaDanych.Content = new FormularzEdycji();
        }
    }
}

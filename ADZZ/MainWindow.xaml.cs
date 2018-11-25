
using ADZZ.Rozliczenia___okno_i_strony;
using ADZZ.Zarządzanie_zwierzetami___okno_i_strony;
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
using NewCalendar;
using ADZZ.Statystyki___okno_i_strony;

namespace ADZZ
{
    /// <summary>
    /// Logika interakcji dla klasy MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        
        public MainWindow()
        {
            InitializeComponent();
            TypZwierzat.UzupelnijTypy();
        }
        /// <summary>
        /// Zamykanie całego programu
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtExit_Click(object sender, RoutedEventArgs e)
        {
            Close();                                                    //Metoda do zamykania aplikacji z pozycji MainWindow
        }
        #region Metody otwierania okienek dla przyciskow z głównego menu


        private void BtWydatkiPrzychody_Click(object sender, RoutedEventArgs e)
        {
            Rozliczenia przejscie = new Rozliczenia(); //Tworze instancje klasy WydatkiPrzychody aby przejsc do nowego okna
            //przejscie.ShowDialog();

        }

        private void BtStatystyki_Click(object sender, RoutedEventArgs e)
        {
            
           
        }

        #endregion

        private void BtDodajZwierze_Click(object sender, RoutedEventArgs e)
        {
            //ramkaAkcji.Content = new FormularzDodaniaZwierzecia();
            ramkaAkcji.Content = new WyborTypow(typeof(FormularzDodaniaZwierzecia));
            
          
            
        }

        private void ramkaAkcji_Navigated(object sender, NavigationEventArgs e)
        {
            ramkaAkcji.NavigationService.RemoveBackEntry();
        }

        private void BtKalendarz_Click(object sender, RoutedEventArgs e)
        {
            ramkaAkcji.Content = new NowyCalendar();
            
        }

        private void btnDodajRozliczenie_Click(object sender, RoutedEventArgs e)
        {
            ramkaAkcji.Content = new WyborTypow(typeof(FormularzDodaniaRozliczenia));
        }

        private void BtListaZwierzat_Click(object sender, RoutedEventArgs e)
        {
            //ramkaAkcji.Content = new ListaZwierzat(ramkaAkcji);
            ramkaAkcji.Content = new WyborTypow(typeof(ListaZwierzat));
            
        }

        private void ramkaAkcji_ContentRendered(object sender, EventArgs e)
        {
           
        
        }

        private void btnStatystykiZwierzat_Click(object sender, RoutedEventArgs e)
        {
            ramkaAkcji.Content = new StatystykiZwierzat();
        }
    }
}

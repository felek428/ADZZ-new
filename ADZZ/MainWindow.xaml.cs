
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
        private void BtZarzadzaj_Click(object sender, RoutedEventArgs e)
        {
            ZarzadzanieZwierzetami przejscie = new ZarzadzanieZwierzetami(); //Tworze instancje okna od zarzadzania aby do niego przejsc
            przejscie.ShowDialog(); //Showdialog poniewaz blokuje mi mainwindow dzieki czemu nie mozna uzywac innego poza tym wlaczonym
        }

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
            ramkaAkcji.Content = new FormularzDodaniaZwierzecia();
        }

        private void ramkaAkcji_Navigated(object sender, NavigationEventArgs e)
        {
            ramkaAkcji.NavigationService.RemoveBackEntry();
        }

        private void BtKalendarz_Click(object sender, RoutedEventArgs e)
        {
            ramkaAkcji.Content = new NowyCalendar();
        }

        private void BtZnajdzZwierze_Click(object sender, RoutedEventArgs e)
        {
            ramkaAkcji.Content = new FormularzDodaniaZwierzecia();
        }

        private void btnDodajRozliczenie_Click(object sender, RoutedEventArgs e)
        {
            ramkaAkcji.Content = new FormularzDodaniaRozliczenia();
        }
    }
}

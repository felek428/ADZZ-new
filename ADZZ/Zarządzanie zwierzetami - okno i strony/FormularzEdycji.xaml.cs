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

namespace ADZZ.Zarządzanie_zwierzetami___okno_i_strony
{
    /// <summary>
    /// Logika interakcji dla klasy FormularzEdycji.xaml
    /// </summary>
    public partial class FormularzEdycji : Page
    {
        public FormularzEdycji()
        {
            InitializeComponent();
        }
        
        /// <summary>
        /// Wyswietla formularz dodania zwierzecia z uzupełnionymi danymi po podaniu kryterium jakim jest kod zwierzecia
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtSzukaj_Click(object sender, RoutedEventArgs e)
        {

            FormularzDodaniaZwierzecia obj = new FormularzDodaniaZwierzecia();
            obj.tbKolczyk.Text = "testttt";
            obj.tbKolczyk.IsEnabled = false;
            obj.btDodaj.Content = "Zmień";
            BtUsun.Visibility = Visibility.Visible;
            UzupelnionyFormularzEdycji.Content = obj;
            UzupelnionyFormularzEdycji.Visibility = Visibility.Visible;
        }
        /// <summary>
        /// Usuwa wpis z bazy 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtUsun_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Usunięto!");
            UzupelnionyFormularzEdycji.Visibility = Visibility.Hidden;
            BtUsun.Visibility = Visibility.Hidden;
        }

        /// <summary>
        /// Metoda, która czyści "History stack" dzieki czemu strony nie układają sie na stosie
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UzupelnionyFormularzEdycji_Navigated(object sender, NavigationEventArgs e) => UzupelnionyFormularzEdycji.NavigationService.RemoveBackEntry();
    }
}

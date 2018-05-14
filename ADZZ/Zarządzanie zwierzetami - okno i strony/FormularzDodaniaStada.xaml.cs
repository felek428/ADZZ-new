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
        public FormularzDodaniaStada()
        {
            InitializeComponent();
        }
        /// <summary>
        /// Dodaje wpis do bazy
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btDodaj_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Dodales stado!");
        }
    }
}

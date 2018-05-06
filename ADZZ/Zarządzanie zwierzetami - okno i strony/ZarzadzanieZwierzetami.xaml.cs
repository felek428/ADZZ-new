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
    /// Logika interakcji dla klasy ZarzadzanieZwierzetami.xaml
    /// </summary>
    public partial class ZarzadzanieZwierzetami : Window
    {
        public ZarzadzanieZwierzetami()
        {
            InitializeComponent();
            TypZwierzat.UstawListeTypow(WyborTypu);
           
        }
        /// <summary>
        /// Powrot do głównego okna
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnWroc_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}

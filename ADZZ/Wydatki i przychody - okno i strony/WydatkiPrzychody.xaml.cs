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

namespace ADZZ.Wydatki_i_przychody___okno_i_strony
{
    /// <summary>
    /// Logika interakcji dla klasy WydatkiPrzychody.xaml
    /// </summary>
    public partial class WydatkiPrzychody : Window
    {
        public WydatkiPrzychody()
        {
            InitializeComponent();
        }

        private void BtnWroc_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}

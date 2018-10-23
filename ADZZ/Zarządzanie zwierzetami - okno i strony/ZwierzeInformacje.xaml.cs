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
    /// Logika interakcji dla klasy ZwierzeInformacje.xaml
    /// </summary>
    public partial class ZwierzeInformacje : Page
    {
        private Frame ramkaAkcji;
        public ZwierzeInformacje()
        {
            InitializeComponent();
        }
        public ZwierzeInformacje(Frame ramka)
        {
            InitializeComponent();
            ramkaAkcji = ramka;
        }

        private void btnPowrot_Click(object sender, RoutedEventArgs e)
        {
            ramkaAkcji.Content = new ListaZwierzat(ramkaAkcji);
        }
    }
}

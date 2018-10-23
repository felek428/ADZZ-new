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
    /// Logika interakcji dla klasy ListaZwierzat.xaml
    /// </summary>
    public partial class ListaZwierzat : Page
    {
        private Frame ramkaAkcji;
        PolaczenieBazaDataContext Polaczenie = new PolaczenieBazaDataContext();
        public ListaZwierzat(Frame ramka)
        {
            InitializeComponent();
            var query = (from Zwierze in Polaczenie.Zwierze           
                         select Zwierze).ToList();
            Listview.ItemsSource = query;
            Datagrid.ItemsSource = query;
            ramkaAkcji = ramka;
        }

        public ListaZwierzat()
        {

        }

        private void Listview_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {

        }

        private void ListViewItem_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var item = sender as ListViewItem;
            if (item != null && item.IsSelected)
            {
                ramkaAkcji.Content = new ZwierzeInformacje(ramkaAkcji);
            }
        }
    }
}

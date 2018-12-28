using ADZZ.Rozliczenia___okno_i_strony;
using ADZZ.Statystyki___okno_i_strony;
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

namespace ADZZ
{
    /// <summary>
    /// Logika interakcji dla klasy WyborTypow.xaml
    /// </summary>
    public partial class WyborTypow : Page
    {
        Type nowyTyp;
        public WyborTypow(Type typWybranejStrony)
        {
            InitializeComponent();
            TypZwierzat noweTypy = new TypZwierzat();
            noweTypy.WypelnienieCBTypami(cbTypZwierzat);
            nowyTyp = typWybranejStrony;
            cbTypZwierzat.SelectedIndex = 0;
        }
        public WyborTypow()
        {
            InitializeComponent();
        }

        private void wyswietlStrone(Type typ)
        {
            if(typ == typeof(FormularzDodaniaZwierzecia))
            {
                if (cbTypZwierzat.SelectedIndex == 0)
                {
                    RamkaAkcjiTypy.Content = new FormularzDodaniaZwierzecia();

                }
                else if (cbTypZwierzat.SelectedIndex == 1)
                {
                    RamkaAkcjiTypy.Content = new FormularzDodaniaStada();
                }
            }
            else if(typ == typeof(ListaZwierzat))
            {
                RamkaAkcjiTypy.Content = new ListaZwierzat(RamkaAkcjiTypy, cbTypZwierzat.SelectedIndex);
            }
            else if(typ == typeof(FormularzDodaniaRozliczenia))
            {
                RamkaAkcjiTypy.Content = new FormularzDodaniaRozliczenia(cbTypZwierzat.SelectedIndex);
            }
            else if(typ == typeof(StatystykiZwierzat))
            {
                RamkaAkcjiTypy.Content = new StatystykiZwierzat(cbTypZwierzat.SelectedIndex);
            }
        }

        private void cbTypZwierzat_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            wyswietlStrone(nowyTyp);
        }

        private void RamkaAkcjiTypy_Navigated(object sender, NavigationEventArgs e)
        {
            RamkaAkcjiTypy.NavigationService.RemoveBackEntry();
        }
    }
}

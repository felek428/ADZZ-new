using System;
using System.Collections.Generic;
using System.Globalization;
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

namespace NewCalendar
{
    /// <summary>
    /// Logika interakcji dla klasy NowyCalendarButton.xaml
    /// </summary>
    public partial class NowyCalendarButton : UserControl
    {
        public string NrMiesiaca { get; set; }
        public NowyCalendarButton()
        {

            InitializeComponent();        
        }

        private void UserControl_MouseEnter(object sender, MouseEventArgs e)
        {
            Background = new SolidColorBrush(Colors.MediumAquamarine);
            Background.Opacity = 0.5;
        }

        private void UserControl_MouseLeave(object sender, MouseEventArgs e)
        {
            Background = new SolidColorBrush(Colors.SkyBlue);
            Background.Opacity = 1;
        }
    }
}

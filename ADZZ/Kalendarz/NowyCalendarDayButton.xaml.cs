using ADZZ.Kalendarz;
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

namespace NewCalendar
{
    /// <summary>
    /// Logika interakcji dla klasy NowyCalendarDayButton.xaml
    /// </summary>
    public partial class NowyCalendarDayButton : UserControl
    {
        public string ContentConnect { get; set; }

        public int DayNumber { get; private set; }
        public int ActualMonth { get; private set; }
        public int ActualYear { get; private set; }
        private NowyCalendarDayButton ClickedDay;

        public NowyCalendarDayButton()
        {
            
            InitializeComponent();

            LinearGradientBrush gradient = new LinearGradientBrush();

            gradient.StartPoint = new Point(0.5, 0);
            gradient.EndPoint = new Point(0.5, 1);

            gradient.GradientStops.Add(new GradientStop(Colors.AliceBlue, 0.0));
            gradient.GradientStops.Add(new GradientStop(Colors.LightSteelBlue, 1.0));

            Tekst.Background = gradient;

            DataContext = this;

        }
        public NowyCalendarDayButton(int day, int month, int year)
        {
            
            DayNumber = day;
            ActualMonth = month;
            ActualYear = year;
            InitializeComponent();

            LinearGradientBrush gradient = new LinearGradientBrush(); //Tworzenie gradientu dla DayButton'a

            gradient.StartPoint = new Point(0.5, 0);
            gradient.EndPoint = new Point(0.5, 1);

            gradient.GradientStops.Add(new GradientStop(Colors.AliceBlue, 0.0));
            gradient.GradientStops.Add(new GradientStop(Colors.LightSteelBlue, 1.0));

            Tekst.Background = gradient;

          
            DataContext = this;

        }
        /// <summary>
        /// Akcja po wybraniu opcji z menu contextowego
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            FormularzDodaniaNotatki obj = new FormularzDodaniaNotatki(ClickedDay);
            obj.ShowDialog();
            
            Console.WriteLine(obj.typNotatkiCB.SelectedItem);

        }
        private void LabelClick(object sender, MouseEventArgs et)
        {
            var zmienna = (sender as Label).Content;
            MessageBox.Show("PL132143141413431\n" + zmienna.ToString());
        }
        /// <summary>
        /// Tworzy menu contextowe po nacisnieciu RPM
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UserControl_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            ClickedDay = sender as NowyCalendarDayButton;
            ContextMenu contextMenu = new ContextMenu();
            MenuItem nowyLabel = new MenuItem();
            nowyLabel.Header = "Dodaj notke";
            nowyLabel.Click += new RoutedEventHandler(MenuItem_Click);
            contextMenu.Items.Add(nowyLabel);
            (sender as UserControl).ContextMenu = contextMenu;
        }
    }
}

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

            switch (day)
            {
                case 0:
                    Label note = new Label();
                    note.Name = "Ala";
                    note.Content = "Ruja";

                    note.ToolTip = note.Content;

                    note.MouseLeftButtonDown += new MouseButtonEventHandler(LabelClick);

                    Border borderNote = new Border();
                    borderNote.BorderBrush = new SolidColorBrush(Colors.SkyBlue);
                    borderNote.BorderThickness = new Thickness(1, 1, 1, 1);
                    borderNote.CornerRadius = new CornerRadius(20, 20, 20, 20);
                    borderNote.Background = new SolidColorBrush(Colors.AliceBlue);
                    borderNote.Child = note;

                    Label test = new Label();
                    test.Content = "ala";

                    Dok.Children.Add(borderNote);
                    DockPanel.SetDock(borderNote, Dock.Top);
                    break;
                default:
                    break;
            }
            DataContext = this;

        }
        /// <summary>
        /// Akcja po wybraniu opcji z menu contextowego
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            Label note = new Label();
            note.Name = "Ala";
            note.Content = "Ruja";
            
            note.ToolTip = note.Content;
            
            note.MouseLeftButtonDown += new MouseButtonEventHandler(LabelClick);

            Border noteBorder = new Border();
            noteBorder.BorderBrush = new SolidColorBrush(Colors.SkyBlue);
            noteBorder.BorderThickness = new Thickness(1, 1, 1, 1);
            noteBorder.CornerRadius = new CornerRadius(20, 20, 20, 20);
            noteBorder.Background = new SolidColorBrush(Colors.AliceBlue);
            noteBorder.Child = note;


            Label test = new Label();


            Dok.Children.Add(noteBorder);
            DockPanel.SetDock(noteBorder, Dock.Top);
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
            ContextMenu contextMenu = new ContextMenu();
            MenuItem nowyLabel = new MenuItem();
            nowyLabel.Header = "Dodaj notke";
            nowyLabel.Click += new RoutedEventHandler(MenuItem_Click);
            contextMenu.Items.Add(nowyLabel);
            (sender as UserControl).ContextMenu = contextMenu;
        }
    }
}

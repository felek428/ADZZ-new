using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.ComponentModel;
using NewCalendar;
using System.Windows.Media;
using System.Windows;
using System.Windows.Input;

namespace ADZZ
{
    class NotatkaKalendarza
    {
        PolaczenieBazaDataContext Polaczenie = new PolaczenieBazaDataContext();

        private NowyCalendarDayButton ClickedDay;
        private Border selectedBorder;
        private string kolczyk;
        public NotatkaKalendarza(NowyCalendarDayButton guzik)
        {
            ClickedDay = guzik;
        }

        /// <summary>
        /// Tworzy label na elemencie NowyCalendarDayButton
        /// </summary>
        /// <param name="typNotatki">String czy jest to ruja czy wycielenie</param>
        /// <param name="kolczyk">Kolczyk zwierzecia</param>
        public void CreateLabel(string typNotatki, string kolczyk)
        {
            Label note = new Label();
            note.Content = typNotatki;
            //note.MouseLeftButtonUp += new MouseButtonEventHandler(Label_MouseLeftButtonUp);
            //note.GotFocus += new RoutedEventHandler(Label_GotFocus);
            this.kolczyk = kolczyk;
            note.ToolTip = note.Content + "\n" + kolczyk;

            note.Focusable = true;
            Border noteBorder = new Border();
            noteBorder.BorderBrush = new SolidColorBrush(Colors.SkyBlue);
            noteBorder.BorderThickness = new Thickness(1, 1, 1, 1);
            noteBorder.CornerRadius = new CornerRadius(20, 20, 20, 20);
            noteBorder.Background = new SolidColorBrush(Colors.AliceBlue);
            noteBorder.Child = note;
            noteBorder.Focusable = true;
            noteBorder.MouseLeftButtonUp += new MouseButtonEventHandler(Label_MouseLeftButtonUp);
            noteBorder.MouseRightButtonUp += new MouseButtonEventHandler(Border_MouseRightButtonUp);
            noteBorder.GotFocus += new RoutedEventHandler(Label_GotFocus);
            noteBorder.LostFocus += new RoutedEventHandler(Border_LostFocus);
            ClickedDay.Dok.Children.Add(noteBorder);
            DockPanel.SetDock(noteBorder, Dock.Top);
        }

        private void Label_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if(e.ClickCount == 1 && (sender as Border).IsFocused == false)
            {

                (sender as Border).Focus();
                
                
            }
            if(e.ClickCount == 1 && (sender as Border).IsFocused == true)
            {
                Keyboard.ClearFocus();
            }
        }
        private void Label_GotFocus(object sender, RoutedEventArgs e)
        {
            (sender as Border).Background = new SolidColorBrush(Colors.LightBlue);
            selectedBorder = (sender as Border);        
        }
        private void Border_LostFocus(object sender, RoutedEventArgs e)
        {
            (sender as Border).Background = new SolidColorBrush(Colors.AliceBlue);
            (sender as Border).ContextMenu = null;
            
            selectedBorder = null;

        }
        private void Border_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            if((sender as Border).IsFocused == true)
            {            
                ContextMenu contextMenu = new ContextMenu();
                MenuItem nowyLabel = new MenuItem();
                nowyLabel.Header = "Usuń";
                nowyLabel.Click += new RoutedEventHandler(MenuItem_Click);
                contextMenu.Items.Add(nowyLabel);
                (sender as Border).ContextMenu = contextMenu;
            }
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            ClickedDay.Dok.Children.Remove(selectedBorder as UIElement);
            
            Rozrod usunNotke = Polaczenie.Rozrod.Single(x => x.Zwierze.nr_kolczyka == kolczyk && x.Data == Convert.ToDateTime(ClickedDay.DayNumber + "." + ClickedDay.ActualMonth + "." +ClickedDay.ActualYear ));

            Polaczenie.Rozrod.DeleteOnSubmit(usunNotke);
            Polaczenie.SubmitChanges();

        }

    }
}

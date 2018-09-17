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

namespace ADZZ
{
    class NotatkaKalendarza
    {
        private NowyCalendarDayButton ClickedDay;

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

            note.ToolTip = note.Content + "\n" + kolczyk;

            Border noteBorder = new Border();
            noteBorder.BorderBrush = new SolidColorBrush(Colors.SkyBlue);
            noteBorder.BorderThickness = new Thickness(1, 1, 1, 1);
            noteBorder.CornerRadius = new CornerRadius(20, 20, 20, 20);
            noteBorder.Background = new SolidColorBrush(Colors.AliceBlue);
            noteBorder.Child = note;

            ClickedDay.Dok.Children.Add(noteBorder);
            DockPanel.SetDock(noteBorder, Dock.Top);
        }
    }
}

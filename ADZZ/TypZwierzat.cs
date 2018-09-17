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
{   /// <summary>
/// Klasa ktora bedzie kontrolowala wybor zwierzat przy wprowadzaniu danych. Zawsze trzeba dokonać wyboru czy chcemy zarządzać pojedynczym zwierzeciem czy stadem
/// </summary>
    class TypZwierzat
    {
        enum Typy
        {
            Pojedyncze_zwierze = 1,
            Stado = 2,
        }

        public static int wybor { get; set; }  //Wybor bedzie 0, 1, 2 i w zaleznosci od dokonanego wyboru w comboboxie bedzie wystawietlana odpowiednia strona dla stada lub pojedynczego zwierzecia
        private NowyCalendarDayButton ClickedDay;
        static List<string> ListaTypow = new List<string>();
        /// <summary>
        /// Uzupelnia liste typow o stale typy zwierzat
        /// </summary>
        public static void UzupelnijTypy()
        {
            ListaTypow.Add("Pojedyncze zwierze");
            ListaTypow.Add("Stado");
        }
        public TypZwierzat(NowyCalendarDayButton guzik)
        {
            ClickedDay = guzik;
        }
        
        /// <summary>
        /// Ustawia zawartosc ComboBoxa
        /// </summary>
        /// <param name="ListaTypow">Kontrolka typu ComboBox</param>
        public static void UstawListeTypow(ComboBox ListaTypowBox)
        {

            ListaTypowBox.ItemsSource = ListaTypow;
            //ListaTypow.ItemsSource = Enum.GetValues(typeof(Typy));
        }

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

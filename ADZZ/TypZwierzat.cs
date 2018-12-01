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
            ListaTypow.Add("Pojedyńcze zwierzę");
            ListaTypow.Add("Stado");
        }
        public TypZwierzat()
        {

        }
        
        /// <summary>
        /// Ustawia zawartosc ComboBoxa
        /// </summary>
        /// <param name="ListaTypow">Kontrolka typu ComboBox</param>
        
        public void WypelnienieCBTypami(ComboBox cbTypy)
        {
            cbTypy.ItemsSource = ListaTypow;
        }
    }
}

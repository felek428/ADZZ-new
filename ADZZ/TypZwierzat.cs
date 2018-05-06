using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace ADZZ
{   /// <summary>
/// Klasa ktora bedzie kontrolowala wybor zwierzat przy wprowadzaniu danych. Zawsze trzeba dokonać wyboru czy chcemy zarządzać pojedynczym zwierzeciem czy stadem
/// </summary>
    class TypZwierzat
    {
        

        private int wybor;  //Wybor bedzie 0, 1, 2 i w zaleznosci od dokonanego wyboru w comboboxie bedzie wystawietlana odpowiednia strona dla stada lub pojedynczego zwierzecia

        static List<string> ListaTypow = new List<string>();
        /// <summary>
        /// Tworzy stałą liste typów zwierząt: stado, pojedyncze zwierze
        /// </summary>
        /// <returns></returns>
        public static List<string> ZwrocListeTypow()
        {
            ListaTypow.Add("Stado");
            ListaTypow.Add("Pojedyncze zwierze");
            return ListaTypow;
        }
        /// <summary>
        /// Ustawia zawartosc ComboBoxa
        /// </summary>
        /// <param name="ListaTypow">Kontrolka typu ComboBox</param>
        public static void UstawListeTypow(ComboBox ListaTypow)
        {
            ListaTypow.ItemsSource = ZwrocListeTypow();
        }
     
    }
}

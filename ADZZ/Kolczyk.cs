using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace ADZZ
{
    class Kolczyk
    {
        PolaczenieBazaDataContext Polaczenie = new PolaczenieBazaDataContext();

        public void walidacjaKolczyk(Control Kolczyk)
        {

            var TypKontrolki = Kolczyk.GetType();

            if(TypKontrolki == typeof(ComboBox))
            {
                WalidacjaCBkolczyk(Kolczyk);
            }
            else if(TypKontrolki == typeof(TextBox))
            {
                WalidacjaTBkolczyk(Kolczyk);
            }
        }

        private void WalidacjaCBkolczyk(Control Kontrolka)
        {
            var kolczyk = Kontrolka as ComboBox;
            var textBox = (kolczyk.Template.FindName("PART_EditableTextBox",
                       kolczyk) as TextBox);
            textBox.MaxLength = 14;             
            KontrolaWprowadzania(textBox);
            
        }
        private void WalidacjaTBkolczyk(Control Kontrolka)
        {
            var kolczyk = Kontrolka as TextBox;
            kolczyk.Select(kolczyk.Text.Length, 0);
            KontrolaWprowadzania(kolczyk);
        }
        /// <summary>
        /// Kontroluje wpisywany tekst i ustala format wpisywanego kolczyka
        /// </summary>
        /// <param name="kolczyk"></param>
        /// <param name="i"></param>
        private void KontrolaWprowadzania(TextBox kolczyk)
        {
            kolczyk.Text = kolczyk.Text.ToUpper();
            
            for (int i = 0; i < kolczyk.Text.Length; i++)
            {
                if (i == 0 && kolczyk.Text[i] != 'P')
                {

                    kolczyk.Text = kolczyk.Text.Remove(i, 1);
                    kolczyk.Focus();
                    kolczyk.Select(kolczyk.Text.Length, 0);

                }
                else if (i == 1 && kolczyk.Text[i] != 'L')
                {

                    kolczyk.Text = kolczyk.Text.Remove(i, 1);
                    kolczyk.Focus();
                    kolczyk.Select(kolczyk.Text.Length, 0);


                }
                else if (i > 1 && !Char.IsDigit(kolczyk.Text[i]))
                {

                    kolczyk.Text = kolczyk.Text.Remove(i, 1);
                    kolczyk.Focus();
                    kolczyk.Select(kolczyk.Text.Length, 0);

                }
            }
            
        }
        public void WypelnienieCbKolczykZwierze(ComboBox cbKolczyk)
        {
            var queryKolczyk = (from Zwierze in Polaczenie.Zwierze
                                select Zwierze.nr_kolczyka).ToList();
            cbKolczyk.ItemsSource = queryKolczyk;
        }
        public void WypelnienieCbKolczykStado(ComboBox cbKolczyk)
        {
            var queryKolczyk = (from Stado in Polaczenie.Stado
                                select Stado.nr_stada).ToList();
            cbKolczyk.ItemsSource = queryKolczyk;
        }
    }
}

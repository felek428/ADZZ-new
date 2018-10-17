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
            var Kolczyk = Kontrolka as ComboBox;
            
            for (int i = 0; i < Kolczyk.Text.Length; i++)
            {
                var textBox = (Kolczyk.Template.FindName("PART_EditableTextBox",
                       Kolczyk) as TextBox);
                textBox.MaxLength = 14;
                if (i == 0 && Kolczyk.Text[i] != 'P')
                {
                    
                    textBox.Text = Kolczyk.Text.Remove(i, 1);
                    textBox.Focus();
                    textBox.Select(textBox.Text.Length, 0);

                } else if (i == 1 && Kolczyk.Text[i] != 'L')
                {
                    
                    textBox.Text = Kolczyk.Text.Remove(i, 1);
                    textBox.Focus();
                    textBox.Select(textBox.Text.Length, 0);


                } else if (i > 1 && !Char.IsDigit(Kolczyk.Text[i]))
                {
                    
                    textBox.Text = Kolczyk.Text.Remove(i, 1);
                    textBox.Focus();
                    textBox.Select(textBox.Text.Length, 0);

                }
            }
        }
        private void WalidacjaTBkolczyk(Control Kontrolka)
        {
            var Kolczyk = Kontrolka as TextBox;
            for(int i = 0; i < Kolczyk.Text.Length; i++)
            {
                if (!Char.IsDigit(Kolczyk.Text[i]))
                {
                    
                    Kolczyk.Text = Kolczyk.Text.Remove(i, 1);
                    Kolczyk.Select(Kolczyk.Text.Length, 0);
                    break;
                }
            }
        }
    }
}

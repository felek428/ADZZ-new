using NewCalendar;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using System.Windows.Shapes;


namespace ADZZ.Kalendarz
{
    /// <summary>
    /// Logika interakcji dla klasy FormularzDodaniaNotatki.xaml
    /// </summary>
    public partial class FormularzDodaniaNotatki : Window
    {
        PolaczenieBazaDataContext Polaczenie = new PolaczenieBazaDataContext();
        private NowyCalendarDayButton objekt;
        public int MaxLenght { get; set; }
        public FormularzDodaniaNotatki(NowyCalendarDayButton ClickedDay)
        {

            InitializeComponent();
            objekt = ClickedDay;
            typNotatkiCB.Items.Add("Ruja");
            typNotatkiCB.Items.Add("Wycielenie");
            Kolczyk ZawartoscCbKolczyk = new Kolczyk();
            ZawartoscCbKolczyk.WypelnienieCbKolczykZwierze(cbKolczyk);

        }
        public FormularzDodaniaNotatki(NowyCalendarDayButton ClickedDay,string kappa)
        {


            objekt = ClickedDay;



        }
        /// <summary>
        /// Zatwierdznie notatki i dodanie jej do bazy
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnDodaj_Click(object sender, RoutedEventArgs e)
        {
            Console.WriteLine(cbKolczyk.SelectedItem);

            if (typNotatkiCB.SelectedItem != null && cbKolczyk.SelectedItem != null)
            {
                

                Rozrod NowaNotka = new Rozrod();
                
                var queryZwierze = (from Zwierze in Polaczenie.Zwierze
                                   where Zwierze.nr_kolczyka.Equals(cbKolczyk.SelectedItem)
                                   select Zwierze.Id).FirstOrDefault();
                if(queryZwierze == 0)
                {
                    MessageBox.Show("W bazie nie istnieje takie zwierze!");

                }
                else
                {
                    NotatkaKalendarza notka = new NotatkaKalendarza(objekt);
                    notka.CreateLabel(typNotatkiCB.SelectedItem.ToString(), cbKolczyk.SelectedItem.ToString());

                    this.Close();


                    var dataNotki = objekt.DayNumber + "." + objekt.ActualMonth + "." + objekt.ActualYear;

                    NowaNotka.Data = Convert.ToDateTime(dataNotki);
                    if (typNotatkiCB.SelectedItem.Equals("Ruja"))
                    {
                        NowaNotka.czyRuja = 1;
                    }
                    else
                    {
                        NowaNotka.czyRuja = 0;
                    }

                    NowaNotka.id_zwierze = queryZwierze;
                    Polaczenie.Rozrod.InsertOnSubmit(NowaNotka);
                    Polaczenie.SubmitChanges();
                }
                

                
            }
           else
            {
                MessageBox.Show("Uzupełnij wszystkie pola!");
            }
        }
        /// <summary>
        /// Sprawdza czy wprowadzona tresc jest cyfra
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbKolczyk_TextChanged(object sender, TextChangedEventArgs e)
        {
           
            Kolczyk kolczyk = new Kolczyk();
            kolczyk.walidacjaKolczyk(cbKolczyk);
                    
        }
        
        
    }
}

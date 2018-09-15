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
        public FormularzDodaniaNotatki(NowyCalendarDayButton sender)
        {

            InitializeComponent();
            objekt = sender;
            typNotatkiCB.Items.Add("Ruja");
            typNotatkiCB.Items.Add("Wycielenie");


        }
        /// <summary>
        /// Zatwierdznie notatki i dodanie jej do bazy
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnDodaj_Click(object sender, RoutedEventArgs e)
        {


            if (typNotatkiCB.SelectedItem != null && tbKolczyk.Text != "")
            {
                

                Rozrod NowaNotka = new Rozrod();
                
                var queryZwierze = (from Zwierze in Polaczenie.Zwierze
                                   where Zwierze.nr_kolczyka == "PL" + tbKolczyk.Text
                                   select Zwierze.Id).FirstOrDefault();
                if(queryZwierze == 0)
                {
                    MessageBox.Show("W bazie nie istnieje takie zwierze!");

                }
                else
                {
                    Label note = new Label();
                    note.Content = typNotatkiCB.SelectedItem;

                    note.ToolTip = note.Content;

                    //note.MouseLeftButtonDown += new MouseButtonEventHandler(LabelClick);

                    Border noteBorder = new Border();
                    noteBorder.BorderBrush = new SolidColorBrush(Colors.SkyBlue);
                    noteBorder.BorderThickness = new Thickness(1, 1, 1, 1);
                    noteBorder.CornerRadius = new CornerRadius(20, 20, 20, 20);
                    noteBorder.Background = new SolidColorBrush(Colors.AliceBlue);
                    noteBorder.Child = note;


                    Label test = new Label();


                    objekt.Dok.Children.Add(noteBorder);
                    DockPanel.SetDock(noteBorder, Dock.Top);
                    this.Close();


                    var dataNotki = objekt.DayNumber + "." + objekt.ActualMonth + "." + objekt.ActualYear;
                    NowaNotka.Data = Convert.ToDateTime(dataNotki);
                    if (typNotatkiCB.SelectedItem.Equals("Ruja"))
                    {
                        NowaNotka.czyRuja = 0;
                    }
                    else
                    {
                        NowaNotka.czyRuja = 1;
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
        private void tbKolczyk_TextChanged(object sender, TextChangedEventArgs e)
        {
            ObservableCollection<char> kolczyk = new ObservableCollection<char>();
            for(int i = 0; i < tbKolczyk.Text.Length; i++)
            {
                if (!Char.IsDigit(tbKolczyk.Text[i]))
                {

                    tbKolczyk.Text = tbKolczyk.Text.Remove(i, 1);
                    tbKolczyk.Focus();
                    tbKolczyk.Select(tbKolczyk.Text.Length, 0);
                    MessageBox.Show("Wpisany znak nie jest cyfrą!");
                    break;
                }
            }            
        }
    }
}

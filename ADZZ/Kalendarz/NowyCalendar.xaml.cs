﻿using ADZZ;
using ADZZ.Kalendarz;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Reflection;
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
    /// Logika interakcji dla klasy NowyCalendar.xaml
    /// </summary>
    public partial class NowyCalendar : UserControl, INotifyPropertyChanged
    {
        PolaczenieBazaDataContext Polaczenie = new PolaczenieBazaDataContext();
        /// <summary>
        /// Przechowuje nazwe miesiaca
        /// </summary>
        public string MonthName { get; set; }
        /// <summary>
        /// limit do tworzenia dni poprzedniego miesiaca oznaczony przez indeks kolumny w ktorym zostal oznaczony 1 dzien obecnego miesiaca
        /// </summary>
        private int previousDaysLimit;
        /// <summary>
        /// Indeks rzedu z ktorego ma zaczac dodawac dni kolejnego miesiaca;
        /// </summary>
        private int rowStart;
        /// <summary>
        /// Przechowuje numer stanu w jakim znajduje sie kalendarz
        /// </summary>
        private int states = 0;
        /// <summary>
        /// Obecnie wyswietlany miesiac w widoku
        /// </summary>
        public int actualMonth = DateTime.Now.Month;
        /// <summary>
        /// Obecnie wyswietlany rok w widkou
        /// </summary>
        //private int actualYear = DateTime.Now.Year;
        private int _actualYear;
        public int ActualYear
        {
            get { return _actualYear; }
            set
            {
                _actualYear = value;
                RaisePropertyChanged("actualYear");
            }
        }
        /// <summary>
        /// Przechowuje miesiac poprzedzajacy obecnie wyswietlany
        /// </summary>
        private int previousMonth;
        /// <summary>
        /// Przechowuje nastepny miesiac wzgledem obecnie wyswietanego
        /// </summary>
        private int nextMonth;
        /// <summary>
        /// Stala liczba "dzieci" w gridzie od wyswietlania dni
        /// </summary>
        private static int gridChildren = 42;
        /// <summary>
        /// Lewy kraniec przedzialu przy wyswietlaniu lat
        /// </summary>
        private int fullYearLeft;
        /// <summary>
        /// Prawy kraniec przedzialu przy wyswietlaniu lat
        /// </summary>
        private int fullYearRight;

        public event PropertyChangedEventHandler PropertyChanged;
        private void RaisePropertyChanged(string propName)
        {
            if(PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propName));
            }
        }
        public  int GetMonth
        {
            get { return actualMonth; }
        }

        public NowyCalendar()
        {        
            InitializeComponent();
            SetDefaultNextPreviousMonth();
            ActualYear = DateTime.Now.Year;
            MonthName = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(actualMonth);
            //MonthYear.Content = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(actualMonth);
            MonthYear.Content = char.ToUpper(CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(actualMonth)[0]) + CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(actualMonth).Substring(1);
            CreateDaysOfWeek();
            CreateCalendarDayButton(GetMonthDaysNumber(ActualYear,actualMonth),GetCurrentMonth());
            CreatePreviousMonthDays(GetLastMonthDaysNumber());
            CreateNextMonthDays();
             /*           
            NowyCalendarDayButton dm = new NowyCalendarDayButton();
            DataContext = dm;
            */
            DataContext = this;
        }

        private void SetDefaultNextPreviousMonth()
        {
            if(DateTime.Now.Month == 1)
            {
                previousMonth = 12;
            }
            else
            {
                previousMonth = (DateTime.Now.Month) - 1;
            }
            
            if(DateTime.Now.Month == 12)
            {
                nextMonth = 1;
            }
            else
            {
                nextMonth = (DateTime.Now.Month + 1);
            }
            
        }

        /// <summary>
        /// Akcje po nacisnieciu glownego buttona
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MonthYear_Click(object sender, RoutedEventArgs e)
        {
            
            switch (states)
            {
                case 0:
                    states = 1;
                    YearView.Children.Clear();
                    MonthView.Visibility = Visibility.Hidden;
                    YearView.Visibility = Visibility.Visible;
                    CreateCalendarButton();
                    MonthYear.Content = ActualYear;
                    DaysOfWeek.Visibility = Visibility.Hidden;
                    obramowanieRok.Visibility = Visibility.Hidden;
                    if (fullYearRight - DateTime.Now.Year >= 100 && fullYearRight == ActualYear)
                    {

                        NextButton.IsEnabled = false;

                    }
                    else
                    {
                        NextButton.IsEnabled = true;
                    }

                    if (fullYearLeft - DateTime.Now.Year <= 100 && fullYearLeft == ActualYear)
                    {

                        PreviousButton.IsEnabled = false;

                    }
                    else
                    {
                        PreviousButton.IsEnabled = true;
                    }


                    break;
                case 1:
                    if(fullYearRight - ActualYear <= 10 && (DateTime.Now.Year - ActualYear) <= -99)
                    {
                       
                        NextButton.IsEnabled = false;
                        
                    }

                    if (fullYearLeft - ActualYear == 0 && (DateTime.Now.Year - ActualYear) >= 99)
                    {

                        PreviousButton.IsEnabled = false;

                    }


                    YearView.Children.Clear();
                    CreateYearSection();
                    MonthYear.Content = (fullYearLeft.ToString() + "-" + fullYearRight.ToString());
                    states = 2;
                    CreateCalendarButton();
                    break;
                case 2:

                    break;
                default:
                    break;
            }
        }
        /// <summary>
        /// Tworzy objekt typu NowyCalendarButton na widoku miesiecy i lat
        /// </summary>
        private void CreateCalendarButton()
        {
            var yearBuffor = fullYearLeft;
            var monthNumber = 1; //Wykorzystywana przy uzupelnianiu contentu buttona poprzez nazwy miesiecy
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 4; j++)
                {               
                    NowyCalendarButton month = new NowyCalendarButton();

                    switch (states)
                    {
                        case 1:
                            var monthName = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(monthNumber);
                            var monthNameBuffor = monthName.Substring(1); // przechowuje nazwe miesiaca z pominieciem 1 znaku
                            monthName = monthName.Substring(0, 1).ToUpper(); // powieksza 1 litere miesiacca
                            month.Content = monthName + monthNameBuffor; // skleja 1 litere z reszta nazwy miesiaca i przypisuje do Contentu guzika
                            month.VerticalContentAlignment = VerticalAlignment.Center;
                            month.HorizontalContentAlignment = HorizontalAlignment.Center;
                            monthNumber += 1;
                            var zmienna = DateTime.ParseExact(month.Content.ToString().ToLower(), "MMMM", CultureInfo.CurrentCulture).Month;
                            break;
                        case 2:
                            month.VerticalContentAlignment = VerticalAlignment.Center;
                            month.HorizontalContentAlignment = HorizontalAlignment.Center;
                            CreateYearSection();
                            if (i == 0 && j == 0)
                            {
                                month.Content = fullYearLeft - 1;
                                month.Opacity = 0.5;
                                month.IsEnabled = false;
                            }
                            else if (i == 2 && j == 3)
                            {
                                month.Content = fullYearRight + 1;
                                month.Opacity = 0.5;
                                month.IsEnabled = false;
                            }
                            else if (i == 0 && j == 1)
                            {
                                month.Content = fullYearLeft;
                            }
                            else
                            {
                                yearBuffor += 1;
                                month.Content = yearBuffor;
                            }
                            break;
                        default:
                            break;
                    }
                    month.MouseLeftButtonDown += new MouseButtonEventHandler(ClickDayButton); //Przypisuje do nowego objektu metode
                    YearView.Children.Add(month);
                    Grid.SetColumn(month, j);
                    Grid.SetRow(month, i);
                }
            }
        }
        /// <summary>
        /// Tworzenie krancy przedzialu lat
        /// </summary>
        private void CreateYearSection()
        {
            var first2Numbers = ActualYear.ToString().Substring(0, 2);
            var last2Numbers = ActualYear.ToString().Substring(2, 1);
            last2Numbers = last2Numbers.Substring(0, 1) + 0;
            fullYearLeft = Convert.ToInt32(first2Numbers + last2Numbers); // lewy kraniec przedzialu
            last2Numbers = last2Numbers.Substring(0, 1) + 9;
            fullYearRight = Convert.ToInt32(first2Numbers + last2Numbers);
        }
        /// <summary>
        /// Akcje po nacisnieciu na guzik NowyCallendarButton. Akcja zależy od  stanu w jakim jest kalendarz
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ClickDayButton(object sender, MouseButtonEventArgs e)
        {
        
            if(sender is NowyCalendarButton && states == 1 )
            {
                YearView.Children.Clear();
                YearView.Visibility = Visibility.Hidden;
                MonthView.Visibility = Visibility.Visible;
                DaysOfWeek.Visibility = Visibility.Visible;
                obramowanieRok.Visibility = Visibility.Visible;

                actualMonth = DateTime.ParseExact((sender as NowyCalendarButton).Content.ToString().ToLower(), "MMMM", CultureInfo.CurrentCulture).Month; // pobiera miesiac po nacisnieciu guzika i nastepnie zamienia go na int
                previousMonth = actualMonth - 1;
                nextMonth = actualMonth + 1;
                CreateCalendarDayButton(GetMonthDaysNumber(ActualYear, actualMonth), actualMonth);
                
                if(actualMonth == 1)
                {
                    previousMonth = 12;
                }

                if(actualMonth == 12)
                {
                    nextMonth = 1;
                }

                if(fullYearRight == ActualYear && nextMonth > 1)
                {
                    NextButton.IsEnabled = true;
                }
                if (fullYearLeft == ActualYear && previousMonth < 12)
                {
                    PreviousButton.IsEnabled = true;
                }
                else if(fullYearLeft == ActualYear && previousMonth == 12)
                {
                    PreviousButton.IsEnabled = false;
                }


                CreatePreviousMonthDays(GetMonthDaysNumber(ActualYear, previousMonth));
                CreateNextMonthDays();
                MonthYear.Content = char.ToUpper(CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(actualMonth)[0]) + CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(actualMonth).Substring(1); // przypisanie na nowo nazwy obecnego miesiaca i wyswietlenie na buttonie
                states = 0;
            }
            else if (sender is NowyCalendarButton && states == 2)
            {
                YearView.Children.Clear();
                ActualYear = Convert.ToInt32((sender as NowyCalendarButton).Content);
                MonthYear.Content = ActualYear;
                states = 1;
                CreateCalendarButton();
                if (fullYearRight - DateTime.Now.Year == 100 && fullYearRight == ActualYear)
                {

                    NextButton.IsEnabled = false;

                }
                else
                {
                    NextButton.IsEnabled = true;
                }

                if (fullYearLeft - DateTime.Now.Year == -100 && fullYearLeft == ActualYear)
                {

                    PreviousButton.IsEnabled = false;

                }
                else
                {
                    PreviousButton.IsEnabled = true;
                }

            }
        }    
        /// <summary>
        /// Tworzy dni obecnego miesiaca w widoku
        /// </summary>
        /// <param name="daysNumber"></param>
        private void CreateCalendarDayButton(int daysNumber, int month)
        {
            MonthView.Children.Clear();
            var indexRow = 0;

            var queryRozrod = (from Rozrod in Polaczenie.Rozrod
                              where Rozrod.Data.Month == month && Rozrod.Data.Year == ActualYear
                              select Rozrod).ToList();
            
            for (int i = 0; i < daysNumber; i++)
            {
                var dayOfWeek = DateTime.Parse((ActualYear + "/" + month + "/" + (i+1).ToString()).ToString()).DayOfWeek.ToString();                                                                          //sprawdza jaki to dzien tygodnia
                
                var lista = DaysOfWeek.ColumnDefinitions.ToList();                                                                                                                                            // pobieram kolumny do listy 
                var indexCol = lista.IndexOf(DaysOfWeek.ColumnDefinitions.Where(c => c.Name == dayOfWeek).SingleOrDefault());                                                                                 //pobieram index kolumny podanego dnia
                
                if(i == 0)                                                                                                                     //pobieram numer kolumny w ktorej zostal wpisany 1 dzien podczas 1 iteracji
                {
                    previousDaysLimit = indexCol;
                }
                NowyCalendarDayButton day = new NowyCalendarDayButton(i+1,actualMonth,ActualYear);
                Border dayBorder = new Border();
                dayBorder.BorderThickness = new Thickness(1, 1, 1, 1);
                dayBorder.BorderBrush = new SolidColorBrush(Colors.SkyBlue);
     
                var dayList = queryRozrod.Where(x => x.Data.Day == i + 1).ToList();
                for(int j = 0; j < dayList.Count(); j++)
                {
                    
                    NotatkaKalendarza notka = new NotatkaKalendarza(day);
                    var typNotki = dayList[j].czyRuja;
                        
                    switch (typNotki)
                     {
                        case 0:
                            notka.CreateLabel("Ruja", dayList[j].Zwierze.nr_kolczyka);
                            break;
                        case 1:
                            notka.CreateLabel("Wycielenie", dayList[j].Zwierze.nr_kolczyka);
                            break;
                     }                  
                 }
                    
                dayBorder.Child = day;

                MonthView.Children.Add(dayBorder);
                Grid.SetColumn(dayBorder, indexCol);
                Grid.SetRow(dayBorder, indexRow);

                if (dayOfWeek.Equals("Sunday"))
                {
                    indexRow++;
                }
                rowStart = indexRow;
            }
        }
        /// <summary>
        /// tworzy nagłówek dla kolumn z  nazwami dni tygodnia
        /// </summary>
        private void CreateDaysOfWeek() 
        {
            for(int i = 0; i < 7; i++)
            {
                Thickness margines = new Thickness();
                margines.Left = 1;
                margines.Right = 1;
                margines.Top = 1;
                margines.Bottom = 1;

                TextBlock tbDay = new TextBlock();
                tbDay.VerticalAlignment = VerticalAlignment.Center;
                tbDay.FontSize = 12;
                tbDay.Foreground = new SolidColorBrush(Colors.Black);  
                tbDay.Text = DaysNameTranslate(DaysOfWeek.ColumnDefinitions[i].Name);
                tbDay.TextAlignment = TextAlignment.Center;
                DaysOfWeek.Children.Add(tbDay);
                Grid.SetColumn(tbDay, i);
            }
        }
        /// <summary>
        /// Tlumaczy dni tygodnia na polski jezyk
        /// </summary>
        /// <param name="dzienEng"> String z dniem tygodnia po ang</param>
        /// <returns></returns>
        private string DaysNameTranslate(string dzienEng) //tlumaczy angielskie nazwy dni tygodnia na polski
        {
            switch(dzienEng)
            {
                case "Monday":
                    return "Poniedziałek";
                case "Tuesday":
                    return "Wtorek";
                case "Wednesday":
                    return "Środa";
                case "Thursday":
                    return "Czwartek";
                case "Friday":
                    return "Piątek";
                case "Saturday":
                    return "Sobota";
                case "Sunday":
                    return "Niedziela";
                default:
                    return "0";

            }
            
        }
        /// <summary>
        /// Tworzy dni poprzedniego miesiaca w widoku obecnego
        /// </summary>
        private void CreatePreviousMonthDays(int monthDays)
        {
            var indexRow = 0;
            for (int i = previousDaysLimit; i > 0; i--)
            {
                     
                NowyCalendarDayButton day = new NowyCalendarDayButton(monthDays - (i - 1), 0, 0);
                
                day.Opacity = 0.2;
                day.IsEnabled = false;

                Border dayBorder = new Border();
                dayBorder.BorderThickness = new Thickness(1, 1, 1, 1);
                dayBorder.BorderBrush = new SolidColorBrush(Colors.SkyBlue);
                dayBorder.Child = day;

                MonthView.Children.Add(dayBorder);
                Grid.SetColumn(dayBorder, (previousDaysLimit-i));
                Grid.SetRow(dayBorder, indexRow);
            }
        }
        /// <summary>
        /// Tworzy dni kolejnego miesiaca w widoku obecnego
        /// </summary>
        private void CreateNextMonthDays()
        {
            var buffor = 0;
            var yearBuffor = ActualYear;
            var indexRow = rowStart;
            var MonthDays = Convert.ToInt32(DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month).ToString());                                       //pobiera liczbe dni wybranego miesiaca w roku
            for (int i = 0; i < gridChildren-(previousDaysLimit+GetMonthDaysNumber(ActualYear,actualMonth)); i++)                                            // 42 stala liczba "dzieci" w gridzie, dlatego wpisana statycznie
            {
                if (actualMonth == 12)
                {
                    buffor = 1;
                    yearBuffor = ActualYear + 1;
                }
                else
                {
                    buffor = actualMonth+1;
                }

                var dayOfWeek = DateTime.Parse((yearBuffor + "/" + buffor + "/" + (i + 1).ToString()).ToString()).DayOfWeek.ToString();                             //sprawdza jaki to dzien tygodnia
                var lista = DaysOfWeek.ColumnDefinitions.ToList();
                var indexCol = lista.IndexOf(DaysOfWeek.ColumnDefinitions.Where(c => c.Name == dayOfWeek).SingleOrDefault());                               //pobieram index kolumny podanego dnia
        
                NowyCalendarDayButton day = new NowyCalendarDayButton(i+1, 0, 0); 
                day.Opacity = 0.2;
                day.IsEnabled = false;

                Border dayBorder = new Border();
                dayBorder.BorderThickness = new Thickness(1, 1, 1, 1);
                dayBorder.BorderBrush = new SolidColorBrush(Colors.SkyBlue);
                dayBorder.Child = day;

                MonthView.Children.Add(dayBorder);
                Grid.SetColumn(dayBorder, indexCol);
                Grid.SetRow(dayBorder, indexRow);

                if (dayOfWeek.Equals("Sunday"))
                {
                    indexRow++;
                }
            }
        }
        /// <summary>
        /// Akcje wykonywane po nacisnieciu guzika poprzedni
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Previous_Click(object sender, RoutedEventArgs e)
        {
            if(states == 0)
            {
                if (actualMonth == 1)
                {
                    actualMonth = 12;
                    ActualYear -= 1;
                }
                else
                {
                    actualMonth -= 1;
                }

                if (previousMonth == 1)
                {
                    previousMonth = 12;
                    nextMonth -= 1;
                }else if(previousMonth == 11)
                {
                    previousMonth -= 1;
                    nextMonth = 12;
                }
                else
                {
                    previousMonth -= 1;
                    nextMonth -= 1;
                }
                MonthName = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(actualMonth);
                MonthYear.Content = char.ToUpper(CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(actualMonth)[0]) + CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(actualMonth).Substring(1); //pobiera nazwe miesiaca w jezyku jaki jest ustawiony na komputerze na podstawie int'a
                CreateCalendarDayButton(GetMonthDaysNumber(ActualYear, actualMonth), GetCurrentMonth());

                CreatePreviousMonthDays(GetMonthDaysNumber(ActualYear, previousMonth));
                CreateNextMonthDays();
            }
            else if (states == 1)
            {
                ActualYear -= 1;
                MonthYear.Content = ActualYear;
            }
            else if (states == 2)
            {
                ActualYear -= 10;
                YearView.Children.Clear();
                CreateYearSection();
                CreateCalendarButton();
                MonthYear.Content = (fullYearLeft.ToString() + "-" + fullYearRight.ToString());
                
            }
            
            if ((DateTime.Now.Year - ActualYear) > 99 && states == 2)
            {
                PreviousButton.IsEnabled = false;
            }
            else if ((DateTime.Now.Year - ActualYear) > 108 && states == 1)
            {
                PreviousButton.IsEnabled = false;
            }
            else if (previousMonth == 12 && states == 0 && (DateTime.Now.Year - ActualYear) > 108)
            {
                PreviousButton.IsEnabled = false;
            }
            else
            {
                NextButton.IsEnabled = true;
            }


        }
        /// <summary>
        /// Akcje po nacisnieciu guzika nastepny
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NextButton_Click(object sender, RoutedEventArgs e)
        {    
            if(states == 0)
            {
                if (actualMonth == 12)
                {
                    actualMonth = 1;
                    ActualYear += 1;
                }
                else
                {
                    actualMonth += 1;
                }

                if (nextMonth == 12)
                {
                    nextMonth = 1;
                    previousMonth += 1;
                }
                else if (nextMonth == 2)
                {
                    previousMonth = 1;
                    nextMonth += 1;
                }               
                else
                {
                    previousMonth += 1;
                    nextMonth += 1;
                }
                MonthYear.Content = char.ToUpper(CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(actualMonth)[0]) + CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(actualMonth).Substring(1);
                CreateCalendarDayButton(GetMonthDaysNumber(ActualYear, actualMonth), GetCurrentMonth());
                CreatePreviousMonthDays(GetMonthDaysNumber(ActualYear, previousMonth));
                CreateNextMonthDays();
            }else if(states == 1)
            {

                ActualYear += 1;
                MonthYear.Content = ActualYear;
            }
            else if(states == 2)
            {
                ActualYear += 10;
                YearView.Children.Clear();
                CreateYearSection();
                CreateCalendarButton();
                MonthYear.Content = (fullYearLeft.ToString() + "-" + fullYearRight.ToString());
                
                
            }

            if ((DateTime.Now.Year - ActualYear) <= -99 && states == 2)
            {
                NextButton.IsEnabled = false;
            }
            else if ((DateTime.Now.Year - ActualYear) < -99 && states == 1 )
            {
                NextButton.IsEnabled = false;
            }
            else if(nextMonth == 1 && states == 0 && (DateTime.Now.Year - ActualYear) <= -99)
            {
                NextButton.IsEnabled = false;
            }
            else
            {
                PreviousButton.IsEnabled = true;
            }
        }
        /// <summary>
        /// akcja po nacisnieciu przycisku z rokiem lub miesiacem
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CalendarDayButtonClick(object sender, RoutedEventArgs e)
        {
            if(sender is Button && states == 1)
            {
                YearView.Children.Clear();
                YearView.Visibility = Visibility.Hidden;
                MonthView.Visibility = Visibility.Visible;
                states = 0;
            }else if (sender is Button && states == 2)
            {
                states = 1;
                CreateCalendarButton();
                
            }
        }
        /// <summary>
        /// Pobiera liczbe dni podanego miesiaca
        /// </summary>
        /// <param name="year">Rok</param>
        /// <param name="month">Miesiac w ktorym pobieram liczbe dni</param>
        /// <returns></returns>
        private int GetMonthDaysNumber(int year, int month)
        {
            var daysNumber = DateTime.DaysInMonth(year, month);
            
            return daysNumber;
        }
        /// <summary>
        /// Pobiera liczbe dni poprzedniego miesiaca
        /// </summary>
        /// <returns></returns>
        private int GetLastMonthDaysNumber()
        {
            int buffor;            
            if(actualMonth == 1)
            {
                buffor = 12;
            }
            else
            {
                buffor = actualMonth - 1;
            }

            return GetMonthDaysNumber(ActualYear,buffor);
        }      
        private int GetCurrentMonth()
        {
            return actualMonth;
        }

        private void NextButton_IsEnabledChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            
        }
    }
}

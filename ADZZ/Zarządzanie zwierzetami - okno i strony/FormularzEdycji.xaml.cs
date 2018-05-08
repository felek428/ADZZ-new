﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ADZZ.Zarządzanie_zwierzetami___okno_i_strony
{
    /// <summary>
    /// Logika interakcji dla klasy FormularzEdycji.xaml
    /// </summary>
    public partial class FormularzEdycji : Page
    {
        public FormularzEdycji()
        {
            InitializeComponent();
        }
        
        
        private void BtSzukaj_Click(object sender, RoutedEventArgs e)
        {

            FormularzDodaniaZwierzecia obj = new FormularzDodaniaZwierzecia();
            obj.tbDataUr.Text = "test";
            obj.tbGatunek.Text = "testt";
            obj.tbKod.Text = "testttt";
            obj.tbKod.IsEnabled = false;
            obj.btDodaj.Content = "Zmień";
            BtUsun.Visibility = Visibility.Visible;
            UzupelnionyFormularzEdycji.Content = obj;
            UzupelnionyFormularzEdycji.Visibility = Visibility.Visible;
        }

        private void BtUsun_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Usunięto!");
            UzupelnionyFormularzEdycji.Visibility = Visibility.Hidden;
            BtUsun.Visibility = Visibility.Hidden;
        }
    }
}

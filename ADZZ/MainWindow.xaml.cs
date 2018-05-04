﻿using System;
using System.Collections.Generic;
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

namespace ADZZ
{
    /// <summary>
    /// Logika interakcji dla klasy MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        /// <summary>
        /// Zamykanie całego programu
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtExit_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
        /// <summary>
        /// Przechodzenie do okna zarzadzania zwierzetami
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtZarzadzaj_Click(object sender, RoutedEventArgs e)
        {
            
        }
    }
}

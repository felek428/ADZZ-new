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

namespace ADZZ.Zarządzanie_zwierzetami___okno_i_strony
{
    /// <summary>
    /// Logika interakcji dla klasy StadoInformacje.xaml
    /// </summary>
    public partial class StadoInformacje : Page
    {
        public StadoInformacje(string kolczyk)
        {
            InitializeComponent();
            ramkaInformacji.Content = new FormularzDodaniaStada(kolczyk);
            
        }
        public StadoInformacje()
        {
            InitializeComponent();
        }
    }
}

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
using BenBank2Data;

namespace BenBank2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            DataStore.LoadFromDb();

            InitializeComponent();

            foreach (FinancialEntity fe in DataStore.FinancialEntities)
            {
                StackPanel_FinancialEntities.Children.Add(new Controls.UserControl_FinancialEntity(fe));
            }
        }
    }
}

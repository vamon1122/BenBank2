using System;
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
using BenBank2.Controls;
using System.Diagnostics;

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
                ListBox_FinancialEntities.Items.Add(new Controls.UserControl_FinancialEntity(fe));
            }
        }

        private void Button_Pay_Click(object sender, RoutedEventArgs e)
        {
            double TextBoxAmmount;

            if (ValidateTextBoxAmmount())
            {
                foreach(UserControl_FinancialEntity control in ListBox_FinancialEntities.SelectedItems)
                {
                    Debug.WriteLine(string.Format("Financial entity \"{0}\" was selected. It has {1}", control.MyFinancialEntity.Name, control.MyFinancialEntity.Balance.ToString("£0.00")));
                }
            }

            bool ValidateTextBoxAmmount()
            {
                try
                {
                    TextBoxAmmount = Convert.ToDouble(TextBox_Ammount.Text);
                }
                catch
                {
                    return false;
                }
                return true;
            }
        }
    }
}

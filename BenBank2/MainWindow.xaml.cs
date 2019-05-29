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

            RefreshFinancialEntities();
            
        }

        private void RefreshFinancialEntities()
        {
            ListBox_Payers.Items.Clear();
            ListBox_Payees.Items.Clear();

            foreach (FinancialEntity fe in DataStore.FinancialEntities)
            {
                ListBox_Payers.Items.Add(new Controls.UserControl_FinancialEntity(fe));
            }

            foreach (FinancialEntity fe in DataStore.FinancialEntities)
            {
                ListBox_Payees.Items.Add(new Controls.UserControl_FinancialEntity(fe));
            }
        }

        private void Button_Pay_Click(object sender, RoutedEventArgs e)
        {
            double Ammount = 0;

            if (ValidateTextBoxAmmount())
            {
                int PayerNo = 0;
                foreach (UserControl_FinancialEntity payer in ListBox_Payers.SelectedItems)
                {
                    PayerNo++;
                    Debug.WriteLine(string.Format("Payer {0}/{1} \"{2}\" was selected. It has {3}",PayerNo, ListBox_Payers.SelectedItems.Count, payer.MyFinancialEntity.Name, payer.MyFinancialEntity.Balance.ToString("£0.00")));
                }

                int PayeeNo = 0;
                foreach (UserControl_FinancialEntity payee in ListBox_Payers.SelectedItems)
                {
                    PayeeNo++;
                    Debug.WriteLine(string.Format("Payee {0}/{1} \"{2}\" was selected. It has {3}", PayeeNo, ListBox_Payees.SelectedItems.Count, payee.MyFinancialEntity.Name, payee.MyFinancialEntity.Balance.ToString("£0.00")));
                }

                Debug.WriteLine(string.Format("Payee \"{0}\" was selected. It has {1}", ((UserControl_FinancialEntity)ListBox_Payees.SelectedItem).MyFinancialEntity.Name, ((UserControl_FinancialEntity)ListBox_Payees.SelectedItem).MyFinancialEntity.Balance.ToString("£0.00")));

                PayerNo = 0;
                PayeeNo = 0;
                foreach (UserControl_FinancialEntity payer in ListBox_Payers.SelectedItems)
                {
                    PayerNo++;
                    foreach (UserControl_FinancialEntity payee in ListBox_Payees.SelectedItems)
                    {
                        PayeeNo++;
                        Transaction transaction = new Transaction();
                        transaction.Sender = payer.MyFinancialEntity;
                        transaction.Recipient = payee.MyFinancialEntity;
                        transaction.Ammount = Ammount;
                        transaction.DoTransaction();
                        Debug.WriteLine(string.Format("Payer {0}/{1} \"{2}\" payed payee {3}/{4} \"{5}\" {6}. {2} now has {7}", PayerNo, ListBox_Payers.SelectedItems.Count, transaction.Sender.Name, PayeeNo, ListBox_Payees.SelectedItems.Count ,transaction.Recipient.Name, Ammount.ToString("£0.00"), transaction.Sender.Balance.ToString("£0.00")));
                    }
                    PayeeNo = 0;
                    
                }
                RefreshFinancialEntities();

            }

            bool ValidateTextBoxAmmount()
            {
                try
                {
                    Ammount = Convert.ToDouble(TextBox_Ammount.Text);
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

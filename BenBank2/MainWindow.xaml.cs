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
            Refresh(ListBox_Payers, ComboBox_PayerSort);
            Refresh(ListBox_Payees, ComboBox_PayeeSort);

            void Refresh(ListBox listOfFinancialEntities, ComboBox financialEntityGroup)
            {
                listOfFinancialEntities.Items.Clear();

                string group;

                if (financialEntityGroup.SelectedValue == null)
                    group = "";
                else
                    group = financialEntityGroup.SelectedValue.ToString().Split(':')[1].Trim();

                List<FinancialEntity> financialEntities = GetFinancialEntitiesByGroup(group);

                foreach (FinancialEntity financialEntity in financialEntities)
                {
                    listOfFinancialEntities.Items.Add(new UserControl_FinancialEntity(financialEntity));
                }
            }

            List<FinancialEntity> GetFinancialEntitiesByGroup(string payerGroup)
            {
                List<FinancialEntity> financialEntityGroup;

                switch (payerGroup)
                {
                    case "People":
                        Debug.WriteLine("People!");
                        //Derived class to base class:
                        //https://stackoverflow.com/questions/1817300/convert-listderivedclass-to-listbaseclass
                        financialEntityGroup = DataStore.People.ConvertAll(x => (FinancialEntity)x);
                        break;
                    case "Governments":
                        financialEntityGroup = DataStore.Governments.ConvertAll(x => (FinancialEntity)x);
                        break;
                    case "Businesses":
                        financialEntityGroup = DataStore.Businesses.ConvertAll(x => (FinancialEntity)x);
                        break;
                    case "Banks":
                        financialEntityGroup = DataStore.Banks.ConvertAll(x => (FinancialEntity)x);
                        break;
                    case "Bank Accounts":
                        financialEntityGroup = DataStore.BankAccounts.ConvertAll(x => (FinancialEntity)x);
                        break;
                    default:
                        financialEntityGroup = DataStore.FinancialEntities;
                        break;
                }

                return financialEntityGroup;
            }

            

            
        }

        
        private void ListBoxPayers_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            foreach(UserControl_FinancialEntity payer in ListBox_Payers.Items)
            {
                if (ListBox_Payers.SelectedItems.Contains(payer))
                    payer.Selected = true;
                else
                    payer.Selected = false;
            }
        }

        private void ListBoxPayees_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            foreach (UserControl_FinancialEntity payee in ListBox_Payees.Items)
            {
                if (ListBox_Payees.SelectedItems.Contains(payee))
                    payee.Selected = true;
                else
                    payee.Selected = false;
            }
        }

        //Fixes broken scroll wheel when hovering over ScrollViewer
        //Source: https://stackoverflow.com/questions/16234522/scrollviewer-mouse-wheel-not-working
        private void ScrollViewer_PreviewMouseWheel(object sender, System.Windows.Input.MouseWheelEventArgs e)
        {
            ScrollViewer scv = (ScrollViewer)sender;
            scv.ScrollToVerticalOffset(scv.VerticalOffset - e.Delta);
            e.Handled = true;
        }

        private void Button_Pay_Click(object sender, RoutedEventArgs e)
        {
            if (ListBox_Payers.SelectedItems.Count > 0)
            {
                if (ListBox_Payees.SelectedItems.Count > 0)
                {

                    double ammount = 0;

                    if (AmmountIsDouble())
                    {
                        int payerNo = 0;
                        foreach (UserControl_FinancialEntity payer in ListBox_Payers.SelectedItems)
                        {
                            payerNo++;
                            Debug.WriteLine(string.Format("Payer {0}/{1} \"{2}\" was selected. It has {3}", payerNo, ListBox_Payers.SelectedItems.Count, payer.MyFinancialEntity.Name, payer.MyFinancialEntity.Balance.ToString("£0.00")));
                        }

                        int payeeNo = 0;
                        foreach (UserControl_FinancialEntity payee in ListBox_Payers.SelectedItems)
                        {
                            payeeNo++;
                            Debug.WriteLine(string.Format("Payee {0}/{1} \"{2}\" was selected. It has {3}", payeeNo, ListBox_Payees.SelectedItems.Count, payee.MyFinancialEntity.Name, payee.MyFinancialEntity.Balance.ToString("£0.00")));
                        }

                        Debug.WriteLine(string.Format("Payee \"{0}\" was selected. It has {1}", ((UserControl_FinancialEntity)ListBox_Payees.SelectedItem).MyFinancialEntity.Name, ((UserControl_FinancialEntity)ListBox_Payees.SelectedItem).MyFinancialEntity.Balance.ToString("£0.00")));

                        payerNo = 0;
                        payeeNo = 0;
                        int noOfTransactions = ListBox_Payers.SelectedItems.Count * ListBox_Payees.SelectedItems.Count;
                        int transactionNo = 0;
                        foreach (UserControl_FinancialEntity payer in ListBox_Payers.SelectedItems)
                        {
                            payerNo++;
                            foreach (UserControl_FinancialEntity payee in ListBox_Payees.SelectedItems)
                            {
                                transactionNo++;
                                payeeNo++;
                                Transaction transaction = new Transaction();
                                transaction.Sender = payer.MyFinancialEntity;
                                transaction.Recipient = payee.MyFinancialEntity;
                                transaction.Ammount = ammount;

                                double senderStartingBalance = transaction.Sender.Balance;
                                double recipientStartingBalance = transaction.Recipient.Balance;

                                if((bool)CheckBox_ApplyVAT.IsChecked && (bool)CheckBox_ApplyIncomeTax.IsChecked)
                                {
                                    throw new InvalidOperationException();
                                }

                                if ((bool)CheckBox_ApplyVAT.IsChecked)
                                {
                                    transaction.ExecuteWithVAT();
                                }
                                else if ((bool)CheckBox_ApplyIncomeTax.IsChecked)
                                {
                                    transaction.ExecuteWithIncomeTax();
                                }
                                else
                                {
                                    transaction.Execute();
                                }
                                
                                Debug.WriteLine(string.Format("Transaction {0}/{1} - Payer {2}/{3} \"{4}\" payed payee {5}/{6} \"{7}\" {8}. {4} started with {9} but now has {10}. {7} started with {11} but now has {12}.", transactionNo, noOfTransactions, payerNo, ListBox_Payers.SelectedItems.Count, transaction.Sender.Name, payeeNo, ListBox_Payees.SelectedItems.Count, transaction.Recipient.Name, ammount.ToString("£0.00"), senderStartingBalance.ToString("£0.00"), transaction.Sender.Balance.ToString("£0.00"), recipientStartingBalance.ToString("£0.00"), transaction.Recipient.Balance.ToString("£0.00")));
                            }
                            payeeNo = 0;
                        }
                        RefreshFinancialEntities();
                    }
                    else
                    {
                        //Ammount is not a double
                        MessageBox.Show(string.Format("\"{0}\" is an invalid payment ammount!", TextBox_Ammount.Text), "Invalid Ammount");
                    }

                    bool AmmountIsDouble()
                    {
                        try
                        {
                            ammount = Convert.ToDouble(TextBox_Ammount.Text);
                        }
                        catch
                        {
                            return false;
                        }
                        return true;
                    }
                }
                else
                {
                    //No payee
                    MessageBox.Show("There is no payee selected!", "No Payee");
                }
            }
            else
            {
                //No payer
                MessageBox.Show("There is no payer selected!", "No Payer");
            }
        }

        private void ComboBox_PayerSort_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Debug.WriteLine("Payer sort 1 = " + ComboBox_PayerSort.Text);
            RefreshFinancialEntities();
        }

        private void ComboBox_PayeeSort_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            RefreshFinancialEntities();
        }
    }
}

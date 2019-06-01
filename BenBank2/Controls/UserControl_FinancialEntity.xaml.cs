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

namespace BenBank2.Controls
{
    /// <summary>
    /// Interaction logic for UserControl_FinancialEntity.xaml
    /// </summary>
    public partial class UserControl_FinancialEntity : UserControl
    {
        public FinancialEntity MyFinancialEntity;

        private bool _selected;
        public bool Selected
        {
            get
            {
                return _selected;
            }
            set
            {
                if (value == true)
                {
                    Background = Brushes.Yellow;
                }
                else
                {
                    Background = Brushes.Transparent;
                }
                
                _selected = value;
            }
        }

        public UserControl_FinancialEntity(FinancialEntity financialEntity)
        {
            MyFinancialEntity = financialEntity;

            InitializeComponent();

            FinancialEntity_Name.Text = financialEntity.Name;
            FinancialEntity_Balance.Text = financialEntity.Balance.ToString("£0.00");
        }
    }
}

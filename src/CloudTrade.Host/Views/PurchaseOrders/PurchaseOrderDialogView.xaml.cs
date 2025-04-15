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
using System.Windows.Shapes;

namespace CloudTrade.Host.Views.PurchaseOrders
{
    /// <summary>
    /// PurchaseOrderDialogView.xaml 的交互逻辑
    /// </summary>
    public partial class PurchaseOrderDialogView : Window
    {
        public PurchaseOrderDialogView()
        {
            InitializeComponent();
        }

        private void NumericUpDown_ValueChanged(object sender, HandyControl.Data.FunctionEventArgs<double> e)
        {
            MessageBox.Show("");
        }
    }
}

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

namespace CloudTrade.Host.Views.Mains
{
    /// <summary>
    /// ApplicationSettingView.xaml 的交互逻辑
    /// </summary>
    public partial class ApplicationSettingView : UserControl
    {
        public ApplicationSettingView()
        {
            InitializeComponent();
        }

        private void ColorPicker_Confirmed(object sender, HandyControl.Data.FunctionEventArgs<Color> e)
        {
            MessageBox.Show(e.Info.ToString());
        }

        private void ColorPicker_SelectedColorChanged(object sender, HandyControl.Data.FunctionEventArgs<Color> e)
        {
            //var color = e.Info;
            //MessageBox.Show(color.ToString());
        }

        private void ColorPicker_Canceled(object sender, EventArgs e)
        {
            MessageBox.Show("取消了");
        }
    }
}

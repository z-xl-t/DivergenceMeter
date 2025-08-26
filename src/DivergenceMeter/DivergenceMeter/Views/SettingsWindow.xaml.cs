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
using DivergenceMeter.ViewModels;

namespace DivergenceMeter.Views
{
    /// <summary>
    /// SettingsWindow.xaml 的交互逻辑
    /// </summary>
    public partial class SettingsWindow : Window
    {
        public SettingsWindow()
        {
            InitializeComponent();
            Closing += SettingsWindow_Closing;
        }
        private void SettingsWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            // 劫持关闭事件，修改为隐藏
            e.Cancel = true;
            var vm = this.DataContext as SettingsWindowViewModel;
            vm.SaveSettings();
            Hide();             
        }
    }
}

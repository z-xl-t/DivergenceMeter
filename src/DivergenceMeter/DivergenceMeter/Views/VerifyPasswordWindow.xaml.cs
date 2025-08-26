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
    /// VerifyPasswordWindow.xaml 的交互逻辑
    /// </summary>
    public partial class VerifyPasswordWindow : Window
    {
        public VerifyPasswordWindow()
        {
            InitializeComponent();
            Topmost = true;
            Top = 0;
            Left = 0;
            Width = SystemParameters.PrimaryScreenWidth;
            Height = SystemParameters.PrimaryScreenHeight;
        }

        protected override void OnSourceInitialized(EventArgs e)
        {
            base.OnSourceInitialized(e);
            var window = Window.GetWindow(this);
            var vm = this.DataContext as VerifyPasswordWindowViewModel;
            vm.HiddenTheTaskbar(window);
        }
    }
}

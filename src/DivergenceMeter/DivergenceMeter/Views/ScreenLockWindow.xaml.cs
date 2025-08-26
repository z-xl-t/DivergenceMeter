using System;
using System.Windows;
using DivergenceMeter.ViewModels;

namespace DivergenceMeter.Views
{
    /// <summary>
    /// ScreenLockWindow.xaml 的交互逻辑
    /// </summary>
    public partial class ScreenLockWindow : Window
    {
        public ScreenLockWindow()
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
            var vm = this.DataContext as ScreenLockWindowViewModel;
            vm.HiddenTheTaskBar(window);
        }
    }
}

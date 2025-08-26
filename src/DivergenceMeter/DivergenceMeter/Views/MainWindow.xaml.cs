using System;
using System.Windows;
using DivergenceMeter.ViewModels;

namespace DivergenceMeter.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        protected override void OnSourceInitialized(EventArgs e)
        {
            base.OnSourceInitialized(e);
            var window = Window.GetWindow(this);
            var vm = this.DataContext as MainWindowViewModel;
            vm.CreateWindowAndVMAfter(window);
        }
    }
}

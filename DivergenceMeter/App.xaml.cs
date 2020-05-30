using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Hardcodet.Wpf.TaskbarNotification;

namespace DivergenceMeter
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        // 覆写 OnStartup 函数
        protected override void OnStartup(StartupEventArgs e)
        {
            var taskbar = (TaskbarIcon) FindResource("Taskbar");
            base.OnStartup(e);
        }
        // 退出应用
        // 配合 app.xaml 中 ShutdownMode 属性使用
        private void ClickToExit(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
   
}
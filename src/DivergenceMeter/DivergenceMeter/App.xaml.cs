using System;
using System.Windows;
using DivergenceMeter.Helpers;
using DivergenceMeter.Models;
using DivergenceMeter.Views;
using Prism.Ioc;
using Prism.Unity;

namespace DivergenceMeter
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : PrismApplication
    {
        protected override Window CreateShell()
        {
            // 在 .NET 4.5 以上， TextBox 无法输入 浮动数 需要做如下修改
            FrameworkCompatibilityPreferences.KeepTextBoxDisplaySynchronizedWithTextProperty = false;
            var w = Container.Resolve<MainWindow>();
            return w;
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            // 全局单例注入
            containerRegistry.RegisterSingleton<Settings>(() => Settings.SettingsFactory());
            containerRegistry.RegisterSingleton<Helper>(() => new Helper());
        }
    }
}

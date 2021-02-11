using DivergenceMeter.Models;
using DivergenceMeter.Views;
using Prism.Ioc;
using Prism.Unity;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace DivergenceMeter
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : PrismApplication
    {
        protected override Window CreateShell()
        {
            var w = Container.Resolve<MainWindow>();
            return w;
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            // 全局单例注入
            // 把 设置 和 prism 都注入进去
            // 稍微改造一些 各种的 Viemodel 
            containerRegistry.RegisterSingleton<Settings>(() => new Settings());
            containerRegistry.RegisterSingleton<PrismApplication>(() => this);
        }
    }
}

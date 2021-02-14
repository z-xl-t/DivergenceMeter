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
            // 在 .NET 4.5 以上， TextBox 无法输入 浮动数 需要做如下修改
            FrameworkCompatibilityPreferences.KeepTextBoxDisplaySynchronizedWithTextProperty = false;
            var w = Container.Resolve<MainWindow>();
            return w;
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            // 全局单例注入
            // 把 设置 和 prism 都注入进去
            // 稍微改造一些 各种的 Viemodel 
            containerRegistry.RegisterSingleton<Settings>(() => SettingsFactory());
            containerRegistry.RegisterSingleton<PrismApplication>(() => this);



        }

        private Settings SettingsFactory()
        {
            var path = $@"{AppDomain.CurrentDomain.BaseDirectory}Settings.json";
            var settings = Settings.LoadSettings(path);

            if (settings != null)
            {
                return settings;
            }
            settings = new Settings();

            var width = settings.Width;
            var height = settings.Height;

            var workAreaWidth = SystemParameters.WorkArea.Width;
            var workAreaHeight = SystemParameters.WorkArea.Height;
            settings.Left = workAreaWidth / 2 - width / 2;
            settings.Top = workAreaHeight / 2 - height / 2;

            Settings.SaveSettings(path, settings);

            return settings;


        }
    }
}

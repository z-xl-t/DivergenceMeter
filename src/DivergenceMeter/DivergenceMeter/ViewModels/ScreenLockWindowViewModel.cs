using Prism.Commands;
using Prism.Ioc;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using DivergenceMeter.Helpers;
using DivergenceMeter.Views;
using DivergenceMeter.Models;
using DivergenceMeter.ViewModels;

namespace DivergenceMeter.ViewModels
{
    public class ScreenLockWindowViewModel : BindableBase
    {
        private readonly IContainerExtension _container;
        private readonly Helper _helper;
        private readonly Settings _settings;
        private Window _slw;
        private Window _vpw;
        private string password;
        public string Password
        {
            get { return password; }
            set { SetProperty(ref password, value); }
        }
        private string passwordTip;

        public string PasswordTip
        {
            get { return passwordTip; }
            set { SetProperty(ref passwordTip, value); }
        }

        public DelegateCommand OpenPasswordWindowCommand { get; set; }

        public ScreenLockWindowViewModel(IContainerExtension container)
        {
            _container = container;
            _helper = _container.Resolve<Helper>();
            _settings = _container.Resolve<Settings>();
            OpenPasswordWindowCommand = new DelegateCommand(OpenPasswordWindow);
        }

        private void OpenPasswordWindow()
        {
            _vpw.Show();
        }

        public void SetScreenLockAndVerifyPasswordWindowInSLW(Window slw, Window vpw)
        {
            _slw = slw;
            _vpw = vpw;
        }
        public void Start()
        {
            _slw.Show();
        }
        public void End()
        {
            if (string.IsNullOrWhiteSpace(Password) || string.IsNullOrEmpty(Password))
            {
                return;
            }
            if (Password.Trim() != _settings.Password.Trim())
            {
                Password = string.Empty;
                PasswordTip = "密码输入错误";
                return;
            }

            Password = string.Empty;
            PasswordTip = string.Empty;

            _slw.Hide();
        }


        public void HiddenTheTaskBar(Window window)
        {
            _helper.HiddenWindowTaskbar(window);
        }

    }
}

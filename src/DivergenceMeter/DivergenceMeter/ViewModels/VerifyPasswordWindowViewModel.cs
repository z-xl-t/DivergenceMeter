using System.Windows;
using DivergenceMeter.Helpers;
using DivergenceMeter.Models;
using Prism.Commands;
using Prism.Ioc;
using Prism.Mvvm;

namespace DivergenceMeter.ViewModels
{
    public class VerifyPasswordWindowViewModel: BindableBase
    {

        private Window _slw;
        private Window _vpw;

        private string _password;
        public string Password
        {
            get { return _password; }
            set { SetProperty(ref _password, value); }
        }

        private string _oldpassword;
        public string OldPassword
        {
            get { return _oldpassword; }
            set { SetProperty(ref _oldpassword, value); }
        }
        private string _newpassword;
        public string NewPassword
        {
            get { return _newpassword; }
            set { SetProperty(ref _newpassword, value); }
        }

        private string _newpassword2;
        public string NewPassword2
        {
            get { return _newpassword2; }
            set { SetProperty(ref _newpassword2, value); }
        }

        private string passwordTip;
        public string PasswordTip
        {
            get { return passwordTip; }
            set { SetProperty(ref passwordTip, value); }
        }



        private string _passwordChangeTip;
        public string PasswordChangeTip
        {
            get { return _passwordChangeTip; }
            set { SetProperty(ref _passwordChangeTip, value); }
        }

        private bool _showChangePassword;
        public bool ShowChangePassword
        {
            get { return _showChangePassword; }
            set { SetProperty(ref _showChangePassword, value); }
        }

        private readonly Settings _settings;
        private readonly Helper _helper;
        public DelegateCommand VerifyPasswordCommand { get; set; }
        public DelegateCommand CloseTheWindowCommand { get; set; }
        public DelegateCommand PreChangePasswordCommand { get; set; }
        public DelegateCommand ChangePasswordCommand { get; set; }
        public DelegateCommand UndoChangePasswordCommand { get; set; }
        public VerifyPasswordWindowViewModel(IContainerExtension container)
        {
            Password = string.Empty;
            PasswordTip = string.Empty;
            NewPassword = string.Empty;
            NewPassword2 = string.Empty;
            PasswordChangeTip = string.Empty;
            ShowChangePassword = false;
            _settings = container.Resolve<Settings>();
            _helper = container.Resolve<Helper>();
            VerifyPasswordCommand = new DelegateCommand(VerifyPassword);
            CloseTheWindowCommand = new DelegateCommand(CloseWindow);
            PreChangePasswordCommand = new DelegateCommand(PreChangePassword);
            ChangePasswordCommand = new DelegateCommand(ChangePassword);
            UndoChangePasswordCommand = new DelegateCommand(UndoChangePassword);
        }
        private void PreChangePassword()
        {
            ShowChangePassword = true;
        }
        private void UndoChangePassword()
        {
            NewPassword = string.Empty;
            NewPassword2 = string.Empty;
            PasswordChangeTip = string.Empty;
            ShowChangePassword = false;
        }
        private void ChangePassword()
        {
            if (string.IsNullOrWhiteSpace(OldPassword) || string.IsNullOrEmpty(OldPassword))
            {
                PasswordChangeTip = "旧密码为空，请输入";
                return;
            }

            if (OldPassword.Trim() != _settings.Password.Trim())
            {
                PasswordChangeTip = "旧密码错误";
                OldPassword = string.Empty;
                return;
            }
            if (NewPassword.Trim() != NewPassword2.Trim())
            {
                PasswordChangeTip = "两次密码输入不一致";
                return;
            }
            _settings.Password = NewPassword;
            Settings.SaveSettings(Settings.Default_File_Path, _settings);
            PasswordChangeTip = "密码修改成功";
        }
        private void CloseWindow()
        {
            ShowChangePassword = false;
            OldPassword = string.Empty;
            NewPassword = string.Empty;
            NewPassword2 = string.Empty;
            PasswordChangeTip = string.Empty;
            _vpw.Hide();
        }
        public void HiddenTheTaskbar(Window window)
        {
            _helper.HiddenWindowTaskbar(window);
        }
        private void VerifyPassword()
        {

            if (string.IsNullOrWhiteSpace(Password) || string.IsNullOrEmpty(Password))
            {

                PasswordTip = "密码为空，请输入";
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
            _vpw.Hide();
        }
        public void SetScreenLockAndVerifyPasswordWindowInVPW(Window slw, Window vpw)
        {
            _slw = slw;
            _vpw = vpw;
        }
    }
}

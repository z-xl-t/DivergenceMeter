using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using DivergenceMeter.Helpers;
using DivergenceMeter.Models;
using DivergenceMeter.Views;
using Prism.Commands;
using Prism.Ioc;
using Prism.Mvvm;

namespace DivergenceMeter.ViewModels
{

    // 窗体穿透效果 会 阻止双击和窗体拖拽
    // 需要借助 User32.dll 需要获取 Window Handle
    public class MainWindowViewModel: BindableBase
    {
        private readonly IContainerExtension _container;
        private readonly Helper _helper;
        private readonly SettingsWindow _sw;
        private readonly ScreenLockWindow _slw;
        private readonly VerifyPasswordWindow _vpw;
        private BitmapImage[] _allImages = new BitmapImage[13];
        private Window _mainWindow;
        private Settings _settings;
        public Settings Settings
        {
            get => _settings;
            set => SetProperty(ref _settings, value);
        }
        public ObservableCollection<BitmapImage> ClockImages { get; set; } = new ObservableCollection<BitmapImage>();
        private int[] _clockImagesIndex = new int[8];
        private Timer _clockTimer;
        private Timer _worldLineTimer;
        private int _maxWorldLineChangedCount = 50;
        private int _currentWorldLineChangedCount;
        private bool _lockWorldLine = false;

        public DelegateCommand<object> DragMoveCommand { get; set; }
        public DelegateCommand OpenSettingsWindowCommand { get; set; }
        public DelegateCommand LockWindowCommand { get; set; }
        public DelegateCommand ExitTheAppCommand { get; set; }
        public MainWindowViewModel(IContainerExtension container)
        {
            for (var i = 0; i < _allImages.Length; ++i)
            {
                _allImages[i] = new BitmapImage(new Uri($"/Images/{i}.png", UriKind.Relative));

            }
            var clockImages = new BitmapImage[8];
            for (int i = 0; i < _clockImagesIndex.Length; ++i)
            {
                _clockImagesIndex[i] = 0;
                clockImages[i] = _allImages[_clockImagesIndex[i]];
            }
            ClockImages.AddRange(clockImages);
            _container = container;
            Settings = _container.Resolve<Settings>();
            _helper = _container.Resolve<Helper>();

            _sw = _container.Resolve<SettingsWindow>();
            _slw = _container.Resolve<ScreenLockWindow>();
            _vpw = _container.Resolve<VerifyPasswordWindow>();
            var slw_vm = _slw.DataContext as ScreenLockWindowViewModel;
            var vpw_vm = _vpw.DataContext as VerifyPasswordWindowViewModel;
            slw_vm.SetScreenLockAndVerifyPasswordWindowInSLW(_slw, _vpw);
            vpw_vm.SetScreenLockAndVerifyPasswordWindowInVPW(_slw, _vpw);

            Settings.CanClickThroughChangedEvent += SetTheClickThrough;
            Settings.CanStartupChangedEvent += Helper.StartUpTheApp;
            DragMoveCommand = new DelegateCommand<object>(DragMove);
            OpenSettingsWindowCommand = new DelegateCommand(OpenSettingsWindow);
            LockWindowCommand = new DelegateCommand(LockWindow);
            ExitTheAppCommand = new DelegateCommand(ExitTheApp);
        }
        public void CreateWindowAndVMAfter(Window window)
        {
            _mainWindow = window;
            var task = new Action(async () =>
            {
                InitialClockTimer();
                InitialWorldLineTimer();
                StartWorldLineTimer();
                await Task.Delay(1000);
                StartClockTimer();
            });
            task.Invoke();
            SetTheClickThrough();
            HiddenWindowTaskbar();
            SetStartup();
        }

        private void LockWindow()
        {
            var slw_vm = _slw.DataContext as ScreenLockWindowViewModel;
            slw_vm.Start();
        }

        public void SetStartup()
        {
            Helper.StartUpTheApp(Settings.CanStartup);
        }

        private void ExitTheApp()
        {
            var path = $@"{AppDomain.CurrentDomain.BaseDirectory}Settings.json";
            Settings.SaveSettings(path, Settings);
            Application.Current.Shutdown();
        }

        private void OpenSettingsWindow()
        {
            _sw.Show();
        }

        public void SetTheClickThrough()
        {
            SetTheClickThrough(Settings.CanClickThrough);

        }
        private void SetTheClickThrough(bool canClickThrough)
        {
            if (canClickThrough)
            {
                _helper.ClickThrough(_mainWindow);
            }
            else
            {
               _helper.UnClickThrough(_mainWindow);
            }
        }
        #region 
        private void StartWorldLineTimer()
        {
            _worldLineTimer?.Start();
        }
        private void StopWorldLineTimer()
        {
            _worldLineTimer?.Stop();
        }
        private void TheWorldLine(object sender, ElapsedEventArgs e)
        {

            if (_currentWorldLineChangedCount > _maxWorldLineChangedCount && (_clockImagesIndex[0] == 0 || _clockImagesIndex[0] == 1))
            {
                // stop and reset
                StopWorldLineTimer();
                _currentWorldLineChangedCount = 0;
                _lockWorldLine = false;
                return;

            }

            _currentWorldLineChangedCount++;
            Random rd = new Random();
            _clockImagesIndex[0] = rd.Next(0, 9);
            _clockImagesIndex[1] = 11; // 不变
            _clockImagesIndex[2] = rd.Next(0, 9);
            _clockImagesIndex[3] = rd.Next(0, 9);
            _clockImagesIndex[4] = rd.Next(0, 9);
            _clockImagesIndex[5] = rd.Next(0, 9);
            _clockImagesIndex[6] = rd.Next(0, 9);
            _clockImagesIndex[7] = rd.Next(0, 9);

            for(var i=0; i<_clockImagesIndex.Length; ++i)
            {
                ClockImages[i] = _allImages[_clockImagesIndex[i]];
            }
        }

        private void InitialWorldLineTimer()
        {

            _worldLineTimer = new Timer();
            _worldLineTimer.Interval = 20;
            _worldLineTimer.Elapsed += TheWorldLine;
        }
        #endregion

        #region 动态时钟
        private void InitialClockTimer()
        {
            _clockTimer = new Timer();//  System.Timers
            _clockTimer.Interval = 100; // 毫秒
            _clockTimer.Elapsed += TheClock;
        }
        private void StartClockTimer()
        {
            _clockTimer?.Start();

        }
        private void StopClockTimer()
        {
            _clockTimer?.Stop();
        }

        private void TheClock(object sender, ElapsedEventArgs e)
        {
            var nowTime = DateTime.Now;
            var hour = nowTime.Hour;
            var min = nowTime.Minute;
            var sec = nowTime.Second;
            var millis = nowTime.Millisecond;

            _clockImagesIndex[0] = hour < 10 ? 0 : hour / 10;
            _clockImagesIndex[1] = hour < 10 ? hour : hour % 10;
            _clockImagesIndex[2] = millis < 500 ? 11 : 12;
            _clockImagesIndex[3] = min < 10 ? 0 : min / 10;
            _clockImagesIndex[4] = min < 10 ? min : min % 10;
            _clockImagesIndex[5] = millis < 500 ? 11 : 12;
            _clockImagesIndex[6] = sec < 10 ? 0 : sec / 10;
            _clockImagesIndex[7] = sec < 10 ? sec : sec % 10;

            for (var i=0;  i<_clockImagesIndex.Length; ++i)
            {
                ClockImages[i] = _allImages[_clockImagesIndex[i]];
            }
        }
        #endregion


        private async void DragMove(object obj)
        {
            var e = obj as MouseButtonEventArgs;
            if (e != null && e.LeftButton == MouseButtonState.Pressed && Settings.CanDragMove)
            {
                _mainWindow.DragMove(); 

                if (Settings.CanAttachEdge)
                {
                    var workArea = SystemParameters.WorkArea;
                    if (Settings.Left - 10 < 0)
                    {
                        Settings.Left = 0;
                    }
                    else if (Settings.Left + Settings.Width - 10 > workArea.Width)
                    {
                        Settings.Left = workArea.Width - Settings.Width;
                    }

                    if (Settings.Top - 10 < 0)
                    {
                        Settings.Top = 0;
                    }
                    else if(Settings.Top + Settings.Height - 10 > workArea.Height)
                    {
                        Settings.Top = workArea.Height - Settings.Height;
                    }
                }

            }

            if (e.ClickCount == 2)
            {
                // 双击效果

                if (_lockWorldLine == true)
                {
                    return;
                } else
                {
                    _lockWorldLine = true;
                }
                StopClockTimer();
                await Task.Delay(1000);
                StartWorldLineTimer();
                await Task.Delay(2000);
                StartClockTimer();
            }
        }
        public void HiddenWindowTaskbar()
        {
            _helper.HiddenWindowTaskbar(_mainWindow);
        }
    }
}

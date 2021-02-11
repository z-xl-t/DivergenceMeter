using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using DivergenceMeter.Models;
using Prism.Commands;
using Prism.Mvvm;
using PInvoke;
using Prism.Unity;
using DivergenceMeter.Views;

namespace DivergenceMeter.ViewModels
{
    public class MainWindowViewModel: BindableBase
    {
        private PrismApplication _app;
        // 加载所有图片
        private BitmapImage[] _allImages = new BitmapImage[13];
        private Window _mainWindow = Application.Current.MainWindow;
        private IntPtr _mainWindowHandle = IntPtr.Zero;
        // Settings
        private Settings _settings;
        public Settings Settings
        {
            get => _settings;
            set => SetProperty(ref _settings, value);
        }
        // 动态时钟
        // 与 xaml 关联的图片
        public ObservableCollection<BitmapImage> ClockImages { get; set; } = new ObservableCollection<BitmapImage>();
        private int[] _clockImagesIndex = new int[8];
        private Timer _clockTimer;
        private Timer _worldLineTimer;
        private int _maxWorldLineChangedCount = 50;
        private int _currentWorldLineChangedCount;
        // DelegateCommand
        public DelegateCommand<object> DragMoveCommand { get; set; }
        public DelegateCommand OpenSettingsWindowCommand { get; set; }
        public DelegateCommand ExitTheAppCommand { get; set; }
        public MainWindowViewModel(PrismApplication app, Settings settings)
        {
            _app = app;
            Settings = settings;

            IninialClockImages();

            DragMoveCommand = new DelegateCommand<object>(DragMove);
            OpenSettingsWindowCommand = new DelegateCommand(OpenSettingsWindow);
            ExitTheAppCommand = new DelegateCommand(ExitTheApp);

            var task = new Action(async () =>
            {
                // 时钟效果
                InitialClockTimer();

                // 世界线效果
                InitialWorldLineTimer();

                StartWorldLineTimer();
                await Task.Delay(1000);
                StartClockTimer();
            });
            task.Invoke();

            // 窗体穿透效果 会 阻止 双击 和 窗体拖拽

            // 需要借助 User32.dll 需要获取 Window Handle

        }

        private void ExitTheApp()
        {
            Application.Current.Shutdown();
        }

        private void OpenSettingsWindow()
        {
            // 开启设置窗口
            // 这里可以选择直接创建窗体，绑定 ViewModel， 也能够交由 Prism 托管

            // 选择使用 Prism

            var sw = (SettingsWindow)_app.Container.Resolve(typeof(SettingsWindow));
            sw.ShowDialog();
        }

        public void SetTheClickThrough(IntPtr handle)
        {
            if (handle == IntPtr.Zero) return;
            _mainWindowHandle = handle;
            SetTheClickThrough();

        }
        private void SetTheClickThrough()
        {
            if (_mainWindowHandle == IntPtr.Zero) return;
            // 函数重载
            if (Settings.CanClickThrough)
            {
                // 设置穿透效果
                User32.SetWindowLong(_mainWindowHandle, User32.WindowLongIndexFlags.GWL_EXSTYLE, User32.SetWindowLongFlags.WS_EX_TRANSPARENT);
            }
            else
            {
                // 回复到正常效果
                User32.SetWindowLong(_mainWindowHandle, User32.WindowLongIndexFlags.GWL_EXSTYLE, User32.SetWindowLongFlags.WS_EX_LAYERED);
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
        private void IninialClockImages()
        {
            _allImages = LoadAllImage();
            var clockImages = new BitmapImage[8];
            for(int i=0; i<_clockImagesIndex.Length; ++i)
            {
                _clockImagesIndex[i] = 0;
                clockImages[i] = _allImages[_clockImagesIndex[i]];
            }
            ClockImages.AddRange(clockImages);
        }

        private async void DragMove(object obj)
        {
            var e = obj as MouseButtonEventArgs;
            if (e != null && e.LeftButton == MouseButtonState.Pressed && Settings.CanDragMove)
            {
                _mainWindow.DragMove(); 
            }

            if (e.ClickCount == 2)
            {
                // 双击效果
                StopClockTimer();
                await Task.Delay(1000);
                StartWorldLineTimer();
                await Task.Delay(2000);
                StartClockTimer();
            }
        }

        private BitmapImage[] LoadAllImage()
        {
            var allImages = new BitmapImage[13];
            for(var i=0; i<allImages.Length; ++i)
            {
                allImages[i] = new BitmapImage(new Uri($"/Images/{i}.png", UriKind.Relative));
            }
            
            return allImages;
        }


        public void HiddenWindowTaskbar(IntPtr handle)
        {
            // 隐藏任务栏图标 和 Alt + Tab 时的窗口

            int exStyle = User32.GetWindowLong(handle, User32.WindowLongIndexFlags.GWL_EXSTYLE);
            exStyle = exStyle | (int)User32.SetWindowLongFlags.WS_EX_TOOLWINDOW;
            User32.SetWindowLong(handle, User32.WindowLongIndexFlags.GWL_EXSTYLE, (User32.SetWindowLongFlags)exStyle);

        }

    }
}

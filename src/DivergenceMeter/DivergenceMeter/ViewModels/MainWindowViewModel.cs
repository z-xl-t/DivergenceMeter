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

namespace DivergenceMeter.ViewModels
{
    public class MainWindowViewModel: BindableBase
    {
        // 加载所有图片
        private BitmapImage[] _allImages = new BitmapImage[13];
        private Window _mainWindow = Application.Current.MainWindow;

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
        // DelegateCommand
        public DelegateCommand<object> DragMoveCommand { get; set; }

        public MainWindowViewModel()
        {
            Settings = new Settings();
            IninialClockImages();

            DragMoveCommand = new DelegateCommand<object>(DragMove);
            _clockTimer = new Timer();//  System.Timers
            _clockTimer.Interval = 100; // 毫秒
            _clockTimer.Elapsed += TheClock;
            _clockTimer.Start();

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

        private void DragMove(object obj)
        {
            var e = obj as MouseButtonEventArgs;
            if (e != null && e.LeftButton == MouseButtonState.Pressed)
            {
                _mainWindow.DragMove(); 
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
    }
}

using System;
using System.IO;
using System.Text.Json;
using System.Windows;
using Prism.Mvvm;

namespace DivergenceMeter.Models
{
    public class Settings: BindableBase
    {
        public static string Default_File_Path = $@"{AppDomain.CurrentDomain.BaseDirectory}Settings.json";
        // 优化 Settings 类
        private double _radio = 8.0 / 3;
        private bool _canLockRadio;
        public bool CanLockRadio
        {
            get => _canLockRadio;
            set => SetProperty(ref _canLockRadio, value);
        }
        private double _left;
        public double Left
        {
            get => _left;
            set => SetProperty(ref _left, value);
        }
        private double _top;
        public double Top
        {
            get => _top;
            set => SetProperty(ref _top, value);
        }

        private bool _ifWidthChanged;
        private double _width;
        public  double Width
        {
            get => _width;
            set
            {
                _ifWidthChanged = true;
                SetProperty(ref _width, value);
                OnWidthChanged(value);
                _ifWidthChanged = false;
            }
        }
        private bool _ifHeightChanged;
        private double _height;
        public double Height
        {
            get => _height;
            set
            {
                _ifHeightChanged = true;
                SetProperty(ref _height, value);
                OnHeightChanged(value);
                _ifHeightChanged = false;
            }
        }

        private double _opacity;
        public double Opacity
        {
            get => _opacity;
            set
            {
                if (value < 0)
                {
                    SetProperty(ref _opacity, 0);
                } 
                else if (value > 1)
                {
                    SetProperty(ref _opacity, 1);
                }
                else
                {
                    SetProperty(ref _opacity, value);
                }
            }
        }
        private bool _canTopmost;
        public bool CanTopmost
        {
            get => _canTopmost;
            set => SetProperty(ref _canTopmost, value);
        }
        // 无需与 xmal 元素关联
        public bool CanDragMove { get; set; }
        public bool CanAttachEdge { get; set; }
        // CanClickThrough 需要一个事件
        public event Action<bool> CanClickThroughChangedEvent;
        private bool _canClickThrough;
        public bool CanClickThrough
        {
            get => _canClickThrough;
            set
            {
                SetProperty(ref _canClickThrough, value);
                CanClickThroughChangedEvent?.Invoke(value);
            }
        }
        public event Action<bool> CanStartupChangedEvent;
        private bool _canStartup;
        public bool CanStartup
        {
            get => _canStartup;
            set
            {
                SetProperty(ref _canStartup, value);
                CanStartupChangedEvent?.Invoke(value);
            }
        }
        private string _password;
        public string Password { 
            get => _password;
            set { 
                SetProperty(ref _password, value);
            } 
        }

        public Settings()
        {
            // 八张图片，一张图片宽高比为 1：3，最终为 8：3
            CanLockRadio = true;
            Width = 288;
            Opacity = 1;
            CanDragMove = true;
            CanTopmost = true;
            CanAttachEdge = true;
            Password = "123456789";
        }

        private void OnWidthChanged(double width)
        {
            // 进行检测， 使用 _ifWidthChanged 和 _ifHeightChanged 来阻止循环变化

            if (CanLockRadio && !_ifHeightChanged)
            {
                Height = width / _radio;
            }
        }
        private void OnHeightChanged(double height)
        {
            if (CanLockRadio && !_ifWidthChanged)
            {
                Width = Height * _radio;
            }
        }

        public static bool SaveSettings(string path, Settings settings)
        {
            var options = new JsonSerializerOptions
            {
                WriteIndented = true
            };

            var json = JsonSerializer.Serialize(settings, options);
            File.WriteAllText(path, json);

            return true;
        }

        public static Settings LoadSettings(string path)
        {
            if (!File.Exists(path))
            {
                return null;
            }
            var jsonString = File.ReadAllText(path);

            var options = new JsonSerializerOptions
            {
                AllowTrailingCommas = true,
                ReadCommentHandling = JsonCommentHandling.Skip
            };
            var settings = JsonSerializer.Deserialize<Settings>(jsonString, options);
            return settings;


        }

        public static Settings SettingsFactory()
        {
            var settings = Settings.LoadSettings(Default_File_Path);

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

            Settings.SaveSettings(Default_File_Path, settings);

            return settings;


        }
    }
}

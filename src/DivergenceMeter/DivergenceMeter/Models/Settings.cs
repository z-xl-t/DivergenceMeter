using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prism.Mvvm;

namespace DivergenceMeter.Models
{
    public class Settings: BindableBase
    {
        private double _width;
        public  double Width
        {
            get => _width;
            set => SetProperty(ref _width, value);
        }
        private double _height;
        public double Height
        {
            get => _height;
            set => SetProperty(ref _height, value);
        }

        private double _opacity;
        public double Opacity
        {
            get => _opacity;
            set => SetProperty(ref _opacity, value);
        }
        private bool _canTopmost;
        public bool CanTopmost
        {
            get => _canTopmost;
            set => SetProperty(ref _canTopmost, value);
        }
        // 无需与 xmal 元素关联
        public bool CanDragMove { get; set; }
        public bool CanClickThrough { get;set; }
        public Settings()
        {
            // 八张图片，一张图片宽高比为 1：3，最终为 8：3
            Width = 800;
            Height = 300;
            Opacity = 1;
            CanDragMove = true;
        }
    }
}

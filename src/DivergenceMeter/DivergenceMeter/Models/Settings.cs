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
        public Settings()
        {
            // 八张图片，一张图片宽高比为 1：3，最终为 8：3
            Width = 800;
            Height = 300;
        }
    }
}

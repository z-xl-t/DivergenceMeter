using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using Prism.Mvvm;

namespace DivergenceMeter.ViewModels
{
    public class MainWindowViewModel: BindableBase
    {
        // 加载所有图片
        private BitmapImage[] _allImages = new BitmapImage[13];

        // 与 xaml 关联的图片
        public ObservableCollection<BitmapImage> ClockImages { get; set; } = new ObservableCollection<BitmapImage>();

        public MainWindowViewModel()
        {
            _allImages = LoadAllImage();

            // test
            ClockImages.Add(_allImages[0]);
            ClockImages.Add(_allImages[0]);
            ClockImages.Add(_allImages[0]);
            ClockImages.Add(_allImages[0]);
            ClockImages.Add(_allImages[0]);
            ClockImages.Add(_allImages[0]);
            ClockImages.Add(_allImages[0]);
            ClockImages.Add(_allImages[0]);
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

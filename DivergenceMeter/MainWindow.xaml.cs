using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
namespace DivergenceMeter
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    
    public partial class MainWindow : Window
    {
        private int WindowsInitWidth = 1440;
        private int windowsInitHeight = 460;
        private double scale = 0.2;
        private DateTime _nowTime;
        private DispatcherTimer _meterClock;
        private List<BitmapImage> AllImages = new List<BitmapImage>();
        #region 主函数
        public MainWindow()
        {
            InitializeComponent();
            this.Width = WindowsInitWidth * scale;
            this.Height = windowsInitHeight * scale;
            LoadAllImage(ref AllImages);
            StartClock();
        }
        #endregion
        #region 窗体拖动事件
        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            
            this.DragMove();
        }
        #endregion
        #region 开启计时器
        private void StartClock()
        {
            if (_meterClock == null)
            {
                _meterClock = new DispatcherTimer();
                _meterClock.Tick += new EventHandler(Clock_Tick);
                _meterClock.Interval = new TimeSpan(0,0,0, 0, 500);
                _meterClock.Start(); 
            }

        }
        #endregion
        #region 计时器事件函数
        private  void Clock_Tick(object sender, EventArgs e)
        {
            _nowTime = DateTime.Now;
            var hour = _nowTime.Hour;
            var minute = _nowTime.Minute;
            var second = _nowTime.Second;
            var millisecond = _nowTime.Millisecond;
            var aa = hour < 10 ? 0 : hour / 10;
            var bb = hour < 10 ? hour : hour % 10;
            var cc = millisecond < 500 ? 11 : 12;
            var dd = minute < 10 ? 0 : minute / 10;
            var ee = minute < 10 ? minute : minute % 10;
            var ff = millisecond < 500 ? 11 : 12;
            var gg = second < 10 ? 0 :second / 10;
            var hh = second < 10 ? second : second % 10;

            A.Source = AllImages[aa];
            B.Source = AllImages[bb];
            C.Source = AllImages[cc];
            D.Source = AllImages[dd];
            E.Source = AllImages[ee];
            F.Source = AllImages[ff];
            G.Source = AllImages[gg];
            H.Source = AllImages[hh];
        }

        #endregion
        #region 加载所有图片
        private void LoadAllImage( ref List<BitmapImage> images)
        {
            int[] num = new int[] {0, 1, 2, 3, 4, 5,6,7,8,9,10,11,12};

            foreach (var item in num)
            {
                String imagePath = @$"/images/{item}.png";
                BitmapImage bi = new BitmapImage();
                bi.BeginInit();
                bi.UriSource = new Uri(imagePath,UriKind.RelativeOrAbsolute);
                bi.EndInit();
                images.Add(bi); 
            }

            
        }

        #endregion
        
    }
}
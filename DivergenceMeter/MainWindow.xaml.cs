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
        private DateTime _nowTime;
        private DispatcherTimer _meterClock;
        public MainWindow()
        {
            InitializeComponent();
            StartClock();
        }

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

        #region 计时器事件函数
        private  void Clock_Tick(object sender, EventArgs e)
        {
            _nowTime = DateTime.Now;
            var hour = _nowTime.Hour;
            var minute = _nowTime.Minute;
            var second = _nowTime.Second;
            var millisecond = _nowTime.Millisecond;
            var aa = hour < 10 ? "0" : Convert.ToString(hour / 10);
            var bb = hour < 10 ? Convert.ToString(hour) : Convert.ToString(hour % 10);
            var cc = millisecond < 500 ? "left" : "right";
            var dd = minute < 10 ? "0" : Convert.ToString(minute / 10);
            var ee = minute < 10 ? Convert.ToString(minute) : Convert.ToString(minute % 10);
            var ff = millisecond < 500 ? "left" : "right";
            var gg = second < 10 ? "0" : Convert.ToString(second / 10);
            var hh = second < 10 ? Convert.ToString(second) : Convert.ToString(second % 10);
            
            A.Text = Convert.ToString(aa);
            B.Text = Convert.ToString(bb);
            C.Text = Convert.ToString(cc);
            D.Text = Convert.ToString(dd);
            E.Text = Convert.ToString(ee);
            F.Text = Convert.ToString(ff);
            G.Text = Convert.ToString(gg);
            H.Text = Convert.ToString(hh);
        }

        #endregion
       
        
    }
}
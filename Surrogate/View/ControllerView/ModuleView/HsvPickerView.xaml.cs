using Emgu.CV;
using Emgu.CV.Structure;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Surrogate.View.ControllerView.ModuleView
{
    /// <summary>
    /// Interaction logic for HsvPickerView.xaml
    /// </summary>
    public partial class HsvPickerView : ModuleViewBase
    {
        private int h_Upper = 255;
        private int h_Lower = 0;
        private int s_Upper = 255;
        private int s_Lower = 0;
        private int v_Upper = 255;
        private int v_Lower = 0;

        private readonly VideoCapture _capturer;
        private readonly DispatcherTimer _timer;
        private readonly Window _parent;

        public event EventHandler<HsvBounds> SaveClicked;

        public HsvPickerView(Window parent, int camId = 0)
        {
            _parent = parent;
            _capturer = new VideoCapture(camId);
            
            _timer = new DispatcherTimer();
            _timer.Tick += UpdateImage;
            _timer.Interval = new TimeSpan(0, 0, 0, 0, 10);
            InitializeComponent();
            _timer.Start();
            _parent.Closing += new CancelEventHandler(OnClosing);
        }

        private void UpdateImage(object sender, EventArgs e)
        {
            using (Image<Hsv, Byte> imageFrame = _capturer.QueryFrame().ToImage<Hsv, Byte>())
            {
                Image<Gray, Byte> mask = imageFrame.InRange(new Hsv(h_Lower, s_Lower, v_Lower), new Hsv(h_Upper, s_Upper, v_Upper));
                Mat filtered = new Mat();
                CvInvoke.BitwiseAnd(imageFrame, imageFrame, filtered, mask: mask);

                var pimage = filtered.Clone(); // clone image because we are going to run async on gui thread
                Application
                    .Current
                    .Dispatcher
                    .BeginInvoke(DispatcherPriority.Normal, new Action(() =>
                    imView.Source = Utils.UI.BitmapSourceConvert.ToBitmapSource(pimage)));
            }
        }



        
        private  void OnClosing(object sender, CancelEventArgs e)
        {
            _timer.Stop();
            _capturer.Dispose();
            System.Diagnostics.Debug.WriteLine("HsvView closed");
        }

        /// <summary>
        /// Invokes the SaveClicked EventHandler and closes the window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Save_Clicked(object sender, RoutedEventArgs e)
        {
            SaveClicked?.Invoke(this, new HsvBounds(new Hsv(h_Lower, s_Lower, v_Lower), new Hsv(h_Upper, s_Upper, v_Upper)));
            _parent.Close();
        }

        private void SHUpper_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            h_Upper = (int)e.NewValue;
        }

        private void SHLower_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            h_Lower = (int)e.NewValue;
        }

        private void SVUpper_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            v_Upper = (int)e.NewValue;
        }

        private void SSUpper_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            s_Upper = (int)e.NewValue;
        }

        private void SVLower_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            s_Lower = (int)e.NewValue;
        }

        private void SSLower_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            s_Lower = (int)e.NewValue;
        }
    }

    public class HsvBounds : EventArgs
    {

        private readonly Hsv _lower;
        private readonly Hsv _upper;

        public Hsv Lower { get => _lower; }
        public Hsv Upper { get => _upper; }

        public HsvBounds(Hsv lower, Hsv upper)
        {
            _lower = lower;
            _upper = upper;
        }
    }
}

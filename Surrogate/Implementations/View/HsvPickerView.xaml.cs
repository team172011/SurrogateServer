// This file belongs to the source code of the "Surrogate Project"
// Copyright (c) 2018 All Rights Reserved
// Martin-Luther-Universitaet Halle-Wittenberg
// Lehrstuhl Wirtschaftsinformatik und Operation Research
// Autor: Wimmer, Simon-Justus Wimmer (simonjustuswimmer@googlemail.com)
using Emgu.CV;
using Emgu.CV.Structure;
using Surrogate.Implementations.Controller.Module;
using Surrogate.Implementations.Model;
using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace Surrogate.View
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

        private volatile bool inverted = false;

        private readonly VideoCapture _capturer;
        private readonly DispatcherTimer _timer;
        private readonly Window _parent;

        public event EventHandler<HsvBounds> SaveClicked;

        public HsvPickerView(Window parent, IHsvProperties props = null)
        {
            _parent = parent;
            _capturer = new VideoCapture(props.CamNum);
            
            _timer = new DispatcherTimer();
            _timer.Tick += UpdateImage;
            _timer.Interval = new TimeSpan(0, 0, 0, 0, 10);
            InitializeComponent();
            InitBounds(props);
            _timer.Start();
            _parent.Closing += new CancelEventHandler(OnClosing);
        }

        private void InitBounds(IHsvProperties props)
        {
            if (props == null) return;

            Hsv lower = props.Lower;
            Hsv upper = props.Upper;

            sHLower.Value = lower.Hue;
            sHUpper.Value = upper.Hue;

            sVLower.Value = lower.Value;
            sVUpper.Value = upper.Value;

            sSLower.Value = lower.Satuation;
            sSUpper.Value = upper.Satuation;

            cbInverted.IsChecked = props.Inverted;
        }

        private void UpdateImage(object sender, EventArgs e)
        {
            using (Image<Hsv, Byte> imageFrame = _capturer.QueryFrame().ToImage<Hsv, Byte>())
            {
                Image<Gray, Byte> mask = imageFrame.InRange(new Hsv(h_Lower, s_Lower, v_Lower), new Hsv(h_Upper, s_Upper, v_Upper));
                Mat filtered = new Mat();
                //System.Diagnostics.Debug.WriteLine(new Hsv(h_Lower, s_Lower, v_Lower) + " " + new Hsv(h_Upper, s_Upper, v_Upper) + inverted);
                if (inverted)
                {
                    mask = mask.Not();
                }
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
            SaveClicked?.Invoke(this, new HsvBounds(new Hsv(h_Lower, s_Lower, v_Lower), new Hsv(h_Upper, s_Upper, v_Upper), inverted));
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
            v_Lower = (int)e.NewValue;
        }

        private void SSLower_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            s_Lower = (int)e.NewValue;
        }

        private void InvertUpdated(object sender, RoutedEventArgs e)
        {
            var box = sender as CheckBox;
            inverted = (bool) box.IsChecked;
            System.Diagnostics.Debug.WriteLine(inverted);
        }
    }
}

using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using Surrogate.Implementations.Model;
using Surrogate.Model;
using Surrogate.Model.Module;
using Surrogate.Modules;
using Surrogate.View;
using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace Surrogate.Implementations.Controller.Module
{
    public class BallFollowingModule : VisualModule<BallFollowingProperties, BallFollowingInfo>
    {
        private Image _currentImage;
        private VideoCapture _currentVideoCapture;
        private BackgroundWorker _worker;

        public BallFollowingModule() : base(new BallFollowingProperties())
        {
        }

        public override IModuleProperties Properties => GetProperties();

        public void BallFollowing(object sender, EventArgs e)
        {
            log.Debug(String.Format("Using lower: {0} and upper: {1} as hsv space", GetProperties().Lower, GetProperties().Upper));
            while (!ShouldStop)
            {
                using (Image<Bgr, byte> imageFrame = _currentVideoCapture.QueryFrame().ToImage<Bgr, Byte>())
                {

                    if (imageFrame != null)
                    {
                        Image<Bgr, Byte> original = imageFrame.Clone();
                        Image<Gray, Byte> mask = imageFrame.Convert<Hsv, Byte>().InRange(GetProperties().Lower, GetProperties().Upper);
                        if (GetProperties().Inverted)
                        {
                            mask = mask.Not();
                        }
                        Mat filtered = new Mat();
                        CvInvoke.BitwiseAnd(imageFrame, imageFrame, filtered, mask: mask); // filter image by specific upper and lower hsv space values

                        var smoothed = filtered.ToImage<Gray, Byte>().SmoothGaussian(5);
                        var eroded = smoothed.Erode(8);
                        var dilated = eroded.Dilate(8);

                        VectorOfVectorOfPoint contours = new VectorOfVectorOfPoint();

                        CvInvoke.FindContours(dilated, contours, null, RetrType.List, ChainApproxMethod.ChainApproxSimple);
                        if (contours.Size > 0)
                        {
                            var biggestContour = contours[0];
                            var biggestContourSize = CvInvoke.ContourArea(biggestContour);
                            for (int i = 1; i < contours.Size; i++) // find the biggest contour
                            {
                                var sizeCurrent = CvInvoke.ContourArea(contours[i]);
                                if (sizeCurrent > biggestContourSize)
                                {
                                    biggestContour = contours[i];
                                    biggestContourSize = sizeCurrent;
                                }

                            }

                            //Rectangle rec = CvInvoke.BoundingRectangle(biggestContour);
                            //RotatedRect rrec = CvInvoke.MinAreaRect(biggestContour);
                            CircleF rrec = CvInvoke.MinEnclosingCircle(biggestContour);
                            //original.Draw(rec, new Bgr(0, 0, 255), 2);
                            original.Draw(rrec, new Bgr(0, 255, 0), 2);
                            original.Draw(new CircleF(rrec.Center, 2), new Bgr(0, 255, 255), 2);
                            original.Draw(biggestContour.ToArray(), new Bgr(255, 0, 0), 2);

                            double x = rrec.Center.X;
                            double width = imageFrame.Width;
                            double leftMin = width * 0.33;
                            double rightMax = width * 0.66;

                            if (x < leftMin)
                            {
                                log.Debug("nach rechts");
                            }
                            else if (x > rightMax)
                            {
                                log.Debug("nach links");
                            }
                            else
                            {
                                log.Debug("Gerade aus");
                            }
                        }
                        else
                        {
                            log.Debug("Anhalten/Suchen");
                        }

                        PublishFrame(1, imageFrame, "Original Input");
                        PublishFrame(2, filtered.ToImage<Bgr, Byte>(), "Filtered");
                        PublishFrame(3, smoothed.Convert<Bgr, Byte>(), "Smoothed");
                        PublishFrame(4, dilated.Convert<Bgr, Byte>(), "Eroded & Delitated");
                        PublishFrame(5, original, "Result");
                    }

                }
            }
        }

        public override UserControl GetPage()
        {
            return new BallFollowingView(this);
        }

        /// <summary>
        /// Publishs the Image on the _view
        /// </summary>
        /// <param name="imageFrame"></param>
        /// <param name="viewNum"></param>
        private void PublishFrame(int viewNum, Image<Hsv, byte> imageFrame, string name = "")
        {
            try
            {
                var pimage = imageFrame.Clone(); // clone image because we are going to run async on gui thread
                pimage.Draw(fontScale: 2, message: viewNum.ToString() + " " + name, bottomLeft: new System.Drawing.Point(1, 50), fontFace: FontFace.HersheyComplex, color: new Hsv(135, 99, 100), thickness: 2);
                Application
                    .Current
                    .Dispatcher
                    .BeginInvoke(DispatcherPriority.Normal, new Action(() =>
                    _currentImage.Source = Utils.UI.BitmapSourceConvert.ToBitmapSource(pimage)));

            }
            catch (CvException cve)
            {
                log.Error("Fehler beim veröffentlichen eines Frames: " + cve.Message);
            }
        }

        /// <summary>
        /// Publishs the Image on the _view
        /// </summary>
        /// <param name="imageFrame"></param>
        /// <param name="viewNum"></param>
        private void PublishFrame(int viewNum, Image<Bgr, byte> imageFrame, string name = "")
        {
            try
            {
                var pimage = imageFrame.Clone(); // clone image because we are going to run async on gui thread
                pimage.Draw(fontScale: 2, message: viewNum.ToString() + " " + name, bottomLeft: new System.Drawing.Point(1, 50), fontFace: FontFace.HersheyComplex, color: new Bgr(0, 255, 0), thickness: 2);
                Application
                    .Current
                    .Dispatcher
                    .BeginInvoke(DispatcherPriority.Normal, new Action(() =>
                    _currentImage.Source = Utils.UI.BitmapSourceConvert.ToBitmapSource(pimage)));

            }
            catch (CvException cve)
            {
                log.Error("Fehler beim veröffentlichen eines Frames: " + cve.Message);
            }
        }


        public override bool IsRunning()
        {
            return _worker != null && _worker.IsBusy;
        }

        public override void OnDisselected()
        {
            base.OnDisselected();
        }

        public override void OnSelected()
        {
            base.OnSelected();
        }

        public override void Start(BallFollowingInfo info)
        {
            _currentImage = info.Image;
            ShouldStop = false;
            if (IsRunning()) return;
            _currentVideoCapture = new VideoCapture(GetProperties().CamNum);
            _worker = new BackgroundWorker();
            _worker.DoWork += BallFollowing;
            _worker.RunWorkerAsync();
        }

        public override void Stop()
        {
            base.Stop();
            _currentVideoCapture?.Dispose();
        }

        /// <summary>
        /// Updates the upper and lower bounds of the used hsv space
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        internal void UpdateHsvSpace(object sender, HsvBounds e)
        {
            log.Debug("Hsv Farbraum eingegrenzt:" + e.Lower.ToString() + e.Upper.ToString());
            GetProperties().SetBounds(e);
        }
    }

    public class BallFollowingProperties : HsvModulProperties
    {

        public BallFollowingProperties() : base("Ball folgen", "Nutzt eine Kamera, um einem speziellen Ball zu folgen", motor: true, floorCam: true)
        {
            SetProperty(KeyImagePath, @"C:\Users\ITM1\source\repos\Surrogate\Surrogate\resources\ballFollowing_controller_icon.png");
            Load();

            // set default values if no exist
            SetProperty(Key_H_Lower, 0, false);
            SetProperty(Key_S_Lower, 0, false);
            SetProperty(Key_V_Lower, 0, false);

            SetProperty(Key_H_Upper, 255, false);
            SetProperty(Key_S_Upper, 255, false);
            SetProperty(Key_V_Upper, 255, false);

            SetProperty(Key_Inverted, false, false);
            SetProperty(Key_CamNum, CamNum, false);
            Save();
        }
    }

    public class BallFollowingInfo : ModuleInfo
    {
        public Image Image { get; set; }
        
        public BallFollowingInfo(Image image)
        {
            Image = image;
        }
    }
}

using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using Surrogate.Implementations.Model;
using Surrogate.Model;
using Surrogate.Model.Module;
using Surrogate.Modules;
using Surrogate.Roboter.MMotor;
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

        /// <summary>
        /// Starts the ball following logic
        /// </summary>
        /// <param name="sender">not needed</param>
        /// <param name="e">not needed</param>
        public void BallFollowing(object sender=null, EventArgs e=null)
        {
            Motor.Instance.Start();
            Image<Hsv, byte> initFrame = _currentVideoCapture.QueryFrame().ToImage<Hsv, Byte>();
            double max_x = initFrame.Width;
            double middle_x = max_x/2;
            double entity = 1000 / middle_x;

            log.Debug(String.Format("Using lower: {0} and upper: {1} as hsv space", GetProperties().Lower, GetProperties().Upper));
            while (!ShouldStop)
            {
                using (Image<Hsv, byte> imageFrame = _currentVideoCapture.QueryFrame().ToImage<Hsv, Byte>())
                {

                    if (imageFrame != null)
                    {
                        Image<Hsv, Byte> original = imageFrame.Clone();
                        CircleF detectedCircle = SearchCircle(imageFrame);

                        if (detectedCircle.Radius >= GetProperties().MinRadius)
                        {
                            original.Draw(detectedCircle, new Hsv(0, 255, 255), 2);
                            original.Draw(new CircleF(detectedCircle.Center, 2), new Hsv(0, 255, 255), 2);

                            double current_x = detectedCircle.Center.X;
                            double p = 0; 

                            if (current_x >= middle_x)
                            {
                                p = entity * (current_x - middle_x); // p = 1000 => ganz rechts vom roboter
                            }
                            else
                            {
                                p = -entity * (middle_x - current_x); // p = -1000 => ganz links vom roboter
                            }

                            WriteMotorCommand(p, detectedCircle.Radius);
                        }
                        else
                        {
                            Motor.Instance.LeftSpeedValue = 0;
                            Motor.Instance.RightSpeedValue = 0;
                        }
                    }
                    PublishFrame(1, imageFrame, "Original Input");
                }

            }
        }

        /// <summary>
        /// Write motor commands for the parameters p and radius
        /// </summary>
        /// <param name="p">a value between -1000 and 1000 describing relative x position on frame</param>
        /// <param name="radius">the radius of the detected circle</param>
        private void WriteMotorCommand(double p, float radius)
        {
            var properties = GetProperties();
            if (radius < properties.MinRadius || p > 1000 || p < -1000)
            {
                return;
            }
            double acceleration = 1 - (radius-properties.MinRadius) / properties.MaxRadius;
            int speed = (int)(acceleration * 100); // 100 = maximum speed
            if (speed < 0)
            {
                speed = 0;
            }

            int leftspeed = speed;
            int rightspeed = speed;

            if (p < 0 - properties.Tolerance) // p < 0 => ball is on the left side, steer to the left
            {
                double steermultiplier = p / -1000;
                leftspeed = (int)(speed - (steermultiplier * 100) * 2); //ToDo maybe do not multiply with 2
                rightspeed = (int)(speed + (steermultiplier * 100) * 2);

            } else if( p > 0 + properties.Tolerance) // p < 0 => ball is on the tight side, steer to the right
            {
                double steermultiplier = p / 1000;
                leftspeed = (int)(speed + (steermultiplier * 100) * 2);
                rightspeed = (int)(speed - (steermultiplier * 100) * 2);
            }

            if (leftspeed >= 0)
            {
                leftspeed = Math.Min(100, leftspeed);
            }
            else
            {
                leftspeed = Math.Max(-100, leftspeed);
            }

            if (rightspeed >= 0)
            {
                rightspeed = Math.Min(100, rightspeed);
            }
            else
            {
                rightspeed = Math.Max(-100, rightspeed);
            }

            Motor.Instance.LeftSpeedValue = leftspeed;
            Motor.Instance.RightSpeedValue = rightspeed;

            //log.Debug(String.Format("leftspeed: {0}, rightspeed: {1}", leftspeed, rightspeed));
        }

        /// <summary>
        /// Search for a circle structure in the imageFrame at a specific hsv level.
        /// 
        /// </summary>
        /// <param name="imageFrame"></param>
        /// <returns>
        ///     A CircleF instance that contains radius and coordinates of the detected circle structure. 
        ///     Center(-1,-1) and Radius(-1) if no circle has been detected 
        /// </returns>
        private CircleF SearchCircle(Image<Hsv, Byte> imageFrame)
        {
            Image<Gray, Byte> mask = imageFrame.InRange(GetProperties().Lower, GetProperties().Upper);
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
            return CvInvoke.MinEnclosingCircle(biggestContour);
        }
        else return new CircleF(new System.Drawing.PointF(-1,-1),-1);
            
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
            SurrogateFramework.MainController.ProcessHandler.StartProcess(FrameworkConstants.ControllerProcessName);
        }

        public override void OnSelected()
        {
            base.OnSelected();
            SurrogateFramework.MainController.ProcessHandler.EndProcess(FrameworkConstants.ControllerProcessName);
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
        public readonly string Key_MinRadius = "Min_Radius";
        public readonly string Key_MaxRadius = "Max_Radius";
        public readonly string Key_Tolerance = "Tolerance";

        /// <summary>
        /// get/set the MinRadius property
        /// </summary>
        public double MinRadius { get => GetDoubleProperty(Key_MinRadius, 20); set => SetProperty(Key_MinRadius, value); }

        /// <summary>
        /// get/set the MaxRadius property
        /// </summary>
        public double MaxRadius { get => GetDoubleProperty(Key_MaxRadius, 100); set => SetProperty(Key_MaxRadius, value); }

        /// <summary>
        /// get/set the Tolerance Property. This property describes the size of the middle area. If the x-coordinates of the ball are within
        /// this area, the roboter will drive straight on
        /// </summary>
        public int Tolerance { get => GetIntegerProperty(Key_Tolerance, 100); set => SetProperty(Key_Tolerance, value); }

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
            SetProperty(Key_MinRadius, 20, false);
            SetProperty(Key_MaxRadius, 100, false);

            SetProperty(Key_Tolerance, 100, true);
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

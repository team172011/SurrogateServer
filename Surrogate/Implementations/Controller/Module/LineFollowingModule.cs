using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.LineDescriptor;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using Surrogate.Implementations.Model;
using Surrogate.Model;
using Surrogate.Model.Module;
using Surrogate.Modules;
using Surrogate.Roboter.MMotor;
using Surrogate.View;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace Surrogate.Implementations.Controller.Module
{
    public class LineFollowingModule : VisualModule<LineFollowingProperties, LineFollowingInfo>
    {
        private readonly LineFollowingModuleView _view;
        private BackgroundWorker _worker;


        private IDictionary<int, System.Windows.Controls.Image> _currentImages;
        private VideoCapture _currentVideoCapture;

        public override IModuleProperties Properties => GetProperties();

        public LineFollowingModule() : base(new LineFollowingProperties())
        {
            _view = new LineFollowingModuleView(this);
        }

        /// <summary>
        /// Start the line following logic
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LineFollowing(object sender, EventArgs e)
        {
            log.Debug(String.Format("Using lower: {0} and upper: {1} as hsv space", GetProperties().Lower, GetProperties().Upper));
            while (!ShouldStop)
            {
                using (Image<Bgr, byte> imageFrame = _currentVideoCapture.QueryFrame().ToImage<Bgr, Byte>())
                {
                    
                    if (imageFrame != null)
                    {
                        Image<Bgr, Byte> original = imageFrame.Clone();
                        Image<Gray,Byte> mask = imageFrame.Convert<Hsv,Byte>().InRange(GetProperties().Lower, GetProperties().Upper);
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
                        if(contours.Size > 0)
                        {
                            var biggestContour = contours[0];
                            var biggestContourSize = CvInvoke.ContourArea(biggestContour);
                            for (int i = 1; i < contours.Size; i++)
                            {
                                var sizeCurrent = CvInvoke.ContourArea(contours[i]);
                                if (sizeCurrent > biggestContourSize)
                                {
                                    biggestContour = contours[i];
                                    biggestContourSize = sizeCurrent;
                                }

                            }

                            Rectangle rec = CvInvoke.BoundingRectangle(biggestContour);
                            RotatedRect rrec = CvInvoke.MinAreaRect(biggestContour);
                            original.Draw(rec, new Bgr(0, 0, 255), 2);
                            original.Draw(rrec, new Bgr(0, 255, 0), 2);
                            original.Draw(biggestContour.ToArray(), new Bgr(255, 0, 0), 2);

                            var difference = rec.Height*rec.Width - rrec.Size.Height*rrec.Size.Width;

                            var tresdiff = 5000; // allowed difference between bounding rec and detected rec
                            var maxdiff=0;

                            if(difference < tresdiff) // the bounding rec and the detected rec are equal, drive straight on
                            {
                                Motor.Instance.LeftSpeedValue = (50);
                                Motor.Instance.RightSpeedValue = (50);
                                log.Debug("gerade aus");
                            }
                            else {
                                log.Debug(difference);
                            }
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
                pimage.Draw(fontScale: 2, message: viewNum.ToString() +" "+name, bottomLeft: new System.Drawing.Point(1, 50), fontFace: FontFace.HersheyComplex, color: new Hsv(135, 99, 100), thickness: 2);
                Application
                    .Current
                    .Dispatcher
                    .BeginInvoke(DispatcherPriority.Normal, new Action(() => 
                    _currentImages[viewNum].Source = Utils.UI.BitmapSourceConvert.ToBitmapSource(pimage)));
                
            }
            catch (CvException cve)
            {
                log.Error("Fehler beim veröffentlichen eines Frames: "+cve.Message);
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
                pimage.Draw(fontScale: 2,message: viewNum.ToString() + " " + name, bottomLeft: new System.Drawing.Point(1, 50), fontFace: FontFace.HersheyComplex,color: new Bgr(0,255,0),thickness:2);
                Application
                    .Current
                    .Dispatcher
                    .BeginInvoke(DispatcherPriority.Normal, new Action(() =>
                    _currentImages[viewNum].Source = Utils.UI.BitmapSourceConvert.ToBitmapSource(pimage)));

            }
            catch (CvException cve)
            {
                log.Error("Fehler beim veröffentlichen eines Frames: " + cve.Message);
            }
        }

        public override UserControl GetPage()
        {
            return _view;
        }

        public override void Start(LineFollowingInfo info)
        {
            ShouldStop = false;
            if (IsRunning()) return;
            _currentImages = info.Images;
            _currentVideoCapture = new VideoCapture(GetProperties().CamNum);
            _worker = new BackgroundWorker();
            _worker.DoWork += LineFollowing;
            _worker.RunWorkerAsync();
        }

        public override bool IsRunning()
        {
            return _worker != null && _worker.IsBusy;
        }

        public override void Stop()
        {
            base.Stop();
            _currentVideoCapture?.Dispose();
        }
    }

    public class LineFollowingInfo : ModuleInfo
    {
        private readonly IDictionary<int, System.Windows.Controls.Image> _images;
        public IDictionary<int, System.Windows.Controls.Image> Images => _images;



        public LineFollowingInfo(IDictionary<int, System.Windows.Controls.Image> images)
        {
            _images = images;
        }
    }

    public class LineFollowingProperties : ModulePropertiesBase
    {
        public readonly string Key_H_Lower = "H_Lower";
        public readonly string Key_S_Lower = "S_Lower";
        public readonly string Key_V_Lower = "V_Lower";

        public readonly string Key_H_Upper = "H_Upper";
        public readonly string Key_S_Upper = "S_Upper";
        public readonly string Key_V_Upper = "V_Upper";

        public readonly string Key_Inverted = "Inverted";
        public readonly string Key_CamNum = "Camera";

        public Hsv Lower { get => new Hsv(GetIntegerProperty(Key_H_Lower,0), GetIntegerProperty(Key_S_Lower, 0), GetIntegerProperty(Key_V_Lower, 0)); } 
        public Hsv Upper { get => new Hsv(GetIntegerProperty(Key_H_Upper, 255), GetIntegerProperty(Key_S_Upper, 255), GetIntegerProperty(Key_V_Upper, 255)); }
        public bool Inverted { get => GetBooleanProperty(Key_Inverted, false); }

        private int _camNum = 0;
        public int CamNum { get => _camNum; set => value = _camNum; }

        public LineFollowingProperties() : base("Linie folgen","Modul zum Folgen einer farbigen Linie", motor:true, floorCam:true)
        {
            SetProperty(KeyImagePath, @"C:\Users\ITM1\source\repos\Surrogate\Surrogate\resources\linefollowing_controller_icon.png");
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

        public void SetBounds(HsvBounds e)
        {
            SetProperty(Key_H_Lower, e.Lower.Hue);
            SetProperty(Key_S_Lower, e.Lower.Satuation);
            SetProperty(Key_V_Lower, e.Lower.Value);

            SetProperty(Key_H_Upper, e.Upper.Hue);
            SetProperty(Key_S_Upper, e.Upper.Satuation);
            SetProperty(Key_V_Upper, e.Upper.Value);

            SetProperty(Key_Inverted, e.Inverted);
            Save();
        }
    }
}

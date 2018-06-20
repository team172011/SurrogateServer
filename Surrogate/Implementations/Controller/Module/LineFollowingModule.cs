﻿using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.LineDescriptor;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using OpenTok;
using Surrogate.Implementations.Model;
using Surrogate.Model;
using Surrogate.Model.Module;
using Surrogate.Modules;
using Surrogate.Roboter.MCamera;
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
        private int CamNum;

        public override IModuleProperties Properties => GetProperties();

        public LineFollowingModule() : base(new LineFollowingProperties())
        {
            _view = new LineFollowingModuleView(this);
            ModuleDisselected += new EventHandler((o, e) => Stop());
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
                using (Image<Hsv, byte> imageFrame = _currentVideoCapture.QueryFrame().ToImage<Hsv, Byte>())
                using (Image<Bgr, byte> original = _currentVideoCapture.QueryFrame().ToImage<Bgr, Byte>())
                {

                    if (imageFrame != null)
                    {
                        Image<Gray, Byte> mask = imageFrame.Convert<Hsv, Byte>().InRange(GetProperties().Lower, GetProperties().Upper);
                        System.Diagnostics.Debug.WriteLine(GetProperties().Lower+" "+GetProperties().Upper+ GetProperties().Inverted);
                        if (GetProperties().Inverted)
                        {
                            mask = mask.Not();
                        }
                        Mat filtered = new Mat();
                        CvInvoke.BitwiseAnd(imageFrame, imageFrame, filtered, mask: mask); // filter image by upper and lower hsv space values

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
                            RotatedRect rrec = CvInvoke.MinAreaRect(biggestContour);
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

        public void ChangeCamera()
        {
            Stop();
            int NextCam = ++CamNum % FrameworkConstants.Numbercams; // increase cam index by 
            log.Debug("Switching to Camera " + NextCam);
            GetProperties().CamNum = NextCam;
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
            if(_currentVideoCapture != null)
            {
                _currentVideoCapture.Dispose();
            }
            _currentVideoCapture = new VideoCapture(GetProperties().CamNum);
            try {
                var test = _currentVideoCapture.QueryFrame().ToImage<Bgr, Byte>();
            } catch (Exception e)
            {
                log.Error("Could not use camera. Switching to default "+e.Message);
                GetProperties().CamNum = 0;
                Start(info);
            }
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

    public class LineFollowingProperties : HsvModulProperties
    {


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
    }
}

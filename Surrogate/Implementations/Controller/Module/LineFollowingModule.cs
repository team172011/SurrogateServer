
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using Surrogate.Model;
using Surrogate.Model.Module;
using Surrogate.Modules;
using Surrogate.View;
using Surrogate.View.ControllerView.ModuleView;
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

        private Hsv _upperHsv;
        private Hsv _lowerHsv;

        public override IModuleProperties Properties => GetProperties();

        public LineFollowingModule() : base(new LineFollowingProperties())
        {
            _view = new LineFollowingModuleView(this);
            _upperHsv = GetProperties().Upper;
            _lowerHsv = GetProperties().Lower;
        }

        private void LineFollowing(object sender, EventArgs e)
        {
            while (!ShouldStop)
            {
                using (Image<Hsv, byte> imageFrame = _currentVideoCapture.QueryFrame().ToImage<Hsv, Byte>())
                {
                    
                    if (imageFrame != null)
                    {
                        Image<Gray,Byte> mask = imageFrame.InRange(_lowerHsv, _upperHsv);
                        Mat filtered = new Mat();
                        CvInvoke.BitwiseAnd(imageFrame, imageFrame, filtered, mask: mask); // filter image by specific upper and lower hsv space values
                        var contours = new VectorOfVectorOfPoint();
                        var smoothed = filtered.ToImage<Gray, Byte>().SmoothGaussian(5);
                        var tres = smoothed.ThresholdBinaryInv(new Gray(0), new Gray(255));
                        var eroded = tres.Erode(8);
                        var dilated = eroded.Dilate(8);

                        CvInvoke.FindContours(dilated, contours, null, RetrType.List, ChainApproxMethod.ChainApproxSimple);
                        log.Debug("Contours found: "+contours.Size);
                        /*
                        LineSegment2D[] lines = CvInvoke.HoughLinesP(cannyEdges, 1, Math.PI / 45, 50, 100, 10);
                        log.Debug("Detected lines: " + lines.Length);
                        foreach(LineSegment2D line in lines)
                        {
                            imageFrame.Draw(line, new Bgr(Color.Green), 2);
                        }

                        using (VectorOfVectorOfPoint contours = new VectorOfVectorOfPoint()) { 
                            CvInvoke.FindContours(cannyEdges, contours, null, RetrType.List, ChainApproxMethod.ChainApproxSimple);
                            for(int i = 0; i < contours.Size; i++)
                            {
                                using (VectorOfPoint contour = contours[i])
                                using (VectorOfPoint approxContour = new VectorOfPoint())
                                {
                                    CvInvoke.ApproxPolyDP(contour, approxContour, CvInvoke.ArcLength(contour, true)*0.05, true);  // maby false, false??
                                    if (CvInvoke.ContourArea(approxContour) > 10){ // 250?
                                        //TODO
                                    }
                                }
                            }
                        }
                        */
                        PublishFrame(5, dilated.Convert<Bgr,byte>());
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
            _lowerHsv = e.Lower;
            _upperHsv = e.Upper;
        }

    
        private List<RotatedRect> FindRectangles(Image<Gray,Byte> grayImage)
        {
            List<RotatedRect> boxList = new List<RotatedRect>();
            using (VectorOfVectorOfPoint contours = FindContours(grayImage))
            {
                for (int i = 0; i < contours.Size; i++)
                {
                    using (VectorOfPoint contour = contours[i])
                    using (VectorOfPoint approxContour = new VectorOfPoint())
                    {
                        CvInvoke.ApproxPolyDP(contour, approxContour, CvInvoke.ArcLength(contour, true) * 0.05, true);

                        if (approxContour.Size == 4) // approximated contour has 4 vertices, it is a rectangle
                        {
                            boxList.Add(CvInvoke.MinAreaRect(approxContour));
                        }
                    }
                }
            }

            return boxList;
        }

        private VectorOfVectorOfPoint FindContours(Image<Gray, byte> image, ChainApproxMethod method = ChainApproxMethod.ChainApproxSimple,
            RetrType type = RetrType.List)
        { 
            VectorOfVectorOfPoint result = new VectorOfVectorOfPoint();
            CvInvoke.FindContours(image, result, null, type, method);
            return result;
        }


        /// <summary>
        /// Publishs the Image on the _view
        /// </summary>
        /// <param name="imageFrame"></param>
        /// <param name="viewNum"></param>
        private void PublishFrame(int viewNum, Image<Bgr, byte> imageFrame)
        {
            try
            {
                var pimage = imageFrame.Clone(); // clone image because we are going to run async on gui thread
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

        public override UserControl GetPage()
        {
            return _view;
        }

        public override void Start(LineFollowingInfo info)
        {
            ShouldStop = false;
            if (IsRunning()) return;
            _currentImages = info.Images;
            _currentVideoCapture = new VideoCapture();
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
        private readonly Hsv _lower;
        private readonly Hsv _upper;

        public Hsv Lower { get => _lower; } 
        public Hsv Upper { get => _upper; } 

        public LineFollowingProperties() : base("Linie folgen","Modul zum folgen einer farbigen Linie", motor:true, floorCam:true)
        {
            _lower = new Hsv(25, 50, 50);
            _upper = new Hsv(32, 255, 255);
        }
    }
}

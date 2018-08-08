// This file belongs to the source code of the "Surrogate Project"
// Copyright (c) 2018 All Rights Reserved
// Martin-Luther-Universitaet Halle-Wittenberg
// Lehrstuhl Wirtschaftsinformatik und Operation Research
// Autor: Wimmer, Simon-Justus Wimmer (simonjustuswimmer@googlemail.com)
namespace Surrogate.Implementations.FaceDetection
{
    using System.Windows;
    using System.Windows.Controls;
    using Surrogate.Model;
    using Surrogate.Model.Module;
    using Surrogate.Modules;
    using Surrogate.View;

    using Emgu.CV.Structure;
    using Emgu.CV;
    using System.Windows.Threading;
    using System;
    using System.Drawing;

    using Surrogate.Utils.UI;
    using System.IO;

    public class FaceDetectionModule : VisualModule<FaceDetectionProperties, FaceDetectionInfo>
    {
        private readonly FaceDetectionView _view;

        public override IModuleProperties Properties => GetProperties();
        private readonly CascadeClassifier _eyeCascade;
        private readonly CascadeClassifier _faceCascade;

        private DispatcherTimer _timer;

        private System.Windows.Controls.Image _currentImage;
        private VideoCapture _currentVideoCapture;


        public FaceDetectionModule() : base(new FaceDetectionProperties())
        {
            _view = new FaceDetectionView(this);
            System.Diagnostics.Debug.WriteLine(Directory.GetCurrentDirectory());
            _eyeCascade = new CascadeClassifier(@"Resources\haarcascade_eye.xml");
            _faceCascade = new CascadeClassifier(@"Resources\haarcascade_frontalface_default.xml");
        }

        private void StartSearchAndMark(object sender, EventArgs e)
        {
            SearchAndMark(_currentVideoCapture, _currentImage);
        }

        /// <summary>
        /// This function search for a face (and eyes) in frames from the Emgu.Cv.VideoCapture video stream and
        /// marks them with red and yellow lines
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SearchAndMark(VideoCapture capture, System.Windows.Controls.Image imView)
        {
            using (var imageFrame = capture.QueryFrame().ToImage<Bgr, Byte>())
            {
                if (imageFrame != null)
                {
                    var grayframe = imageFrame.Convert<Gray, byte>();
                    var faces = _faceCascade.DetectMultiScale(grayframe, 1.1, 10, System.Drawing.Size.Empty); 
                    foreach (var face in faces)
                    {

                        var eyes = _eyeCascade.DetectMultiScale(grayframe, 1.1, 10, System.Drawing.Size.Empty); // for every face, search for eyes
                        imageFrame.Draw(face, new Bgr(Color.BurlyWood), 3);
                        foreach (var eye in eyes) // draw eyes
                        {
                            imageFrame.Draw(eye, new Bgr(Color.Red), 3);
                        }
                    }
                }
                imView.Source = BitmapSourceConvert.ToBitmapSource(imageFrame);
            }
        }

        public override UserControl GetPage()
        {
            return _view;
        }

        public override void OnDisselected()
        {
            Stop();
        }

        public override void OnSelected()
        {
            
        }

        public override bool IsRunning()
        {
            return _timer.IsEnabled;
        }

        public override void Start(FaceDetectionInfo info)
        {
            _currentImage = info.ImgView;
            _currentVideoCapture = new VideoCapture(info.CamId);
            if(_currentVideoCapture.QueryFrame() == null)
            {
                MessageBox.Show("Die aktuelle Kamera wurde nicht gefunden!", "Kamera nicht gefunden");
                _currentVideoCapture.Dispose();
                return;
            }
 
            _timer = new DispatcherTimer();
            _timer.Tick +=  StartSearchAndMark;
            _timer.Interval = new TimeSpan(0, 0, 0, 0, 10);
            _timer.Start();
            
        }

        public override void Stop()
        {
            _timer?.Stop();
            if(_currentImage != null) _currentImage.Source = null;
            _currentVideoCapture?.Dispose();
        }

        public override string ToString()
        {
            return base.ToString();
        }
    }

    public class FaceDetectionProperties : ModulePropertiesBase
    {
        public FaceDetectionProperties() : base("Gesichtserkennung", "Modul zum visuellen Vorführen der Gesichtserkennung", false, true, false,false, false)
        {
            SetProperty(base.KeyImagePath, System.IO.Directory.GetCurrentDirectory() + "/Resources/facedetection_controller_icon.png");
        }
    }

    public class FaceDetectionInfo: ModuleInfo
    {

        public System.Windows.Controls.Image ImgView { get; }
        public int CamId { get; }

        public FaceDetectionInfo(System.Windows.Controls.Image image, int CamId=0)
        {
            ImgView = image;
            this.CamId = CamId;
        }
    }
}
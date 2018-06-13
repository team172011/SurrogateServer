﻿// This file belongs to the source code of the "Surrogate Project"
// Copyright (c) 2018 All Rights Reserved
// Martin-Luther-Universitaet Halle-Wittenberg
// Lehrstuhl Wirtschaftsinformatik und Operation Research
// Autor: Wimmer, Simon-Justus Wimmer

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
            _eyeCascade = new CascadeClassifier(@"C:\Users\ITM1\source\repos\Surrogate\Surrogate\resources\haarcascade_eye.xml");
            _faceCascade = new CascadeClassifier(@"C:\Users\ITM1\source\repos\Surrogate\Surrogate\resources\haarcascade_frontalface_default.xml");


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
            _currentImage = info.Image;
            _currentVideoCapture = new VideoCapture();
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
        public FaceDetectionProperties() : base("Gesichtserkennung", "Module zum visuellen Vorführen der Gesichtserkennund mithilfe der Opencv Bibliotheken", false, true, false,false, false)
        {
        }
    }

    public class FaceDetectionInfo: ModuleInfo
    {

        private readonly System.Windows.Controls.Image _image;
        public System.Windows.Controls.Image Image => _image;

        public FaceDetectionInfo(System.Windows.Controls.Image image)
        {
            _image = image;
        }
    }
}
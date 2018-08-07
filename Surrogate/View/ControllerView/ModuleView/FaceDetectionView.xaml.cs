// This file belongs to the source code of the "Surrogate Project"
// Copyright (c) 2018 All Rights Reserved
// Martin-Luther-Universitaet Halle-Wittenberg
// Lehrstuhl Wirtschaftsinformatik und Operation Research
// Autor: Wimmer, Simon-Justus Wimmer (simonjustuswimmer@googlemail.com)
using Surrogate.Implementations.FaceDetection;
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
using System.Windows.Shapes;

namespace Surrogate.View
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class FaceDetectionView : ModuleViewBase
    {
        public FaceDetectionView(FaceDetectionModule controller):base(controller)
        {
            InitializeComponent();
        }


        private void OnStartPressed(object sender, RoutedEventArgs e)
        {
            var controller = Controller as FaceDetectionModule;
            controller.Start(new FaceDetectionInfo(imView));
        }

        private void OnStopPressed(object sender, RoutedEventArgs e)
        {
            var controller = Controller as FaceDetectionModule;
            controller.Stop();
        }
    }
}

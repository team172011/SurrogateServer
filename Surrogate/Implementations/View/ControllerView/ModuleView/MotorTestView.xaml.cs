// This file belongs to the source code of the "Surrogate Project"
// Copyright (c) 2018 All Rights Reserved
// Martin-Luther-Universitaet Halle-Wittenberg
// Lehrstuhl Wirtschaftsinformatik und Operation Research
// Autor: Wimmer, Simon-Justus Wimmer (simonjustuswimmer@googlemail.com)
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;
using Surrogate.Implementations;
using Surrogate.Modules;
using Surrogate.Roboter.MController;
using static Surrogate.Implementations.MotorTestModule;

namespace Surrogate.View
{
    /// <summary>
    /// View Component of the ControllerTestModule
    /// </summary>
    public partial class MotorTestView : ModuleViewBase
    {

        public MotorTestView(MotorTestModule controller):base(controller)
        {
            InitializeComponent();
            motorImage.Source = new BitmapImage(new Uri(
             System.IO.Directory.GetCurrentDirectory() + "/Resources/Skizze_Fahrwerk_transp.png"));
        }

        public void Forwards(object sender, RoutedEventArgs e)
        {
            MotorTestInfo info = new MotorTestInfo(MotorTestInfo.Direction.Forwards);
            var controller = Controller as MotorTestModule;
            controller.Start(info);
        }

        public void Backwards(object sender, RoutedEventArgs e)
        {
            MotorTestInfo info = new MotorTestInfo(MotorTestInfo.Direction.Backwards);
            var controller = Controller as MotorTestModule;
            controller.Start(info);
        }

        public void Left(object sender, RoutedEventArgs e)
        {
            MotorTestInfo info = new MotorTestInfo(MotorTestInfo.Direction.Left);
            var controller = Controller as MotorTestModule;
            controller.Start(info);
        }

        public void Right(object sender, RoutedEventArgs e)
        {
            MotorTestInfo info = new MotorTestInfo(MotorTestInfo.Direction.Right);
            var controller = Controller as MotorTestModule;
            controller.Start(info);
        }

        public void OnlyLeft(object sender, RoutedEventArgs e)
        {
            MotorTestInfo info = new MotorTestInfo(MotorTestInfo.Direction.OnlyLeft);
            var controller = Controller as MotorTestModule;
            controller.Start(info);
        }

        public void OnlyRight(object sender, RoutedEventArgs e)
        {
            MotorTestInfo info = new MotorTestInfo(MotorTestInfo.Direction.OnlyRight);
            var controller = Controller as MotorTestModule;
            controller.Start(info);
        }

        public void StopTest(object sender, RoutedEventArgs e)
        {
            MotorTestInfo info = new MotorTestInfo(MotorTestInfo.Direction.Stop);
            var controller = Controller as MotorTestModule;
            controller.Start(info);
        }
    }
}

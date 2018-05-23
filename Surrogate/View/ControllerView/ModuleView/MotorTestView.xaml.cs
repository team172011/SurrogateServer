// This file belongs to the source code of the "Surrogate Project"
// Copyright (c) 2018 All Rights Reserved
// Martin-Luther-Universitaet Halle-Wittenberg
// Lehrstuhl Wirtschaftsinformatik und Operation Research
// Autor: Wimmer, Simon-Justus Wimmer

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Surrogate.Implementations;
using Surrogate.Modules;
using Surrogate.Roboter.MController;
using static Surrogate.Implementations.MotorTestModule;

namespace Surrogate.View
{
    /// <summary>
    /// View Component of the ControllerTestModule
    /// </summary>
    public partial class MotorTestView
    {

        // referenc to the module
        private MotorTestModule parentModule;

        public MotorTestView(MotorTestModule parentModule)
        {
            this.parentModule = parentModule;
            InitializeComponent();
        }

        public void Forwards(object sender, RoutedEventArgs e)
        {
            MotorTestInfo info = new MotorTestInfo(MotorTestInfo.Direction.Forwards);
            parentModule.Start(info);
        }

        public void Backwards(object sender, RoutedEventArgs e)
        {
            MotorTestInfo info = new MotorTestInfo(MotorTestInfo.Direction.Backwards);
            parentModule.Start(info);
        }

        public void Left(object sender, RoutedEventArgs e)
        {
            MotorTestInfo info = new MotorTestInfo(MotorTestInfo.Direction.Left);
            parentModule.Start(info);
        }

        public void Right(object sender, RoutedEventArgs e)
        {
            MotorTestInfo info = new MotorTestInfo(MotorTestInfo.Direction.Right);
            parentModule.Start(info);
        }

        public void OnlyLeft(object sender, RoutedEventArgs e)
        {
            MotorTestInfo info = new MotorTestInfo(MotorTestInfo.Direction.OnlyLeft);
            parentModule.Start(info);
        }

        public void OnlyRight(object sender, RoutedEventArgs e)
        {
            MotorTestInfo info = new MotorTestInfo(MotorTestInfo.Direction.OnlyRight);
            parentModule.Start(info);
        }

        public void StopTest(object sender, RoutedEventArgs e)
        {
            MotorTestInfo info = new MotorTestInfo(MotorTestInfo.Direction.Stop);
            parentModule.Start(info);
        }
    }
}

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
using System.Windows.Threading;
using Surrogate.Implementations;
using Surrogate.Modules;
using Surrogate.Roboter.MController;
using Surrogate.Utils.Event;

namespace Surrogate.View
{
    /// <summary>
    /// View Component of the ControllerTestModule
    /// </summary>
    public partial class ControllerTestView : ModuleView
    {

        private ControllerTestModule parentModule;
        private bool motor;

        public ControllerTestView(ControllerTestModule parentModule)
        {
            this.parentModule = parentModule;
            InitializeComponent();
            parentModule.ControllerAvailable += ControllerAvailabe;
            parentModule.MotorAvailable += MotorAvailable;
            parentModule.IsRunningChanged += RunningChanged;
        }

        private void RunningChanged(object sender, BooleanEventArgs e)
        {
            Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Normal, new Action(() =>
                    btnStop.IsEnabled = e.Value));

        }

        private void MotorAvailable(object sender, BooleanEventArgs e)
        {
            Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Normal, new Action(() =>
            motor = e.Value));
            parentModule.Log("Motor available: "+e.Value);

        }

        private void ControllerAvailabe(object sender, BooleanEventArgs e)
        {
          
            Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Normal, new Action(() =>
            {
                btnControllRobot.IsEnabled = e.Value && motor;
                btnTestMotor.IsEnabled = e.Value;
                btnController.IsEnabled = e.Value;
            })); 
            parentModule.Log("Controller available: "+ e.Value);
            

        }



        /// <summary>
        /// Start the controller test without connecting to the motor.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void TestControllerOutput(object sender, RoutedEventArgs e)
        {
            
           Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Normal, new Action(()
               => btnStop.IsEnabled = true));
            parentModule.Start(new ControllerTestInfo(ControllerTestInfo.TestCase.ControllerOutput));
        }

        /// <summary>
        /// Start the controller test and connect to the motor. User will be able to
        /// controll the roboter.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void TestMotorOutput(object sender, RoutedEventArgs e)
        {
            btnStop.IsEnabled = true;
            parentModule.Start(new ControllerTestInfo(ControllerTestInfo.TestCase.MotorOutput));
        }

        public void ControllRobot(object sender, RoutedEventArgs e)
        {
            parentModule.Start(new ControllerTestInfo(ControllerTestInfo.TestCase.ControllRobot));
        }

        /// <summary>
        /// Stops the current running test.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void StopTest(object sender, RoutedEventArgs e)
        {
            parentModule.Stop();
        }
    }
}

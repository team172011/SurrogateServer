// This file belongs to the source code of the "Surrogate Project"
// Copyright (c) 2018 All Rights Reserved
// Martin-Luther-Universitaet Halle-Wittenberg
// Lehrstuhl Wirtschaftsinformatik und Operation Research
// Autor: Wimmer, Simon-Justus Wimmer (simonjustuswimmer@googlemail.com)
using System;
using System.ComponentModel;
using Surrogate.Roboter.MMotor;
using Surrogate.Roboter.MController;
using SharpDX.XInput;
using Surrogate.Model;

namespace Surrogate.Implementations.Processes
{
    /// <summary>
    /// A process running in the backgorund responsible for controlling the 
    /// prototype via XboxController
    /// </summary>
    [System.ComponentModel.DesignerCategory("Code")]
    public class ControllerProcess : BackgroundWorker
    {
        private readonly IMotor _motor;
        private readonly IRemoteController _controller;
        protected static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public ControllerProcess(IMotor motor, IRemoteController controller){
            WorkerSupportsCancellation = true;
            _motor = motor;
            _controller = controller;
            DoWork += Work;
        }

        public void Work(object sender, DoWorkEventArgs e)
        {
            log.Debug("ControllerProcess started");
            if (_motor.Status == ConnectionStatus.Ready)
            {
                Motor.Instance.Start();
                Tuple<int, int> speedValues = new Tuple<int, int>(0, 0);
                while (
                         !CancellationPending &&
                         _controller.Status == ConnectionStatus.Ready &&
                         !_controller.Buttons.Equals(GamepadButtonFlags.A))
                {
                    Tuple<int, int> nextSpeedValues = CalculateSpeedValues(_controller);
                    //log.Debug(nextSpeedValues);
                    {
                        speedValues = nextSpeedValues;
                        Motor.Instance.LeftSpeedValue = speedValues.Item1;
                        Motor.Instance.RightSpeedValue = speedValues.Item2;
                    }
                }
                Motor.Instance.PullUp();
            }
            log.Debug("ControllerProcess ended");
            
        }

        /// <summary>
        /// Transforms the input values from controller to left and right speed
        /// values for the motors
        /// </summary>
        /// <param name="controller"></param>
        /// <returns>A Tuple(int, int) with left and right speed values</int></returns>
        private Tuple<int, int> CalculateSpeedValues(IRemoteController controller)
        {
            controller.Update();
            float rightTrigger = controller.RightTrigger;
            float leftTrigger = controller.LeftTrigger;
            double rightThumbX = controller.RightThumb.X;
            int leftspeed = 0;
            int rightspeed = 0;


            // calculate speed based on left and right triggers (right trigger forward, left trigger backward)
            int tempSpeed = (int)((rightTrigger - leftTrigger));// / 255 * 100);

            if (rightThumbX > 0)
            {
                double multiplier = rightThumbX / 100;
                leftspeed = (int)(tempSpeed + multiplier * 255);
                rightspeed = (int)(tempSpeed - multiplier * 255);
            }
            else if (rightThumbX < 0)
            {
                double multiplier = rightThumbX / -100;
                leftspeed = (int)(tempSpeed - multiplier * 255);
                rightspeed = (int)(tempSpeed + multiplier * 255);
            }
            else
            {
                rightspeed = leftspeed = tempSpeed;
            }

            return Tuple.Create(leftspeed, rightspeed);
        }
    }
}

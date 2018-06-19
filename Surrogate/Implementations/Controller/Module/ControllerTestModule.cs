﻿// This file belongs to the source code of the "Surrogate Project"
// Copyright (c) 2018 All Rights Reserved
// Martin-Luther-Universitaet Halle-Wittenberg
// Lehrstuhl Wirtschaftsinformatik und Operation Research
// Autor: Wimmer, Simon-Justus Wimmer



namespace Surrogate.Implementations.Controller.Module
{
    using System;
    using System.Threading;
    using System.Runtime.InteropServices;
    using System.Windows.Controls;
    using Surrogate.Modules;
    using Surrogate.Roboter.MController;
    using Surrogate.View;
    using System.Threading.Tasks;

    using System.Windows;
    using System.Windows.Threading;
    using SharpDX.XInput;
    using Surrogate.Roboter.MMotor;
    using Surrogate.Utils.Event;
    using Surrogate.Model.Module;
    using Surrogate.Model;

    public class ControllerTestModule : VisualModule<ControllerTestProperties, ControllerTestInfo>
    {
        public event EventHandler<BooleanEventArgs> MotorAvailable;
        public event EventHandler<BooleanEventArgs> ControllerAvailable;
        public event EventHandler<BooleanEventArgs> IsRunningChanged;

        public override IModuleProperties Properties => GetProperties();
        private volatile bool _shouldStop = false;
        private readonly XBoxController controller = new XBoxController();

        public ControllerTestModule(ControllerTestProperties modulProperties) : base(modulProperties)
        {
            ModuleSelected += Selected;
            ModuleDisselected += Disselected;
        }

        public ControllerTestModule() : base(new ControllerTestProperties())
        {
            ModuleSelected += Selected;
            ModuleDisselected += Disselected;
        }

        /// <summary>
        /// Should be called if the uptime of the motor changes
        /// </summary>
        /// <param name="available">true if the motor is available, false otherwise</param>
        public virtual void OnMotorAvailableChanged(Boolean available)
        {
            MotorAvailable?.Invoke(this, new BooleanEventArgs(available));
        }

        /// <summary>
        /// Should be called if the uptime of the controller changes
        /// </summary>
        /// <param name="available">true if the controler is available, false otherwise</param>
        public virtual void OnControllerAvailableChanged(Boolean available)
        {
            ControllerAvailable?.Invoke(this, new BooleanEventArgs(available));
        }

        public virtual void OnIsRunngingChanged(Boolean isRunning)
        {
            IsRunningChanged?.Invoke(this, new BooleanEventArgs(isRunning));
        }

        public override UserControl GetPage()
        {
            ControllerTestView view = new ControllerTestView(this);
            FireChangeEvents();
            return view;
        }

        public override void Stop()
        {
            _shouldStop = true;
        }

        /// <summary>
        /// Start the ControllerTest
        /// runs synchronously until it hits an “await” (or throws an exception).
        /// </summary>
        /// <param name="info"></param>
        public override async void Start(ControllerTestInfo info)
        {
            if (_shouldStop)
            {
                return;
            }

            controller.Update();

            switch (info.Case)
            {
                case ControllerTestInfo.TestCase.MotorOutput:
                    await Task.Run(() => TestMotorControl(controller, true));
                    break;
                case ControllerTestInfo.TestCase.ControllerOutput:
                    await Task.Run(() => TestConrollerOutputs(controller));
                    break;
                default:
                    await Task.Run(() => TestMotorControl(controller));
                    break;
            }
        }

        private void TestMotorControl(XBoxController controller, bool simulate = false)
        {
            OnIsRunngingChanged(true);
            try
            {
                if (!simulate)
                {
                    Motor.Instance.Start();
                }
                Tuple<int, int> speedValues = Tuple.Create(0, 0);
                while (!_shouldStop && !controller.buttons.Equals(GamepadButtonFlags.A))
                {
                    Tuple<int, int> nextSpeedValues = CalculateSpeedValues(controller);
                    // only update if values changed
                    {
                        speedValues = nextSpeedValues;
                        Motor.Instance.LeftSpeedValue = (speedValues.Item1);
                        Motor.Instance.RightSpeedValue = (speedValues.Item2);
                        if (simulate)
                        {
                            System.Diagnostics.Debug.WriteLine(String.Format("Leftspeed: {0}, Rightspeed: {1}", speedValues.Item1, speedValues.Item2));
                        }
                    }
                }
                Motor.Instance.PullUp();
                Stop();
            }
            catch (Exception pnve)
            {
                log.Error("A motor problem occured: " + pnve.Message + "\n " + pnve.StackTrace);
            }
            finally
            {
                OnIsRunngingChanged(false);
            }
        }

        /// <summary>
        /// Transforms the input values from controller to left and right speed
        /// values for the motors
        /// </summary>
        /// <param name="controller"></param>
        /// <returns>A Tuple(int, int) with left and right speed values</int></returns>
        private Tuple<int, int> CalculateSpeedValues(XBoxController controller)
        {
            controller.Update();
            float rightTrigger = controller.rightTrigger;
            float leftTrigger = controller.leftTrigger;
            double rightThumbX = controller.rightThumb.X;
            int leftspeed = 0;
            int rightspeed = 0;


            // calculate speed based on left and right triggers (right trigger forward, left trigger backward)
            int tempSpeed = (int)((rightTrigger - leftTrigger) / 255 * 100);

            if (rightThumbX > 0)
            {
                double multiplier = rightThumbX / 100;
                leftspeed = (int)(tempSpeed + multiplier * 100);
                rightspeed = (int)(tempSpeed - multiplier * 100);
            } else if (rightThumbX < 0)
            {
                double multiplier = rightThumbX / -100;
                leftspeed = (int)(tempSpeed - multiplier * 100);
                rightspeed = (int)(tempSpeed + multiplier * 100);
            }
            else
            {
                rightspeed = leftspeed = tempSpeed;
            }

            return Tuple.Create(leftspeed, rightspeed);
        }

        /// <summary>
        /// Log the Contorller values.
        /// </summary>
        /// <param name="controller"></param>
        /// <returns></returns>
        private bool TestConrollerOutputs(XBoxController controller)
        {
            OnIsRunngingChanged(true);
            while (!_shouldStop)
            {
                Thread.Sleep(200); // give console some time after each iteration
                controller.Update();
                Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Normal, new Action(() => {
                    log.Info("\nLeft Thumb X: " + controller.leftThumb.X + " " + "\nLeft Thumb Y: " + controller.leftThumb.Y + "\n" +
                        "A?: " + controller.buttons.Equals(GamepadButtonFlags.A) + "\n" +
                        "Rigth Thumb X: " + controller.rightThumb.X + "\n" +
                        "Rigth Thumb Y: " + controller.rightThumb.Y + "\n" +
                        "Left Trigger: " + controller.leftTrigger + "\n" +
                        "Right Trigger: " + controller.rightTrigger); }));
                //Console.WriteLine(controller.leftThumb + " " + controller.rightThumb + " " + controller.leftTrigger + " " + controller.rightTrigger);
            }
            log.Info("Controller test ended");
            OnIsRunngingChanged(false);
            return true;
        }

        private void Selected(Object sender, EventArgs e)
        {
            //SearchController();
            FireChangeEvents();
        }

        public void Disselected(Object sender, EventArgs e)
        {
            OnControllerAvailableChanged(false);
            OnMotorAvailableChanged(false);
        }

        private void FireChangeEvents()
        {
            OnMotorAvailableChanged(Motor.Instance.IsReady());
            OnControllerAvailableChanged(controller.Connected);
        }

        public override bool IsRunning()
        {
            throw new NotImplementedException();
        }
    }

    public class ControllerTestProperties : ModulePropertiesBase
    {
        public ControllerTestProperties() : base("Controller Test", "Modul zum Testen des Controllers und der Steuerung", true, false, false, false, false, false)
        {
            SetProperty(base.KeyImagePath, @"C:\Users\ITM1\source\repos\Surrogate\Surrogate\resources\xbox_controller_icon.jpg");
        }
    }

    public class ControllerTestInfo : ModuleInfo
    {
        private readonly TestCase _testCase;
        public TestCase Case { get => _testCase; }

        public enum TestCase
        {
            ControllerOutput,
            MotorOutput,
            ControllRobot
        }

        /// <summary>
        /// Information about the test case
        /// </summary>
        /// <param name="testCase">Specifies the test case (Controll robot, print contorller values, print motor speed values</param>
        public ControllerTestInfo(TestCase testCase = TestCase.ControllRobot)
        {
            _testCase = testCase;
        }
    }
}

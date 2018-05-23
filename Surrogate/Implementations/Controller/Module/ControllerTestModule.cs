// This file belongs to the source code of the "Surrogate Project"
// Copyright (c) 2018 All Rights Reserved
// Martin-Luther-Universitaet Halle-Wittenberg
// Lehrstuhl Wirtschaftsinformatik und Operation Research
// Autor: Wimmer, Simon-Justus Wimmer



namespace Surrogate.Implementations
{
    using System;
    using System.Threading;
    using System.Runtime.InteropServices;
    using System.Windows.Controls;
    using Surrogate.Modules;
    using Surrogate.Roboter.MController;
    using Surrogate.View;
    using System.Threading.Tasks;

    using Surrogate.Utils;
    using log4net;
    using System.Windows;
    using System.Windows.Threading;
    using SharpDX.XInput;
    using Surrogate.Roboter.MMotor;
    using Surrogate.Utils.Event;

    public class ControllerTestModule : VisualModule<ModuleProperties, ControllerTestInfo>
    {
        public event EventHandler<BooleanEventArgs> MotorAvailable;
        public event EventHandler<BooleanEventArgs> ControllerAvailable;
        public event EventHandler<BooleanEventArgs> IsRunningChanged;

        private volatile bool _shouldStop = false;
        private volatile bool searchController = true;
        private Motor _motor;
        private readonly XInputController controller = new XInputController();

        public ControllerTestModule(ModuleProperties modulProperties) : base(modulProperties)
        {
            ModuleSelected += Selected;
            ModuleDisselected += Disselected;
        }

        private async void SearchController()
        {
            {
                OnControllerAvailableChanged(controller.connected);
                await Task.Run(() =>
                {
                    bool lastState = controller.connected;
                    while (searchController)
                    {
                        
                        if (controller.connected && lastState != true)
                        { 
                            OnControllerAvailableChanged(controller.connected);
                        }
                        else if (!controller.connected && lastState == true)
                        {
                            OnControllerAvailableChanged(controller.connected);
                        }
                        lastState = controller.connected;
                        Thread.Sleep(1000);
                    }
                });
            }
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

        public override ContentControl GetPage()
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

            switch(info.Case)
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

        private void TestMotorControl(XInputController controller, bool simulate = false)
        {
            OnIsRunngingChanged(true);
            try
            {
                if (!simulate)
                {
                    _motor.Start();
                }
                Tuple<int, int> speedValues = Tuple.Create(0, 0);
                while (!_shouldStop)
                {
                    if (controller.buttons.Equals(GamepadButtonFlags.A))
                    {
                        _motor.PullUp();
                        Stop();
                    }
                    Tuple<int, int> nextSpeedValues = CalculateSpeedValues(controller);
                    // only update if values changed
                    {
                        speedValues = nextSpeedValues;
                        _motor.LeftSpeed = (speedValues.Item1);
                        _motor.RightSpeed = (speedValues.Item2);
                            if (simulate)
                            {
                                System.Diagnostics.Debug.WriteLine(String.Format("Leftspeed: {0}, Rightspeed: {1}", speedValues.Item1, speedValues.Item2));
                            }
                    }
                }
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
        private Tuple<int,int> CalculateSpeedValues(XInputController controller)
        {
            controller.Update();
            float rightTrigger = controller.rightTrigger;
            float leftTrigger = controller.leftTrigger;
            double rightThumbX = controller.rightThumb.X;
            int leftspeed = 0;
            int rightspeed = 0;


            // calculate speed based on left and right triggers (right trigger forward, left trigger backward)
            int tempSpeed = (int)((rightTrigger - leftTrigger) / 255 * 100);

            if(rightThumbX > 0)
            {
                double multiplier = rightThumbX / 100;
                leftspeed = (int)(tempSpeed + multiplier * 100);
                rightspeed = (int)(tempSpeed - multiplier * 100);
            } else if(rightThumbX < 0)
            {
                double multiplier = rightThumbX / -100;
                leftspeed = (int)(tempSpeed + multiplier * 100);
                rightspeed = (int)(tempSpeed - multiplier * 100);
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
        private bool TestConrollerOutputs(XInputController controller)
        {
            OnIsRunngingChanged(true);
            while (!_shouldStop)
            {
                Thread.Sleep(200); // give console some time after each iteration
                controller.Update();
                Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Normal, new Action(() => {
                    log.Info("\nLeft Thumb X: "+controller.leftThumb.X + " " + "\nLeft Thumb Y: " + controller.leftThumb.Y+ "\n"+
                        "A?: "+ controller.buttons.Equals(GamepadButtonFlags.A)+ "\n" +
                        "Rigth Thumb X: " + controller.rightThumb.X + "\n" +
                        "Rigth Thumb Y: " + controller.rightThumb.Y + "\n" + 
                        "Left Trigger: " + controller.leftTrigger + "\n" + 
                        "Right Trigger: " + controller.rightTrigger);}));
                //Console.WriteLine(controller.leftThumb + " " + controller.rightThumb + " " + controller.leftTrigger + " " + controller.rightTrigger);
            }
            log.Info("Controller test ended");
            OnIsRunngingChanged(false);
            return true;
        }

        private void Selected(Object sender, EventArgs e)
        {
            _motor = Motor.GetInstance();
            SearchController();
            FireChangeEvents();
        }

        public void Disselected(Object sender, EventArgs e)
        {
            searchController = false;
            OnControllerAvailableChanged(false);
            OnMotorAvailableChanged(false);
        }

        private void FireChangeEvents()
        {
            OnMotorAvailableChanged(_motor.IsReady());
            OnControllerAvailableChanged(controller.connected);
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

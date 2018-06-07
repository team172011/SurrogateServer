// This file belongs to the source code of the "Surrogate Project"
// Copyright (c) 2018 All Rights Reserved
// Martin-Luther-Universitaet Halle-Wittenberg
// Lehrstuhl Wirtschaftsinformatik und Operation Research
// Autor: Wimmer, Simon-Justus Wimmer

using System;
using System.Windows;
using System.Windows.Threading;
using SharpDX.XInput;
using Surrogate.Implementations;
using Surrogate.Model;

namespace Surrogate.Roboter.MController
{
    /// <summary>
    /// Class to connect and receive controller inputs
    /// </summary>
    class XBoxController : AbstractConnection
    {
        private readonly SharpDX.XInput.Controller controller = new SharpDX.XInput.Controller(UserIndex.One);
        private Gamepad gamepad;
        public bool Connected { get => controller.IsConnected; }

        public override string Name => FrameworkConstants.ControllerName;

        public int deadband = 2500;
        public Point leftThumb, rightThumb = new Point(0, 0);
        public float leftTrigger, rightTrigger;
        public GamepadButtonFlags buttons;
        public volatile bool search = true;

        /// <summary>
        /// Creates a Surrogate wrapper instance to connect to an xbox 
        /// </summary>
        public XBoxController()
        {
            DispatcherTimer searcher = new DispatcherTimer();
            searcher.Tick += Connect;
            searcher.Interval = new TimeSpan(0, 0, 0, 1);
            searcher.Start();
        }
        

        /// <summary>
        /// Function search for a Controller. This function is not really "searching" but checking if the SharpDX.Input
        /// api has found a controller and set the Status property.
        /// </summary>


        // Call this method to update all class values
        public void Update()
        {
            if (!Connected)
                return;

            gamepad = controller.GetState().Gamepad;
            leftThumb.X = (Math.Abs((float)gamepad.LeftThumbX) < deadband) ? 0 : (float)gamepad.LeftThumbX / short.MinValue * -100;
            leftThumb.Y = (Math.Abs((float)gamepad.LeftThumbY) < deadband) ? 0 : (float)gamepad.LeftThumbY / short.MaxValue * 100;
            rightThumb.X = (Math.Abs((float)gamepad.RightThumbX) < deadband) ? 0 : (float)gamepad.RightThumbX / short.MaxValue * 100;
            rightThumb.Y = (Math.Abs((float)gamepad.RightThumbY) < deadband) ? 0 : (float)gamepad.RightThumbY / short.MaxValue * 100;

            leftTrigger = gamepad.LeftTrigger;
            rightTrigger = gamepad.RightTrigger;
            buttons = gamepad.Buttons;
        }

        public BatteryInformation GetBatteryInformation()
        {
            return controller.GetBatteryInformation(BatteryDeviceType.Gamepad);
        }

        public override bool Connect()
        {
            if (controller.IsConnected)
            {
                Status = ConnectionStatus.Ready;
                return true;
            }

            Status = ConnectionStatus.Disconnected;
            return false;
        }

        /// <summary>
        /// Connect function for a event handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <returns></returns>
        public void Connect(object sender, EventArgs e)
        {
            Connect();
        }

        public override bool Disconnect()
        {
            return false;
        }
    }
}

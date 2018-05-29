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
        SharpDX.XInput.Controller controller = new SharpDX.XInput.Controller(UserIndex.One);
        Gamepad gamepad;
        public bool Connected { get => controller.IsConnected; }

        public override string Name => FrameworkConstants.ControllerName;

        public int deadband = 2500;
        public Point leftThumb, rightThumb = new Point(0, 0);
        public float leftTrigger, rightTrigger;
        public GamepadButtonFlags buttons;


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
            bool connected = false;
            if(controller != null)
            {
                connected = controller.IsConnected;
                if (connected)
                {
                    Status = ConnectionStatus.Ready;
                }
            }

            else
            {
                Status = ConnectionStatus.Disconnected;
            }
            return connected;
        }
    }
}

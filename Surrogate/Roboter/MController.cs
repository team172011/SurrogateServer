using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using SharpDX.XInput;

namespace Surrogate.Roboter.MController
{
    /// <summary>
    /// Class to connect and receive controller inputs
    /// </summary>
    class XInputController
    {
        Controller controller;
        Gamepad gamepad;
        public bool connected = false;
        public int deadband = 2500;
        public Point leftThumb, rightThumb = new Point(0, 0);
        public float leftTrigger, rightTrigger;

        public XInputController()
        {
            controller = new Controller(UserIndex.One);
            connected = controller.IsConnected;
        }

        // Call this method to update all class values
        public void Update()
        {
            if (!connected)
                return;

            gamepad = controller.GetState().Gamepad;

            leftThumb.X = (Math.Abs((float)gamepad.LeftThumbX) < deadband) ? 0 : (float)gamepad.LeftThumbX / short.MinValue * -100;
            leftThumb.Y = (Math.Abs((float)gamepad.LeftThumbY) < deadband) ? 0 : (float)gamepad.LeftThumbY / short.MaxValue * 100;
            rightThumb.Y = (Math.Abs((float)gamepad.RightThumbX) < deadband) ? 0 : (float)gamepad.RightThumbX / short.MaxValue * 100;
            rightThumb.X = (Math.Abs((float)gamepad.RightThumbY) < deadband) ? 0 : (float)gamepad.RightThumbY / short.MaxValue * 100;

            leftTrigger = gamepad.LeftTrigger;
            rightTrigger = gamepad.RightTrigger;
        }
    }
}

// This file belongs to the source code of the "Surrogate Project"
// Copyright (c) 2018 All Rights Reserved
// Martin-Luther-Universitaet Halle-Wittenberg
// Lehrstuhl Wirtschaftsinformatik und Operation Research
// Autor: Wimmer, Simon-Justus Wimmer (simonjustuswimmer@googlemail.com)
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
    public class XBoxController : AbstractConnection, IRemoteController
    {
        private static XBoxController _instance;
        public static XBoxController Instance {
            get
            {
                if (_instance == null)
                {
                    _instance = new XBoxController();
                }
                return _instance;
            }
        }
        public Point LeftThumb { get => _leftThumb; }
        public Point RightThumb { get => _rightThumb; }
        public GamepadButtonFlags Buttons { get => _buttons; }
        public float LeftTrigger { get => _leftTrigger; }
        public float RightTrigger { get => _rightTrigger; }
        public override string Name => FrameworkConstants.ControllerName;
        public volatile bool search = true;

        private readonly SharpDX.XInput.Controller controller = new SharpDX.XInput.Controller(UserIndex.One);
        public bool Connected { get => controller.IsConnected; }
        private Point _leftThumb, _rightThumb = new Point(0, 0);
        private GamepadButtonFlags _buttons;
        private float _leftTrigger, _rightTrigger;

        /// <summary>
        /// Creates a Surrogate wrapper instance to connect to an xbox 
        /// </summary>
        private XBoxController()
        {
            DispatcherTimer searcher = new DispatcherTimer();
            searcher.Tick += Connect;
            searcher.Interval = new TimeSpan(0, 0, 0, 1);
            searcher.Start();
        }
        
        /// <summary>
        /// Updates the class fields representing the controller state.
        /// </summary>
        public void Update()
        {
            if (!Connected)
                return;
            int _deadband = 2500;
            Gamepad gamepad = controller.GetState().Gamepad;
            _leftThumb.X = (Math.Abs((float)gamepad.LeftThumbX) < _deadband) ? 0 : (float)gamepad.LeftThumbX / short.MinValue * -100;
            _leftThumb.Y = (Math.Abs((float)gamepad.LeftThumbY) < _deadband) ? 0 : (float)gamepad.LeftThumbY / short.MaxValue * 100;
            _rightThumb.X = (Math.Abs((float)gamepad.RightThumbX) < _deadband) ? 0 : (float)gamepad.RightThumbX / short.MaxValue * 100;
            _rightThumb.Y = (Math.Abs((float)gamepad.RightThumbY) < _deadband) ? 0 : (float)gamepad.RightThumbY / short.MaxValue * 100;

            _leftTrigger = gamepad.LeftTrigger;
            _rightTrigger = gamepad.RightTrigger;
            _buttons = gamepad.Buttons;
        }

        public BatteryInformation GetBatteryInformation()
        {
            return controller.GetBatteryInformation(BatteryDeviceType.Gamepad);
        }

        /// <summary>
        /// Function searching for a Controller. This function is not really "searching" but checking if the SharpDX.Input
        /// api has found a controller and set the Status property.
        /// </summary
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
        /// Connect function for an event handler
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

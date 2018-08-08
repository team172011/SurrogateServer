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
    public class EmergencyStopProcess : BackgroundWorker
    {
        private readonly IMotorConnection _motor;
        private readonly IRemoteController _controller;
        protected static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public EmergencyStopProcess(IMotorConnection motor, IRemoteController controller){
            WorkerSupportsCancellation = true;
            _motor = motor;
            _controller = controller;
            DoWork += Work;
        }

        public void Work(object sender, DoWorkEventArgs e)
        {
            log.Debug("EmergencyStopProcess started");
            if (_motor.Status == ConnectionStatus.Ready)
            {
                Motor.Instance.Start();
                while (!CancellationPending)
                {
                    if (_controller.Buttons == GamepadButtonFlags.X)
                    {
                        Motor.Instance.Kill();
                    }
                }
            }
            log.Debug("EmergencyStopProcess ended");

        }
    }
}

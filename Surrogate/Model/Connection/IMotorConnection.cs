// This file belongs to the source code of the "Surrogate Project"
// Copyright (c) 2018 All Rights Reserved
// Martin-Luther-Universitaet Halle-Wittenberg
// Lehrstuhl Wirtschaftsinformatik und Operation Research
// Autor: Wimmer, Simon-Justus Wimmer (simonjustuswimmer@googlemail.com)
using Surrogate.Model;

namespace Surrogate.Roboter.MMotor
{
    /// <summary>
    /// Representing an interface to a crawler type vehicles motor
    /// </summary>
    public interface IMotorConnection : IConnection
    {
        /// <summary>
        /// Determines the speed value of the left wheels
        /// </summary>
        int LeftSpeedValue { get; set; }

        /// <summary>
        /// Determines the speed value of the right wheels
        /// </summary>
        int RightSpeedValue { get; set; }

        /// <summary>
        /// Starts the motor. After calling this method, the motor should be ready
        /// to process <see cref="LeftSpeedValue"/> and <see cref="RightSpeedValue"/> commands
        /// </summary>
        /// <param name="simulation">if true no motor connection will be established and the motor commands
        /// will be printed into the log</param>
        void Start(bool simulation);

        /// <summary>
        /// Stops the motor
        /// </summary>
        void Stop();

        /// <summary>
        /// 
        /// </summary>
        /// <returns>true if the motor is ready to process <see cref="LeftSpeedValue"/> and <see cref="RightSpeedValue"/> commands</returns>
        bool IsReady();

        /// <summary>
        /// Stops the vehicle in the next possible moment
        /// </summary>
        void PullUp();

        /// <summary>
        /// Kills the motor. This function should try to stop the motor, stop the motor thread and
        /// disconnect from the motor
        /// </summary>
        void Kill();
    }
}
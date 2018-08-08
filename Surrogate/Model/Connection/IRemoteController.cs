// This file belongs to the source code of the "Surrogate Project"
// Copyright (c) 2018 All Rights Reserved
// Martin-Luther-Universitaet Halle-Wittenberg
// Lehrstuhl Wirtschaftsinformatik und Operation Research
// Autor: Wimmer, Simon-Justus Wimmer (simonjustuswimmer@googlemail.com)
using System.Windows;
using SharpDX.XInput;
using Surrogate.Model;

namespace Surrogate.Roboter.MController
{
    public interface IRemoteController : IConnection
    {
        GamepadButtonFlags Buttons { get; }
        Point LeftThumb { get; }
        float LeftTrigger { get; }
        Point RightThumb { get; }
        float RightTrigger { get; }
        void Update();
        BatteryInformation GetBatteryInformation();
    }
}
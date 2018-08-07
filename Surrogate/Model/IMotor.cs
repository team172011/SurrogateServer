// This file belongs to the source code of the "Surrogate Project"
// Copyright (c) 2018 All Rights Reserved
// Martin-Luther-Universitaet Halle-Wittenberg
// Lehrstuhl Wirtschaftsinformatik und Operation Research
// Autor: Wimmer, Simon-Justus Wimmer (simonjustuswimmer@googlemail.com)
using Surrogate.Model;
using System;

namespace Surrogate.Roboter.MMotor
{
    public interface IMotor : IConnection
    {
        int LeftSpeedValue { get; set; }
        int RightSpeedValue { get; set; }

        void Start(bool simulation);
        void Stop();
        bool IsReady();
        void PullUp();
        void Kill();
    }
}
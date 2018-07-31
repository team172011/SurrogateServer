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
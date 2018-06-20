using System;

namespace Surrogate.Roboter.MMotor
{
    public interface IMotor
    {
        byte LeftSpeedValue { get; set; }
        byte RightSpeedValue { get; set; }

        void Start(bool simulation);
        void Stop();
        bool IsReady();
        void PullUp();
        void Kill();
    }
}
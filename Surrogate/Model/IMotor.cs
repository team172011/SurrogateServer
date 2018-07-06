using System;

namespace Surrogate.Roboter.MMotor
{
    public interface IMotor
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
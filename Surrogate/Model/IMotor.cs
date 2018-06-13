using System;

namespace Surrogate.Roboter.MMotor
{
    public interface IMotor
    {
        int LeftSpeed { get; set; }
        int RightSpeed { get; set; }

        event EventHandler<EventArgs> SpeedChanged;

        void Start(bool simulation);
        void Stop();
        bool IsReady();
        void PullUp();
        void Kill();
    }
}
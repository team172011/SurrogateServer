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
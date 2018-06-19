using Surrogate.Controller;
using Surrogate.Implementations.Controller;
using Surrogate.Model;
using System;

namespace Surrogate.Implementations
{
    /// <summary>
    /// Static class. Representing the Surrogate Framework API.
    /// </summary>
    public static class SurrogateFramework
    {
        private static readonly MainController _controller = new MainController();
        public static MainController MainController { get => _controller; }
        public static double two = 2;

        public static void AddModule(IController module)
        {
            _controller.ModulHandler.AddModule(module);

            // if module is connection, register the connection
            if (module is IConnection connection)
            {
                AddConnection(connection);
            }
            
        }

        public static void AddConnection(IConnection connection)
        {
            _controller.ConnectionHandler.RegisterConnection(connection.Name, connection);
        }
    }

    public static class FrameworkConstants
    {
        public static readonly string MotorName = "Motor";
        public static readonly string DatabaseName = "Datenbank";
        public static readonly string InternetName = "Internet";
        public static readonly string ControllerName = "Controller";
        public static readonly string TouchpadName = "Touchpad";
        public static readonly string Camera1Name = "Kamera 1";
        public static readonly string Camera2Name = "Kamera 2";

        public static readonly string DefaultImagePath = @"C:\Users\ITM1\source\repos\Surrogate\Surrogate\resources\robot.jpg";

        public static readonly string InternalCameraName = "Interne Kamera";

        public static readonly int Numbercams = 3;
    }
}

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

        public static void AddModule(IModule module)
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
        public static readonly String MotorName = "Motor";
        public static readonly String DatabaseName = "Datenbank";
        public static readonly String InternetName = "Internet";
        public static readonly String ControllerName = "Controller";
        public static readonly String TouchpadName = "Touchpad";
    }
}

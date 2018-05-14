

namespace Surrogate.Implementations
{
    using System;
    using System.Threading;
    using System.Runtime.InteropServices;
    using System.Windows.Controls;
    using Surrogate.Modules;
    using Surrogate.Roboter.MController;
    using Surrogate.View;
    using System.Threading.Tasks;

    using Surrogate.Utils;
    using log4net;
    using System.Windows;
    using System.Windows.Threading;

    public class ControllerTestModule : Module<ModulProperties, ControllerTestInfo>
    {

        private volatile bool _shouldStop = false;

        public ControllerTestModule(ModulProperties modulProperties) : base(modulProperties)
        {
        }

        public override ContentControl GetPage()
        {
            return new ControllerTestView(this);
        }

        public override void Stop()
        {
            this._shouldStop = true;
        }

        /// <summary>
        /// Start the ControllerTest
        /// runs synchronously until it hits an “await” (or throws an exception).
        /// </summary>
        /// <param name="info"></param>
        public override async void Start(ControllerTestInfo info)
        {
            if (_shouldStop)
            {
                return;
            }

            XInputController controller = new XInputController();
            bool isConnected = controller.connected;
            if (!isConnected)
            {
                log.Info("No Controller connected!");
                return;
            }
            log.Info("Controller connected!");
            controller.Update();

            if (!info.IsSimulation)
            {
                await Task.Run(() => TestingMotorSimulation(controller));
            }
            bool i = await Task.Run(() => TestingSimulation(controller));
            
        }

        private void TestingMotorSimulation(XInputController controller)
        {
            throw new NotImplementedException();
        }

        private bool TestingSimulation(XInputController controller)
        {            
            while (!_shouldStop)
            {
                Thread.Sleep(100); // give console some time after each iteration
                controller.Update();
                Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Normal, new Action(() => { log.Info(controller.leftThumb + " " + controller.rightThumb + " " + controller.leftTrigger + " " + controller.rightTrigger);}));
                //Console.WriteLine(controller.leftThumb + " " + controller.rightThumb + " " + controller.leftTrigger + " " + controller.rightTrigger);
            }
            log.Info("Controller test ended");
            return true;
        }
    }

    public class ControllerTestInfo : ModuleInfo
    {
        public readonly bool IsSimulation;

        /// <summary>
        /// Information about the test case
        /// </summary>
        /// <param name="isSimulation">If true the controller input will not be routed to the motor, but printed on the log</param>
        public ControllerTestInfo(bool isSimulation)
        {
            this.IsSimulation = isSimulation;
        }
    }
}

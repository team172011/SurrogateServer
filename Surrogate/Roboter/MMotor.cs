using log4net;
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace Surrogate.Roboter.MMotor
{
    /// <summary>
    /// Class for connecting and controlling the motor via serial usb port
    /// Singeton pattern, get instance with <see cref="GetInstance"/> function.
    /// </summary>
    public class Motor
    {

        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType); //TODO: remove
        private volatile bool _shouldStop = false;
        private static Motor _instance;
        private static readonly int _commandStart = 0;
        private static readonly int _commandEnd = 6;

        /// <summary>
        /// This field is used by the motor thread to create the speed commands for the left wheels
        /// </summary>        
        private static volatile int _leftSpeedValue;
        /// <summary>
        /// This fields are used by the motor thread to create the speed commands for the right wheels
        /// </summary>
        private static volatile int _rightSpeedValue;

        /// <summary>
        /// Reference to the motor thread
        /// </summary>
        private System.Threading.Thread _motorThread;

        /// <summary>
        /// Enum for storing the codes for each motor command.
        /// </summary>
        public enum Motors
        {
            M1F = 11, // Motor 1 forwards
            M1B = 12, // Motor 2 backwards..
            M2F = 22,
            M2B = 21,
            M3F = 31,
            M3B = 32,
            M4F = 42,
            M4B = 41
        }

        /// <summary>
        /// Private Consturctor.
        /// Starting the motor thread (see <see cref="Start"/> and <see cref="RunEngine(SerialPort)"/>)
        /// </summary>
        private Motor()
        {
            Start();
        }

        /// <summary>
        /// Returns the instance representing the motor. During first initialization of this instance a
        /// thread responsible for sending motor controlls will be started. (See also Constructor: <see cref="Motor"/>)
        /// </summary>
        /// <exception cref="PortNotValidException">If ther is no port connected to the motor controller</exception>
        /// <returns>The Instance representing the motor api</returns>
        public static Motor GetInstance()
        {
            if (_instance != null)
            {
                return _instance;
            }
            _instance = new Motor();
            return _instance;
        }

        private void Start()
        {
            string[] ports = SerialPort.GetPortNames();
            if(ports.Length <= 0)
            {
                throw new Exception("No serial port found!");
            }
            foreach (var p in ports)
            {
                Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Normal, new Action(() =>
                {
                    log.Debug("Port found: " + p);
                }));
            }
            SerialPort port = new SerialPort();
            port.PortName = ports[0];
            port.BaudRate = 115200;
            port.Parity = Parity.None;
            port.StopBits = StopBits.One;
            port.DataBits = 8;
            port.Handshake = Handshake.None;
            port.Open();
            if (port.IsOpen != true)
            {
                throw new PortNotValidException(port);
            }

            Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Normal, new Action(() =>
            {
                log.Debug("Starting motor thread");
            }));
            
            this._motorThread = new System.Threading.Thread(() => RunEngine(port));
            _motorThread.Start();

            }

        /// <summary>
        /// After the serial port to the controller as been opened (<see cref="Start"/>) this
        /// function can be used to write commands to the cagebot controller 
        /// </summary>
        /// <param name="port"> The SerialPort class representing the usb port</param>
        private void RunEngine(SerialPort port)
        {
            while (!_shouldStop)
            {
                // create the micro commands first
                byte[] m1 = GetM1Command();
                byte[] m2 = GetM2Command();
                byte[] m3 = GetM3Command();
                byte[] m4 = GetM4Command();

                // write micro commands afap to the serial port
                port.Write(m1, _commandStart, _commandEnd);
                port.Write(m2, _commandStart, _commandEnd);
                port.Write(m3, _commandStart, _commandEnd);
                port.Write(m4, _commandStart, _commandEnd);
            }
        }

        /*****helper functions to build micro commands of byte arrays for the four motors*******/
        private byte[] GetM4Command()
        {
            int speed = _rightSpeedValue;
            if (speed > 0)
            {
                return BuildCommand(Motors.M4F, speed);
            }
            else if (speed < 0)
            {
                return BuildCommand(Motors.M4B, Math.Abs(speed));
            }
            return BuildCommand(Motors.M4F, 0);
        }

        private byte[] GetM3Command()
        {
            int speed = _leftSpeedValue;
            if (speed > 0)
            {
                return BuildCommand(Motors.M3F, speed);
            }
            else if (speed < 0)
            {
                return BuildCommand(Motors.M3B, Math.Abs(speed));
            }
            return BuildCommand(Motors.M3F, 0);
        }

        private byte[] GetM2Command()
        {
            int speed = _rightSpeedValue;
            if (speed > 0)
            {
                return BuildCommand(Motors.M2F, speed);
            }
            else if (speed < 0)
            {
                return BuildCommand(Motors.M2B, Math.Abs(speed));
            }
            return BuildCommand(Motors.M2F, 0);
        }

        private byte[] GetM1Command()
        {
            int speed = _leftSpeedValue;
            if(speed > 0)
            {
                return BuildCommand(Motors.M1F, speed);
            }
                else if (speed < 0)
            {
                return BuildCommand(Motors.M1B, Math.Abs(speed));
            }
            return BuildCommand(Motors.M1F, 0);
        }

        private byte[] BuildCommand(Motors motorId, int speedValue)
        {
            return new byte[]{ 1, 249, 3, 6, (byte)((int)motorId), (byte)speedValue  };
        }

        /*****************************************************************************************/

        /// <summary>
        /// Setter for the left speed value
        /// </summary>
        /// <param name="speedValue"></param>
        public void SetLeftSpeed(int speedValue)
        {
            _leftSpeedValue = speedValue;
        }

        /// <summary>
        /// Setter for the right speed value
        /// </summary>
        /// <param name="speedValue"></param>
        public void SetRightSpeed(int speedValue)
        {
            _rightSpeedValue = speedValue;
        }

        public void PullUp()
        {
            SetLeftSpeed(0);
            SetRightSpeed(0);
        }

        public static void Kill()
        {
            if (_instance == null)
            {
                return;
            }
            Motor instance = _instance;
            instance._motorThread.Abort();
        }

        public bool isMotorThreadAlive()
        {
            return _motorThread.IsAlive && _motorThread.ThreadState == System.Threading.ThreadState.Running;
        }

    }

    public class PortNotValidException : Exception
    {
        public PortNotValidException(SerialPort port):base(BuildMessage(port))
        {

        }

        private static string BuildMessage(SerialPort port)
        {
            return String.Format("Could not open port with name:{0}  (baud rate: {1}",
                port.PortName, port.BaudRate);
        }
    }
}

// This file belongs to the source code of the "Surrogate Project"
// Copyright (c) 2018 All Rights Reserved
// Martin-Luther-Universitaet Halle-Wittenberg
// Lehrstuhl Wirtschaftsinformatik und Operation Research
// Autor: Wimmer, Simon-Justus Wimmer (simonjustuswimmer@googlemail.com)
using Surrogate.Implementations;
using Surrogate.Model;
using System;
using System.IO.Ports;
using System.Text;
using System.Windows;
using System.Windows.Threading;

namespace Surrogate.Roboter.MMotor
{
    /// <summary>
    /// Class for connecting and controlling the motor via connection to a serial usb port
    /// Singeton pattern, get instance with <see cref="GetInstance"/> function.
    /// </summary>
    public class Motor : AbstractConnection, IMotorConnection
    {
        private static volatile Motor _instance;
        private static readonly object syncRoot = new Object();

        #region singleton property
        public static Motor Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (syncRoot)
                    {
                        if (_instance == null)
                        {
                            _instance = new Motor();
                        }

                    }
                }
                return _instance;
            }
        }
        #endregion singleton property

        private volatile bool _shouldStop = false;
        private SerialPort port;   
        private volatile int _leftSpeedValue;
        private volatile int _rightSpeedValue;
        private bool _isSimulation = false;

        /// <summary>
        /// A timer thread is used to send current left speed and right speed values to
        /// the motor every 100 milliseconds (<seealso cref="RunEngine(object)"/>. This guarantees
        /// that there will be no queue for motor commands.
        /// </summary>
        private System.Threading.Timer Timer;

        public override string Name => FrameworkConstants.MotorName;

        public override ConnectionStatus Status
        {
            get
            {
                if (IsReady())
                {
                    return ConnectionStatus.Ready;
                }
                else
                {
                    return ConnectionStatus.Disconnected;
                }
            }
        }

        public int RightSpeedValue { get => _rightSpeedValue; set => _rightSpeedValue = value; }
        public int LeftSpeedValue { get => _leftSpeedValue; set => _leftSpeedValue = value; }

        /// <summary>
        /// Private Consturctor.
        /// </summary>
        private Motor()
        {
        }

        /// <summary>
        /// Enum for storing the codes for each motor command.
        /// </summary>
        public enum Motors
        {
            M1B = 11, // Motor 1 forwards
            M1F = 12, // Motor 2 backwards..
            M2F = 22,
            M2B = 21,
            M3F = 31,
            M3B = 32,
            M4B = 42,
            M4F = 41
        }

        /// <summary>
        /// Search for the serial port and connect to the motor controller (if not simulation)
        /// Starting the motor thread (see <see cref="Start"/> and <see cref="RunEngine(SerialPort)"/>)
        /// </summary>
        /// <param name="simulation"></param>
        public void Start(bool simulation = false)
        {
            _shouldStop = false;
            Timer?.Dispose();
            Stop();
            if (simulation)
            {
                Timer = new System.Threading.Timer(SimulateEngine,null, 0, 1);
            } else if (Connect() ){
                Timer = new System.Threading.Timer(RunEngine, null, 0, 1);
                
            } else
            {
                Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Normal, new Action(() => MessageBox.Show("Der Motor ist nicht mehr erreichbar", "Motorfehler", MessageBoxButton.OK)));
            }
            log.Debug("Motor started. Simulation: "+simulation);
        }

        public void Stop()
        {
            _shouldStop = true;
            Timer?.Dispose();
        }

        public bool Connect(int portId = 1)
        {
            if (port != null)
            {
                if (port.IsOpen)
                {
                    return true; // already connected to port
                } else
                {
                    port.Close();
                }
            }
            port = new SerialPort();
            {
                string[] ports = SerialPort.GetPortNames();
                if (ports.Length <= 0)
                {
                    return false; // no port found
                }

                port.PortName = ports[0];
                port.BaudRate = 115200;
                port.Parity = Parity.None;
                port.StopBits = StopBits.One;
                port.DataBits = 8;
                port.Handshake = Handshake.None;
                port.DataReceived += OnDataReceived;
                port.Encoding = Encoding.Unicode; 
                port.Open();
                
            }
            return IsReady();
        }

        private void OnDataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            //log.Debug(port.ReadExisting());
        }

        public bool IsReady()
        {
            return port.IsOpen;
        }

        private void SimulateEngine(object state)
        {
            while (!_shouldStop)
            {
                System.Threading.Thread.Sleep(200); // give gui some time...
                byte[] m1 = GetM1Command();
                byte[] m2 = GetM2Command();
                byte[] m3 = GetM3Command();
                byte[] m4 = GetM4Command();
                Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Normal, new Action(() =>
                {
                    log.Info("m1" + m1[4]+" "+ m1[5]);
                    log.Info("m2" + m2[4]+" "+ m2[5]);
                    log.Info("m3" + m3[4]+" "+ m3[5]);
                    log.Info("m4" + m4[4]+" "+ m4[5]);

                }));
            }
        }

        /// <summary>
        /// After the serial port to the controller as been opened (<see cref="Start"/>) this
        /// function can be used to write commands to the cagebot controller 
        /// </summary>
        /// <param name="port"> The SerialPort class representing the usb port</param>
        private void RunEngine(object state)
        {
            //while (!_shouldStop)
            {
                // create micro commands
                byte[] m1 = GetM1Command();
                byte[] m2 = GetM2Command();
                byte[] m3 = GetM3Command();
                byte[] m4 = GetM4Command();
                try
                {
                    port.Write(m1, 0, 6);
                    port.Write(m2, 0, 6);
                    port.Write(m3, 0, 6);
                    port.Write(m4, 0, 6);
                }
                // If Motor is not available, show a message
                catch (System.InvalidOperationException ioe)
                {
                    Stop();
                    Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Normal, new Action(() => MessageBox.Show("Der Motor ist nicht mehr erreichbar","Motorfehler", MessageBoxButton.OK)));
                }

            }
        }

        /***** helper functions to build micro commands of byte arrays *******/
        private byte[] GetM4Command()
        {
            int speed = _leftSpeedValue;
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
            int speed = RightSpeedValue;
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
            int speed = RightSpeedValue;
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
            return new byte[]{1, 249, 3, 6, (byte)((int)motorId), (byte)speedValue};
        }

        /*****************************************************************************************/

        public void PullUp()
        {
            LeftSpeedValue = 0;
            RightSpeedValue = 0;
        }

        public void Kill()
        {
            _shouldStop = true;
            _instance?.port?.Close();
        }

        public bool IsConnected()
        {
            return IsReady();
        }

        public override bool Connect()
        {
            return Connect(0);
        }

        public override bool Disconnect()
        {
            try
            {
                port?.Close();
                return true;
            } catch(System.IO.IOException ioe)
            {
                log.Error("Verbindung zum Motor konnte nicht getrennt werden: " + ioe.Message);
                return false;
            }
            
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class PortNotValidException : Exception
    {
        public PortNotValidException(SerialPort port):base(BuildMessage(port))
        {

        }

        private static string BuildMessage(SerialPort port)
        {
            return String.Format("A Problem occured with port with name:{0}  (baud rate: {1})",
                port.PortName, port.BaudRate);
        }
    }

}

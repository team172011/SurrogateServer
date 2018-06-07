// This file belongs to the source code of the "Surrogate Project"
// Copyright (c) 2018 All Rights Reserved
// Martin-Luther-Universitaet Halle-Wittenberg
// Lehrstuhl Wirtschaftsinformatik und Operation Research
// Autor: Wimmer, Simon-Justus Wimmer

using Surrogate.Implementations;
using Surrogate.Model;
using System;
using System.IO.Ports;
using System.Windows;
using System.Windows.Threading;

namespace Surrogate.Roboter.MMotor
{
    /// <summary>
    /// Class for connecting and controlling the motor via serial usb port
    /// Singeton pattern, get instance with <see cref="GetInstance"/> function.
    /// </summary>
    public class Motor : AbstractConnection
    {
        private static volatile Motor _instance;
        private static object syncRoot = new Object();
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

        private volatile bool _shouldStop = false;
        private SerialPort port;   
        private volatile int _leftSpeedValue;
        private volatile int _rightSpeedValue;
        /// <summary>
        /// This field is used by the motor thread to create the speed commands for the left wheels
        /// </summary>        
        public int LeftSpeed
        {
            set
            {
                _leftSpeedValue = value;
                OnSpeedChanged();
            }
            get
            {
                return _leftSpeedValue;
            }
        }
        /// <summary>
        /// This fields are used by the motor thread to create the speed commands for the right wheels
        /// </summary>
        public int RightSpeed
        {
            set
            {
                _rightSpeedValue = value;
                OnSpeedChanged();
            }
            get
            {
                return _rightSpeedValue;
            }
        }

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

        /// <summary>
        /// This event handler will be called if one speed value changes
        /// </summary>
        public event EventHandler SpeedChanged;

        /// <summary>
        /// Private Consturctor.
        /// </summary>
        private Motor()
        {
            Connect();
        }

        public virtual void OnSpeedChanged()
        {
            SpeedChanged?.Invoke(this, EventArgs.Empty);
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
            if (simulation)
            {
                SpeedChanged += SimulateEngine;
            } else if (Connect()){
                SpeedChanged += RunEngine;
            } else
            {
                throw new Exception("Could not start motor");
            }
        }

        public bool Connect(int portId = 0)
        {
            port = new SerialPort();
            {
                string[] ports = SerialPort.GetPortNames();
                if (ports.Length <= 0)
                {
                    return false; // no possible port found
                }

                port.PortName = ports[portId];
                port.BaudRate = 115200;
                port.Parity = Parity.None;
                port.StopBits = StopBits.One;
                port.DataBits = 8;
                port.Handshake = Handshake.None;
                /*********************************************************************************************************/
                //port.Open();
            }
            return IsReady();
        }

        public bool IsReady()
        {
            return port.IsOpen;
        }

        private void SimulateEngine(object sender, EventArgs e)
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
        private void RunEngine(object sender, EventArgs e)
        {
            //while (!_shouldStop)
            {
                // create micro commands
                byte[] m1 = GetM1Command();
                byte[] m2 = GetM2Command();
                byte[] m3 = GetM3Command();
                byte[] m4 = GetM4Command();

                port.Write(m1, 0, 6);
                port.Write(m2, 0, 6);
                port.Write(m3, 0, 6);
                port.Write(m4, 0, 6);
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
            int speed = _rightSpeedValue;
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
            LeftSpeed = 0;
            RightSpeed = 0;
        }

        public static void Kill()
        {
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

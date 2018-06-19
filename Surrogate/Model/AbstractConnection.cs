using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Surrogate.Model
{
    public abstract class AbstractConnection : IConnection
    {
        /// <summary>
        /// Static reference to a logger. Enables logging to console, gui (TextBox) and file
        /// </summary>
        protected static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// EventHandler to manage events regarding changed connections
        /// Each time the Status <see cref="ConnectionStatus"/> of the connection changes, this handler will be invoked
        /// </summary>
        public event EventHandler<ConnectionStatus> ConnectionStatusHandler;
        public abstract string Name { get; }
        protected ConnectionStatus _status;
        public virtual ConnectionStatus Status { get => _status;
            set {
                if (value == _status) return;
                _status = value;
                ConnectionStatusHandler?.Invoke(this, value);
            }
        }

        public abstract bool Connect();

        public abstract bool Disconnect();

        /// <summary>
        /// Constructor with no parameters. Initializes the status with <see cref="ConnectionStatus.Disconnected"/> and calls
        /// the <see cref="Connect"/> prozedure.
        /// </summary>
        public AbstractConnection()
        {
            _status = ConnectionStatus.Disconnected;
            Connect();
        }
    }

    public enum ConnectionStatus
    {
        Connected,
        Disconnected,
        Ready
    }
}

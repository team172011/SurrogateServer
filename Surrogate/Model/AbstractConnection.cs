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

        public event EventHandler<ConnectionStatus> ConnectionStatusHandler;
        public abstract string Name { get; }
        protected ConnectionStatus _status;
        public virtual ConnectionStatus Status { get => _status; set { _status = value; ConnectionStatusHandler?.Invoke(this, value); } }

        public abstract bool Connect();
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

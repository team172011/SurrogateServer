using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Surrogate.Model
{
    public interface IConnection
    {
        event EventHandler<ConnectionStatus> ConnectionStatusHandler;
        string Name { get; }
        ConnectionStatus Status { get; }
        bool Connect();
        bool Disconnect();
    }
}

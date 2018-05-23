using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Surrogate.Model
{
    public interface IConnection
    {
        bool IsConnected();
        bool IsReady();
        bool Connect();
    }
}

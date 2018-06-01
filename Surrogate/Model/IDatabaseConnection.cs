using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Surrogate.Model
{
    public interface IDatabaseConnection
    {

        bool CreateTable(string name, IDictionary<string, SqlDbType> columns);
    }
}

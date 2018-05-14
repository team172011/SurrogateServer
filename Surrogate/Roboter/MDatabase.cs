using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Surrogate.Roboter.MDatabase
{
    using System.Data.SqlClient;

    /// <summary>
    /// Class to connect to a sql database and for creating, inserting and loading data and tables for
    /// different modules
    /// </summary>
    public class Database
    {
        private static SqlConnection _connection = null;


        private SqlConnection Connection()
        {
            if (_connection != null)
            {
                return _connection;
            }
            SqlConnection conn = new SqlConnection("user id=surrogate;" +
                           "password=surrogate;server=serverurl;" +
                           "Trusted_Connection=yes;" +
                           "database=surrogate; " +
                           "connection timeout=30");
            conn.Open();
            return conn;
        }    
    }
}

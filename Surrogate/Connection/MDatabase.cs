// This file belongs to the source code of the "Surrogate Project"
// Copyright (c) 2018 All Rights Reserved
// Martin-Luther-Universitaet Halle-Wittenberg
// Lehrstuhl Wirtschaftsinformatik und Operation Research
// Autor: Wimmer, Simon-Justus Wimmer

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Surrogate.Roboter.MDatabase
{
    using Surrogate.Implementations;
    using Surrogate.Model;
    using System.Data.SqlClient;

    /// <summary>
    /// Class to connect to a sql database and for creating, inserting and loading data and tables for
    /// different modules
    /// </summary>
    public class Database : AbstractConnection
    {
        private static SqlConnection _connection = null;

        public override string Name => FrameworkConstants.DatabaseName;

        public override ConnectionStatus Status => ConnectionStatus.Disconnected;


        public override bool Connect()
        {
            throw new NotImplementedException();
        }

        public bool IsConnected()
        {
            throw new NotImplementedException();
        }

        public bool IsReady()
        {
            throw new NotImplementedException();
        }

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

// This file belongs to the source code of the "Surrogate Project"
// Copyright (c) 2018 All Rights Reserved
// Martin-Luther-Universitaet Halle-Wittenberg
// Lehrstuhl Wirtschaftsinformatik und Operation Research
// Autor: Wimmer, Simon-Justus Wimmer (simonjustuswimmer@googlemail.com)
using System;
using System.Collections.Generic;
using System.Text;


namespace Surrogate.Roboter.MDatabase
{
    using Surrogate.Implementations;
    using Surrogate.Model;
    using System.Data;
    using System.Data.SqlClient;

    /// <summary>
    /// Class to connect to a sql database and for creating, inserting and loading data and tables for
    /// different modules
    /// </summary>
    public class SystemDatabase : AbstractConnection, IDatabaseConnection
    {
        private static SqlConnection _connection = null;

        public override string Name => FrameworkConstants.DatabaseName;

        public SystemDatabase()
        {
            Connect();
        }

        public override bool Connect()
        {
            Connection();
            var connected = IsConnected();
            if (connected)
            {
                Status = ConnectionStatus.Ready;
            } else
            {
                Status = ConnectionStatus.Disconnected;
            }
            return connected;
        }

        public override bool Disconnect()
        {
            try
            {
                Connection().Close();
                return true;
            } catch(SqlException sqle)
            {
                log.Error("Verbindung zur Datenbank konnte nicht geschlossen werden: " + sqle.Message);
                return false;
            }
            
        }

        /// <summary>
        /// Executes the passed sql command that returns no query (UPDATE, INSERT, or DELETE statements)
        /// Only for internal usages, sql injection possible
        /// </summary>
        /// <param name="command">the sql command as string</param>
        /// <returns></returns>
        public bool ExecuteNonQuery(string command)
        {
            SqlCommand sqlCommand = new SqlCommand(command, Connection());
            sqlCommand.ExecuteNonQuery();
            return true;
        }

        /// <summary>
        /// Executes the passed sql command that returns a query (SELECT statements)
        /// Only for internal usages, sql injection possible
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public SqlDataReader ExecuteQuery(string command)
        {
            SqlCommand sqlCommand = new SqlCommand(command, Connection());
            return sqlCommand.ExecuteReader();
        }

        public bool IsConnected()
        {
            return _connection.State != System.Data.ConnectionState.Closed && _connection.State != System.Data.ConnectionState.Broken;
        }

        public bool IsReady()
        {
            return _connection.State == System.Data.ConnectionState.Open;
        }

        private SqlConnection Connection()
        {
            if (_connection == null || _connection.State == ConnectionState.Broken || _connection.State == ConnectionState.Closed)
            {
                _connection = new SqlConnection(FrameworkConstants.DbmsConnectionString);
                try
                {
                    _connection.Open();
                    log.Debug("Connected to Database: " + _connection.Database);
                }
                catch (SqlException sqle)
                {
                    log.Error(sqle.Message + ": " + sqle.StackTrace);
                }
            }
            
            return _connection;
        }

        public void ExecuteProcedure(string procedure, params string[] values)
        {
            throw new NotImplementedException();
        }
    }
}

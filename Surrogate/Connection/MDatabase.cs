// This file belongs to the source code of the "Surrogate Project"
// Copyright (c) 2018 All Rights Reserved
// Martin-Luther-Universitaet Halle-Wittenberg
// Lehrstuhl Wirtschaftsinformatik und Operation Research
// Autor: Wimmer, Simon-Justus Wimmer

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
    public class Database : AbstractConnection, IDatabaseConnection
    {
        private static SqlConnection _connection = null;

        public override string Name => FrameworkConstants.DatabaseName;

        public Database()
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
            try
            {
                sqlCommand.ExecuteNonQuery();
                return true;
            } catch(Exception e)
            {
                log.Error("Fehler beim ausfuehren des Sql Statements (no query):" + command + "\n" + e.Message);
                return false;
            }
        }

        public SqlDataReader ExecuteQuery(string command)
        {
            SqlCommand sqlCommand = new SqlCommand(command, Connection());
            return sqlCommand.ExecuteReader();
        }


        public bool InsertIntoTable(string tableName, IList<string> tableColumns, IDictionary<string, SqlDbType> columns)
        {
            StringBuilder para = new StringBuilder();
            StringBuilder ats = new StringBuilder();
            IList<string> values = new List<string>();
            int i = 1;
            foreach (var col in columns)
            {
                para.Append(col.Key + ",");
                ats.Append("@param" + i++);
            }
            var command = String.Format("INSERT INTO {0}({1}) VALUES({2})", tableName, para.ToString(), ats.ToString());
            SqlCommand sqlCommand = new SqlCommand(command, Connection());
            int j = 1;
            foreach (var val in values)
            {
                sqlCommand.Parameters.Add("@param" + j++, columns[val]).Value = val;
            }
            try
            {

                sqlCommand.CommandType = CommandType.Text;
                sqlCommand.ExecuteNonQuery();
                return true;
            } catch (Exception e)
            {
                log.Error("Fehler beim Einfuegen eines Datensatzes in " + tableName + ": Query: " + sqlCommand.ToString() + "\n"+ e.Message);
                return false;
            }

        }

        /// <summary>
        /// Create a new table with specified table columns and corresponding types
        /// </summary>
        /// <param name="name"></param>
        /// <param name="columns"></param>
        /// <returns></returns>
        public bool CreateTableIfNotExists(string name, IDictionary<string, SqlDbType> columns)
        {
            var commandTable = String.Format("CREATE TABLE {0} ",name);
            StringBuilder commandColumns = new StringBuilder("(");
            foreach(var k in columns)
            {
                commandColumns.Append(String.Format("{0} {1},",k.Key.ToString(), k.Value));
            }
            commandColumns.Remove(commandColumns.Length - 1, 1);
            commandColumns.Append(")");

            var commandString = commandTable + commandColumns.ToString();
            using (var con = Connection())
            {
                try
                {
                    log.Info(commandString);
                    using (SqlCommand command = new SqlCommand(
                        commandString, con))
                    {
                        command.ExecuteNonQuery();
                        return true;
                    }
                }
                catch
                {
                    log.Debug(String.Format("Tabelle {0} konnte nicht erstellt werden. Existiert bereits eine Tabelle: ",name));
                    return false;
                }
            }
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
                _connection = new SqlConnection(@"server=localhost\SQLEXPRESS;database=Surrogate;Integrated Security=True;");
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
    }
}

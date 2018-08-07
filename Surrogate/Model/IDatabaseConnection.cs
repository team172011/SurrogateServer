// This file belongs to the source code of the "Surrogate Project"
// Copyright (c) 2018 All Rights Reserved
// Martin-Luther-Universitaet Halle-Wittenberg
// Lehrstuhl Wirtschaftsinformatik und Operation Research
// Autor: Wimmer, Simon-Justus Wimmer (simonjustuswimmer@googlemail.com)
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Surrogate.Model
{
    public interface IDatabaseConnection
    {

        bool CreateTableIfNotExists(string name, IDictionary<string, SqlDbType> columns);
        bool InsertIntoTable(string tableName, IList<string> tableColumns, IDictionary<string, SqlDbType> columns);

        bool ExecuteNonQuery(string command);

        SqlDataReader ExecuteQuery(string command);
    }
}

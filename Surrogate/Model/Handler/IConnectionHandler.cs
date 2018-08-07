// This file belongs to the source code of the "Surrogate Project"
// Copyright (c) 2018 All Rights Reserved
// Martin-Luther-Universitaet Halle-Wittenberg
// Lehrstuhl Wirtschaftsinformatik und Operation Research
// Autor: Wimmer, Simon-Justus Wimmer (simonjustuswimmer@googlemail.com)
using Surrogate.Controller;
using Surrogate.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Surrogate.Model.Handler
{
    public interface IConnectionHandler : IVisualModule
    {
        event EventHandler<ConnectionArgs> ConnectionChangedStatus;
        event EventHandler<ConnectionArgs> ConnectionAdded;

        void RegisterConnection(string name, IConnection connection);
        void Connect(string name);
        IConnection GetConnection(string name);
        void ConnectAll();
    }

    public class ConnectionArgs : EventArgs
    {

        private readonly string _name;
        public string Name { get => _name; }

        private readonly IConnection _connection;
        public IConnection Module { get => _connection; }

        private readonly ConnectionStatus _status;
        public ConnectionStatus Status { get => _status; }

        public ConnectionArgs(IConnection arg)
        {
            _connection = arg;
            _status = arg.Status;
            _name = arg.Name;
        }
    }
}

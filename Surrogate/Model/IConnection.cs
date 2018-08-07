// This file belongs to the source code of the "Surrogate Project"
// Copyright (c) 2018 All Rights Reserved
// Martin-Luther-Universitaet Halle-Wittenberg
// Lehrstuhl Wirtschaftsinformatik und Operation Research
// Autor: Wimmer, Simon-Justus Wimmer (simonjustuswimmer@googlemail.com)
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

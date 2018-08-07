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

namespace Surrogate.Utils.Event
{
    public class BooleanEventArgs : EventArgs
    {
        private readonly bool _value;
    
        public BooleanEventArgs(bool value)
        {
            _value = value;
        }

        public bool Value { get => _value; }
    }
}

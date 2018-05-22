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

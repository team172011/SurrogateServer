// This file belongs to the source code of the "Surrogate Project"
// Copyright (c) 2018 All Rights Reserved
// Martin-Luther-Universitaet Halle-Wittenberg
// Lehrstuhl Wirtschaftsinformatik und Operation Research
// Autor: Wimmer, Simon-Justus Wimmer (simonjustuswimmer@googlemail.com)
using Surrogate.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Surrogate.Controller
{
    public interface IController
    {
        IModuleProperties Properties { get; }
        void Log(string message);

        /// <summary>
        /// Returns true if the IController is executing a/the function
        /// </summary>
        /// <returns></returns>
        bool IsRunning();

        void Start();
        void Stop();

        string ToString();
    }
}

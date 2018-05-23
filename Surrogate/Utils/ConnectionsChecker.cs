// This file belongs to the source code of the "Surrogate Project"
// Copyright (c) 2018 All Rights Reserved
// Martin-Luther-Universitaet Halle-Wittenberg
// Lehrstuhl Wirtschaftsinformatik und Operation Research
// Autor: Wimmer, Simon-Justus Wimmer

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Surrogate.Utils.ConnectionsChecker
{
    public class ConnectionsChecker : BackgroundWorker
    {

        public ConectionsChecker()
        {
            DoWork += CheckingConnections;
            RunWorkerCompleted += WorkCompleted;
        }

        private void CheckingConnections(object sender, DoWorkEventArgs e)
        {
            while(CancellationPending == false)
            {

            }
            e.Cancel = true;
            
        }

        private void WorkCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}

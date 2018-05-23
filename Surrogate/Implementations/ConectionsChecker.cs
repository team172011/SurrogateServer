// This file belongs to the source code of the "Surrogate Project"
// Copyright (c) 2018 All Rights Reserved
// Martin-Luther-Universitaet Halle-Wittenberg
// Lehrstuhl Wirtschaftsinformatik und Operation Research
// Autor: Wimmer, Simon-Justus Wimmer

using Surrogate.Modules;
using Surrogate.View.ConnectionsChecker;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Surrogate.Implementations
{
    public class ConnectionsCheckerModule : VisualModule<ModuleProperties, ModuleInfo>
    {
        public ConnectionsCheckerModule(ModuleProperties modulProperties) : base(modulProperties)
        {
        }

        public override ContentControl GetPage()
        {
            throw new NotImplementedException();
        }

        public override void Start(ModuleInfo info)
        {
            throw new NotImplementedException();
        }

        public override void Stop()
        {
            throw new NotImplementedException();
        }
    }

    class ConnectionsChecker : BackgroundWorker
    {


        public ConnectionsChecker()
        {
            DoWork += CheckingConnections;
            RunWorkerCompleted += WorkCompleted;
        }

        private void CheckingConnections(object sender, DoWorkEventArgs e)
        {
            while(CancellationPending == false)
            {
                bool motor = CheckMotor();
                bool controller = CheckController();
                bool database = CheckDatabaseAcces();
                bool touchPad = CheckTouchPad();
                bool camera1 =  CheckCamera1();
                bool camera2 = CheckCamera2();
                updateListView(motor, controller, database, touchPad, camera1, camera2);
            }
            e.Cancel = true;
            
        }

        private void updateListView(bool motor, bool controller, bool database, bool touchPad, bool camera1, bool camera2)
        {
            throw new NotImplementedException();
        }

        private bool CheckCamera2()
        {
            throw new NotImplementedException();
        }

        private bool CheckCamera1()
        {
            throw new NotImplementedException();
        }

        private bool CheckTouchPad()
        {
            throw new NotImplementedException();
        }

        private bool CheckDatabaseAcces()
        {
            throw new NotImplementedException();
        }

        private bool CheckController()
        {
            throw new NotImplementedException();
        }

        private bool CheckMotor()
        {
            throw new NotImplementedException();
        }

        private void WorkCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}

// This file belongs to the source code of the "Surrogate Project"
// Copyright (c) 2018 All Rights Reserved
// Martin-Luther-Universitaet Halle-Wittenberg
// Lehrstuhl Wirtschaftsinformatik und Operation Research
// Autor: Wimmer, Simon-Justus Wimmer

namespace Surrogate.View.Logging
{
    using System;
    using System.Windows.Controls;
    
    public partial class LoggingView : UserControl
    {

        private static readonly log4net.ILog log =
    log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public LoggingView()
        {
            InitializeComponent();

            tbLogging.TextChanged += ScrollToEnd;

            log.Info("Scroll To End");
        }

        public void ScrollToEnd(object e, EventArgs args)
        {
            tbLogging.ScrollToEnd();
        }
    }
}

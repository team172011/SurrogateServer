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

            tbLogging.TextChanged += scrollToEnd;

            log.Info("Scroll To End");
        }

        public void scrollToEnd(object e, EventArgs args)
        {
            tbLogging.ScrollToEnd();
        }
    }
}

// This file belongs to the source code of the "Surrogate Project"
// Copyright (c) 2018 All Rights Reserved
// Martin-Luther-Universitaet Halle-Wittenberg
// Lehrstuhl Wirtschaftsinformatik und Operation Research
// Autor: Wimmer, Simon-Justus Wimmer

namespace Surrogate.Utils.Logging
{
    using log4net.Appender;
    using System.ComponentModel;
    using System.IO;
    using System.Globalization;
    using log4net;
    using log4net.Core;

    /// <summary>
    /// Custom log4net appender to log on a TextBox
    /// following: https://peteohanlon.wordpress.com/2009/10/12/logging-display-and-wpf/
    /// </summary>
    public class NotifyAppender : AppenderSkeleton, INotifyPropertyChanged
    {
        #region Members and events
        private static string _notification;
        private event PropertyChangedEventHandler _propertyChanged;

        public event PropertyChangedEventHandler PropertyChanged
        {
            add { _propertyChanged += value; }
            remove { _propertyChanged -= value; }
        }
        #endregion

        /// <summary>
        /// Get or set the notification message.
        /// </summary>
        public string Notification
        {
            get
            {
                return _notification; ;
            }
            set
            {
                if (_notification != value)
                {
                    if(_notification?.Length > 1000){
                        _notification = string.Empty;
                    }
                    _notification = value;
                    OnChange();
                }
            }
        }

        /// <summary>
        /// Raise the change notification.
        /// </summary>
        private void OnChange()
        {
            PropertyChangedEventHandler handler = _propertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(string.Empty));
            }
        }

        /// <summary>
        /// Get a reference to the log instance.
        /// </summary>
        public NotifyAppender Appender
        {
            get
            {
                return Log.Appender;
            }

        }

        /// <summary>
        /// Append the log information to the notification.
        /// </summary>
        /// <param name="loggingEvent">The log event.</param>
        protected override void Append(LoggingEvent loggingEvent)
        {
            StringWriter writer = new StringWriter(CultureInfo.InvariantCulture);
            Layout.Format(writer, loggingEvent);
            Notification += writer.ToString();
        }
    }
}

// This file belongs to the source code of the "Surrogate Project"
// Copyright (c) 2018 All Rights Reserved
// Martin-Luther-Universitaet Halle-Wittenberg
// Lehrstuhl Wirtschaftsinformatik und Operation Research
// Autor: Wimmer, Simon-Justus Wimmer

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Surrogate.Implementations
{
    using System.Windows.Controls;
    using Surrogate.Modules;

    using OpenTok;
    using Surrogate.View;
    using System.Collections.ObjectModel;
    using Newtonsoft.Json.Linq;
    using Surrogate.Roboter;
    using Surrogate.Model.Module;

    /// <summary>
    /// Module for the video chat application using the TopBox libraries
    /// </summary>
    public class VideoChatModule : VisualModule<VideoChatProperties, VideoChatInfo>
    {
        private Session _session;
        private Publisher _publisher;
        private VideoChatView _view;

        private volatile bool _isRunning = false;

        private ObservableCollection<VCKontakt> _contacts;

        public ObservableCollection<VCKontakt> KontaktsList { get => _contacts; }
        public bool IsRunning { get => _isRunning;
        }
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="modulProperties"></param>
        public VideoChatModule(VideoChatProperties modulProperties) : base(modulProperties)
        {
            _view = new VideoChatView(this);
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="modulProperties"></param>
        public VideoChatModule() : this(new VideoChatProperties())
        {
            _view = new VideoChatView(this);
        }

        public override ModuleView GetPage()
        {
            return _view;
        }

        public override void Start(VideoChatInfo info)
        {
            _isRunning = true;
            ///init the Session field with informations from VideoChatProperties
            _session = new Session(Context.Instance, Properties.ApiKey, Properties.SessionID);

            _session.Connected += Session_Connected;
            _session.Disconnected += Session_Disconnected;
            _session.Error += Session_Error;
            _session.StreamReceived += Session_StreamReceived;

            _session.Connect(Properties.Token);
            _publisher = new Publisher(Context.Instance, renderer: _view.PublisherVideo);


        }

        public override void Stop()
        {
            if(_session != null)
            {
                _session.Disconnect();
            }

            if (_publisher != null)
            {
                _publisher.Dispose();
            }
            
            _isRunning = false;
        }

        private void Session_Connected(object sender, EventArgs e)
        {
            log.Debug("Connected to session.");
            _session.Publish(_publisher);
        }

        private void Session_Disconnected(object sender, EventArgs e)
        {
            log.Debug("Disconnected from session.");
        }

        private void Session_Error(object sender, Session.ErrorEventArgs e)
        {
            log.Debug("Session error: " + e.ErrorCode);
        }

        private void Session_StreamReceived(object sender, Session.StreamEventArgs e)
        {
            log.Debug("Stream received in session.");
            Subscriber subscriber = new Subscriber(Context.Instance, e.Stream, _view.SubscriberVideo);
            _session.Subscribe(subscriber);
        }

        public override void OnSelected()
        {
            
        }

        public override void OnDisselected()
        {
            Stop();
        }
    }

    public class VideoChatProperties : ModuleProperties
    {
        private readonly string _API_KEY;
        private readonly string _SESSION_ID;
        private readonly string _TOKEN;
        private static readonly string _URI = @"https://pscagebot.herokuapp.com/session";

        public string ApiKey { get => _API_KEY; }
        public string SessionID { get => _SESSION_ID; }
        public string Token { get => _TOKEN; }

        public VideoChatProperties() : base("Video Chat", "Modul zum kommunizieren mittels Sprach- und Videochat", false, true, false, false, true, true)
        {
            JObject json = MInternet.GetJSON(_URI);
            _API_KEY = json.GetValue("apiKey").ToString();
            _SESSION_ID = json.GetValue("sessionId").ToString();
            _TOKEN = json.GetValue("token").ToString();
        }
    }

    public class VideoChatInfo : ModuleInfo
    {

    }
    
    public class VCKontakt
    {
        public string firstname;
        public string lastname;
        public int id;
    }
}

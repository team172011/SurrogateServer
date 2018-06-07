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
    using Surrogate.Model;
    using Surrogate.Roboter.MInternet;
    using Surrogate.Roboter.MDatabase;
    using System.Data;
    using System.Data.SqlClient;

    /// <summary>
    /// Module for the video chat application using the TopBox libraries
    /// </summary>
    public class VideoChatModule : VisualModule<VideoChatProperties, VideoChatInfo>
    {
        private readonly Session _session;
        private Publisher _publisher;

        private VideoChatModuleView _view;
        private readonly Dictionary<string, VideoChatContact> _availableContacts = new Dictionary<string, VideoChatContact>();

        public event EventHandler<VideoChatContact> ContactAddedHandler;
        public event EventHandler<VideoChatContact> ContactStatusChangedHandler; // true = online, false otherwise
        public event EventHandler<VideoChatContact> ContactRemovedHandler; 


        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="modulProperties"></param>
        public VideoChatModule(VideoChatProperties modulProperties) : base(modulProperties)
        {
            _view = new VideoChatModuleView(this);

            ///init the Session field with informations from VideoChatProperties
            _session = new Session(Context.Instance, Properties.ApiKey, Properties.SessionID);
            

            _session.Connected += Session_Connected;
            _session.Disconnected += Session_Disconnected;
            _session.Error += Session_Error;
            _session.StreamReceived += Session_StreamReceived;
            _session.StreamDropped += Session_StreamDropped;
            _session.Connect(Properties.Token);

            Database db = (Database)SurrogateFramework.MainController.ConnectionHandler.GetConnection(FrameworkConstants.DatabaseName);
            IDictionary<string, SqlDbType> columns = new Dictionary<string, SqlDbType>
            {
                { "ID", SqlDbType.Int },
                { "username", SqlDbType.Text},
                { "Firstname", SqlDbType.Text},
                { "Name", SqlDbType.Text }
            };
            if(db.CreateTableIfNotExists(VideoChatProperties.TableName, columns))
            {
                // Add example Data
                db.ExecuteNonQuery(String.Format("INSERT INTO {0} (ID, username, Firstname, Name) VALUES(1,'simonjw','Simon','Wimmer')", VideoChatProperties.TableName));
                db.ExecuteNonQuery(String.Format("INSERT INTO {0} (ID, username, Firstname, Name) VALUES(2,'drmueller','Dr. Hans','Mueller')", VideoChatProperties.TableName));
            }

            SqlDataReader resultsReader = db.ExecuteQuery(String.Format("SELECT * FROM {0}", VideoChatProperties.TableName));
            if (resultsReader.HasRows)
            {
                while (resultsReader.Read())
                {
                    var contact = new VideoChatContact(resultsReader.GetInt32(0), resultsReader.GetString(1), resultsReader.GetString(2), resultsReader.GetString(3));
                    _availableContacts.Add(contact.Username,contact);
                }
            }
        }

        /// <summary>
        /// Iterates over all available contacts and invokes the handler
        /// </summary>
        public void InvokeContactData()
        {
            foreach(var contact in _availableContacts)
            {
                ContactAddedHandler?.Invoke(this, contact.Value);
            }
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="modulProperties"></param>
        public VideoChatModule() : this(new VideoChatProperties())
        {
            _view = new VideoChatModuleView(this);
        }

        public override UserControl GetPage()
        {
            return _view;
        }

        public override void Start(VideoChatInfo info)
        {

        }

        public override void Stop()
        {
            _session?.Disconnect();
            _publisher?.Dispose();
        }

        private void Session_Connected(object sender, EventArgs e)
        {
            log.Debug(String.Format("Connected to session with {0} {1} {0}",Properties.ApiKey, Properties.SessionID, Properties.Token));
            
        }

        private void Session_Disconnected(object sender, EventArgs e)
        {
            log.Debug("Disconnected from session.");
        }

        private void Session_Error(object sender, Session.ErrorEventArgs e)
        {
            log.Debug("Session error: " + e.ErrorCode);
        }

        /// <summary>
        /// Called if another client publishes a stream to the session
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Session_StreamReceived(object sender, Session.StreamEventArgs e)
        {
            
            log.Debug("Stream received in session. "+e.Stream.Name);
            var contact = _availableContacts[e.Stream.Name];
            contact.VidoeStream = e.Stream;
            contact.IsOnline = true;
            ContactStatusChangedHandler?.Invoke(this, contact);
        }

        /// <summary>
        /// Called if another client stops publishing a stream to the session
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Session_StreamDropped(object sender, Session.StreamEventArgs e)
        {
            log.Debug("Stream stopped in session. " + e.Stream.Name);
            var contact = _availableContacts[e.Stream.Name];
            contact.VidoeStream = e.Stream;
            contact.IsOnline = false;
            ContactStatusChangedHandler?.Invoke(this, contact);

        }

        /// <summary>
        /// Pushes the corresponding stream to the view frame
        /// </summary>
        /// <param name="stream"></param>
        public void ShowStream(String username)
        {
            Stream stream = _availableContacts[username]?.VidoeStream;
            if(stream == null)
            {
                return;
            }
            Subscriber subscriber = new Subscriber(Context.Instance, stream, _view.SubscriberVideo);
            _session.Subscribe(subscriber);
        }

        public override void OnSelected()
        {
            _publisher = new Publisher(Context.Instance, renderer: _view.PublisherVideo);
            _session.Publish(_publisher);
        }

        public override void OnDisselected()
        {
            _publisher?.Dispose();
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

        public static readonly string _tableName = "VideoChatContacts";
        public static string TableName { get => _tableName; }

        public VideoChatProperties() : base("Video Chat", "Modul zum kommunizieren mittels Sprach- und Videochat", false, true, false, false, true, true)
        {
            JObject json = Internet.GetJSON(_URI);
            _API_KEY = json.GetValue("apiKey").ToString();
            _SESSION_ID = json.GetValue("sessionId").ToString();
            _TOKEN = json.GetValue("token").ToString();
        }
    }

    public class VideoChatInfo : ModuleInfo
    {

    }

    public class VideoChatContact
    {
        private readonly string name;
        private readonly string firstname;
        private readonly string username;
        private readonly Int32 id;
        private bool _isOnline = false;
        private Stream _stream;

        public string Name => name;
        public string Firstname => firstname;
        public string Username => username;
        public int Id => id;
        public bool IsOnline {
            get => _isOnline;
            set => _isOnline = value;
        }

        public Stream VidoeStream {
            get => _stream;
            set => _stream = value; } 

        public VideoChatContact(Int32 id, string username, string firstname, string name)
        {
            this.username = username;
            this.name = name;
            this.firstname = firstname;
            this.id = id;
        }
    }
}

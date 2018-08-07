// This file belongs to the source code of the "Surrogate Project"
// Copyright (c) 2018 All Rights Reserved
// Martin-Luther-Universitaet Halle-Wittenberg
// Lehrstuhl Wirtschaftsinformatik und Operation Research
// Autor: Wimmer, Simon-Justus Wimmer (simonjustuswimmer@googlemail.com)
using System;
using System.Collections.Generic;

namespace Surrogate.Implementations
{
    using System.Windows.Controls;
    using Surrogate.Modules;

    using OpenTok;
    using Surrogate.View;
    using Newtonsoft.Json.Linq;
    using Surrogate.Model.Module;
    using Surrogate.Model;
    using Surrogate.Roboter.MDatabase;
    using System.Data;
    using System.Data.SqlClient;
    using Surrogate.Roboter.MCamera;

    /// <summary>
    /// Module for the video chat application using the TopBox libraries
    /// </summary>
    public class VideoChatModule : VisualModule<VideoChatProperties, VideoChatInfo>
    {

        private readonly Session _session; // the openTok session
        private Publisher _CurrentPublisher; // instance for publishing streams
        private Subscriber _CurrentSubscriber; // instance for receiving streams

        private VideoChatModuleView _view;
        private readonly Dictionary<string, VideoChatContact> _availableContacts = new Dictionary<string, VideoChatContact>();

        public override IModuleProperties Properties => GetProperties();

        public event EventHandler<VideoChatContact> ContactAddedHandler;
        public event EventHandler<VideoChatContact> ContactStatusChangedHandler; // true = online, false otherwise
        public event EventHandler<VideoChatContact> ContactRemovedHandler;


        private int CamNum = 0;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="modulProperties"></param>
        public VideoChatModule() : base(new VideoChatProperties())
        {

            _view = new VideoChatModuleView(this);

            ///init the Session field with informations from VideoChatProperties
            _session = new Session(Context.Instance, GetProperties().GetProperty(GetProperties().Key_API_KEY,"no api key"), GetProperties().GetProperty(GetProperties().Key_SESSION_ID, "no session id"));
            

            _session.Connected += Session_Connected;
            _session.Disconnected += Session_Disconnected;
            _session.Error += Session_Error;
            _session.StreamReceived += Session_StreamReceived;
            _session.StreamDropped += Session_StreamDropped;
            _session.Connect(GetProperties().GetProperty(GetProperties().Key_TOKEN, "no token available"));

            SystemDatabase db = (SystemDatabase)SurrogateFramework.MainController.ConnectionHandler.GetConnection(FrameworkConstants.DatabaseName);
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
                db.ExecuteNonQuery(String.Format("INSERT INTO {0} (ID, username, Firstname, Name) VALUES(2,'drmueller','Dr. Mueller','Hans')", VideoChatProperties.TableName));
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
            resultsReader.Close();
            InvokeContactData();
        }

        internal void HangUp()
        {
            if (_CurrentSubscriber != null)
            {
                _session.Unsubscribe(_CurrentSubscriber);
                _CurrentSubscriber = null;
            }
            
        }

        public void ChangeCamera()
        {
            var NextCam = ++CamNum % FrameworkConstants.Numbercams; // increase cam index by one
            var connection = SurrogateFramework.MainController.ConnectionHandler.GetConnection(FrameworkConstants.InternalCameraName) as ICameraConnection<IVideoCapturer>;
            log.Debug("Switching to Camera " + NextCam);
            switch (NextCam)
            {
                case 1:
                    {
                        connection = SurrogateFramework.MainController.ConnectionHandler.GetConnection(FrameworkConstants.Camera2Name) as ICameraConnection<IVideoCapturer>;
                        break;
                    }
                case 2:
                    {
                        connection = SurrogateFramework.MainController.ConnectionHandler.GetConnection(FrameworkConstants.Camera1Name) as ICameraConnection<IVideoCapturer>;
                        break;
                    }
                default:
                    {
                        break;
                    }
            }
            _session.Unpublish(_CurrentPublisher);
            _CurrentPublisher.Dispose();
            _CurrentPublisher = new Publisher(Context.Instance, renderer: _view.PublisherVideo, capturer: connection.GetCamera());
            _session.Publish(_CurrentPublisher);
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
            _CurrentPublisher?.Dispose();
        }

        private void Session_Connected(object sender, EventArgs e)
        {
            log.Debug(String.Format("Connected to session with {0} {1} {2}", GetProperties().GetProperty(GetProperties().Key_API_KEY,"api key not available"),
                GetProperties().GetProperty(GetProperties().Key_SESSION_ID, "session id not available"), GetProperties().GetProperty(GetProperties().Key_TOKEN, "token not available")));
            
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
            _CurrentSubscriber = new Subscriber(Context.Instance, stream, _view.SubscriberVideo);
            
            try
            {
                _session.Subscribe(_CurrentSubscriber);
            }
            catch (OpenTokException e)
            {
                System.Diagnostics.Debug.WriteLine(e.InnerException);
                log.Error(e.Message);
            }
            
        }


        public override void OnSelected()
        {
            _CurrentPublisher = new Publisher(Context.Instance, renderer: _view.PublisherVideo,name:"Surrogate");
            _session.Publish(_CurrentPublisher);
        }

        public override void OnDisselected()
        {
            _CurrentPublisher?.Dispose();
        }

        public override bool IsRunning()
        {
            return true;
        }
    }



    public class VideoChatProperties : ModulePropertiesBase
    {
        public readonly string Key_API_KEY = "API_KEY";
        public readonly string Key_SESSION_ID = "SESSION_ID";
        public readonly string Key_TOKEN = "TOKEN";
        public static readonly string Key_URI = "URI";
           


        public static readonly string _tableName = "VideoChatContacts";
        public static string TableName { get => _tableName; }

        public VideoChatProperties() : base("Video Chat", "Modul zum Kommunizieren mittels Sprach- und Videochat", false, true, true, false, true, true)
        {
            JObject json = Roboter.MInternet.Internet.GetJSON(@"https://pscagebot.herokuapp.com/session");

            SetProperty(base.KeyImagePath, @"C:\Users\ITM1\source\repos\Surrogate\Surrogate\resources\videochat_controller_icon.jpg");
            SetProperty(Key_API_KEY, json.GetValue("apiKey").ToString());
            SetProperty(Key_URI, @"https://pscagebot.herokuapp.com/session");
            SetProperty(Key_SESSION_ID,json.GetValue("sessionId").ToString());
            SetProperty(Key_TOKEN,json.GetValue("token").ToString());
            Save(GetProperty(KeyName,"Video Chat") + ".txt");
            Load(GetProperty(KeyName, "Video Chat") + ".txt");
        }
    }

    public class VideoChatInfo : ModuleInfo
    {

    }

    public class VideoChatContact
    {

        public string Name { get; }
        public string Firstname { get; }
        public string Username { get; }
        public int Id { get; }
        public bool IsOnline { get; set; }

        public Stream VidoeStream { get; set; }

        public VideoChatContact(Int32 id, string username, string firstname, string name)
        {
            Username = username;
            Name = name;
            Firstname = firstname;
            Id = id;
        }
    }
}

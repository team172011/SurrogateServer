﻿// This file belongs to the source code of the "Surrogate Project"
// Copyright (c) 2018 All Rights Reserved
// Martin-Luther-Universitaet Halle-Wittenberg
// Lehrstuhl Wirtschaftsinformatik und Operation Research
// Autor: Wimmer, Simon-Justus Wimmer (simonjustuswimmer@googlemail.com)
using System.Windows;

namespace Surrogate.View
{
    using Surrogate.Implementations;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Windows.Controls;

    /// <summary>
    /// Interaktionslogik für VideoChatView.xaml
    /// </summary>
    public partial class VideoChatModuleView : ModuleViewBase
    {

        private ObservableCollection<VideoChatItem> contacts = new ObservableCollection<VideoChatItem>();

        public VideoChatModuleView(VideoChatModule parentModule):base(parentModule)
        {
            InitializeComponent();
            lvContacts.ItemsSource = contacts;
            parentModule.ContactAddedHandler += FillList;
            parentModule.ContactStatusChangedHandler += UpdateList;
            parentModule.InvokeContactData();
        }

        private void UpdateList(object sender, VideoChatContact e)
        {
            foreach(var con in contacts)
            {
                if (con.UserName.Equals(e.Username))
                {
                    con.IsOnline = e.IsOnline;
                }
            }
        }

        private void FillList(object sender, VideoChatContact e)
        {
            contacts.Add(new VideoChatItem(e));
        }

        private void ListViewItem_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            var item = sender as ListViewItem;
            var vcItem = item.Content as VideoChatItem;
            ((VideoChatModule)Controller).ShowStream(vcItem.UserName);
        }

        private void BtnAddContact_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Bitte fuegen Sie Kontakte über das DBMS hinzu und richten Sie den Klienten mit entsprechendem Nutzernamen ein");
        }

        private void BtnChangeCamera_Click(object sender, RoutedEventArgs e)
        {
            ((VideoChatModule)Controller).ChangeCamera();
        }

        private void BtnHangUp_Click(object sender, RoutedEventArgs e)
        {
            ((VideoChatModule)Controller).HangUp();
        }

        private void BtnNewWindow_Click(object sender, RoutedEventArgs e)
        {
            subscriberWrapper.Children.Remove(SubscriberVideo);
            SubscriberVideo.Height = 800;
            SubscriberVideo.Width = 1280;
            Grid.SetRow(SubscriberVideo,0);
            Grid.SetColumn(SubscriberVideo, 0);
            Grid g = new Grid();
            g.Children.Add(SubscriberVideo);
            Window w = new Window{Content = g, Height=800, Width=1280};
            w.Show();
            w.Closed += (o, s) =>{
                SubscriberVideo.Height = 500;
                SubscriberVideo.Width = 500;
                g.Children.Remove(SubscriberVideo);
                subscriberWrapper.Children.Add(SubscriberVideo);
            };
        }

        private void BtnMute_Click(object sender, RoutedEventArgs e)
        {
            if (btnMute.IsChecked == true)
            {
                ((VideoChatModule)Controller).Mute(true);
            }
            else
            {
                ((VideoChatModule)Controller).Mute(false);
            }
        }

        private void BtnConnect_Click(object sender, RoutedEventArgs e)
        {
            if (btnOnline.IsChecked == true)
            {
                btnOnline.Content = "Offline gehen";
                Controller.Start();
            }
            else
            {
                btnOnline.Content = "Online gehen";
                Controller.Stop();
            }
        }
    }

    class VideoChatItem : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private readonly string _firstName;
        private readonly string _lastname;
        private readonly string _userName;
        private bool _isOnline;

        public bool IsOnline { get => _isOnline;
            set {
                if(value == _isOnline) return;
                _isOnline = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("IsOnline"));
            } }

        public string UserName => _userName;
        public string Lastname => _lastname;
        public string FirstName => _firstName;

        public VideoChatItem(VideoChatContact contact, bool isOnline = false) : this(contact.Firstname, contact.Name, contact.Username, isOnline) { 
        
        }

        public VideoChatItem(string firstname, string lastname, string userName, bool isOnline)
        {
            _firstName = firstname; _lastname = lastname; _userName = userName; _isOnline = isOnline;
                
        }
    }
}

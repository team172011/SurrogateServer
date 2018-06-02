// This file belongs to the source code of the "Surrogate Project"
// Copyright (c) 2018 All Rights Reserved
// Martin-Luther-Universitaet Halle-Wittenberg
// Lehrstuhl Wirtschaftsinformatik und Operation Research
// Autor: Wimmer, Simon-Justus Wimmer

using System;
using System.Windows;

namespace Surrogate.View
{
    using Surrogate.Implementations;
    using Surrogate.Modules;
    using System.Collections.ObjectModel;
    using System.ComponentModel;

    /// <summary>
    /// Interaktionslogik für VideoChatView.xaml
    /// </summary>
    public partial class VideoChatView : ModuleView
    {
        private readonly VideoChatModule _parentModule;
        private ObservableCollection<VideoChatItem> contacts = new ObservableCollection<VideoChatItem>();

        public VideoChatView(VideoChatModule parentModule)
        {
            _parentModule = parentModule;
            InitializeComponent();
            btnStartCall.Click += _handleCall;
            lvContacts.ItemsSource = contacts;
        }

        private void _handleCall(Object sender, RoutedEventArgs e)
        {
            _parentModule.Start(new VideoChatInfo()); // TODO add contact details in VideoChatInfo
        }

        private void ListViewItem_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {

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
                if(value=_isOnline) return;
                _isOnline = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("IsOnline"));
            } }

        public string UserName => _userName;
        public string Lastname => _lastname;
        public string FirstName => _firstName;

        public VideoChatItem(string firstname, string lastname, string userName, bool isOnline)
        {
            _firstName = firstname; _lastname = lastname; _userName = userName; _isOnline = isOnline;
        }
    }
}

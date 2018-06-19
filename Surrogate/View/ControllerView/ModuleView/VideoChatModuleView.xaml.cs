// This file belongs to the source code of the "Surrogate Project"
// Copyright (c) 2018 All Rights Reserved
// Martin-Luther-Universitaet Halle-Wittenberg
// Lehrstuhl Wirtschaftsinformatik und Operation Research
// Autor: Wimmer, Simon-Justus Wimmer

using System;
using System.Windows;

namespace Surrogate.View
{
    using OpenTok;
    using Surrogate.Implementations;
    using Surrogate.Modules;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Windows.Controls;

    /// <summary>
    /// Interaktionslogik für VideoChatView.xaml
    /// </summary>
    public partial class VideoChatModuleView : ModuleViewBase
    {
        private readonly VideoChatModule _parentModule;
        private ObservableCollection<VideoChatItem> contacts = new ObservableCollection<VideoChatItem>();

        public VideoChatModuleView(VideoChatModule parentModule)
        {
            _parentModule = parentModule;
            InitializeComponent();
            lvContacts.ItemsSource = contacts;
            _parentModule.ContactAddedHandler += FillList;
            _parentModule.ContactStatusChangedHandler += UpdateList;
            _parentModule.InvokeContactData();
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
            _parentModule.ShowStream(vcItem.UserName);
        }

        private void BtnAddContact_Click(object sender, RoutedEventArgs e)
        {
            // open contacts input mask
        }

        private void BtnChangeCamera_Click(object sender, RoutedEventArgs e)
        {
            _parentModule.ChangeCamera();
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

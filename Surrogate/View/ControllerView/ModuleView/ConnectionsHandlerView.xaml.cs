// This file belongs to the source code of the "Surrogate Project"
// Copyright (c) 2018 All Rights Reserved
// Martin-Luther-Universitaet Halle-Wittenberg
// Lehrstuhl Wirtschaftsinformatik und Operation Research
// Autor: Wimmer, Simon-Justus Wimmer


using Surrogate.Implementations.Handler;
using Surrogate.Model;
using Surrogate.Model.Handler;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Surrogate.View.Handler
{
    
    /// <summary>
    /// Interaktionslogik für Window1.xaml
    /// <see cref="Handler"/>
    /// </summary>
    public partial class ConnectionsHandlerView : ModuleView
    {
        private readonly ObservableCollection<ConnectionItem> items = new ObservableCollection<ConnectionItem>();
        public ObservableCollection<ConnectionItem> Items { get => items; }

        public ConnectionsHandlerView(ConnectionsHandler controller):base(controller)
        {
            InitializeComponent();
            lvConnections.ItemsSource = items;
            foreach (var connection in controller.Connections)
            {
                items.Add(new ConnectionItem(connection.Key, connection.Value.Status));
            }
            controller.ConnectionAdded += OnConnectionAdded;
            controller.ConnectionChangedStatus += OnConnectionStatusChanged;
        }

        private void OnConnectionStatusChanged(object sender, ConnectionArgs e)
        {
            //TODO maybe better use a map
            IConnection changed = e.Module;
            foreach(ConnectionItem con in items)
            {
                if (con.Name.Equals(changed.Name))
                {
                    con.Status = changed.Status;
                    // workaround for not working color bindings
                }
                
            }
        }

        private void OnConnectionAdded(object sender, ConnectionArgs e)
        {
            items.Add(new ConnectionItem(e.Module.Name, e.Status));

        }

        private void ListViewItem_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (sender is ListViewItem lvItem)
            {
                if (lvItem.Content is ConnectionItem item)
                {
                    var name = item.Name;
                    var controller = Controller as IConnectionHandler;
                    controller.Connect(name);
                }
            }
        }
    }

    public class ConnectionItem : INotifyPropertyChanged
    {
        public ConnectionItem(string name, ConnectionStatus currentStatus)
        {
            Name = name;
            Status = currentStatus;
        }

        public string Name { get; }
        private ConnectionStatus _status;

        public event PropertyChangedEventHandler PropertyChanged;

        public ConnectionStatus Status
        {
            get => _status;
            set
            {
                if (value == _status) return;
                _status = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Status"));
            }
        }
    }
}

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
                items.Add(new ConnectionItem(connection.Key));
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
            items.Add(new ConnectionItem(e.Module.Name));

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

    public class ConnectionItem 
    {
        public ConnectionItem(string name)
        {
            Name = name;
        }

        public string Name { get; }
        private string _color = "Red";
        public string Color { get => _color; }
        private ConnectionStatus _status;
        public ConnectionStatus Status
        {
            set
            {
                if (value == _status)
                {
                    return;
                }
                else
                {
                    switch (value)
                    {
                        case ConnectionStatus.Connected:
                            _color = "Yellow";
                            break;
                        case ConnectionStatus.Ready:
                            _color = "Green";
                            break;
                        default:
                            _color = "Red";
                            break;
                    }
                }
            }
        }
    }
}

using InventoryCommon.Models;
using InventoryCommon.Networking;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows;


namespace InventoryClient.MVVM.ViewModel
{
    class MainViewModel
    {

        private ServerConnection _server;
        public MainViewModel()
        {
            _server = new ServerConnection();
            _server.MessageReceivedEvent += HandleWelcome;
        }

        private void HandleWelcome(string message)
        {
            Application.Current.Dispatcher.Invoke(() => MessageBox.Show(message));

        }
    }




}


using ChatClient.MVVM.Core;
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
using System.Windows.Controls;


namespace InventoryClient.MVVM.ViewModel
{
    class MainViewModel
    {

        private ServerConnection _server;

        public RelayCommand ConnectToServerCommand { get; set; }
        public string Username { get; set; }
       public  string PasswordHash { get; set; }

        public MainViewModel()
        {
            _server = new ServerConnection();
            _server.WelcomeMessage += HandleWelcome;
            _server.NotInOurDataBaseMessage += HandleErrorLogin;
            //_server.ProductsReceived += functieCeva;

            

            ConnectToServerCommand = new RelayCommand(o =>
            {
                var passwordBox = o as PasswordBox;
                string password = passwordBox?.Password;


                if (!string.IsNullOrEmpty(Username) && !string.IsNullOrEmpty(password))
                {
                    _server.ConnectToServer(Username, password);
                }
                else if (!string.IsNullOrEmpty(Username) && string.IsNullOrEmpty(password))
                {
                    MessageBox.Show("Please write a password!");
                }
                else if (string.IsNullOrEmpty(Username) && !string.IsNullOrEmpty(password))
                {
                    MessageBox.Show("Please write a username!");
                }
                else
                {
                    MessageBox.Show("Please fill in both fields!");
                }
            });
        }

        private void HandleWelcome(string message,string role)
        {
            Application.Current.Dispatcher.Invoke(() =>
           {
               MessageBox.Show($"{message} \nYour role is: {role}");
           });
            
        }
        private void HandleErrorLogin(string message)
        {
            Application.Current.Dispatcher.Invoke(() => MessageBox.Show(message));

        }
        
    }




}


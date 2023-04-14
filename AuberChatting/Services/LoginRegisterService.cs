using AuberChattingDomain;
using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace AuberChatting.Services
{
    public class LoginRegisterService
    {
        public event Action<UserModel> Registered;
        public event Action<UserModel> LoggedIn;

        private readonly HubConnection _connection;

        public LoginRegisterService(HubConnection connection)
        {
            _connection = connection;

            _connection.On<UserModel>("Registered", (user) => Registered.Invoke(user));
            _connection.On<UserModel>("LoggedIn", (user) => LoggedIn.Invoke(user));
        }

        public async Task Connect()
        {
            await _connection.StartAsync();
        }

        public async Task Registrate(string nickname, string password)
        {
            MessageBox.Show("Registering...");
            await _connection.SendAsync("Register", nickname, password);
        }

        public async Task LogIn(string nickname, string password)
        {
            await _connection.SendAsync("LogIn", nickname, password);
        }
    }
}

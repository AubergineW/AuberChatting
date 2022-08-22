using AuberChatting.MVVM.ViewModels;
using AuberChatting.Services;
using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace AuberChatting
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            HubConnection loginRegisterConnection = new HubConnectionBuilder()
                .WithUrl("http://26.88.209.221:6892/loginregister")
                .Build();

            StartViewModel startViewModel = new StartViewModel(new LoginRegisterService(loginRegisterConnection));
            StartWindow startWindow = new StartWindow()
            {
                DataContext = startViewModel
            };

            startWindow.Show();
        }
    }
}

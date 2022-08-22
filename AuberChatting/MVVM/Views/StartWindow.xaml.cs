using AuberChatting.MVVM.ViewModels;
using AuberChatting.Services;
using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace AuberChatting
{
    public partial class StartWindow : Window
    {
        public StartWindow()
        {
            InitializeComponent();
            StartViewModel.OnWindowSwitch += SwitchWindow;
        }

        private void SwitchWindow()
        {
            HubConnection friendshipConnection = new HubConnectionBuilder()
                .WithUrl("http://26.88.209.221:6892/friendship")
                .Build();

            HubConnection chattingConnection = new HubConnectionBuilder()
                .WithUrl("http://26.88.209.221:6892/chatting")
                .Build();

            MainViewModel mainViewModel = new MainViewModel(new FriendshipService(friendshipConnection), new ChattingService(chattingConnection));
            MainWindow mainWindow = new MainWindow()
            {
                DataContext = mainViewModel,
            };


            mainWindow.Show();

            this.Close();
        }

        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
                DragMove();
        }
    }
}

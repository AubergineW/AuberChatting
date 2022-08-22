using AuberChatting.Core;
using AuberChatting.MVVM.Models;
using AuberChatting.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace AuberChatting.MVVM.ViewModels
{
    public class FindFriendViewModel : BaseViewModel
    {
        public RelayCommand FindFriendCommand { get; set; }

        private string _friendName;
        public string FriendName
        {
            get { return _friendName; }
            set
            {
                _friendName = value;
                OnPropertyChanged();
            }
        }

        private FriendshipService _friendshipService;

        public FindFriendViewModel(FriendshipService friendshipService)
        {
            _friendshipService = friendshipService;

            FindFriendCommand = new RelayCommand(async o => 
            {
                await _friendshipService.SendFriendRequest(FriendName, SelfModel.Nickname);
                FriendName = "";
            });

            _friendshipService.SentFriendRequest += SentFriendRequestCallback;
        }

        private void SentFriendRequestCallback(object success)
        {
            if (success != null)
            {
                MessageBox.Show("Successfully sent friend request!"); // TODO beautiful notifications
            }
            else
            {
                MessageBox.Show("There is no account with this name"); // TODO normal to client shit
            }
        }
    }
}

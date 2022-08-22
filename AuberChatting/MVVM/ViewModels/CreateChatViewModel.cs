using AuberChatting.Core;
using AuberChatting.MVVM.Models;
using AuberChatting.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace AuberChatting.MVVM.ViewModels
{
    public class CreateChatViewModel : BaseViewModel
    {
        public RelayCommand CreateChatCommand { get; set; }

        private string _chatName;
        public string ChatName 
        {
            get { return _chatName; }
            set 
            {
                _chatName = value;
                OnPropertyChanged();
            } 
        }

        public ObservableCollection<FriendModel> AllFriends { get; set; }

        private ChattingService _chattingService;

        public CreateChatViewModel(ChattingService chattingService)
        {
            _chattingService = chattingService;

            AllFriends = SelfModel.AllFriends;

            CreateChatCommand = new RelayCommand(async o =>
            {
                ObservableCollection<FriendModel> friends = new ObservableCollection<FriendModel>();

                foreach (FriendModel friend in (o as ListView).SelectedItems)
                {
                    friends.Add(friend);
                }

                if (friends.Count == 0)
                    return;

                await _chattingService.CreateChat(ChatName, SelfModel.Nickname, friends);
            });
        }
    }
}

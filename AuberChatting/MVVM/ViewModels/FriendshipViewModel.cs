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
    public class FriendshipViewModel : BaseViewModel
    {
        public RelayCommand FindFriendViewModelCommand { get; set; }
        public RelayCommand PendingFriendshipViewModelCommand { get; set; }
        public RelayCommand AllFriendsViewModelCommand { get; set; }

        private FriendshipService _friendshipService;

        private BaseViewModel _currentFriendshipViewModel;
        public BaseViewModel CurrentFriendshipViewModel 
        { 
            get { return _currentFriendshipViewModel; }
            set
            {
                _currentFriendshipViewModel = value;
                OnPropertyChanged();
            } 
        }

        private BaseViewModel FindFriendViewModel { get; set; }
        private BaseViewModel PendingFriendshipViewModel { get; set; }
        private BaseViewModel AllFriendsViewModel { get; set; }


        public FriendshipViewModel(FriendshipService friendshipService)
        {
            _friendshipService = friendshipService;

            _friendshipService.Connect().ContinueWith(task =>
            {
                if (task.Exception != null)
                    MessageBox.Show("Unable to connect to server!");
            });

            _friendshipService.SetConnectionID(SelfModel.Nickname);

            FindFriendViewModel = new FindFriendViewModel(_friendshipService);
            PendingFriendshipViewModel = new PendingFriendshipViewModel(_friendshipService);
            AllFriendsViewModel = new AllFriendsViewModel(_friendshipService);

            FindFriendViewModelCommand = new RelayCommand(o =>
            {
                CurrentFriendshipViewModel = FindFriendViewModel;
            });

            PendingFriendshipViewModelCommand = new RelayCommand(o =>
            {
                CurrentFriendshipViewModel = PendingFriendshipViewModel;
            });

            AllFriendsViewModelCommand = new RelayCommand(o =>
            {
                CurrentFriendshipViewModel = AllFriendsViewModel;
            });

            CurrentFriendshipViewModel = FindFriendViewModel;
        }
    }
}

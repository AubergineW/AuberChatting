using AuberChatting.Core;
using AuberChatting.MVVM.Models;
using AuberChatting.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuberChatting.MVVM.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        public string Nickname { get; set; }

        public RelayCommand FriendshipViewModelCommand { get; set; }
        public RelayCommand ChattingViewModelCommand { get; set; }

        private BaseViewModel _currentViewModel;
        public BaseViewModel CurrentViewModel
        {
            get { return _currentViewModel; }
            set
            {
                _currentViewModel = value;
                OnPropertyChanged();
            }
        }

        private FriendshipViewModel _friendshipViewModel;
        private ChattingViewModel _chattingViewModel;

        private FriendshipService _friendshipService;
        private ChattingService _chattingService;

        public MainViewModel(FriendshipService friendshipService, ChattingService chattingService)
        {
            _friendshipService = friendshipService;
            _chattingService = chattingService;

            _friendshipViewModel = new FriendshipViewModel(_friendshipService);
            _chattingViewModel = new ChattingViewModel(_chattingService);

            FriendshipViewModelCommand = new RelayCommand(o =>
            {
                CurrentViewModel = _friendshipViewModel;
            });

            ChattingViewModelCommand = new RelayCommand(o =>
            {
                CurrentViewModel = _chattingViewModel;
            });

            Nickname = SelfModel.Nickname;

            CurrentViewModel = _friendshipViewModel;
        }
    }
}

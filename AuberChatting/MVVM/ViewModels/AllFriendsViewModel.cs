using AuberChatting.Core;
using AuberChatting.MVVM.Models;
using AuberChatting.Services;
using AuberChattingDomain;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuberChatting.MVVM.ViewModels
{
    public class AllFriendsViewModel : BaseViewModel
    {
        private FriendshipService _friendshipService;

        private ObservableCollection<FriendModel> _allFriends;
        public ObservableCollection<FriendModel> AllFriends
        {
            get { return _allFriends; }
            set
            {
                _allFriends = value;
                OnPropertyChanged();
                SelfModel.AllFriends = AllFriends;
            }
        }

        public AllFriendsViewModel(FriendshipService friendshipService)
        {
            _friendshipService = friendshipService;
            AllFriends = new ObservableCollection<FriendModel>();

            _friendshipService.FriendRequestAccepted += FriendRequestAcceptedCallback;
            _friendshipService.GotFriends += GotFriendsCallback;
            _friendshipService.FriendRequestAnswered += FriendRequestAnsweredCallback;

            _friendshipService.DeletedFriendship += DeletedFriendshipCallback;
            _friendshipService.FriendshipDeleted += FriendshipDeletedCallback;

            AskFriends(SelfModel.Nickname);
        }

        private void FriendshipDeletedCallback(string deleterNickname)
        {
            AllFriends.Remove(AllFriends.FirstOrDefault(f => f.Nickname == deleterNickname));
        }

        private void DeletedFriendshipCallback(string deleteeNickname)
        {
            AllFriends.Remove(AllFriends.FirstOrDefault(f => f.Nickname == deleteeNickname));
        }

        private void FriendRequestAnsweredCallback(string senderNickname, bool isAccept)
        {
            if (isAccept)
            {
                AllFriends.Add(new FriendModel()
                {
                    Nickname = senderNickname,
                    DeleteFriend = new RelayCommand(o => { DeleteFriend(SelfModel.Nickname, senderNickname); })
                });
            }
        }

        private async void AskFriends(string nickname)
        {
            await _friendshipService.GetFriends(nickname);
        }

        private void GotFriendsCallback(List<string> friends)
        {
            foreach (string nickname in friends)
            {
                AllFriends.Add(new FriendModel()
                {
                    Nickname = nickname,
                    DeleteFriend = new RelayCommand(o => { DeleteFriend(SelfModel.Nickname, nickname); })
                });
            }
        }

        private void FriendRequestAcceptedCallback(string receiverNickname)
        {
            AllFriends.Add(new FriendModel()
            {
                Nickname = receiverNickname,
                DeleteFriend = new RelayCommand(o => { DeleteFriend(SelfModel.Nickname, receiverNickname); })
            });
        }

        private async void DeleteFriend(string deleterNickname, string deleteeNickname)
        {
            await _friendshipService.DeleteFriendship(deleterNickname, deleteeNickname);
        }
    }
}

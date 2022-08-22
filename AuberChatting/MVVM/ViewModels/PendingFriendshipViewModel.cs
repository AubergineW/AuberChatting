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
using System.Windows;

namespace AuberChatting.MVVM.ViewModels
{
    public class PendingFriendshipViewModel : BaseViewModel
    {
        private ObservableCollection<SentPendingFriendshipRequestModel> _sentPendingRequests;
        public ObservableCollection<SentPendingFriendshipRequestModel> SentPendingRequests 
        { 
            get { return _sentPendingRequests; }
            set
            {
                _sentPendingRequests = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<ReceivedPendingFriendshipRequestModel> _receivedPendingRequests;
        public ObservableCollection<ReceivedPendingFriendshipRequestModel> ReceivedPendingRequests
        {
            get { return _receivedPendingRequests; }
            set
            {
                _receivedPendingRequests = value;
                OnPropertyChanged();
            }
        }

        private FriendshipService _friendshipService;

        public PendingFriendshipViewModel(FriendshipService friendshipService)
        {
            _friendshipService = friendshipService;

            SentPendingRequests = new ObservableCollection<SentPendingFriendshipRequestModel>();
            ReceivedPendingRequests = new ObservableCollection<ReceivedPendingFriendshipRequestModel>();

            AskPendingRequests();

            _friendshipService.GotReceivedPendingRequests += GotReceivedPendingRequestsCallback;
            _friendshipService.GotSentPendingRequests += GotSentPendingRequestsCallback;

            _friendshipService.ReceivedFriendRequest += ReceivedFriendRequestCallback;
            _friendshipService.SentFriendRequest += SentFriendshipRequestCallback;

            _friendshipService.FriendRequestAccepted += FriendRequestAcceptedCallback;
            _friendshipService.FriendRequestDeclined += FriendRequestDeclinedCallback;

            _friendshipService.FriendRequestAnswered += FriendRequestAnsweredCallback;

            _friendshipService.DeletedFriendRequest += DeletedFriendRequestCallback;
            _friendshipService.FriendRequestDeleted += FriendRequestDeletedCallback;
        }

        private void FriendRequestDeletedCallback(string senderNickname)
        {
            ReceivedPendingRequests.Remove(ReceivedPendingRequests.FirstOrDefault(r => r.Nickname == senderNickname));
        }

        private void DeletedFriendRequestCallback(string receiverNickname)
        {
            SentPendingRequests.Remove(SentPendingRequests.FirstOrDefault(r => r.Nickname == receiverNickname));
        }

        private void FriendRequestAnsweredCallback(string requesterName, bool isAccept)
        {
            ReceivedPendingRequests.Remove(ReceivedPendingRequests.FirstOrDefault(r => r.Nickname == requesterName));
        }

        private void FriendRequestDeclinedCallback(string receiverNickname)
        {
            MessageBox.Show($"{receiverNickname} declined friend request >:(");

            SentPendingRequests.Remove(SentPendingRequests.FirstOrDefault(r => r.Nickname == receiverNickname));
        }

        private void FriendRequestAcceptedCallback(string receiverNickname)
        {
            MessageBox.Show($"{receiverNickname} accepted friend request!");

            SentPendingRequests.Remove(SentPendingRequests.FirstOrDefault(r => r.Nickname == receiverNickname));
        }

        private void GotSentPendingRequestsCallback(List<string> requests)
        {
            foreach (string nickname in requests)
            {
                SentPendingRequests.Add(new SentPendingFriendshipRequestModel()
                {
                    Nickname = nickname,
                    DeleteFriendRequest = new RelayCommand(o => { DeleteSentFriendRequest(nickname, SelfModel.Nickname); })
                });
            }
        }

        private void GotReceivedPendingRequestsCallback(List<string> requests)
        {
            foreach (string nickname in requests)
            {
                ReceivedPendingRequests.Add(new ReceivedPendingFriendshipRequestModel
                {
                    Nickname = nickname,
                    AcceptFriendshipRequest = new RelayCommand(o => { AnswerFriendRequest(nickname, SelfModel.Nickname, true); }),
                    DeclineFriendshipRequest = new RelayCommand(o => { AnswerFriendRequest(nickname, SelfModel.Nickname, false); }),
                });
            }
        }

        private async void AskPendingRequests()
        {
            await _friendshipService.GetPendingRequests(SelfModel.Nickname);
        }

        private void SentFriendshipRequestCallback(string receiverName)
        {
            SentPendingRequests.Add(new SentPendingFriendshipRequestModel()
            {
                Nickname = receiverName,
                DeleteFriendRequest = new RelayCommand(o => { DeleteSentFriendRequest(receiverName, SelfModel.Nickname); })
            });
        }

        private void ReceivedFriendRequestCallback(string senderName)
        {
            MessageBox.Show($"{senderName} sent you a friend request!");

            ReceivedPendingRequests.Add(new ReceivedPendingFriendshipRequestModel
            {
                Nickname = senderName,
                AcceptFriendshipRequest = new RelayCommand(o => { AnswerFriendRequest(senderName, SelfModel.Nickname, true); }),
                DeclineFriendshipRequest = new RelayCommand(o => { AnswerFriendRequest(senderName, SelfModel.Nickname, false); }),
            });
        }

        private async void AnswerFriendRequest(string requesterName, string selfNickname, bool isAccept)
        {
            await _friendshipService.AnswerFriendRequest(requesterName, selfNickname, isAccept);
            SentPendingRequests.Remove(SentPendingRequests.FirstOrDefault(r => r.Nickname == requesterName));
        }

        private async void DeleteSentFriendRequest(string receiverName, string selfNickname)
        {
            await _friendshipService.DeleteSentFriendRequest(receiverName, selfNickname);
        }
    }
}

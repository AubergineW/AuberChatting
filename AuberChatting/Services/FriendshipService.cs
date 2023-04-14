using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace AuberChatting.Services
{
    public class FriendshipService
    {
        public event Action<List<string>> GotReceivedPendingRequests;
        public event Action<List<string>> GotSentPendingRequests;
        public event Action<List<string>> GotFriends;

        public event Action<string> SentFriendRequest;
        public event Action<string> ReceivedFriendRequest;

        public event Action<string> FriendRequestDeclined;
        public event Action<string> FriendRequestAccepted;

        public event Action<string, bool> FriendRequestAnswered;

        public event Action<string> FriendRequestDeleted; // this one is for receiver
        public event Action<string> DeletedFriendRequest; // this one is for sender

        public event Action<string> FriendshipDeleted; // deletee
        public event Action<string> DeletedFriendship; // deleter

        private readonly HubConnection _connection;

        public FriendshipService(HubConnection connection)
        {
            _connection = connection;

            _connection.On<List<string>>("GotReceivedPendingRequests", (pendingRequests) => GotReceivedPendingRequests.Invoke(pendingRequests));
            _connection.On<List<string>>("GotSentPendingRequests", (pendingRequests) => GotSentPendingRequests.Invoke(pendingRequests));
            _connection.On<List<string>>("GotFriends", (friends) => GotFriends.Invoke(friends));

            _connection.On<string>("SentFriendRequest", (receiverName) => SentFriendRequest.Invoke(receiverName));
            _connection.On<string>("ReceivedFriendRequest", (senderName) => ReceivedFriendRequest.Invoke(senderName));

            _connection.On<string>("OnFriendRequestAccepted", (accepterName) => FriendRequestAccepted.Invoke(accepterName));
            _connection.On<string>("OnFriendRequestDeclined", (declinerName) => FriendRequestDeclined.Invoke(declinerName));

            _connection.On<string, bool>("OnAnsweredFriendRequest", (askerName, isAccept) => FriendRequestAnswered.Invoke(askerName, isAccept));

            _connection.On<string>("OnFriendRequestDeleted", (senderNickname) => FriendRequestDeleted.Invoke(senderNickname));
            _connection.On<string>("OnDeletedFriendRequest", (receiverNickname) => DeletedFriendRequest.Invoke(receiverNickname));

            _connection.On<string>("OnFriendshipDeleted", (deleterNickname) => FriendshipDeleted.Invoke(deleterNickname));
            _connection.On<string>("OnDeletedFriendship", (deleteeNickname) => DeletedFriendship.Invoke(deleteeNickname));
        }

        public async Task Connect()
        {
            await _connection.StartAsync();
        }

        public void SetConnectionID(string nickname)
        {
            _connection.SendAsync("SetConnectionID", nickname);
        }

        public async Task GetPendingRequests(string nickname)
        {
            await _connection.SendAsync("GetPendingRequests", nickname);
        }

        public async Task GetFriends(string nickname)
        {
            await _connection.SendAsync("GetFriends", nickname);
        }

        public async Task SendFriendRequest(string friendNickname, string selfNickname)
        {
            MessageBox.Show("trying to send friend request...");
            await _connection.SendAsync("SendFriendRequest", friendNickname, selfNickname);
        }

        public async Task AnswerFriendRequest(string requesterName, string selfNickname, bool isAccept)
        {
            await _connection.SendAsync("AnswerFriendRequest", requesterName, selfNickname, isAccept);
        }

        public async Task DeleteSentFriendRequest(string receiverName, string selfNickname)
        {
            await _connection.SendAsync("DeleteSentFriendRequest", receiverName, selfNickname);
        }

        public async Task DeleteFriendship(string deleterNickname, string deleteeNickname)
        {
            await _connection.SendAsync("DeleteFriendship", deleterNickname, deleteeNickname);
        }
    }
}

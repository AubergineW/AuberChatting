using AuberChattingServer.DataBase;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuberChattingServer.Hubs
{
    public class FriendshipHub : Hub
    {
        private IDataBase _dataBase = DataBaseProvider.GetDataBase();

        public void SetConnectionID(string nickname)
        {
            _dataBase.SetUserFriendshipConnectionID(nickname, Context.ConnectionId);
        }

        public async Task GetPendingRequests(string nickname)
        {
            await Clients.Caller.SendAsync("GotReceivedPendingRequests", _dataBase.GetPendingFriendshipsToUser(nickname));
            await Clients.Caller.SendAsync("GotSentPendingRequests", _dataBase.GetPendingFriendshipsFromUser(nickname));
        }

        public async Task GetFriends(string nickname)
        {
            await Clients.Caller.SendAsync("GotFriends", _dataBase.GetUserFriends(nickname));
        }

        public async Task SendFriendRequest(string friendNickname, string senderNickname)
        {
            Console.WriteLine($"{senderNickname} is sending friend request to {friendNickname}");

            if (_dataBase.GetUserByNickname(friendNickname) != null && 
                _dataBase.PendingFriendshipExists(senderNickname, friendNickname) == false && 
                _dataBase.FriendshipExists(senderNickname, friendNickname) == false)
            {
                await Clients.Caller.SendAsync("SentFriendRequest", friendNickname);

                _dataBase.AddPendingFriendship(senderNickname, friendNickname);

                await Clients.Client(_dataBase.GetUserByNickname(friendNickname).FriendshipConnectionID).SendAsync("ReceivedFriendRequest", senderNickname);
            }
            else
            {
                await Clients.Caller.SendAsync("SentFriendRequest", null);
            }
        }

        public async Task AnswerFriendRequest(string requesterName, string receiverName, bool isAccept)
        {
            if (isAccept)
            {
                Console.WriteLine($"{receiverName} is accepting friend request from {requesterName}");

                _dataBase.DeletePendingFriendship(requesterName, receiverName);
                _dataBase.AddFriendship(requesterName, receiverName);
                _dataBase.AddFriendship(receiverName, requesterName);

                await Clients.Caller.SendAsync("OnAnsweredFriendRequest", requesterName, isAccept);
                await Clients.Client(_dataBase.GetUserByNickname(requesterName).FriendshipConnectionID).SendAsync("OnFriendRequestAccepted", receiverName);
            }
            else
            {
                Console.WriteLine($"{receiverName} is not accepting friend request from {requesterName}");
                _dataBase.DeletePendingFriendship(requesterName, receiverName);

                await Clients.Caller.SendAsync("OnAnsweredFriendRequest", requesterName, isAccept);
                await Clients.Client(_dataBase.GetUserByNickname(requesterName).FriendshipConnectionID).SendAsync("OnFriendRequestDeclined", receiverName);
            }
        }

        public async Task DeleteSentFriendRequest(string receiverNickname, string senderNickname)
        {
            Console.WriteLine($"{senderNickname} is deleting friend request sent to {receiverNickname}");

            _dataBase.DeletePendingFriendship(senderNickname, receiverNickname);

            await Clients.Caller.SendAsync("OnDeletedFriendRequest", receiverNickname);
            await Clients.Client(_dataBase.GetUserByNickname(receiverNickname).FriendshipConnectionID).SendAsync("OnFriendRequestDeleted", senderNickname);
        }

        public async Task DeleteFriendship(string deleterNickname, string deleteeNickname)
        {
            Console.WriteLine($"{deleterNickname} is deleting friendship with {deleteeNickname}");

            _dataBase.DeleteFriendship(deleterNickname, deleteeNickname);
            _dataBase.DeleteFriendship(deleteeNickname, deleterNickname);

            await Clients.Caller.SendAsync("OnDeletedFriendship", deleteeNickname);
            await Clients.Client(_dataBase.GetUserByNickname(deleteeNickname).FriendshipConnectionID).SendAsync("OnFriendshipDeleted", deleterNickname);
        }
    }
}

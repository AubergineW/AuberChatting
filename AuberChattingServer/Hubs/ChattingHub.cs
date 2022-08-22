using AuberChattingDomain;
using AuberChattingServer.DataBase;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace AuberChattingServer.Hubs
{
    public class ChattingHub : Hub
    {
        private IDataBase _dataBase = DataBaseProvider.GetDataBase();

        public void SetConnectionID(string nickname)
        {
            _dataBase.SetUserChattingConnectionID(nickname, Context.ConnectionId);
        }

        public async Task GetChats(string nickname)
        {
            await Clients.Caller.SendAsync("GotChats", _dataBase.GetUserChats(nickname));
            await Clients.Caller.SendAsync("GotChatInvites", _dataBase.GetUserInvites(nickname));
        }

        public async Task CreateChat(string chatName, string creatorName, List<UserModel> invitees)
        {
            if (_dataBase.GetChat(chatName) != null)
            {
                await Clients.Caller.SendAsync("ChatCreated", null);
                return;
            }

            List<string> inviteesNames = new List<string>();

            foreach (UserModel invitee in invitees)
            {
                inviteesNames.Add(invitee.Nickname);
            }

            string message = "";

            foreach (string inviteeName in inviteesNames)
            {
                message += $" {inviteeName}";
            }

            Console.WriteLine(message += $" were invited by {creatorName} to {chatName}");

            _dataBase.AddChatMember(chatName, creatorName);

            foreach (string inviteeName in inviteesNames)
            {
                _dataBase.AddChatInvention(chatName, creatorName, inviteeName);

                await Clients.Client(_dataBase.GetUserByNickname(inviteeName).ChattingConnectionID).SendAsync("InvitedToChat", chatName, creatorName);
            }

            _dataBase.AddChatMessage(chatName, "Sex Master 3000", "This is the start of your Chat!");

            ChatModel chat = new ChatModel()
            {
                ChatName = chatName,
                IsInvention = false,
                Members = new List<UserModel>() { new UserModel() { Nickname = creatorName, } },
                InvitedMembers = invitees,
                Messages = _dataBase.GetChatMessages(chatName),
            };

            await Clients.Caller.SendAsync("ChatCreated", chat);
        }

        public async Task JoinChat(string chatName, string joinerNickname)
        {
            _dataBase.AddChatMessage(chatName, "Sex Master 3000", $"{joinerNickname} joined Chat!");

            await Clients.Caller.SendAsync("JoinerJoinedChat", new ChatModel()
            {
                ChatName = chatName,
                IsInvention = false,
                Members = _dataBase.GetChatUsers(chatName),
                InvitedMembers = _dataBase.GetChatInvitees(chatName),
                Messages = _dataBase.GetChatMessages(chatName),
            });

            _dataBase.DeleteChatInvention(chatName, joinerNickname);

            foreach (UserModel user in _dataBase.GetChatUsers(chatName))
            {
                await Clients.Client(_dataBase.GetUserByNickname(user.Nickname).ChattingConnectionID).SendAsync("MembersJoinedChat", chatName, joinerNickname);
            }

            _dataBase.AddChatMember(chatName, joinerNickname);
        }

        public async Task SendMessage(string chatName, MessageModel message)
        {
            _dataBase.AddChatMessage(chatName, message.SenderName, message.MessageText);

            await Clients.Caller.SendAsync("SentMessage", chatName, message);

            foreach (UserModel user in _dataBase.GetChatUsers(chatName))
            {
                await Clients.Client(_dataBase.GetUserByNickname(user.Nickname).ChattingConnectionID).SendAsync("ReceivedMessage", chatName, message);
            }
        }
    }
}
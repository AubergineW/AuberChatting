using AuberChatting.MVVM.Models;
using AuberChattingDomain;
using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuberChatting.Services
{
    public class ChattingService
    {
        public event Action<List<ChatModel>> GotChats;

        public event Action<string, string> InvitedToChat;
        public event Action<ChatModel> ChatCreated;

        public event Action<ChatModel> JoinerJoinedChat;
        public event Action<string, string> ChatMembersJoinedChat;

        public event Action<string, MessageModel> MessageSent;
        public event Action<string, MessageModel> MessageReceived;

        private HubConnection _connection;

        public ChattingService(HubConnection connection)
        {
            _connection = connection;

            _connection.On<string, string>("InvitedToChat", (chatName, inviterName) => InvitedToChat.Invoke(chatName, inviterName));
            _connection.On<ChatModel>("ChatCreated", (chat) => ChatCreated.Invoke(chat));

            _connection.On<ChatModel>("JoinerJoinedChat", (chat) => JoinerJoinedChat.Invoke(chat));
            _connection.On<string, string>("MembersJoinedChat", (chatName, userNickname) => ChatMembersJoinedChat.Invoke(chatName, userNickname));

            _connection.On<List<ChatModel>>("GotChats", (chat) => GotChats.Invoke(chat));
            _connection.On<List<ChatModel>>("GotChatInvites", (chat) => GotChats.Invoke(chat));

            _connection.On<string, MessageModel>("SentMessage", (chatName, message) => MessageSent.Invoke(chatName, message));
            _connection.On<string, MessageModel>("ReceivedMessage", (chatName, message) => MessageReceived.Invoke(chatName, message));
        }

        public async Task Connect()
        {
            await _connection.StartAsync();
        }

        public void SetConnectionID(string nickname)
        {
            _connection.SendAsync("SetConnectionID", nickname);
        }

        public async Task AskChats(string nickname)
        {
            await _connection.SendAsync("GetChats", nickname);
        }

        public async Task CreateChat(string chatName, string selfNickname, ObservableCollection<FriendModel> invitees)
        {
            await _connection.SendAsync("CreateChat", chatName, selfNickname, invitees);
        }

        public async Task JoinChat(string chatName, string nickname)
        {
            await _connection.SendAsync("JoinChat", chatName, nickname);
        }

        public async Task RefuseJoinChat(string chatName, string nickname)
        {
            await _connection.SendAsync("RefuseJoinChat", chatName, nickname);
        }

        public async Task SendMessage(string chatName, MessageModel message)
        {
            await _connection.SendAsync("SendMessage", chatName, message);
        }

        public async Task LeaveChat()
        {

        }
    }
}

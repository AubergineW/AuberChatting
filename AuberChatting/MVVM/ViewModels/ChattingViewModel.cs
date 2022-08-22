using AuberChatting.Core;
using AuberChatting.MVVM.Models;
using AuberChatting.MVVM.Views;
using AuberChatting.Services;
using AuberChattingDomain;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace AuberChatting.MVVM.ViewModels
{
    public class ChattingViewModel : BaseViewModel
    {
        public RelayCommand CreateChatCommand { get; set; }
        public RelayCommand SendMessageCommand { get; set; }

        public static event Action MembersListUpdated;
        public static event Action MessagesListUpdated;

        private string _messageText;
        public string MessageText
        {
            get { return _messageText; }
            set 
            { 
                _messageText = value;
                OnPropertyChanged();
            }
        }


        private ObservableCollection<ChatModel> _allChats;
        public ObservableCollection<ChatModel> AllChats
        {
            get { return _allChats; }
            set
            {
                _allChats = value;
                OnPropertyChanged();
            }
        }

        private ChatModel _selectedChat;
        public ChatModel SelectedChat
        {
            get { return _selectedChat; }
            set 
            { 
                _selectedChat = value;
                OnPropertyChanged();
            }
        }


        private ChattingService _chattingService;

        public ChattingViewModel(ChattingService chattingService)
        {
            _chattingService = chattingService;

            AllChats = new ObservableCollection<ChatModel>();

            _chattingService.Connect().ContinueWith(task =>
            {
                if (task.Exception != null)
                    MessageBox.Show("Unable to connect to the server");
            });

            _chattingService.SetConnectionID(SelfModel.Nickname);

            CreateChatCommand = new RelayCommand(o =>
            {
                CreateChatWindow createGroupWindow = new CreateChatWindow
                {
                    DataContext = new CreateChatViewModel(_chattingService)
                };

                createGroupWindow.Show();
            });

            SendMessageCommand = new RelayCommand(async o => 
            {
                await _chattingService.SendMessage(SelectedChat.ChatName, new MessageModel() 
                {
                    SenderName = SelfModel.Nickname,
                    MessageText = MessageText
                });
            });

            _chattingService.InvitedToChat += InvitedToChatCallback;
            _chattingService.ChatCreated += ChatCreatedCallback;

            _chattingService.JoinerJoinedChat += JoinerJoinedChatCallback;
            _chattingService.ChatMembersJoinedChat += ChatMembersJoinedChatCallback;

            _chattingService.MessageSent += MessageSentCallback;
            _chattingService.MessageReceived += MessageReceivedCallback;

            _chattingService.GotChats += GotChatsCallback;

            GetChats(SelfModel.Nickname);
        }

        private void MessageReceivedCallback(string chatName, MessageModel message)
        {
            AllChats.FirstOrDefault(c => c.ChatName == chatName).Messages.Add(message);

            MessagesListUpdated.Invoke();
        }

        private void MessageSentCallback(string chatName, MessageModel message)
        {
            MessageBox.Show($"Message sent to {chatName}");
        }

        private async void GetChats(string nickname)
        {
            await _chattingService.AskChats(nickname);
        }

        private void GotChatsCallback(List<ChatModel> chats)
        {
            foreach (ChatModel chat in chats)
            {
                if (chat.IsInvention == true)
                {
                    chat.JoinChatCommand = new RelayCommand(async o =>
                    {
                        MessageBox.Show("Joining chat...");
                        await AnswerChatInvention(chat.ChatName, SelfModel.Nickname, true);
                    });

                    chat.RefuseJoinChatCommand = new RelayCommand(async o =>
                    {
                        MessageBox.Show("you are shitass");
                        await AnswerChatInvention(chat.ChatName, SelfModel.Nickname, false);
                    });
                }

                AllChats.Add(chat);
            }
        }

        private void ChatMembersJoinedChatCallback(string chatName, string joinerName)
        {
            ChatModel joineeChat = AllChats.FirstOrDefault(c => c.ChatName == chatName);
            joineeChat.InvitedMembers.Remove(joineeChat.InvitedMembers.FirstOrDefault(u => u.Nickname == joinerName));
            joineeChat.Members.Add(new UserModel()
            {
                Nickname = joinerName,
            });

            MembersListUpdated.Invoke();

            joineeChat.Messages.Add(new MessageModel()
            {
                SenderName = "Sex Master 3000",
                MessageText = $"{joinerName} joined chat!"
            });
        }

        private void JoinerJoinedChatCallback(ChatModel chat)
        {
            AllChats.Remove(AllChats.FirstOrDefault(c => c.ChatName == chat.ChatName));
            AllChats.Add(chat);
            chat.Members.Add(chat.InvitedMembers.FirstOrDefault(c => c.Nickname == SelfModel.Nickname));
            chat.InvitedMembers.Remove(chat.InvitedMembers.FirstOrDefault(c => c.Nickname == SelfModel.Nickname));

            MembersListUpdated.Invoke();
        }

        private void ChatCreatedCallback(ChatModel chat)
        {
            if (chat == null)
            {
                MessageBox.Show("This chat name is already taken");
                return;
            }

            AllChats.Add(chat);
            chat.Messages.Add(new MessageModel()
            {
                SenderName = "Sex Master 3000",
                MessageText = "This is the start of your AuberChat!"
            });
        }

        private void InvitedToChatCallback(string chatName, string inviterName)
        {
            MessageBox.Show($"You were invited to chat '{chatName}' by {inviterName}");

            AllChats.Add(new ChatModel
            {
                ChatName = chatName,
                IsInvention = true,

                JoinChatCommand = new RelayCommand(async o =>
                {
                    MessageBox.Show("Joining chat...");
                    await AnswerChatInvention(chatName, SelfModel.Nickname, true);
                }),

                RefuseJoinChatCommand = new RelayCommand(async o =>
                {
                    MessageBox.Show("you are shitass");
                    await AnswerChatInvention(chatName, SelfModel.Nickname, false);
                })
            });
        }

        private async Task AnswerChatInvention(string chatName, string nickname, bool isAccept)
        {
            if (isAccept)
                await _chattingService.JoinChat(chatName, nickname);
            else
                await _chattingService.RefuseJoinChat(chatName, nickname);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AuberChattingDomain;

namespace AuberChattingServer.DataBase
{
    public interface IDataBase
    {
        void AddUser(UserModel user);

        void SetUserLoginRegisterConnectionID(string userNickname, string connectionID);
        void SetUserFriendshipConnectionID(string userNickname, string connectionID);
        void SetUserChattingConnectionID(string userNickname, string connectionID);

        UserModel GetUser(string Nickname, string id, string Password);
        UserModel GetUserByNickname(string nickname);
        UserModel GetUserByPassword(string password);
        UserModel GetUserByID(int id);
        UserModel GetUserByLoginRegisterConnectionID(string connectionID);

        void AddPendingFriendship(string user1Nickname, string user2Nickname);
        void DeletePendingFriendship(string user1Nickname, string user2Nickname);

        bool PendingFriendshipExists(string user1Nickname, string user2Nickname);

        void AddFriendship(string user1Nickname, string user2Nickname);
        void DeleteFriendship(string user1Nickname, string user2Nickname);

        bool FriendshipExists(string user1Nickname, string user2Nickname);

        List<string> GetPendingFriendshipsFromUser(string user1Nickname);
        List<string> GetPendingFriendshipsToUser(string user2Nickname);

        List<string> GetUserFriends(string user1Nickname);

        void AddChatMember(string chatName, string creatorName);
        void AddChatInvention(string chatName, string inviterName, string inviteeNickname);

        void AddChatMessage(string chatName, string senderName, string messageText);

        List<ChatModel> GetUserChats(string userName);
        List<ChatModel> GetUserInvites(string userName);
        List<UserModel> GetChatUsers(string chatName);

        void DeleteUserFromChat(string chatName, string userName);
        void DeleteChat(string chatName);

        ChatModel GetChat(string chatName);
        List<UserModel> GetChatInvitees(string chatName);
        List<MessageModel> GetChatMessages(string chatName);
        void DeleteChatInvention(string chatName, string joinerNickname);
    }
}

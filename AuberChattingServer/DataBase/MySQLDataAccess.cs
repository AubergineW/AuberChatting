using AuberChattingDomain;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace AuberChattingServer.DataBase
{
    public class MySQLDataAccess : IDataBase
    {
        private MySqlConnection _connection;
        private readonly IConfigurationRoot _configuration;

        public MySQLDataAccess()
        {
            _configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();

            _connection = new MySqlConnection(_configuration.GetConnectionString("Users"));
            _connection.Open();
        }


        public void AddUser(UserModel user)
        {
            MySqlCommand command = new MySqlCommand($"Insert into Users VALUES(0,'{user.Nickname}','{user.Password}','{user.LoginRegisterConnectionID}','waiting','waiting')", _connection);
            command.ExecuteNonQuery();
        }

        public UserModel GetUser(string Nickname, string id, string Password)
        {
            throw new NotImplementedException();
        }

        public UserModel GetUserByID(int id)
        {
            MySqlCommand command = new MySqlCommand($"Select * from Users Where id = '{id}'", _connection);
            MySqlDataReader reader = command.ExecuteReader();

            DataTable dataTable = new DataTable();
            dataTable.Load(reader);

            if (dataTable.Rows.Count <= 0)
                return null;

            return new UserModel()
            {
                Nickname = dataTable.Rows[0]["Nickname"].ToString(),
                Password = dataTable.Rows[0]["Password"].ToString(),
                LoginRegisterConnectionID = dataTable.Rows[0]["ConnectionID"].ToString()
            };
        }

        public UserModel GetUserByNickname(string nickname)
        {
            MySqlCommand command = new MySqlCommand($"Select * from Users Where Nickname = '{nickname}'", _connection);
            MySqlDataReader reader = command.ExecuteReader();

            DataTable dataTable = new DataTable();
            dataTable.Load(reader);

            if (dataTable.Rows.Count <= 0)
                return null;

            return new UserModel()
            {
                Nickname = dataTable.Rows[0]["Nickname"].ToString(),
                Password = dataTable.Rows[0]["Password"].ToString(),
                LoginRegisterConnectionID = dataTable.Rows[0]["LoginRegisterConnectionID"].ToString(),
                FriendshipConnectionID = dataTable.Rows[0]["FriendshipConnectionID"].ToString(),
                ChattingConnectionID = dataTable.Rows[0]["ChattingConnectionID"].ToString(),
            };
        }

        public UserModel GetUserByPassword(string password)
        {
            MySqlCommand command = new MySqlCommand($"Select * from Users Where Password = '{password}'", _connection);
            MySqlDataReader reader = command.ExecuteReader();

            DataTable dataTable = new DataTable();
            dataTable.Load(reader);

            if (dataTable.Rows.Count <= 0)
                return null;

            return new UserModel()
            {
                Nickname = dataTable.Rows[0]["Nickname"].ToString(),
                Password = dataTable.Rows[0]["Password"].ToString(),
                LoginRegisterConnectionID = dataTable.Rows[0]["LoginRegisterConnectionID"].ToString(),
                FriendshipConnectionID = dataTable.Rows[0]["FriendshipConnectionID"].ToString(),
                ChattingConnectionID = dataTable.Rows[0]["ChattingConnectionID"].ToString(),
            };
        }

        public void SetUserLoginRegisterConnectionID(string userNickname, string connectionID)
        {
            MySqlCommand command = new MySqlCommand($"Update Users set LoginRegisterConnectionID = '{connectionID}' where Nickname = '{userNickname}'", _connection);
            command.ExecuteNonQuery();
        }

        public UserModel GetUserByLoginRegisterConnectionID(string connectionID)
        {
            MySqlCommand command = new MySqlCommand($"Select * from Users Where LoginRegisterConnectionID = '{connectionID}'", _connection);
            MySqlDataReader reader = command.ExecuteReader();

            DataTable dataTable = new DataTable();
            dataTable.Load(reader);

            if (dataTable.Rows.Count <= 0)
                return null;


            return new UserModel()
            {
                Nickname = dataTable.Rows[0]["Nickname"].ToString(),
                Password = dataTable.Rows[0]["Password"].ToString(),
                LoginRegisterConnectionID = dataTable.Rows[0]["LoginRegisterConnectionID"].ToString(),
                FriendshipConnectionID = dataTable.Rows[0]["FriendshipConnectionID"].ToString(),
                ChattingConnectionID = dataTable.Rows[0]["ChattingConnectionID"].ToString(),
            };
        }

        public void SetUserFriendshipConnectionID(string userNickname, string connectionID)
        {
            MySqlCommand command = new MySqlCommand($"Update Users set FriendshipConnectionID = '{connectionID}' where Nickname = '{userNickname}'", _connection);
            command.ExecuteNonQuery();
        }

        public UserModel GetUserByFriendshipConnectionID(string connectionID)
        {
            MySqlCommand command = new MySqlCommand($"Select * from Users Where FriendshipConnectionID = '{connectionID}'", _connection);
            MySqlDataReader reader = command.ExecuteReader();

            DataTable dataTable = new DataTable();
            dataTable.Load(reader);

            if (dataTable.Rows.Count <= 0)
                return null;

            return new UserModel()
            {
                Nickname = dataTable.Rows[0]["Nickname"].ToString(),
                Password = dataTable.Rows[0]["Password"].ToString(),
                LoginRegisterConnectionID = dataTable.Rows[0]["LoginRegisterConnectionID"].ToString(),
                FriendshipConnectionID = dataTable.Rows[0]["FriendshipConnectionID"].ToString(),
                ChattingConnectionID = dataTable.Rows[0]["ChattingConnectionID"].ToString(),
            };
        }

        public void AddPendingFriendship(string user1Nickname, string user2Nickname)
        {
            MySqlCommand command = new MySqlCommand($"Insert into PendingFriendship Values('{user1Nickname}','{user2Nickname}')", _connection);
            command.ExecuteNonQuery();
        }

        public void DeletePendingFriendship(string user1Nickname, string user2Nickname)
        {
            MySqlCommand command = new MySqlCommand($"delete from PendingFriendship where user1 = '{user1Nickname}' and user2 = '{user2Nickname}'", _connection);
            command.ExecuteNonQuery();
        }

        public bool PendingFriendshipExists(string user1Nickname, string user2Nickname)
        {
            bool exists;
            MySqlCommand command = new MySqlCommand($"Select * From PendingFriendship Where user1 = '{user1Nickname}' and user2 = '{user2Nickname}'", _connection);
            MySqlDataReader reader = command.ExecuteReader();

            DataTable dataTable = new DataTable();
            dataTable.Load(reader);

            exists = dataTable.Rows.Count > 0;

            if (exists)
                return exists;

            command = new MySqlCommand($"Select * From PendingFriendship Where user1 = '{user2Nickname}' and user2 = '{user1Nickname}'", _connection);
            reader = command.ExecuteReader();

            dataTable = new DataTable();
            dataTable.Load(reader);

            exists = dataTable.Rows.Count > 0;

            return exists;
        }

        public List<string> GetPendingFriendshipsFromUser(string user1Nickname)
        {
            MySqlCommand command = new MySqlCommand($"Select * From PendingFriendship Where user1 = '{user1Nickname}'", _connection);
            MySqlDataReader reader = command.ExecuteReader();

            DataTable dataTable = new DataTable();
            dataTable.Load(reader);

            List<string> friendRequests = new List<string>();

            if (dataTable.Rows.Count <= 0)
                return null;

            foreach (DataRow row in dataTable.Rows)
            {
                friendRequests.Add(row["user2"].ToString());
            }

            return friendRequests;
        }

        public List<string> GetPendingFriendshipsToUser(string user2Nickname)
        {
            MySqlCommand command = new MySqlCommand($"Select * From PendingFriendship Where user2 = '{user2Nickname}'", _connection);
            MySqlDataReader reader = command.ExecuteReader();

            DataTable dataTable = new DataTable();
            dataTable.Load(reader);

            List<string> friendRequests = new List<string>();

            if (dataTable.Rows.Count <= 0)
                return null;

            foreach (DataRow row in dataTable.Rows)
            {
                friendRequests.Add(row["user1"].ToString());
            }

            return friendRequests;
        }

        public void AddFriendship(string user1Nickname, string user2Nickname)
        {
            MySqlCommand command = new MySqlCommand($"Insert into Friendship Values('{user1Nickname}','{user2Nickname}')", _connection);
            command.ExecuteNonQuery();
        }

        public void DeleteFriendship(string user1Nickname, string user2Nickname)
        {
            MySqlCommand command = new MySqlCommand($"delete from Friendship where user1 = '{user1Nickname}' and user2 = '{user2Nickname}'", _connection);
            command.ExecuteNonQuery();
        }

        public bool FriendshipExists(string user1Nickname, string user2Nickname)
        {
            MySqlCommand command = new MySqlCommand($"Select * From Friendship Where user1 = '{user1Nickname}' and user2 = '{user2Nickname}'", _connection);
            MySqlDataReader reader = command.ExecuteReader();

            DataTable dataTable = new DataTable();
            dataTable.Load(reader);

            return dataTable.Rows.Count > 0;
        }

        public List<string> GetUserFriends(string userNickname)
        {
            MySqlCommand command = new MySqlCommand($"Select * From Friendship Where user1 = '{userNickname}'", _connection);
            MySqlDataReader reader = command.ExecuteReader();

            DataTable dataTable = new DataTable();
            dataTable.Load(reader);

            List<string> friendRequests = new List<string>();

            if (dataTable.Rows.Count <= 0)
                return null;

            foreach (DataRow row in dataTable.Rows)
            {
                friendRequests.Add(row["user2"].ToString());
            }

            return friendRequests;
        }

        public void SetUserChattingConnectionID(string userNickname, string connectionID)
        {
            MySqlCommand command = new MySqlCommand($"Update Users set ChattingConnectionID = '{connectionID}' where Nickname = '{userNickname}'", _connection);
            command.ExecuteNonQuery();
        }

        public UserModel GetUserByChattingConnectionID(string connectionID)
        {
            throw new NotImplementedException();
        }

        public void AddChatMember(string chatName, string userNickname)
        {
            MySqlCommand command = new MySqlCommand($"Insert into groupMembers values('{chatName}', '{userNickname}')", _connection);
            command.ExecuteNonQuery();
        }

        public void AddChatInvention(string chatName, string inviterName, string inviteeNickname)
        {
            MySqlCommand command = new MySqlCommand($"Insert into groupInvites values('{chatName}', '{inviterName}', '{inviteeNickname}')", _connection);
            command.ExecuteNonQuery();
        }

        public List<ChatModel> GetUserChats(string userName)
        {
            MySqlCommand command = new MySqlCommand($"Select * from groupMembers where memberName = '{userName}'", _connection);
            MySqlDataReader reader = command.ExecuteReader();

            DataTable dataTable = new DataTable();
            dataTable.Load(reader);

            List<ChatModel> chats = new List<ChatModel>();

            if (dataTable.Rows.Count <= 0)
                return null;

            foreach (DataRow row in dataTable.Rows)
            {
                chats.Add(new ChatModel()
                {
                    ChatName = row["groupName"].ToString(),
                    IsInvention = false,
                    Members = GetChatUsers(row["groupName"].ToString()),
                    InvitedMembers = GetChatInvitees(row["groupName"].ToString()),
                    Messages = GetChatMessages(row["groupName"].ToString()),
                });
            }

            return chats;
        }

        public List<UserModel> GetChatUsers(string chatName)
        {
            MySqlCommand command = new MySqlCommand($"Select * from groupMembers where groupName = '{chatName}'", _connection);
            MySqlDataReader reader = command.ExecuteReader();

            DataTable dataTable = new DataTable();
            dataTable.Load(reader);

            List<UserModel> users = new List<UserModel>();

            if (dataTable.Rows.Count <= 0)
                return null;

            foreach (DataRow row in dataTable.Rows)
            {
                users.Add(new UserModel()
                {
                    Nickname = row["memberName"].ToString(),
                });
            }

            return users;
        }

        public void DeleteUserFromChat(string chatName, string userName)
        {
            throw new NotImplementedException();
        }

        public void DeleteChat(string chatName)
        {
            throw new NotImplementedException();
        }

        public ChatModel GetChat(string chatName)
        {
            MySqlCommand command = new MySqlCommand($"Select * from groupMembers where groupName = '{chatName}'", _connection);
            MySqlDataReader reader = command.ExecuteReader();

            DataTable dataTable = new DataTable();
            dataTable.Load(reader);

            ChatModel chat = new ChatModel();

            if (dataTable.Rows.Count <= 0)
                return null;

            foreach (DataRow row in dataTable.Rows)
            {
                chat = new ChatModel()
                {
                    ChatName = row["groupName"].ToString(),
                };
            }

            return chat;
        }

        public List<UserModel> GetChatInvitees(string chatName)
        {
            MySqlCommand command = new MySqlCommand($"Select * from groupInvites where groupName = '{chatName}'", _connection);
            MySqlDataReader reader = command.ExecuteReader();

            DataTable dataTable = new DataTable();
            dataTable.Load(reader);

            List<UserModel> invitees = new List<UserModel>();

            if (dataTable.Rows.Count <= 0)
                return null;

            foreach (DataRow row in dataTable.Rows)
            {
                invitees.Add(new UserModel()
                {
                    Nickname = row["inviteeName"].ToString(),
                });
            }

            return invitees;
        }

        public List<MessageModel> GetChatMessages(string chatName)
        {
            MySqlCommand command = new MySqlCommand($"Select * from groupMessages where groupName = '{chatName}'", _connection);
            MySqlDataReader reader = command.ExecuteReader();

            DataTable dataTable = new DataTable();
            dataTable.Load(reader);

            List<MessageModel> messages = new List<MessageModel>();

            if (dataTable.Rows.Count <= 0)
                return null;

            foreach (DataRow row in dataTable.Rows)
            {
                messages.Add(new MessageModel()
                {
                    SenderName = row["senderName"].ToString(),
                    MessageText = row["messageText"].ToString()
                });
            }

            return messages;
        }

        public void AddChatMessage(string chatName, string senderName, string messageText)
        {
            MySqlCommand command = new MySqlCommand($"Insert into groupMessages values('{chatName}', '{senderName}', '{messageText}')", _connection);
            command.ExecuteNonQuery();
        }

        public void DeleteChatInvention(string chatName, string userNickname)
        {
            MySqlCommand command = new MySqlCommand($"delete from groupInvites where groupName = '{chatName}' and inviteeName = '{userNickname}'", _connection);
            command.ExecuteNonQuery();
        }

        public List<ChatModel> GetUserInvites(string userName)
        {
            MySqlCommand command = new MySqlCommand($"Select * from groupInvites where inviteeName = '{userName}'", _connection);
            MySqlDataReader reader = command.ExecuteReader();

            DataTable dataTable = new DataTable();
            dataTable.Load(reader);

            List<ChatModel> chats = new List<ChatModel>();

            if (dataTable.Rows.Count <= 0)
                return null;

            foreach (DataRow row in dataTable.Rows)
            {
                chats.Add(new ChatModel()
                {
                    ChatName = row["groupName"].ToString(),
                    IsInvention = true,
                });
            }

            return chats;
        }
    }
}

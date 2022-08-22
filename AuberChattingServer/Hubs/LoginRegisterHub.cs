using AuberChattingDomain;
using AuberChattingServer.DataBase;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuberChattingServer.Hubs
{
    public class LoginRegisterHub : Hub
    {
        private IDataBase _dataBase = DataBaseProvider.GetDataBase();

        public async Task Register(string nickname, string password)
        {
            Console.WriteLine($"{nickname} is trying to register... {Context.ConnectionId}");

            if (_dataBase.GetUserByNickname(nickname) != null)
            {
                await Clients.Caller.SendAsync("Registered", null);
                Console.WriteLine($"{nickname} is already taken");

                return;
            }

            UserModel user = new UserModel()
            {
                Nickname = nickname,
                Password = password,
                LoginRegisterConnectionID = Context.ConnectionId,
            };

            _dataBase.AddUser(user);
            await Clients.Caller.SendAsync("Registered", user);

            Console.WriteLine($"{nickname} registered!");
        }

        public async Task LogIn(string nickname, string password)
        {
            Console.WriteLine($"{nickname} is trying to log in");

            if (_dataBase.GetUserByNickname(nickname) == null) 
            {
                await Clients.Caller.SendAsync("LoggedIn", null);
                Console.WriteLine($"There is no account with {nickname}");
            }

            if (_dataBase.GetUserByNickname(nickname).Nickname != _dataBase.GetUserByPassword(password).Password)
            {
                await Clients.Caller.SendAsync("LoggedIn", null);
                Console.WriteLine($"The password is incorrect!");
            }

            _dataBase.SetUserLoginRegisterConnectionID(nickname, Context.ConnectionId);

            await Clients.Caller.SendAsync("LoggedIn", new UserModel()
            {
                Nickname = nickname,
                LoginRegisterConnectionID = Context.ConnectionId,
            });

            Console.WriteLine($"{nickname} logged in! {Context.ConnectionId}");
        }
    }
}

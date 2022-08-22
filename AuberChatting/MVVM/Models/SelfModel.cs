using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuberChatting.MVVM.Models
{
    public static class SelfModel
    {
        public static string Nickname { get; private set; }
        public static ObservableCollection<FriendModel> AllFriends { get; set; }

        public static void SetSelfModel(string nickname)
        {
            Nickname = nickname;
        }
    }
}

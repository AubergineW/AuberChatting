using AuberChatting.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuberChatting.MVVM.Models
{
    public class ReceivedPendingFriendshipRequestModel
    {
        public string Nickname { get; set; }
        public RelayCommand AcceptFriendshipRequest { get; set; }
        public RelayCommand DeclineFriendshipRequest { get; set; }
    }
}

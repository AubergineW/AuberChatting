using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace AuberChattingDomain
{
    public class ChatModel
    {
        public string ChatName { get; set; }
        public bool IsInvention { get; set; }
        public List<UserModel> Members { get; set; }
        public List<UserModel> InvitedMembers { get; set; }
        public List<MessageModel> Messages { get; set; }
        public object JoinChatCommand { get; set; }
        public object RefuseJoinChatCommand { get; set; }
    }
}

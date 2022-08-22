using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuberChattingDomain
{
    public class UserModel
    {
        public string Nickname { get; set; }
        public string Password { get; set; }
        public string LoginRegisterConnectionID { get; set; }
        public string FriendshipConnectionID { get; set; }
        public string ChattingConnectionID { get; set; }
    }
}

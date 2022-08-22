using AuberChattingDomain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace AuberChatting.Core
{
    public class ChatCardStyleSelector : StyleSelector
    {
        public Style ChatCard { get; set; }
        public Style ChatInventionCard { get; set; }

        public override Style SelectStyle(object item, DependencyObject container)
        {
            ChatModel chat = (ChatModel)item;

            if (chat.IsInvention == true)
                return ChatInventionCard;
            else
                return ChatCard;
        }
    }
}

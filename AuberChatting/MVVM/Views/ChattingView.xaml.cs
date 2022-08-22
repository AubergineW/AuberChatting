using AuberChatting.MVVM.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace AuberChatting.MVVM.Views
{
    /// <summary>
    /// Логика взаимодействия для ChattingView.xaml
    /// </summary>
    public partial class ChattingView : UserControl
    {
        public ChattingView()
        {
            InitializeComponent();
            ChattingViewModel.MembersListUpdated += UpdateListViews;
            ChattingViewModel.MessagesListUpdated += UpdateMessagesList;
        }

        private void UpdateMessagesList()
        {
            MessagesListView.Items.Refresh();
        }

        private void UpdateListViews()
        {
            MembersListView.Items.Refresh();
            InvitedMembersListView.Items.Refresh();
        }
    }
}

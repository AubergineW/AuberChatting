using AuberChatting.Core;
using AuberChatting.MVVM.Models;
using AuberChatting.Services;
using AuberChattingDomain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Microsoft.MixedReality.WebRTC;

namespace AuberChatting.MVVM.ViewModels
{
    public class StartViewModel : BaseViewModel
    {
        public static event Action OnWindowSwitch;

        private LoginRegisterService _loginRegisterService;
        public StartViewModel(LoginRegisterService loginRegisterService)
        {
            _loginRegisterService = loginRegisterService;

            _loginRegisterService.Connect().ContinueWith(task =>
            {
                if (task.Exception != null)
                    MessageBox.Show("Unable to connect to server!");
            });

            LoginCommand = new RelayCommand(async o =>
            {
                try
                {
                    await _loginRegisterService.LogIn(Nickname, (o as PasswordBox).Password);
                }
                catch (Exception exc)
                {
                    MessageBox.Show(exc.ToString());
                }
            });

            RegistrateCommand = new RelayCommand(async o =>
            {
                try
                {
                    await _loginRegisterService.Registrate(Nickname, (o as PasswordBox).Password);
                }
                catch (Exception exc)
                {
                    MessageBox.Show(exc.ToString());
                }
            });

            _loginRegisterService.Registered += RegisterCallback;
            _loginRegisterService.LoggedIn += LogInCallback;

            
        }

        public RelayCommand LoginCommand { get; set; }
        public RelayCommand RegistrateCommand { get; set; }

        private string _nickname;
        public string Nickname
        {
            get { return _nickname; }
            set
            {
                _nickname = value;
                OnPropertyChanged();
            }
        }

        private void RegisterCallback(UserModel user)
        {
            if (user != null)
            {
                SelfModel.SetSelfModel(user.Nickname);
                OnWindowSwitch?.Invoke();
            }
            else
            {
                MessageBox.Show("Account with this name already exists.");
            }
        }

        private void LogInCallback(UserModel user)
        {
            if (user != null)
            {
                SelfModel.SetSelfModel(user.Nickname);
                OnWindowSwitch?.Invoke();
            }
            else
            {
                MessageBox.Show("Incorrect password or nickname");
            }
        }
    }
}

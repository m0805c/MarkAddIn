

using Newtonsoft.Json.Bson;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Controls;

namespace MarkAddIn
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {





        private string _username = "Usuario";
        public string Username
        {
            get => _username;
            set
            {
                if (_username != value)
                {
                    _username = value;
                    OnPropertyChanged(nameof(Username));
                }
            }
        }

        public void ActualizarUsuario(string nombre)
        {
            Username = nombre ?? "Usuario";
        }



       



        public MainWindowViewModel()
        {
            CurrentView = new HomeView();
        }

        private UserControl _currentView;
        public UserControl CurrentView
        {
            get => _currentView;                               
                                                                
            set
            {
                _currentView = value;
                OnPropertyChanged();
            }
        }




        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

    }
}


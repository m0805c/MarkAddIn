/* MainWindow es la ventana principal que contiene un ContentControl (o similar). Ese ContentControl está enlazado (Binding) a la propiedad CurrentViewModel de tu MainWindowViewModel.*/


using Newtonsoft.Json.Bson;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Controls;

namespace MarkAddIn
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {



        /*metodo para el usuario*/
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







        private UserControl _currentView;
        public UserControl CurrentView
        {
            get => _currentView;
            set
            {
                if (value != _currentView)
                {
                    _currentView = value;
                    OnPropertyChanged();
                }
            }
        }

        public MainWindowViewModel()
        {
            CurrentView = new HomeView();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

    }
}


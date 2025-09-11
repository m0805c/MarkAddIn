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



       



        /*aqui se le asigna valor a current view por primera vez*/
        public MainWindowViewModel()
        {
            CurrentView = new HomeView();
        }

        private UserControl _currentView;
        public UserControl CurrentView
        {
            get => _currentView;                                /*se quito la condicional y se dejó la asignacion del homeview por defecto*/
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


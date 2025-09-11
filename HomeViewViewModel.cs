
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace MarkAddIn
{
    public class HomeViewViewModel : INotifyPropertyChanged


    {
        private string _HomeViewUsername = "HomeViewUsuario";       
        public string HomeViewUsername
        {
            get => _HomeViewUsername;
            set
            {
                if (HomeViewUsername != value)
                {
                    _HomeViewUsername = value;
                    OnPropertyChanged(nameof(HomeViewUsername));
                    
                }
            }
        }



        private object _CurrentHomeViewViewModel;
        public object CurrentHomeViewViewModel
        {
            get => _CurrentHomeViewViewModel;
            set
            {
                if (_CurrentHomeViewViewModel != value)
                {
                    _CurrentHomeViewViewModel = value;
                    OnPropertyChanged(nameof(CurrentHomeViewViewModel));
                }
            }
        }



        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName) => 
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        
    }
}

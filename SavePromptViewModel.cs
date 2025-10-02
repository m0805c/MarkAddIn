using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace MarkAddIn
{
    public class SavePromptViewModel : INotifyPropertyChanged

    {

        private string _username = "Usuario";

        private string Username

        {
            get => _username;
            set
            {
                if(_username != value)
                {
                    _username = value;
                    OnPropertyChanged(nameof(Username));
                }
            }
        }

        private object _currentSavePromptViewModel;




        public object CurrentSavePromptViewModel
        {
            get => _currentSavePromptViewModel;
            set
            {
                if (_currentSavePromptViewModel != value)
                {
                    OnPropertyChanged(nameof(CurrentSavePromptViewModel));
                }
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)=>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}

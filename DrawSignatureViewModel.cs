using Microsoft.Office.Interop.Excel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace MarkAddIn
{

    public class DrawSignatureViewModel : INotifyPropertyChanged

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



        private object _currentDrawSignatureViewModel;

        public object CurrentDrawSignatureViewModel
        {
            get => _currentDrawSignatureViewModel;
            set
            {
                if (_currentDrawSignatureViewModel != value)
                {
                    OnPropertyChanged(nameof(CurrentDrawSignatureViewModel));
                }
            }
        }



        


        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName) =>
             PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}




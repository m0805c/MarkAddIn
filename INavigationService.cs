using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace MarkAddIn
{
    public interface INavigationService
    {
        void NavigateTo<TViewModel>() where TViewModel :  UserControl;
    }

    public class NavigationService : INavigationService
    {
        private readonly MainWindowViewModel _mainWindowViewModel;              
        private readonly IServiceProvider _serviceProvider;             

        public NavigationService(MainWindowViewModel mainWindowViewModel, IServiceProvider serviceProvider)
         
        {
            _mainWindowViewModel = mainWindowViewModel ?? throw new ArgumentNullException(nameof(mainWindowViewModel));
            _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));


        }

        public void NavigateTo<TViewModel>() where TViewModel : UserControl       
        {
            var viewModel = _serviceProvider.GetService(typeof(TViewModel)) as TViewModel; 
            

            if (viewModel != null)

                _mainWindowViewModel.CurrentView = viewModel;
        }                                   
    }


}

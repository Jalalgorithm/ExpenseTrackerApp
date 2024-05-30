using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ExpTracApp.Services;
using MvvmHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpTracApp.ViewModel
{
    [QueryProperty(nameof(NavigId), "id")]
    public partial class ManageExpensesPageViewModel : BaseViewModel
    {
        [ObservableProperty]
        private int navigId;

        private readonly ServiceInterface _service;

        public ManageExpensesPageViewModel(ServiceInterface service)
        {
            _service = service;
            ShowPopup = false;
            SelectedDate = DateTime.Now;
        }

        [RelayCommand]
        private void CheckForQueryStringNavigation()
        {
            if()
        }
    }
}

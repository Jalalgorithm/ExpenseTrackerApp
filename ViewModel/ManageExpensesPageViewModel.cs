using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ExpTracApp.Model;
using ExpTracApp.Services;
using MvvmHelpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpTracApp.ViewModel
{
    [QueryProperty(nameof(NavigationId), "id")]
    public partial class ManageExpensesPageViewModel : ExpensesBaseViewModel
    {
        [ObservableProperty]
        private int navigationId;

        private readonly ServiceInterface _service;

        public ManageExpensesPageViewModel(ServiceInterface service)
        {
            _service = service;
            ShowPopup = false;
            SelectedDate = DateTime.Now;
        }

        [RelayCommand]
        private void CheckForQueryStringNavigate()
        {
            if(NavigationId==1)
            {
                ExpenseModel = new Expenses();
                ShowPopup = true;
                NavigationId = 0;
            }
        }
        [ObservableProperty]
        private Expenses expenseModel;

        [ObservableProperty]
        private bool showPopup;

        [ObservableProperty]
        private DateTime? selectedDate;

        private void OnPickedDateChanged (DateTime? oldValue , DateTime? newValue)
        {
            if (newValue is null)
                SelectedDate = newValue;
        }

        [RelayCommand]
        private void ShowAddExpensePopup()
        {
            ExpenseModel = new Expenses();
            ShowPopup = true;
        }
        [ObservableProperty]
        private string popupMessage;

        [ObservableProperty]
        private string popupMessageTitle;

        [ObservableProperty]
        private bool showMessagePopup;


        [RelayCommand]
        public void CloseMessagePopup()
        {
            ShowPopup = true;
            ShowMessagePopup = false;
        }

        [RelayCommand]
        private async Task SaveExpenses()
        {
            ExpenseModel.DateAdded = SelectedDate.Value.Date;
            if (string.IsNullOrEmpty(ExpenseModel.Name)||ExpenseModel.Amount<=0 ||ExpenseModel.DateAdded is null)
            {
                PopupMessage = $"Sorry , make sure  you have provided all the the information needed.{Environment.NewLine} Name should not be NULL" +
                    $", {Environment.NewLine} Amount shouldnt be NULL,{Environment.NewLine} Date must be greateror equal to today{DateTime.Now.Date}" +
                    $",{Environment.NewLine}Thank You! {Environment.NewLine}";
                PopupMessageTitle = "Alert";
                ShowPopup = false;
                ShowMessagePopup = true;
                return;
            }

            var (flag, message, newData) = await _service.AddorUpdateExpensesAsync(ExpenseModel);
            if (flag)
            {
                if (message.ToLower().Equals("saved"))
                {
                    ExpensesData.Add(newData);
                }
                else
                {
                    var oldData = ExpensesData.FirstOrDefault(d => d.Id == ExpenseModel.Id);
                    ExpensesData.Remove(oldData);
                    ExpensesData.Add(newData);
                }

                ExpenseModel = new();
                ShowPopup = false;
            }
        }

        public ObservableCollection<Expenses> ExpensesData { get; set; } = new();

        [RelayCommand]
        private async Task GetExpenses()
        {
            var result = await _service.GetAllExpensesAsync();

            if (ExpensesData.Count > 0)
                return;

            if (result is null)
            {
                ExpensesData?.Clear();
                return;
            }

            foreach (var expense in result.OrderByDescending(d=>d.DateAdded))
            {
                ExpensesData.Add(expense);
            }


        }

        [ObservableProperty]
        private Expenses selectedRowData;

        partial void OnSelectedRowDataChanged (Expenses oldValue , Expenses newValue)
        {
            if (newValue != null)
                SelectedRowData = newValue;

            ManageSelectedData(SelectedRowData);
        }

        private async Task ManageSelectedData(Expenses SelectedRowData)
        {
            if (SelectedRowData!=null ||SelectedRowData.Id!=0)
            {
                string action = await Shell.Current.DisplayActionSheet("Action: Choose an Option", "Cancel", null, "Edit", "Delete");
                if (string.IsNullOrEmpty(action) ||string.IsNullOrWhiteSpace(action))
                {
                    return;
                }

                if (action.Equals("Cancel")) return;

                if (action.Equals("Edit"))
                {
                    ExpenseModel = new Expenses();
                    ExpenseModel = SelectedRowData;
                    PopupMessageTitle = $"Update {SelectedRowData.Name} Information.";
                    ShowPopup = true;
                }

                if (action.Equals("Delete"))
                {
                    bool answer = await Shell.Current.DisplayAlert("Confirm Operation", $"Are you sure you want to delete{SelectedRowData.Name}?", "Yes", "No");
                    if (!answer)
                        return;

                    var (flag, message) = await _service.DeleteExpensesAsync(SelectedRowData);
                    if (flag)
                    {
                        var getDeleteData = ExpensesData.Where(d => d.Id == SelectedRowData.Id).FirstOrDefault();
                        ExpensesData.Remove(getDeleteData);
                        SelectedRowData = new Expenses();
                    }
                    await Shell.Current.DisplayAlert("Alert", message, "Ok");
                    SelectedRowData = new Expenses();
                }   


            }
        }


    }
}

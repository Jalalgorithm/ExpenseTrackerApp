using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ExpTracApp.Model;
using ExpTracApp.Services;
using ExpTracApp.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpTracApp.ViewModel
{
    public partial class HomePageViewModel : ExpensesBaseViewModel
    {
        private readonly ServiceInterface _services;

        public HomePageViewModel(ServiceInterface services)
        {
            _services = services;
            Title = "Expenses Tracker Dashboard";
            LoadGraphColors();
            ShowPopup = false;
            ThisMonth = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(DateTime.Now.Month); 
        }
        [ObservableProperty]
        private string thisMonth;

        public ObservableCollection<Brush> CustomBrushes { get; set; } = new();

        private void LoadGraphColors()
        {
            CustomBrushes.Add(new SolidColorBrush(Color.FromRgb(77, 288, 225)));
            CustomBrushes.Add(new SolidColorBrush(Color.FromRgb(38, 198, 218)));
            CustomBrushes.Add(new SolidColorBrush(Color.FromRgb(0, 188, 212)));
            CustomBrushes.Add(new SolidColorBrush(Color.FromRgb(0, 172, 192)));
            CustomBrushes.Add(new SolidColorBrush(Color.FromRgb(0, 151, 167)));
            CustomBrushes.Add(new SolidColorBrush(Color.FromRgb(0, 131, 143)));

        }

        public ObservableCollection<Expenses> AllExpensesData { get; set; } = new();
        public ObservableCollection<Expenses> AllYearsData { get; set; } = new();
        public ObservableCollection<Expenses> LastYearData { get; set; } = new();
        public ObservableCollection<Expenses> ThisMonthData { get; set; } = new();
        public ObservableCollection<ExpensesModelForMonth> MonthsInLastYear { get; set; } = new();
        public ObservableCollection<ExpensesModelForYear> YearsData { get; set; } = new();


        [RelayCommand]

        private async Task PopulateData()
        {
            AllExpensesData?.Clear();
            AllYearsData?.Clear();
            LastYearData?.Clear();
            ThisMonthData?.Clear();
            MonthsInLastYear?.Clear();
            YearsData?.Clear();

            var results = await _services.GetAllExpensesAsync();
            if(results.Count!=0)
            {
                foreach(var expense in results)
                {
                    AllExpensesData.Add(expense);
                }
                await PrepareAllExpensesChart();

            }

            return; 
        }

        private async Task PrepareAllExpensesChart()
        {
            var groupedData = AllExpensesData.GroupBy(item => item.DateAdded!.Value.Year);
            foreach (var group in groupedData.OrderBy(e=>e.Key))
            {
                foreach (var item in group)
                {
                    AllYearsData.Add(item);   
                }

                if (group.Key ==DateTime.Now.AddYears(-1).Date.Year)
                {
                    foreach (var item in group)
                    {
                        LastYearData.Add(item);
                    }
                }
            }

            var expenses = AllExpensesData.Where(e=>e.DateAdded!.Value.Year == DateTime.Now.Year&&e.DateAdded!.Value.Month== DateTime.Now.Month).ToList();
            foreach (var item in expenses)
            {
                ThisMonthData.Add(item);
            }

            await GetAvailableFund();
            GetYesterdayExpenses();
            GetLastWeekData();
            GetYesterdayandLastWeekProgressBar();



            var result = LastYearData.GroupBy(d => new { d.DateAdded.Value.Month, d.Name }).Select(g => new { Month = g.Key.Month, Name = g.Key.Name, TotalAmount = g.Sum(d => d.Amount) });
            var groupByMonthNumber = result.GroupBy(d=>new {d.Month}).Select(g=> new { Month = g.Key.Month, Name = g.Key.Month , TotalAmount=g.Sum(d=>d.TotalAmount) });

            List<ExpensesModelForMonth> list = new();
            foreach (var i in groupByMonthNumber.OrderBy(m=>m.Month))
            {
                MonthsInLastYear.Add(new ExpensesModelForMonth() 
                {
                    Month = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(i.Month), TotalAmount = i.TotalAmount, MonthNumber = i.Month 
                });
            }

            YearsData?.Clear();
            var yearResult = AllYearsData.GroupBy(d=> new {d.DateAdded.Value.Year,d.Name}).Select(g => new {Year = g.Key.Year , Name = g.Key.Name,TotalAmount=g.Sum(d=>d.Amount) });
            var groupByYearNumber = yearResult.GroupBy(d => new { d.Year }).Select(g => new { Year = g.Key.Year, TotalAmount = g.Sum(d => d.TotalAmount) });

            List<ExpensesModelForYear> yearlist = new();
            foreach (var yr in groupByYearNumber.OrderBy(e=>e.Year))
            {
                YearsData.Add(new ExpensesModelForYear()
                {
                    Year = yr.Year,
                    TotalAmount = yr.TotalAmount

                });
            }
        }

        

        [ObservableProperty]
        decimal availableFund;
        [ObservableProperty]

        private Fund myFund;
        [ObservableProperty]

        private bool showPopup;

        [RelayCommand]
        private void AddFundPopup()
        {
            myFund = new Fund();
            showPopup = true;
        }
        [RelayCommand]
        private void HideAddFundPopup()
        {
            myFund = new Fund();
            showPopup = false;
        }

        private async Task GetAvailableFund()
        {
            decimal fund  =  await _services.GetAvailableFund();
            if(fund>0)
            {
                AvailableFund = fund - ThisMonthData.Sum(e => e.Amount);
            }
            AvailableFund = fund;
        }

        [RelayCommand]
        private async void AddFund()
        {
            if(MyFund.Amount == 0)
            {
                await Shell.Current.DisplayAlert("Please Enter  Amount ", "Amount must not be 0 ", "Ok");
                ShowPopup = true;
                return;
            }

            int result = await _services.AddFundAsync(MyFund);
            switch(result) 
            {
                case 1:
                    await Shell.Current.DisplayAlert("Fund Added", "Process completed", "Done");
                    HideAddFundPopup();
                    await GetAvailableFund();
                    break;
                case 2:
                    await Shell.Current.DisplayAlert("Fund Addded", "Process Completed", "Done");
                    HideAddFundPopup();
                    await GetAvailableFund();
                    break;
                case 3 or 4:
                    await Shell.Current.DisplayAlert("Amount may be negative ", "Process Failed", "Ok");
                    ShowPopup = true;
                    break;
                case -1:
                    await Shell.Current.DisplayAlert("Unkniwn Error occurred", "Process not sucessful", "Ok");
                    ShowPopup = true;
                    break;
            }
        }

        [ObservableProperty]
        decimal yesterdayExpenses;

        [ObservableProperty]
        decimal lastWeekExpenses;
        [ObservableProperty]
        decimal thisMonthExpenses;

        private void GetYesterdayExpenses()
        {
            YesterdayExpenses = ThisMonthData.Where(e => e.DateAdded!.Value.Date == DateTime.Now.AddDays(-1).Date).ToList().Sum(e => e.Amount);
        }
        private void GetLastWeekData()
        {
            DateTime getLastweekDate = DateTime.Now.AddDays(-7);
            var lastweek = ThisMonthData.OrderByDescending(e=>e.DateAdded!.Value.Date).TakeWhile(e=>e.DateAdded>=getLastweekDate).ToList(); 
            if(lastweek is null)
            {
                LastWeekExpenses = lastweek.Sum(e => e.Amount);
            }
        }

        [ObservableProperty]
        private decimal yesterdayProgress;
        [ObservableProperty]
        private decimal lastweekProgress;

        private void GetYesterdayandLastWeekProgressBar()
        {
            ThisMonthExpenses = ThisMonthData.Sum(e => e.Amount);

            if(LastWeekExpenses>0 &&ThisMonthExpenses>0)
            {
                LastweekProgress = LastWeekExpenses / ThisMonthExpenses * 100;
            }

            if(YesterdayExpenses >0 && ThisMonthExpenses>0)
            {
                YesterdayProgress = LastWeekExpenses / ThisMonthExpenses * 100;
            }
        }

        [RelayCommand]
        private async Task GotoAddExpensePage()
        {
            await Shell.Current.GoToAsync($"{nameof(ManageExpensesPage)}?id=1");
        }


    }
}

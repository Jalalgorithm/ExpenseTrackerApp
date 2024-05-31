using ExpTracApp.ViewModel;

namespace ExpTracApp.View;

public partial class ManageExpensesPage : ContentPage
{
    private readonly ManageExpensesPageViewModel _manageExpensesPageViewModel;

    public ManageExpensesPage(ManageExpensesPageViewModel manageExpensesPageViewModel)
	{
		InitializeComponent();
        BindingContext = manageExpensesPageViewModel;
        _manageExpensesPageViewModel = manageExpensesPageViewModel;
    }

    protected override void OnAppearing()
    {
        _manageExpensesPageViewModel.GetExpensesCommand.Execute(this);
        _manageExpensesPageViewModel.CheckForQueryStringNavigateCommand.Execute(this);
    }
}
using ExpTracApp.ViewModel;

namespace ExpTracApp.View;

public partial class HomePage : ContentPage
{
    private readonly HomePageViewModel _homePageViewModel;

    public HomePage(HomePageViewModel homePageViewModel)
	{
		InitializeComponent();
        _homePageViewModel = homePageViewModel;
        BindingContext = homePageViewModel;

    }
    protected override void OnAppearing()
    {
        _homePageViewModel.PopulateDataCommand.Execute(this);
    }


}
using ExpTracApp.ViewModel;
using System.Diagnostics;

namespace ExpTracApp.View;

public partial class HomePage : ContentPage
{
    private readonly HomePageViewModel _homePageViewModel;

    public HomePage(HomePageViewModel homePageViewModel)
	{
        try
        {
            InitializeComponent();
            _homePageViewModel = homePageViewModel;
            BindingContext = homePageViewModel;

        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Exception during initialization: {ex.Message}");
        }
		

    }
    protected override void OnAppearing()
    {
        try
        {
            _homePageViewModel.PopulateDataCommand.Execute(this);

        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Exception in OnAppearing: {ex.Message}");
        }
    }


}
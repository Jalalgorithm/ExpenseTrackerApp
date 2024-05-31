using ExpTracApp.View;

namespace ExpTracApp
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(ManageExpensesPage), typeof(ManageExpensesPage));
        }
    }
}

using ExpTracApp.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpTracApp.Services
{
    internal interface ServiceInterface
    {
        //Expenses
        Task<(bool flag, string message, Expenses? newData)> AddorUpdateExpensesAsync(Expenses model);
        Task<List<Expenses>> GetAllExpensesAsync();
        Task<(bool flag, string message)> DeleteExpensesAsync(Expenses model);

        //Fund
        Task<int> AddFundAsync(Fund fund);
        Task<decimal> GetAvailableFund();

    }
}


using ExpTracApp.Model;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpTracApp.Services
{
    public class ServiceImplementation : ServiceInterface
    {
        private SQLiteAsyncConnection connection;
        public ServiceImplementation()
        {
            SetupDatabase();
        }

        public async Task<int> AddFundAsync(Fund fund)
        {
            try
            {
                var alreadyFunded = await connection.Table<Fund>().Where(a=>a.Amount>=0).FirstOrDefaultAsync();
                if(alreadyFunded is not null)
                {
                    decimal currentAmount = alreadyFunded.Amount+fund.Amount;
                    if(currentAmount >=0)
                    {
                        await connection.DeleteAsync(alreadyFunded);
                        await connection.InsertAsync(new Fund()
                        {
                            Amount = currentAmount,
                        });

                        return 2; //updated
                    }
                    return 3; //fund cannot be negative
                }
                if (fund.Amount<0)
                {
                    return 4; //fund is negative
                }

                await connection.InsertAsync(fund);
                return 1; //inserted
            }
            catch (Exception)
            {
                return -1; //unknown error
            }
        }

        public async Task<(bool flag, string message, Expenses? newData)> AddorUpdateExpensesAsync(Expenses model)
        {
            if (model is null)
                return (false, "Bad Request", null);

            int result;
            if(model.Id>0)
            {
                var (flag, message, newData) = await UpdateExpensesAsync(model);
                return (flag, message, newData);
            }

            result = await connection.InsertAsync(model);

            if(result >0)
            {
                var data = await connection.Table<Expenses>().ToListAsync();
                var lastdata = data.OrderBy(e => e.Id).Last();
                return(true ,"Saved" , lastdata);
            }

            return (false, "Internal Server error", null);

        }

        public async Task<(bool flag, string message)> DeleteExpensesAsync(Expenses model)
        {
            var result = await connection.DeleteAsync(model);
            if (result>0)
            {
                return (true, "Deleted");
            }

            return (false, "Internal Server Error");
        }

        public async Task<List<Expenses>> GetAllExpensesAsync() => await connection.Table<Expenses>().ToListAsync();

        public async Task<(bool flag , string message , Expenses? newData)> UpdateExpensesAsync(Expenses model)
        {
            var expense = await connection.UpdateAsync(model);
            if (expense > 0)
                return (true, "updated", model);

            return (false, "Internat Server Error", null); 
        }

        public async Task<decimal> GetAvailableFund()
        {
            var allfunds = await connection.Table<Fund>().ToArrayAsync();

            return allfunds.Select(a=>a.Amount).Sum();  
        }

        private async void SetupDatabase()
        {
            if(connection is null)
            {
                string dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "ApplicationDb.db3");
                connection = new SQLiteAsyncConnection(dbPath);
                await connection.CreateTableAsync<Expenses>();
                await connection.CreateTableAsync<Fund>();
            }
        }
    }
}

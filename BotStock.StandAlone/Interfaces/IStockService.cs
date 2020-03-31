using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BotStock.StandAlone.Interfaces
{
    public interface IStockService
    {
        Task<string> GetStockQuote(string stock_code);
    }
}

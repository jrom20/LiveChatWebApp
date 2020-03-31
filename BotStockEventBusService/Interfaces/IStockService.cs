using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BotStockEventBusService.Interfaces
{
    public interface IStockService
    {
        Task<string> GetStockQuote(string stock_code);
    }
}

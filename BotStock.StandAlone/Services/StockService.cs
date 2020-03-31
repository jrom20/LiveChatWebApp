using BotStock.StandAlone.Interfaces;
using BotStock.StandAlone.Services.Request;
using CsvHelper;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace BotStock.StandAlone.Services
{
    public class StockService : IStockService
    {
        private readonly IConfiguration _config;
        public StockService(IConfiguration configuration)
        {
            _config = configuration;
        }

        public async Task<string> GetStockQuote(string input)
        {
            try
            {
                var stockCode = await GetStockCode(input);
                var stockQuote = string.Empty;
                using (var client = new HttpClient())
                {
                    string stockUrl = string.Format(_config["StockServiceUrl"], stockCode);

                    using (var stream = await DownloadAsync(stockUrl))
                    {
                        TextReader reader = new StreamReader(stream);
                        using (var csvFile = new CsvReader(reader, CultureInfo.InvariantCulture))
                        {
                            var stockDescription = csvFile.GetRecords<StockDto>()?.FirstOrDefault();
                            if (stockDescription != null)
                                if (await IsValidResult(stockDescription.Close))
                                    return $"{stockDescription.Symbol} quote is ${stockDescription.Close} per share";

                            stockQuote = $"Unable to find a stock code for '{stockCode}'";
                        }
                    }
                    return stockQuote;
                }
            }catch(Exception ex)
            {
                return null;
            }
        }

        private async Task<bool> IsValidResult(string result)
        {
            string[] invalidValues = { "n/d" };
            if(invalidValues.Contains(result.ToLower().Trim()))
                return false;

            return true;
        }
        private async Task<string> GetStockCode(string input)
        {
            var parsedStockCode = input.ToLower().Replace("/stock=", string.Empty).Trim();
            return await Task.FromResult(parsedStockCode);
        }

        private async Task<Stream> DownloadAsync(string url)
        {
            using (var client = new HttpClient())
            using (var networkStream = await client.GetStreamAsync(url))
            {
                Stream buffer = new MemoryStream();
                try
                {
                    await networkStream.CopyToAsync(buffer, 4096);
                    buffer.Seek(0, SeekOrigin.Begin);

                    return buffer;
                }
                catch
                {
                    buffer.Dispose();
                    throw;
                }
            }
        }

    }
}

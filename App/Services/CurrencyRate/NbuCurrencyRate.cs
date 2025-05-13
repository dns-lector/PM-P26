using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace App.Services.CurrencyRate
{
    public class NbuCurrencyRate : ICurrencyRate
    {
        public async Task<List<CurrencyRate>> GetCurrencyRatesAsync()
        {
            using HttpClient client = new();
            String json = await client.GetStringAsync("https://bank.gov.ua/NBUStatService/v1/statdirectory/exchange?json");
            var nbuRates = JsonSerializer.Deserialize<List<NbuRate>>(json)!; 
            return nbuRates
                .Select(nbuRate => new CurrencyRate
                {
                    FullName  = nbuRate.txt,
                    ShortName = nbuRate.cc,
                    RateBuy   = nbuRate.rate,
                    RateSale  = nbuRate.rate,
                    Date      = DateOnly.Parse(nbuRate.exchangedate)
                })
                .ToList();
        }

        /* ORM для відповіді АРІ НБУ 
         * {
             "r030": 12,
             "txt": "Алжирський динар",
             "rate": 0.31083,
             "cc": "DZD",
             "exchangedate": "14.05.2025"
           },
         */
        private class NbuRate
        {
            public int    r030 { get; set; }
            public String txt  { get; set; }
            public double rate { get; set; }
            public String cc   { get; set; }
            public String exchangedate { get; set; }
        }
    }
}

using App.Services.CurrencyRate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTests.Services.CurrencyRate
{
    [TestClass]
    public sealed class MonoCurrencyRateTest
    {
        [TestMethod]
        public void ConstructorTest()
        {
            var currencyRate = new MonoCurrencyRate();
            // currencyRate = null!;
            Assert.IsNotNull(currencyRate, "Default constructor MUST create object");            
        }

        [TestMethod]
        public void InterfaceTest()
        {
            var currencyRate = new MonoCurrencyRate();
            Assert.IsInstanceOfType<ICurrencyRate>(
                currencyRate,
                "MonoCurrencyRate object MUST inherit ICurrencyRate");

            Assert.IsNotInstanceOfType<App.Services.CurrencyRate.CurrencyRate>(
                currencyRate,
                "MonoCurrencyRate object IS NOT inherit CurrencyRate");
        }

        [TestMethod]
        public void GetCurrencyRatesAsyncTest()
        {
            var currencyRate = new MonoCurrencyRate();
            var task = currencyRate.GetCurrencyRatesAsync();
            Assert.IsNotNull(task,
                "GetCurrencyRatesAsync MUST return non-null result");
            Assert.IsInstanceOfType<Task>(task,
                "GetCurrencyRatesAsync MUST return async Task");
            var rates = task.Result;
            // throw new Exception("Ex");
            // rates = null;
            Assert.IsNotNull(rates,
                "GetCurrencyRatesAsync Result MUST be non-null");
            Assert.IsInstanceOfType<List<App.Services.CurrencyRate.CurrencyRate>>(
                rates,
                "GetCurrencyRatesAsync Result MUST be List<CurrencyRate>");
            Assert.IsTrue(rates.Count > 0,
                "GetCurrencyRatesAsync Result MUST NOT be empty");
        }
    }
}
/* Д.З. Закласти тестовий проєкт (модульні тести)
 * Скласти тести (твердження) для двох обраних класів.
 */

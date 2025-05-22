// See https://aka.ms/new-console-template for more information
// Console.WriteLine(File.ReadAllText("appsettings.json"));

using App.Services.CurrencyRate;

// DIP - впровадежння залежності від абстракції (інтерфейсу)
ICurrencyRate currencyRate = new NbuCurrencyRate();

// --DIP-- не рекомендується - залежність від реалізації (класу)
// NbuCurrencyRate currencyRate = new NbuCurrencyRate();

// Погана асинхронність - один за одним, а не паралельно
// var rates = await currencyRate.GetCurrencyRatesAsync();
// PrintRates(rates);
// Console.WriteLine("-------------------------------");
// currencyRate = new MonoCurrencyRate();
// rates = await currencyRate.GetCurrencyRatesAsync();
// PrintRates(rates);


// Покращена асинхронність - запуск двох задач паралельно
var task1 = new NbuCurrencyRate().GetCurrencyRatesAsync();
var task2 = new MonoCurrencyRate().GetCurrencyRatesAsync();

var rates1 = await task1;
PrintRates(rates1);
Console.WriteLine("-------------------------------");

var rates2 = await task2;
PrintRates(rates2);


void PrintRates(List<CurrencyRate> rates)
{
foreach (var rate in rates)
{
    Console.WriteLine($"{rate.ShortName} {rate.RateBuy} / {rate.RateSale}");
}
}

/* Впровадити у курсові проєкти елементи роботи
 * з мережею та асинхронне виконання коду.
 */


/* Збереження налаштувань, зокрема паролів
 * - налаштування мають бути в окремому файлі з можливістю
 *    їх зміни без перекомпіляції проєкту
 * - налаштування поділяються на приватні та загальні
 *    інколи їх розділяють на різні файли
 * - приватні налаштування вилучають з репозиторію
 *    (.gitignore) додаючи файл з шаблонними полями
 * - додаємо інструкцію до README файла репозиторію
 */

/* Д.З. Створити інструмент керування проєктом (на базі GitHub Projects)
 * Додати картки-задачі, налаштувати терміни (Milestones),
 * описати підзадачі у картках.
 * Задачі - реальні на курсвий проєкт
 */

// Comment from VS

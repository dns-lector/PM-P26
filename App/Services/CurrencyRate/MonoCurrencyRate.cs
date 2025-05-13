using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace App.Services.CurrencyRate
{
    public class MonoCurrencyRate : ICurrencyRate
    {
        public async Task<List<CurrencyRate>> GetCurrencyRatesAsync()
        {
            using HttpClient client = new();
            String json = await client.GetStringAsync("https://api.monobank.ua/bank/currency");
            var monoRates = JsonSerializer.Deserialize<List<MonoRate>>(json)!;
            return monoRates
                .Select(monoRate => new CurrencyRate
                {
                    FullName  = iso4217[monoRate.currencyCodeA].FullName,
                    ShortName = iso4217[monoRate.currencyCodeA].ShortName,
                    RateBuy   = monoRate.rateBuy ?? monoRate.rateCross ?? 0.0,
                    RateSale  = monoRate.rateSell ?? monoRate.rateCross ?? 0.0,
                    Date = DateOnly.FromDateTime( DateTime.FromBinary(monoRate.date) )
                })
                .ToList();
        }


        /*{
            "currencyCodeA": 840,
            "currencyCodeB": 980,
            "date": 1747083673,
            "rateBuy": 41.33,
            "rateSell": 41.8305
          },
         */
        private class MonoRate
        {
            public int     currencyCodeA { get; set; }
            public int     currencyCodeB { get; set; }
            public long    date          { get; set; }
            public double? rateBuy       { get; set; }
            public double? rateSell      { get; set; }
            public double? rateCross     { get; set; }
        }

        private class CurrencyInfo
        {
            public string ShortName { get; set; }
            public string FullName { get; set; }

            public CurrencyInfo(string shortName, string fullName)
            {
                ShortName = shortName;
                FullName = fullName;
            }
        }

        private Dictionary<int, CurrencyInfo> iso4217 = new()
        {
            { 784,  new CurrencyInfo("AED", "United Arab Emirates dirham")},
            { 971,  new CurrencyInfo("AFN", "Afghan afghani")},
            { 8,    new CurrencyInfo("ALL",    "Albanian lek")},
            { 51,   new CurrencyInfo("AMD",    "Armenian dram")},
            { 973,  new CurrencyInfo("AOA", "Angolan kwanza")},
            { 32,   new CurrencyInfo("ARS",    "Argentine peso")},
            { 36,   new CurrencyInfo("AUD",    "Australian dollar")},
            { 533,  new CurrencyInfo("AWG", "Aruban florin")},
            { 944,  new CurrencyInfo("AZN", "Azerbaijani manat")},
            { 977,  new CurrencyInfo("BAM", "Bosnia and Herzegovina convertible mark")},
            { 52,   new CurrencyInfo("BBD",    "Barbados dollar")},
            { 50,   new CurrencyInfo("BDT",    "Bangladeshi taka")},
            { 975,  new CurrencyInfo("BGN", "Bulgarian lev")},
            { 48,   new CurrencyInfo("BHD",    "Bahraini dinar")},
            { 108,  new CurrencyInfo("BIF", "Burundian franc")},
            { 60,   new CurrencyInfo("BMD",    "Bermudian dollar")},
            { 96,   new CurrencyInfo("BND",    "Brunei dollar")},
            { 68,   new CurrencyInfo("BOB",    "Boliviano")},
            { 984,  new CurrencyInfo("BOV", "Bolivian Mvdol (funds code)")},
            { 986,  new CurrencyInfo("BRL", "Brazilian real")},
            { 44,   new CurrencyInfo("BSD",    "Bahamian dollar")},
            { 64,   new CurrencyInfo("BTN",    "Bhutanese ngultrum")},
            { 72,   new CurrencyInfo("BWP",    "Botswana pula")},
            { 933,  new CurrencyInfo("BYN", "Belarusian ruble")},
            { 84,   new CurrencyInfo("BZD",    "Belize dollar")},
            { 124,  new CurrencyInfo("CAD", "Canadian dollar")},
            { 976,  new CurrencyInfo("CDF", "Congolese franc")},
            { 947,  new CurrencyInfo("CHE", "WIR euro (complementary currency)")},
            { 756,  new CurrencyInfo("CHF", "Swiss franc")},
            { 948,  new CurrencyInfo("CHW", "WIR franc (complementary currency)")},
            { 990,  new CurrencyInfo("CLF", "Unidad de Fomento (funds code)")},
            { 152,  new CurrencyInfo("CLP", "Chilean peso")},
            { 156,  new CurrencyInfo("CNY", "Renminbi[6]")},
            { 170,  new CurrencyInfo("COP", "Colombian peso")},
            { 970,  new CurrencyInfo("COU", "Unidad de Valor Real (UVR) (funds code)[7]")},
            { 188,  new CurrencyInfo("CRC", "Costa Rican colon")},
            { 192,  new CurrencyInfo("CUP", "Cuban peso")},
            { 132,  new CurrencyInfo("CVE", "Cape Verdean escudo")},
            { 203,  new CurrencyInfo("CZK", "Czech koruna")},
            { 262,  new CurrencyInfo("DJF", "Djiboutian franc")},
            { 208,  new CurrencyInfo("DKK", "Danish krone")},
            { 214,  new CurrencyInfo("DOP", "Dominican peso")},
            { 12,   new CurrencyInfo("DZD",    "Algerian dinar")},
            { 818,  new CurrencyInfo("EGP", "Egyptian pound")},
            { 232,  new CurrencyInfo("ERN", "Eritrean nakfa")},
            { 230,  new CurrencyInfo("ETB", "Ethiopian birr")},
            { 978,  new CurrencyInfo("EUR", "Euro")},
            { 242,  new CurrencyInfo("FJD", "Fiji dollar")},
            { 238,  new CurrencyInfo("FKP", "Falkland Islands pound")},
            { 826,  new CurrencyInfo("GBP", "Pound sterling")},
            { 981,  new CurrencyInfo("GEL", "Georgian lari")},
            { 936,  new CurrencyInfo("GHS", "Ghanaian cedi")},
            { 292,  new CurrencyInfo("GIP", "Gibraltar pound")},
            { 270,  new CurrencyInfo("GMD", "Gambian dalasi")},
            { 324,  new CurrencyInfo("GNF", "Guinean franc")},
            { 320,  new CurrencyInfo("GTQ", "Guatemalan quetzal")},
            { 328,  new CurrencyInfo("GYD", "Guyanese dollar")},
            { 344,  new CurrencyInfo("HKD", "Hong Kong dollar")},
            { 340,  new CurrencyInfo("HNL", "Honduran lempira")},
            { 332,  new CurrencyInfo("HTG", "Haitian gourde")},
            { 348,  new CurrencyInfo("HUF", "Hungarian forint")},
            { 360,  new CurrencyInfo("IDR", "Indonesian rupiah")},
            { 376,  new CurrencyInfo("ILS", "Israeli new shekel")},
            { 356,  new CurrencyInfo("INR", "Indian rupee")},
            { 368,  new CurrencyInfo("IQD", "Iraqi dinar")},
            { 364,  new CurrencyInfo("IRR", "Iranian rial")},
            { 352,  new CurrencyInfo("ISK", "Icelandic króna (plural: krónur)")},
            { 388,  new CurrencyInfo("JMD", "Jamaican dollar")},
            { 400,  new CurrencyInfo("JOD", "Jordanian dinar")},
            { 392,  new CurrencyInfo("JPY", "Japanese yen")},
            { 404,  new CurrencyInfo("KES", "Kenyan shilling")},
            { 417,  new CurrencyInfo("KGS", "Kyrgyzstani som")},
            { 116,  new CurrencyInfo("KHR", "Cambodian riel")},
            { 174,  new CurrencyInfo("KMF", "Comoro franc")},
            { 408,  new CurrencyInfo("KPW", "North Korean won")},
            { 410,  new CurrencyInfo("KRW", "South Korean won")},
            { 414,  new CurrencyInfo("KWD", "Kuwaiti dinar")},
            { 136,  new CurrencyInfo("KYD", "Cayman Islands dollar")},
            { 398,  new CurrencyInfo("KZT", "Kazakhstani tenge")},
            { 418,  new CurrencyInfo("LAK", "Lao kip")},
            { 422,  new CurrencyInfo("LBP", "Lebanese pound")},
            { 144,  new CurrencyInfo("LKR", "Sri Lankan rupee")},
            { 430,  new CurrencyInfo("LRD", "Liberian dollar")},
            { 426,  new CurrencyInfo("LSL", "Lesotho loti")},
            { 434,  new CurrencyInfo("LYD", "Libyan dinar")},
            { 504,  new CurrencyInfo("MAD", "Moroccan dirham")},
            { 498,  new CurrencyInfo("MDL", "Moldovan leu")},
            { 969,  new CurrencyInfo("MGA", "Malagasy ariary")},
            { 807,  new CurrencyInfo("MKD", "Macedonian denar")},
            { 104,  new CurrencyInfo("MMK", "Myanmar kyat")},
            { 496,  new CurrencyInfo("MNT", "Mongolian tögrög")},
            { 446,  new CurrencyInfo("MOP", "Macanese pataca")},
            { 929,  new CurrencyInfo("MRU", "Mauritanian ouguiya")},
            { 480,  new CurrencyInfo("MUR", "Mauritian rupee")},
            { 462,  new CurrencyInfo("MVR", "Maldivian rufiyaa")},
            { 454,  new CurrencyInfo("MWK", "Malawian kwacha")},
            { 484,  new CurrencyInfo("MXN", "Mexican peso")},
            { 979,  new CurrencyInfo("MXV", "Mexican Unidad de Inversion (UDI) (funds code)")},
            { 458,  new CurrencyInfo("MYR", "Malaysian ringgit")},
            { 943,  new CurrencyInfo("MZN", "Mozambican metical")},
            { 516,  new CurrencyInfo("NAD", "Namibian dollar")},
            { 566,  new CurrencyInfo("NGN", "Nigerian naira")},
            { 558,  new CurrencyInfo("NIO", "Nicaraguan córdoba")},
            { 578,  new CurrencyInfo("NOK", "Norwegian krone")},
            { 524,  new CurrencyInfo("NPR", "Nepalese rupee")},
            { 554,  new CurrencyInfo("NZD", "New Zealand dollar")},
            { 512,  new CurrencyInfo("OMR", "Omani rial")},
            { 590,  new CurrencyInfo("PAB", "Panamanian balboa")},
            { 604,  new CurrencyInfo("PEN", "Peruvian sol")},
            { 598,  new CurrencyInfo("PGK", "Papua New Guinean kina")},
            { 608,  new CurrencyInfo("PHP", "Philippine peso[11]")},
            { 586,  new CurrencyInfo("PKR", "Pakistani rupee")},
            { 985,  new CurrencyInfo("PLN", "Polish złoty")},
            { 600,  new CurrencyInfo("PYG", "Paraguayan guaraní")},
            { 634,  new CurrencyInfo("QAR", "Qatari riyal")},
            { 946,  new CurrencyInfo("RON", "Romanian leu")},
            { 941,  new CurrencyInfo("RSD", "Serbian dinar")},
            { 643,  new CurrencyInfo("RUB", "Russian ruble")},
            { 646,  new CurrencyInfo("RWF", "Rwandan franc")},
            { 682,  new CurrencyInfo("SAR", "Saudi riyal")},
            { 90,   new CurrencyInfo("SBD",    "Solomon Islands dollar")},
            { 690,  new CurrencyInfo("SCR", "Seychelles rupee")},
            { 938,  new CurrencyInfo("SDG", "Sudanese pound")},
            { 752,  new CurrencyInfo("SEK", "Swedish krona (plural: kronor)")},
            { 702,  new CurrencyInfo("SGD", "Singapore dollar")},
            { 654,  new CurrencyInfo("SHP", "Saint Helena pound")},
            { 925,  new CurrencyInfo("SLE", "Sierra Leonean leone (new leone)[12][13][14]")},
            { 706,  new CurrencyInfo("SOS", "Somalian shilling")},
            { 968,  new CurrencyInfo("SRD", "Surinamese dollar")},
            { 728,  new CurrencyInfo("SSP", "South Sudanese pound")},
            { 930,  new CurrencyInfo("STN", "São Tomé and Príncipe dobra")},
            { 222,  new CurrencyInfo("SVC", "Salvadoran colón")},
            { 760,  new CurrencyInfo("SYP", "Syrian pound")},
            { 748,  new CurrencyInfo("SZL", "Swazi lilangeni")},
            { 764,  new CurrencyInfo("THB", "Thai baht")},
            { 972,  new CurrencyInfo("TJS", "Tajikistani somoni")},
            { 934,  new CurrencyInfo("TMT", "Turkmenistan manat")},
            { 788,  new CurrencyInfo("TND", "Tunisian dinar")},
            { 776,  new CurrencyInfo("TOP", "Tongan paʻanga")},
            { 949,  new CurrencyInfo("TRY", "Turkish lira")},
            { 780,  new CurrencyInfo("TTD", "Trinidad and Tobago dollar")},
            { 901,  new CurrencyInfo("TWD", "New Taiwan dollar")},
            { 834,  new CurrencyInfo("TZS", "Tanzanian shilling")},
            { 980,  new CurrencyInfo("UAH", "Ukrainian hryvnia")},
            { 800,  new CurrencyInfo("UGX", "Ugandan shilling")},
            { 840,  new CurrencyInfo("USD", "United States dollar")},
            { 997,  new CurrencyInfo("USN", "United States dollar (next day) (funds code)")},
            { 940,  new CurrencyInfo("UYI", "Uruguay Peso en Unidades Indexadas (URUIURUI) (funds code)")},
            { 858,  new CurrencyInfo("UYU", "Uruguayan peso")},
            { 927,  new CurrencyInfo("UYW", "Unidad previsional[16]")},
            { 860,  new CurrencyInfo("UZS", "Uzbekistani sum")},
            { 926,  new CurrencyInfo("VED", "Venezuelan digital bolívar[17]")},
            { 928,  new CurrencyInfo("VES", "Venezuelan sovereign bolívar[11]")},
            { 704,  new CurrencyInfo("VND", "Vietnamese đồng")},
            { 548,  new CurrencyInfo("VUV", "Vanuatu vatu")},
            { 882,  new CurrencyInfo("WST", "Samoan tala")},
            { 950,  new CurrencyInfo("XAF", "CFA franc BEAC")},
            { 961,  new CurrencyInfo("XAG", "Silver (one troy ounce)")},
            { 959,  new CurrencyInfo("XAU", "Gold (one troy ounce)")},
            { 955,  new CurrencyInfo("XBA", "European Composite Unit (EURCO) (bond market unit)")},
            { 956,  new CurrencyInfo("XBB", "European Monetary Unit (E.M.U.-6) (bond market unit)")},
            { 957,  new CurrencyInfo("XBC", "European Unit of Account 9 (E.U.A.-9) (bond market unit)")},
            { 958,  new CurrencyInfo("XBD", "European Unit of Account 17 (E.U.A.-17) (bond market unit)")},
            { 951,  new CurrencyInfo("XCD", "East Caribbean dollar")},
            { 532,  new CurrencyInfo("XCG", "Netherlands Antillean guilder")},
            { 960,  new CurrencyInfo("XDR", "Special drawing rights")},
            { 952,  new CurrencyInfo("XOF", "CFA franc BCEAO")},
            { 964,  new CurrencyInfo("XPD", "Palladium (one troy ounce)")},
            { 953,  new CurrencyInfo("XPF", "CFP franc (franc Pacifique)")},
            { 962,  new CurrencyInfo("XPT", "Platinum (one troy ounce)")},
            { 994,  new CurrencyInfo("XSU", "SUCRE")},
            { 963,  new CurrencyInfo("XTS", "Code reserved for testing")},
            { 965,  new CurrencyInfo("XUA", "ADB Unit of Account")},
            { 999,  new CurrencyInfo("XXX", "No currency")},
            { 886,  new CurrencyInfo("YER", "Yemeni rial")},
            { 710,  new CurrencyInfo("ZAR", "South African rand")},
            { 967,  new CurrencyInfo("ZMW", "Zambian kwacha")},
            { 924,  new CurrencyInfo("ZWG", "Zimbabwe Gold")},
            { 191,  new CurrencyInfo("HRK", "Croatian kuna")},
            { 694,  new CurrencyInfo("SLL", "Sierra Leonean leone")},
        };
    }
}
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using TesteNeonConversaoMoedas.Domain;
using TesteNeonConversaoMoedas.IService;

namespace TesteNeonConversaoMoedas.Service
{

    public class ConversaoMoedas : IConversaoMoedas
    {
        public List<Moeda> RetornaListaDeCotacaoDasMoedas(string apiUrl)
        {
            var lstMoedas = new List<Moeda>();
            var client = new HttpClient();

            var responseReturn = client.GetAsync(apiUrl).Result;

            using (var content = responseReturn.Content)
            {
                var result = JObject.Parse(content.ReadAsStringAsync().Result);

                bool.TryParse(result["success"].ToString(), out bool sucesso);

                if (sucesso) /*Monta o array com a lista de moedas*/
                {
                    var jsonObj = JObject.Parse(result["quotes"].ToString());

                    foreach (var obj in jsonObj)
                    {
                        lstMoedas.Add(new Moeda { Sigla = obj.Key, Valor = (float)obj.Value });
                    }
                }


            }

            return lstMoedas;
        }

        public ValidaDados ValidarDados(ConverterMoeda converterMoeda)
        {

            var validaDados = new ValidaDados();

            var lstErros = new List<string>();

            if (converterMoeda == null)
            {
                lstErros.Add("Nenhum dado informado.");
                return validaDados;
            }

            if (string.IsNullOrEmpty(converterMoeda.MoedaOrigem))
                lstErros.Add("Favor preencher a moeda de origem");

            if (string.IsNullOrEmpty(converterMoeda.MoedaDestino))
                lstErros.Add("Favor preencher a moeda de destino");

            if (converterMoeda.Valor <= 0)
                lstErros.Add("Favor preencher o valor a converter");

            validaDados.Mensagem = lstErros.Count > 0 ? "Ops! Favor verificar os erros" : "";
            validaDados.lstErros = lstErros;

            return validaDados;
        }

        public ResponseConversao RetornaResponseConversao(List<Moeda> lstCotacoes, ConverterMoeda converterMoeda)
        {
            var responseConversao = new ResponseConversao();
            
            try
            {
               
                if (lstCotacoes.Count > 0)
                {
                    float valorRetorno;

                    var moedaOrigem = lstCotacoes.Find(m => m.Sigla == "USD" + converterMoeda.MoedaOrigem.ToUpper());
                    var moedaDestino = lstCotacoes.Find(m => m.Sigla == "USD" + converterMoeda.MoedaDestino.ToUpper());

                    /*Em virtude da API de consulta das cotacoes nao permitir da alteracao da moeda de 'source' efetuo o calculo para obter o resultado*/

                    /*Se a moeda de origem for o USD trabalha com a multiplicacao*/
                    if (moedaOrigem.Sigla == "USDUSD")
                        valorRetorno = converterMoeda.Valor * moedaDestino.Valor;

                    /*Se a moeda de destino for o USD trabalha com a divisao*/
                    else if ((moedaDestino.Sigla == "USDUSD"))
                        valorRetorno = converterMoeda.Valor / moedaOrigem.Valor;

                    /*Se a moeda de origem e destino forem diferentes do dolar faz-se o calculo de conversao ao USD  e depois a multiplicacao pela moeda de destino*/
                    else
                        valorRetorno = (converterMoeda.Valor / moedaOrigem.Valor) * moedaDestino.Valor;

                    responseConversao = new ResponseConversao
                    {
                        Sucesso = valorRetorno > 0,
                        MoedaOrigem = converterMoeda.MoedaOrigem.ToUpper(),
                        MoedaDestino = converterMoeda.MoedaDestino.ToUpper(),
                        ValorInformado = converterMoeda.Valor,
                        ValorConvertido = valorRetorno
                    };

                }
            }
            catch (Exception)
            {
                responseConversao = new ResponseConversao
                {
                    Sucesso = false,
                    MoedaOrigem = converterMoeda.MoedaOrigem.ToUpper(),
                    MoedaDestino = converterMoeda.MoedaDestino.ToUpper(),
                    ValorInformado = converterMoeda.Valor,
                    ValorConvertido = 0
                };
            }


            return responseConversao;
        }

        public List<KeyValuePair<string, string>> ListagemMoedas()
        {
            var lstMoedas = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("AED", "United Arab Emirates Dirham"),
                new KeyValuePair<string, string>("AFN", "Afghan Afghani"),
                new KeyValuePair<string, string>("ALL", "Albanian Lek"),
                new KeyValuePair<string, string>("AMD", "Armenian Dram"),
                new KeyValuePair<string, string>("ANG", "Netherlands Antillean Guilder"),
                new KeyValuePair<string, string>("AOA", "Angolan Kwanza"),
                new KeyValuePair<string, string>("ARS", "Argentine Peso"),
                new KeyValuePair<string, string>("AUD", "Australian Dollar"),
                new KeyValuePair<string, string>("AWG", "Aruban Florin"),
                new KeyValuePair<string, string>("AZN", "Azerbaijani Manat"),
                new KeyValuePair<string, string>("BAM", "Bosnia-Herzegovina Convertible Mark"),
                new KeyValuePair<string, string>("BBD", "Barbadian Dollar"),
                new KeyValuePair<string, string>("BDT", "Bangladeshi Taka"),
                new KeyValuePair<string, string>("BGN", "Bulgarian Lev"),
                new KeyValuePair<string, string>("BHD", "Bahraini Dinar"),
                new KeyValuePair<string, string>("BIF", "Burundian Franc"),
                new KeyValuePair<string, string>("BMD", "Bermudan Dollar"),
                new KeyValuePair<string, string>("BND", "Brunei Dollar"),
                new KeyValuePair<string, string>("BOB", "Bolivian Boliviano"),
                new KeyValuePair<string, string>("BRL", "Brazilian Real"),
                new KeyValuePair<string, string>("BSD", "Bahamian Dollar"),
                new KeyValuePair<string, string>("BTC", "Bitcoin"),
                new KeyValuePair<string, string>("BTN", "Bhutanese Ngultrum"),
                new KeyValuePair<string, string>("BWP", "Botswanan Pula"),
                new KeyValuePair<string, string>("BYN", "Belarusian Ruble"),
                new KeyValuePair<string, string>("BYR", "Belarusian Ruble"),
                new KeyValuePair<string, string>("BZD", "Belize Dollar"),
                new KeyValuePair<string, string>("CAD", "Canadian Dollar"),
                new KeyValuePair<string, string>("CDF", "Congolese Franc"),
                new KeyValuePair<string, string>("CHF", "Swiss Franc"),
                new KeyValuePair<string, string>("CLF", "Chilean Unit of Account (UF)"),
                new KeyValuePair<string, string>("CLP", "Chilean Peso"),
                new KeyValuePair<string, string>("CNY", "Chinese Yuan"),
                new KeyValuePair<string, string>("COP", "Colombian Peso"),
                new KeyValuePair<string, string>("CRC", "Costa Rican Colón"),
                new KeyValuePair<string, string>("CUC", "Cuba Convertible Peso"),
                new KeyValuePair<string, string>("CUP", "Cuban Peso"),
                new KeyValuePair<string, string>("CVE", "Cape Verdean Escudo"),
                new KeyValuePair<string, string>("CZK", "Czech Republic Koruna"),
                new KeyValuePair<string, string>("DJF", "Djiboutian Franc"),
                new KeyValuePair<string, string>("DKK", "Danish Krone"),
                new KeyValuePair<string, string>("DOP", "Dominican Peso"),
                new KeyValuePair<string, string>("DZD", "Algerian Dinar"),
                new KeyValuePair<string, string>("EEK", "Estonian Kroon"),
                new KeyValuePair<string, string>("EGP", "Egyptian Pound"),
                new KeyValuePair<string, string>("ERN", "Eritrean Nakfa"),
                new KeyValuePair<string, string>("ETB", "Ethiopian Birr"),
                new KeyValuePair<string, string>("EUR", "Euro"),
                new KeyValuePair<string, string>("FJD", "Fijian Dollar"),
                new KeyValuePair<string, string>("FKP", "Falkland Islands Pound"),
                new KeyValuePair<string, string>("GBP", "British Pound Sterling"),
                new KeyValuePair<string, string>("GEL", "Georgian Lari"),
                new KeyValuePair<string, string>("GGP", "Guernsey Pound"),
                new KeyValuePair<string, string>("GHS", "Ghanaian Cedi"),
                new KeyValuePair<string, string>("GIP", "Gibraltar Pound"),
                new KeyValuePair<string, string>("GMD", "Gambian Dalasi"),
                new KeyValuePair<string, string>("GNF", "Guinean Franc"),
                new KeyValuePair<string, string>("GTQ", "Guatemalan Quetzal"),
                new KeyValuePair<string, string>("GYD", "Guyanaese Dollar"),
                new KeyValuePair<string, string>("HKD", "Hong Kong Dollar"),
                new KeyValuePair<string, string>("HNL", "Honduran Lempira"),
                new KeyValuePair<string, string>("HRK", "Croatian Kuna"),
                new KeyValuePair<string, string>("HTG", "Haitian Gourde"),
                new KeyValuePair<string, string>("HUF", "Hungarian Forint"),
                new KeyValuePair<string, string>("IDR", "Indonesian Rupiah"),
                new KeyValuePair<string, string>("ILS", "Israeli New Sheqel"),
                new KeyValuePair<string, string>("IMP", "Manx pound"),
                new KeyValuePair<string, string>("INR", "Indian Rupee"),
                new KeyValuePair<string, string>("IQD", "Iraqi Dinar"),
                new KeyValuePair<string, string>("IRR", "Iranian Rial"),
                new KeyValuePair<string, string>("ISK", "Icelandic Króna"),
                new KeyValuePair<string, string>("JEP", "Jersey Pound"),
                new KeyValuePair<string, string>("JMD", "Jamaican Dollar"),
                new KeyValuePair<string, string>("JOD", "Jordanian Dinar"),
                new KeyValuePair<string, string>("JPY", "Japanese Yen"),
                new KeyValuePair<string, string>("KES", "Kenyan Shilling"),
                new KeyValuePair<string, string>("KGS", "Kyrgystani Som"),
                new KeyValuePair<string, string>("KHR", "Cambodian Riel"),
                new KeyValuePair<string, string>("KMF", "Comorian Franc"),
                new KeyValuePair<string, string>("KPW", "North Korean Won"),
                new KeyValuePair<string, string>("KRW", "South Korean Won"),
                new KeyValuePair<string, string>("KWD", "Kuwaiti Dinar"),
                new KeyValuePair<string, string>("KYD", "Cayman Islands Dollar"),
                new KeyValuePair<string, string>("KZT", "Kazakhstani Tenge"),
                new KeyValuePair<string, string>("LAK", "Laotian Kip"),
                new KeyValuePair<string, string>("LBP", "Lebanese Pound"),
                new KeyValuePair<string, string>("LKR", "Sri Lankan Rupee"),
                new KeyValuePair<string, string>("LRD", "Liberian Dollar"),
                new KeyValuePair<string, string>("LSL", "Lesotho Loti"),
                new KeyValuePair<string, string>("LTL", "Lithuanian Litas"),
                new KeyValuePair<string, string>("LVL", "Latvian Lats"),
                new KeyValuePair<string, string>("LYD", "Libyan Dinar"),
                new KeyValuePair<string, string>("MAD", "Moroccan Dirham"),
                new KeyValuePair<string, string>("MDL", "Moldovan Leu"),
                new KeyValuePair<string, string>("MGA", "Malagasy Ariary"),
                new KeyValuePair<string, string>("MKD", "Macedonian Denar"),
                new KeyValuePair<string, string>("MMEEK", "Myanma Kyat"),
                new KeyValuePair<string, string>("MNT", "Mongolian Tugrik"),
                new KeyValuePair<string, string>("MOP", "Macanese Pataca"),
                new KeyValuePair<string, string>("MRO", "Mauritanian Ouguiya"),
                new KeyValuePair<string, string>("MUR", "Mauritian Rupee"),
                new KeyValuePair<string, string>("MVR", "Maldivian Rufiyaa"),
                new KeyValuePair<string, string>("MWK", "Malawian Kwacha"),
                new KeyValuePair<string, string>("MXN", "Mexican Peso"),
                new KeyValuePair<string, string>("MYR", "Malaysian Ringgit"),
                new KeyValuePair<string, string>("MZN", "Mozambican Metical"),
                new KeyValuePair<string, string>("NAD", "Namibian Dollar"),
                new KeyValuePair<string, string>("NGN", "Nigerian Naira"),
                new KeyValuePair<string, string>("NIO", "Nicaraguan Córdoba"),
                new KeyValuePair<string, string>("NOK", "Norwegian Krone"),
                new KeyValuePair<string, string>("NPR", "Nepalese Rupee"),
                new KeyValuePair<string, string>("NZD", "New Zealand Dollar"),
                new KeyValuePair<string, string>("OMR", "Omani Rial"),
                new KeyValuePair<string, string>("PAB", "Panamanian Balboa"),
                new KeyValuePair<string, string>("PEN", "Peruvian Nuevo Sol"),
                new KeyValuePair<string, string>("PGK", "Papua New Guinean Kina"),
                new KeyValuePair<string, string>("PHP", "Philippine Peso"),
                new KeyValuePair<string, string>("PKR", "Pakistani Rupee"),
                new KeyValuePair<string, string>("PLN", "Polish Zloty"),
                new KeyValuePair<string, string>("PYG", "Paraguayan Guarani"),
                new KeyValuePair<string, string>("QAR", "Qatari Rial"),
                new KeyValuePair<string, string>("RON", "Romanian Leu"),
                new KeyValuePair<string, string>("RSD", "Serbian Dinar"),
                new KeyValuePair<string, string>("RUB", "Russian Ruble"),
                new KeyValuePair<string, string>("RWF", "Rwandan Franc"),
                new KeyValuePair<string, string>("SAR", "Saudi Riyal"),
                new KeyValuePair<string, string>("SBD", "Solomon Islands Dollar"),
                new KeyValuePair<string, string>("SCR", "Seychellois Rupee"),
                new KeyValuePair<string, string>("SDG", "Sudanese Pound"),
                new KeyValuePair<string, string>("SEK", "Swedish Krona"),
                new KeyValuePair<string, string>("SGD", "Singapore Dollar"),
                new KeyValuePair<string, string>("SHP", "Saint Helena Pound"),
                new KeyValuePair<string, string>("SLL", "Sierra Leonean Leone"),
                new KeyValuePair<string, string>("SOS", "Somali Shilling"),
                new KeyValuePair<string, string>("SRD", "Surinamese Dollar"),
                new KeyValuePair<string, string>("STD", "São Tomé and Príncipe Dobra"),
                new KeyValuePair<string, string>("SVC", "Salvadoran Colón"),
                new KeyValuePair<string, string>("SYP", "Syrian Pound"),
                new KeyValuePair<string, string>("SZL", "Swazi Lilangeni"),
                new KeyValuePair<string, string>("THB", "Thai Baht"),
                new KeyValuePair<string, string>("TJS", "Tajikistani Somoni"),
                new KeyValuePair<string, string>("TMT", "Turkmenistani Manat"),
                new KeyValuePair<string, string>("TND", "Tunisian Dinar"),
                new KeyValuePair<string, string>("TOP", "Tongan Pa?anga"),
                new KeyValuePair<string, string>("TRY", "Turkish Lira"),
                new KeyValuePair<string, string>("TTD", "Trinidad and Tobago Dollar"),
                new KeyValuePair<string, string>("TWD", "New Taiwan Dollar"),
                new KeyValuePair<string, string>("TZS", "Tanzanian Shilling"),
                new KeyValuePair<string, string>("UAH", "Ukrainian Hryvnia"),
                new KeyValuePair<string, string>("UGX", "Ugandan Shilling"),
                new KeyValuePair<string, string>("USD", "United States Dollar"),
                new KeyValuePair<string, string>("UYU", "Uruguayan Peso"),
                new KeyValuePair<string, string>("UZS", "Uzbekistan Som"),
                new KeyValuePair<string, string>("VEF", "Venezuelan Bolívar"),
                new KeyValuePair<string, string>("VND", "Vietnamese Dong"),
                new KeyValuePair<string, string>("VUV", "Vanuatu Vatu"),
                new KeyValuePair<string, string>("WST", "Samoan Tala"),
                new KeyValuePair<string, string>("XAF", "CFA Franc BEAC"),
                new KeyValuePair<string, string>("XAG", "Silver (troy ounce)"),
                new KeyValuePair<string, string>("XAU", "Gold (troy ounce)"),
                new KeyValuePair<string, string>("XCD", "East Caribbean Dollar"),
                new KeyValuePair<string, string>("XDR", "Special Drawing Rights"),
                new KeyValuePair<string, string>("XOF", "CFA Franc BCEAO"),
                new KeyValuePair<string, string>("XPF", "CFP Franc"),
                new KeyValuePair<string, string>("YER", "Yemeni Rial"),
                new KeyValuePair<string, string>("ZAR", "South African Rand"),
                new KeyValuePair<string, string>("ZMK", "Zambian Kwacha (pre-2013)"),
                new KeyValuePair<string, string>("ZMW", "Zambian Kwacha"),
                new KeyValuePair<string, string>("ZWL", "Zimbabwean Dollar")
            };



            return lstMoedas.OrderBy(o => o.Value).ToList();
        }
    }
}

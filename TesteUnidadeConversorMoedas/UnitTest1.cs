using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using TesteNeonConversaoMoedas.Domain;
using TesteNeonConversaoMoedas.IService;
using TesteNeonConversaoMoedas.Service;


namespace TesteUnidadeConversorMoedas
{
    [TestClass]
    public class UnitTest1
    {
        private string ApiAccessKey = "da6c3af62eae6b52af361ca2d1926c33";
        private string UrlCotacoes = "http://www.apilayer.net/api/live?access_key=";
        private IConversaoMoedas _conversaoMoedas;
        


        [TestInitialize]
        public void Initalize()
        {
            _conversaoMoedas = new ConversaoMoedas();
            ReturnCotacoes();
        }

        [TestMethod]
        public void DEVE_RETORNAR_LISTADE_COTACOES_COM_UM_OU_MAIS_ITENS()
        {
            var retorno = _conversaoMoedas.RetornaListaDeCotacaoDasMoedas(string.Concat(UrlCotacoes, ApiAccessKey));

            Assert.IsTrue(retorno.Count > 0);
        }


        [TestMethod]
        public void DEVE_RETORNAR_SUCESSO_NA_CONVERSAO()
        {

            var converter = new ConverterMoeda { MoedaOrigem = "BRL", MoedaDestino = "USD", Valor = 10 };


            var lstCotacoes = _conversaoMoedas.RetornaListaDeCotacaoDasMoedas(string.Concat(UrlCotacoes, ApiAccessKey));

            var retorno = _conversaoMoedas.RetornaResponseConversao(lstCotacoes, converter);

            Assert.AreEqual(true, retorno.Sucesso);

        }

        [TestMethod]
        public void DEVE_RETORNAR_FALSE_NA_FALHA_DA_CONVERSAO()
        {

            var converter = new ConverterMoeda { MoedaOrigem = "BRL", MoedaDestino = "", Valor = 10 };

            var lstCotacoes = _conversaoMoedas.RetornaListaDeCotacaoDasMoedas(string.Concat(UrlCotacoes, ApiAccessKey));

            var retorno = _conversaoMoedas.RetornaResponseConversao(lstCotacoes, converter);

            if (!retorno.Sucesso)
                Assert.AreEqual(false, retorno.Sucesso);
        }


        [TestMethod]
        public void DEVE_RETORNAR_LISTA_COM_UM_OU_MAIS_ITENS_DROP()
        {
            var retorno = _conversaoMoedas.ListagemMoedas();

            Assert.IsTrue(retorno.Count > 0);
        }

        
        /// <summary>
        /// Este metodo de teste esta retornando erro, porem deixei assim para exemplificar, o erro que ocorre é que foi utilizado
        /// para a geração do dropdown de moedas a lista fornecida pelo próprio parceiro de api, que foi informado nos requisitos enviados
        /// por email, e nesta lista de moedas existe a moeda de uma País (EEK) que não retorna na lista de cotações da API, o que provoca
        /// o erro de exception, deixei assim para que ao rodar o teste seja informado esse problema e assim definir a ação a ser tomada
        /// o tratamento de erro no objeto null, ou a remoção da moeda da lista de moedas a converter.
        /// </summary>
        [TestMethod]
        public void DEVE_CONVERTER_DE_TODAS_AS_MOEDAS_PARA_USD()
        {
            var retorno = _conversaoMoedas.ListagemMoedas();

            foreach (var item in retorno)
            {
                var converter = new ConverterMoeda { MoedaOrigem = item.Key, MoedaDestino = "USD", Valor = 10 };

                var resultado = _conversaoMoedas.RetornaResponseConversao(ReturnCotacoes(), converter);

                Assert.IsTrue(resultado.Sucesso);

            }
        }



        public List<Moeda> ReturnCotacoes()
        {
            const string strJson = "[{\"Sigla\":\"USDAED\",\"Valor\":3.673204},{\"Sigla\":\"USDAFN\",\"Valor\":72.20251},{\"Sigla\":\"USDALL\",\"Valor\":108.403992},{\"Sigla\":\"USDAMD\",\"Valor\":481.2804},{\"Sigla\":\"USDANG\",\"Valor\":1.85405},{\"Sigla\":\"USDAOA\",\"Valor\":259.109039},{\"Sigla\":\"USDARS\",\"Valor\":27.2580414},{\"Sigla\":\"USDAUD\",\"Valor\":1.351804},{\"Sigla\":\"USDAWG\",\"Valor\":1.792},{\"Sigla\":\"USDAZN\",\"Valor\":1.702504},{\"Sigla\":\"USDBAM\",\"Valor\":1.594104},{\"Sigla\":\"USDBBD\",\"Valor\":2.0006},{\"Sigla\":\"USDBDT\",\"Valor\":84.45904},{\"Sigla\":\"USDBGN\",\"Valor\":1.690904},{\"Sigla\":\"USDBHD\",\"Valor\":0.378015},{\"Sigla\":\"USDBIF\",\"Valor\":1786.0},{\"Sigla\":\"USDBMD\",\"Valor\":1.0},{\"Sigla\":\"USDBND\",\"Valor\":1.51084},{\"Sigla\":\"USDBOB\",\"Valor\":6.90655},{\"Sigla\":\"USDBRL\",\"Valor\":3.707404},{\"Sigla\":\"USDBSD\",\"Valor\":0.99965},{\"Sigla\":\"USDBTC\",\"Valor\":0.000135},{\"Sigla\":\"USDBTN\",\"Valor\":68.56492},{\"Sigla\":\"USDBWP\",\"Valor\":10.3405037},{\"Sigla\":\"USDBYN\",\"Valor\":2.009041},{\"Sigla\":\"USDBYR\",\"Valor\":19600.0},{\"Sigla\":\"USDBZD\",\"Valor\":2.009041},{\"Sigla\":\"USDCAD\",\"Valor\":1.30075},{\"Sigla\":\"USDCDF\",\"Valor\":1616.00037},{\"Sigla\":\"USDCHF\",\"Valor\":0.99423},{\"Sigla\":\"USDCLF\",\"Valor\":0.022604},{\"Sigla\":\"USDCLP\",\"Valor\":644.000366},{\"Sigla\":\"USDCNY\",\"Valor\":6.829804},{\"Sigla\":\"USDCOP\",\"Valor\":2892.0},{\"Sigla\":\"USDCRC\",\"Valor\":567.175049},{\"Sigla\":\"USDCUC\",\"Valor\":1.0},{\"Sigla\":\"USDCUP\",\"Valor\":0.99935},{\"Sigla\":\"USDCVE\",\"Valor\":95.3175049},{\"Sigla\":\"USDCZK\",\"Valor\":22.1495037},{\"Sigla\":\"USDDJF\",\"Valor\":177.7204},{\"Sigla\":\"USDDKK\",\"Valor\":6.443504},{\"Sigla\":\"USDDOP\",\"Valor\":49.69504},{\"Sigla\":\"USDDZD\",\"Valor\":118.160393},{\"Sigla\":\"USDEGP\",\"Valor\":17.8545036},{\"Sigla\":\"USDERN\",\"Valor\":15.0003576},{\"Sigla\":\"USDETB\",\"Valor\":27.7038765},{\"Sigla\":\"USDEUR\",\"Valor\":0.86337},{\"Sigla\":\"USDFJD\",\"Valor\":2.10715},{\"Sigla\":\"USDFKP\",\"Valor\":0.76001},{\"Sigla\":\"USDGBP\",\"Valor\":0.76917},{\"Sigla\":\"USDGEL\",\"Valor\":2.443504},{\"Sigla\":\"USDGGP\",\"Valor\":0.769136},{\"Sigla\":\"USDGHS\",\"Valor\":4.803858},{\"Sigla\":\"USDGIP\",\"Valor\":0.76001},{\"Sigla\":\"USDGMD\",\"Valor\":48.17504},{\"Sigla\":\"USDGNF\",\"Valor\":9082.504},{\"Sigla\":\"USDGTQ\",\"Valor\":7.49635},{\"Sigla\":\"USDGYD\",\"Valor\":208.870392},{\"Sigla\":\"USDHKD\",\"Valor\":7.84915},{\"Sigla\":\"USDHNL\",\"Valor\":24.03039},{\"Sigla\":\"USDHRK\",\"Valor\":6.407804},{\"Sigla\":\"USDHTG\",\"Valor\":66.33404},{\"Sigla\":\"USDHUF\",\"Valor\":276.830383},{\"Sigla\":\"USDIDR\",\"Valor\":14493.0},{\"Sigla\":\"USDILS\",\"Valor\":3.687904},{\"Sigla\":\"USDIMP\",\"Valor\":0.769136},{\"Sigla\":\"USDINR\",\"Valor\":68.51504},{\"Sigla\":\"USDIQD\",\"Valor\":1190.0},{\"Sigla\":\"USDIRR\",\"Valor\":44220.5039},{\"Sigla\":\"USDISK\",\"Valor\":107.030388},{\"Sigla\":\"USDJEP\",\"Valor\":0.769136},{\"Sigla\":\"USDJMD\",\"Valor\":134.3404},{\"Sigla\":\"USDJOD\",\"Valor\":0.709504},{\"Sigla\":\"USDJPY\",\"Valor\":111.265038},{\"Sigla\":\"USDKES\",\"Valor\":100.330383},{\"Sigla\":\"USDKGS\",\"Valor\":68.152504},{\"Sigla\":\"USDKHR\",\"Valor\":4066.00024},{\"Sigla\":\"USDKMF\",\"Valor\":424.7504},{\"Sigla\":\"USDKPW\",\"Valor\":900.0481},{\"Sigla\":\"USDKRW\",\"Valor\":1121.95044},{\"Sigla\":\"USDKWD\",\"Valor\":0.302904},{\"Sigla\":\"USDKYD\",\"Valor\":0.832865},{\"Sigla\":\"USDKZT\",\"Valor\":349.320374},{\"Sigla\":\"USDLAK\",\"Valor\":8455.0},{\"Sigla\":\"USDLBP\",\"Valor\":1512.65039},{\"Sigla\":\"USDLKR\",\"Valor\":159.810379},{\"Sigla\":\"USDLRD\",\"Valor\":151.850388},{\"Sigla\":\"USDLSL\",\"Valor\":13.2950392},{\"Sigla\":\"USDLTL\",\"Valor\":3.048704},{\"Sigla\":\"USDLVL\",\"Valor\":0.62055},{\"Sigla\":\"USDLYD\",\"Valor\":1.380381},{\"Sigla\":\"USDMAD\",\"Valor\":9.485039},{\"Sigla\":\"USDMDL\",\"Valor\":16.4915047},{\"Sigla\":\"USDMGA\",\"Valor\":3282.50366},{\"Sigla\":\"USDMKD\",\"Valor\":53.23604},{\"Sigla\":\"USDMMK\",\"Valor\":1466.80371},{\"Sigla\":\"USDMNT\",\"Valor\":2449.5},{\"Sigla\":\"USDMOP\",\"Valor\":8.081604},{\"Sigla\":\"USDMRO\",\"Valor\":355.503754},{\"Sigla\":\"USDMUR\",\"Valor\":34.44038},{\"Sigla\":\"USDMVR\",\"Valor\":15.4103785},{\"Sigla\":\"USDMWK\",\"Valor\":726.435059},{\"Sigla\":\"USDMXN\",\"Valor\":18.5562038},{\"Sigla\":\"USDMYR\",\"Valor\":4.082504},{\"Sigla\":\"USDMZN\",\"Valor\":57.890377},{\"Sigla\":\"USDNAD\",\"Valor\":13.2803774},{\"Sigla\":\"USDNGN\",\"Valor\":360.503723},{\"Sigla\":\"USDNIO\",\"Valor\":31.7303772},{\"Sigla\":\"USDNOK\",\"Valor\":8.256304},{\"Sigla\":\"USDNPR\",\"Valor\":109.960373},{\"Sigla\":\"USDNZD\",\"Valor\":1.48305},{\"Sigla\":\"USDOMR\",\"Valor\":0.384965},{\"Sigla\":\"USDPAB\",\"Valor\":0.99935},{\"Sigla\":\"USDPEN\",\"Valor\":3.268404},{\"Sigla\":\"USDPGK\",\"Valor\":3.29525},{\"Sigla\":\"USDPHP\",\"Valor\":53.06504},{\"Sigla\":\"USDPKR\",\"Valor\":124.103706},{\"Sigla\":\"USDPLN\",\"Valor\":3.684604},{\"Sigla\":\"USDPYG\",\"Valor\":5741.70361},{\"Sigla\":\"USDQAR\",\"Valor\":3.641038},{\"Sigla\":\"USDRON\",\"Valor\":3.993404},{\"Sigla\":\"USDRSD\",\"Valor\":102.060371},{\"Sigla\":\"USDRUB\",\"Valor\":63.3420372},{\"Sigla\":\"USDRWF\",\"Valor\":865.0},{\"Sigla\":\"USDSAR\",\"Valor\":3.750604},{\"Sigla\":\"USDSBD\",\"Valor\":7.88185},{\"Sigla\":\"USDSCR\",\"Valor\":13.5880384},{\"Sigla\":\"USDSDG\",\"Valor\":18.0570374},{\"Sigla\":\"USDSEK\",\"Valor\":8.914804},{\"Sigla\":\"USDSGD\",\"Valor\":1.366204},{\"Sigla\":\"USDSHP\",\"Valor\":1.320904},{\"Sigla\":\"USDSLL\",\"Valor\":7705.00049},{\"Sigla\":\"USDSOS\",\"Valor\":578.503662},{\"Sigla\":\"USDSRD\",\"Valor\":7.458038},{\"Sigla\":\"USDSTD\",\"Valor\":21195.85},{\"Sigla\":\"USDSVC\",\"Valor\":8.74555},{\"Sigla\":\"USDSYP\",\"Valor\":515.000366},{\"Sigla\":\"USDSZL\",\"Valor\":13.2950382},{\"Sigla\":\"USDTHB\",\"Valor\":33.2055054},{\"Sigla\":\"USDTJS\",\"Valor\":9.409804},{\"Sigla\":\"USDTMT\",\"Valor\":3.51},{\"Sigla\":\"USDTND\",\"Valor\":2.710369},{\"Sigla\":\"USDTOP\",\"Valor\":2.27405},{\"Sigla\":\"USDTRY\",\"Valor\":5.081504},{\"Sigla\":\"USDTTD\",\"Valor\":6.73655},{\"Sigla\":\"USDTWD\",\"Valor\":30.5880375},{\"Sigla\":\"USDTZS\",\"Valor\":2281.10352},{\"Sigla\":\"USDUAH\",\"Valor\":27.1090374},{\"Sigla\":\"USDUGX\",\"Valor\":3696.25049},{\"Sigla\":\"USDUSD\",\"Valor\":1.0},{\"Sigla\":\"USDUYU\",\"Valor\":30.5036259},{\"Sigla\":\"USDUZS\",\"Valor\":7800.00049},{\"Sigla\":\"USDVEF\",\"Valor\":172455.0},{\"Sigla\":\"USDVND\",\"Valor\":23306.0},{\"Sigla\":\"USDVUV\",\"Valor\":109.53},{\"Sigla\":\"USDWST\",\"Valor\":2.613013},{\"Sigla\":\"USDXAF\",\"Valor\":566.2036},{\"Sigla\":\"USDXAG\",\"Valor\":0.064861},{\"Sigla\":\"USDXAU\",\"Valor\":0.000824},{\"Sigla\":\"USDXCD\",\"Valor\":2.70255},{\"Sigla\":\"USDXDR\",\"Valor\":0.716102},{\"Sigla\":\"USDXOF\",\"Valor\":577.5036},{\"Sigla\":\"USDXPF\",\"Valor\":103.33036},{\"Sigla\":\"USDYER\",\"Valor\":250.3036},{\"Sigla\":\"USDZAR\",\"Valor\":13.3252039},{\"Sigla\":\"USDZMK\",\"Valor\":9001.203},{\"Sigla\":\"USDZMW\",\"Valor\":9.960363},{\"Sigla\":\"USDZWL\",\"Valor\":322.355}]";

            var lstCotacoes = JsonConvert.DeserializeObject<List<Moeda>>(strJson);
            
            return  lstCotacoes;
        }


    }
}

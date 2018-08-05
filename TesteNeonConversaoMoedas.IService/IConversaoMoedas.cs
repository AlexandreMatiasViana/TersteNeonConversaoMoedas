using System.Collections.Generic;
using TesteNeonConversaoMoedas.Domain;

namespace TesteNeonConversaoMoedas.IService
{
    public interface IConversaoMoedas
    {
        List<Moeda> RetornaListaDeCotacaoDasMoedas(string apiUrl);

        ValidaDados ValidarDados(ConverterMoeda converterMoeda);

        ResponseConversao RetornaResponseConversao(List<Moeda> lstCotacoes, ConverterMoeda converterMoeda);

        List<KeyValuePair<string, string>> ListagemMoedas();

    }
}

namespace TesteNeonConversaoMoedas.Domain
{
    public class ResponseConversao
    {
        public bool Sucesso { get; set; }
        public string MoedaOrigem { get; set; }
        public string MoedaDestino { get; set; }
        public float ValorInformado { get; set; }
        public float ValorConvertido { get; set; }
    }
}

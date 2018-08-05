
using System;

namespace TesteNeonConversaoMoedas.Domain
{
    public class Response
    {
        public DateTime DataRetorno { get; set; }
        public bool Sucesso { get; set; }
        public string Mensagem { get; set; }
        public object ObjetoRetorno { get; set; }
    }
}

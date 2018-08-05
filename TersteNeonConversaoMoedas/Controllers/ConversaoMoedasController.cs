using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using TesteNeonConversaoMoedas.Domain;
using TesteNeonConversaoMoedas.IService;

namespace TersteNeonConversaoMoedas.Controllers
{
    [Route("api/[controller]")]
    public class ConversaoMoedasController : Controller
    {
        private IConfiguration _configuration;
        private IConversaoMoedas _conversaoMoedas;
        private string ApiAccessKey = "da6c3af62eae6b52af361ca2d1926c33";
        private string UrlCotacoes = "http://www.apilayer.net/api/live?access_key=";
        private readonly Response _response = new Response();

        public ConversaoMoedasController(IConfiguration configuration, IConversaoMoedas conversaoMoedas)
        {
            _configuration = configuration;
            _conversaoMoedas = conversaoMoedas;
        }

        [HttpGet]
        [Route("RetornaCotacoes")]
        public Task<Response> RetornaCotacoes()
        {
            var lstCotacoes = _conversaoMoedas.RetornaListaDeCotacaoDasMoedas(string.Concat(UrlCotacoes, ApiAccessKey));

            _response.DataRetorno = DateTime.Now;
            _response.Mensagem = lstCotacoes.Count > 0 ? "Lista de contações efetuada com sucesso." : "Ops! ocorreu um erro ao listar as cotações.";
            _response.Sucesso = false;
            _response.ObjetoRetorno = lstCotacoes.Count > 0 ? lstCotacoes : null;

            return Task.FromResult(_response);
        }


        [HttpPost]
        [Route("ConverterMoeda")]
        public Task<Response> ConverterMoeda([FromBody] ConverterMoeda converterMoeda)
        {

            var validaDados = _conversaoMoedas.ValidarDados(converterMoeda);
            
            if (validaDados.lstErros.Count > 0)
            {
                _response.DataRetorno = DateTime.Now;
                _response.Mensagem = validaDados.Mensagem;
                _response.Sucesso = false;
                _response.ObjetoRetorno = validaDados.lstErros;

                return Task.FromResult(_response);
            }

            var lstCotacoes = _conversaoMoedas.RetornaListaDeCotacaoDasMoedas(string.Concat(UrlCotacoes, ApiAccessKey));

            var responseConversao = _conversaoMoedas.RetornaResponseConversao(lstCotacoes, converterMoeda);


            _response.DataRetorno = DateTime.Now;
            _response.Mensagem = responseConversao.Sucesso ? "Conversão efetuada com sucesso" : "Ops! erro ao efetuar a conversão";
            _response.Sucesso = responseConversao.Sucesso;
            _response.ObjetoRetorno = responseConversao.Sucesso ? responseConversao : null;


            return Task.FromResult(_response);


        }

        [HttpGet]
        [Route("ListagemMoedas")]
        public Task<Response> ListagemMoedas()
        {
            var listagemMoedas = _conversaoMoedas.ListagemMoedas();

            _response.DataRetorno = DateTime.Now;
            _response.Mensagem = listagemMoedas.Count > 0 ? "Listagem de moedas efetuada com sucesso." : "Ops! ocorreu um erro com a listagem das moedas.";
            _response.Sucesso = listagemMoedas.Count > 0;
            _response.ObjetoRetorno = listagemMoedas.Count > 0 ? listagemMoedas : null;


            return Task.FromResult(_response);
        }
    }
}

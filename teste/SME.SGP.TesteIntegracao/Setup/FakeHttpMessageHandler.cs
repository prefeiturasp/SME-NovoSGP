using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.TesteIntegracao.Setup
{
    public class FakeHttpMessageHandler : HttpMessageHandler
    {
        private readonly Dictionary<string, HttpResponseMessage> _cenarios;

        public FakeHttpMessageHandler()
        {
            _cenarios = new Dictionary<string, HttpResponseMessage>();
        }

        public void AdicionarCenario(string url, HttpStatusCode statusCode, HttpContent conteudo)
        {
            if (string.IsNullOrEmpty(url))
                throw new ArgumentNullException(nameof(url), "A URL para o cenário do FakeHttpMessageHandler não pode ser nula ou vazia. Verifique se a chave de configuração da API existe no appsettings.json do projeto de teste.");

            var resposta = new HttpResponseMessage(statusCode) { Content = conteudo };
            if (!_cenarios.ContainsKey(url))
                _cenarios.Add(url, resposta);
            else
                _cenarios[url] = resposta;
        }

        public void LimparCenarios()
        {
            _cenarios.Clear();
        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var requestUri = request.RequestUri.ToString();

            // Procura por uma URL exata ou que comece com o padrão configurado
            var chaveCenario = _cenarios.Keys.FirstOrDefault(k => requestUri.StartsWith(k));

            if (chaveCenario != null)
            {
                return Task.FromResult(_cenarios[chaveCenario]);
            }

            // Se nenhum cenário for encontrado, retorna 404 Not Found.
            return Task.FromResult(new HttpResponseMessage(HttpStatusCode.NotFound) { RequestMessage = request });
        }
    }
}

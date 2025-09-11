using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using Moq.Protected;
using Newtonsoft.Json;
using SME.SGP.Infra;
using Xunit;

namespace SME.SGP.Aplicacao
{
    public class ObterPerfisPorLoginQueryHandlerTeste
    {
        private readonly Mock<IHttpClientFactory> _httpClientFactoryMock;
        private readonly ObterPerfisPorLoginQueryHandler _handler;
        private readonly Mock<HttpMessageHandler> _httpMessageHandlerMock;

        public ObterPerfisPorLoginQueryHandlerTeste()
        {
            _httpClientFactoryMock = new Mock<IHttpClientFactory>();
            _httpMessageHandlerMock = new Mock<HttpMessageHandler>();

            var httpClient = new HttpClient(_httpMessageHandlerMock.Object)
            {
                BaseAddress = new Uri("http://teste.com/")
            };

            _httpClientFactoryMock.Setup(x => x.CreateClient(ServicosEolConstants.SERVICO))
                .Returns(httpClient);

            _handler = new ObterPerfisPorLoginQueryHandler(_httpClientFactoryMock.Object);
        }

        [Fact]
        public async Task Deve_Retornar_Perfis_Quando_Requisicao_For_Bem_Sucedida()
        {
            var login = "usuario.teste";
            var perfisEsperados = new PerfisApiEolDto { Perfis = new List<Guid> { Guid.NewGuid() } };

            var response = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(JsonConvert.SerializeObject(perfisEsperados))
            };

            _httpMessageHandlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.Is<HttpRequestMessage>(req =>
                        req.Method == HttpMethod.Get &&
                        req.RequestUri.ToString().Contains(login)),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(response);

            var query = new ObterPerfisPorLoginQuery(login);

            var resultado = await _handler.Handle(query, CancellationToken.None);

            Assert.Equal(perfisEsperados.Perfis, resultado.Perfis);
        }

        [Fact]
        public async Task Deve_Retornar_Null_Quando_Requisicao_Falhar()
        {
            var login = "usuario.teste";
            var response = new HttpResponseMessage { StatusCode = HttpStatusCode.NotFound };

            _httpMessageHandlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(response);

            var query = new ObterPerfisPorLoginQuery(login);
            var resultado = await _handler.Handle(query, CancellationToken.None);

            Assert.Null(resultado);
        }

        [Fact]
        public async Task Deve_Lancar_Excecao_Quando_HttpClientFactory_Null()
        {
            await Task.Run(() =>
            {
                var ex = Assert.Throws<ArgumentNullException>(() =>
                    new ObterPerfisPorLoginQueryHandler(null));

                Assert.Equal("httpClientFactory", ex.ParamName);
            });
        }

        [Fact]
        public async Task Deve_Validar_Uri_Correta()
        {
            var login = "usuario.teste";
            var urlEsperada = $"http://teste.com/autenticacaoSgp/carregarperfisporlogin/{login}";

            var response = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(JsonConvert.SerializeObject(new PerfisApiEolDto()))
            };

            HttpRequestMessage requestMessageCapturada = null;

            _httpMessageHandlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .Callback<HttpRequestMessage, CancellationToken>((request, _) =>
                {
                    requestMessageCapturada = request;
                })
                .ReturnsAsync(response);

            var query = new ObterPerfisPorLoginQuery(login);
            await _handler.Handle(query, CancellationToken.None);

            Assert.Equal(urlEsperada.ToLower(), requestMessageCapturada.RequestUri.ToString().ToLower());
        }
    }
}
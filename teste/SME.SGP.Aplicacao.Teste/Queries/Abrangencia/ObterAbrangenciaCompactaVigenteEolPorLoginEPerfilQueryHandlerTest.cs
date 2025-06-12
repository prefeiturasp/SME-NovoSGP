using Moq;
using Moq.Protected;
using Newtonsoft.Json;
using SME.SGP.Dominio;
using SME.SGP.Dto;
using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.Queries.Abrangencia
{
    public class ObterAbrangenciaCompactaVigenteEolPorLoginEPerfilQueryHandlerTest
    {
        [Fact]
        public async Task Deve_Retornar_Abrangencia_Quando_Sucesso()
        {
            var login = "8577099"; //IARA
            var perfil = Guid.Parse("60e1e074-37d6-e911-abd6-f81654fe895d");
            var abrangenciaEsperada = new AbrangenciaCompactaVigenteRetornoEOLDTO
            {
                Login = login,
                IdDres = new[] { "108100" }, //BUTANTÃ
                IdUes = new[] { "090484" }, //PROF ISABEL COLOMBO
                IdTurmas = new[] { "2839524" } //7C
            };

            var respostaJson = JsonConvert.SerializeObject(abrangenciaEsperada);

            var handlerMock = new Mock<HttpMessageHandler>();
            handlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(respostaJson)    
                });

            var httpClient = new HttpClient(handlerMock.Object);
            httpClient.BaseAddress = new Uri("http://localhost"); 
            var httpClientFactoryMock = new Mock<IHttpClientFactory>();
            httpClientFactoryMock.Setup(f => f.CreateClient(It.IsAny<string>())).Returns(httpClient);

            var handler = new ObterAbrangenciaCompactaVigenteEolPorLoginEPerfilQueryHandler(httpClientFactoryMock.Object);
            var query = new ObterAbrangenciaCompactaVigenteEolPorLoginEPerfilQuery(login, perfil);

            var resultado = await handler.Handle(query, CancellationToken.None);

            Assert.NotNull(resultado);
            Assert.Equal(abrangenciaEsperada.Login, resultado.Login);
            Assert.Equal(abrangenciaEsperada.IdDres, resultado.IdDres);
            Assert.Equal(abrangenciaEsperada.IdUes, resultado.IdUes);
            Assert.Equal(abrangenciaEsperada.IdTurmas, resultado.IdTurmas);
        }

        [Fact]
        public async Task Deve_Lancar_Excecao_Quando_Nao_Encontrar_Abrangencia()
        {

            var login = "8577099"; //IARA
            var perfil = Guid.Parse("60e1e074-37d6-e911-abd6-f81654fe895d");

            var handlerMock = new Mock<HttpMessageHandler>();
            handlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.NotFound
                });

            var httpClient = new HttpClient(handlerMock.Object);
            httpClient.BaseAddress = new Uri("http://localhost");
            var httpClientFactoryMock = new Mock<IHttpClientFactory>();
            httpClientFactoryMock.Setup(f => f.CreateClient(It.IsAny<string>())).Returns(httpClient);

            var handler = new ObterAbrangenciaCompactaVigenteEolPorLoginEPerfilQueryHandler(httpClientFactoryMock.Object);
            var query = new ObterAbrangenciaCompactaVigenteEolPorLoginEPerfilQuery(login, perfil);

            await Assert.ThrowsAsync<NegocioException>(() => handler.Handle(query, CancellationToken.None));
        }
    }
}

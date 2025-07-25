using MediatR;
using Moq;
using Moq.Protected;
using Newtonsoft.Json;
using SME.SGP.Dominio;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.Handlers
{
    public class ObterComponentesCurricularesEOLPorTurmasCodigoQueryHandlerTeste
    {
        [Fact]
        public async Task Deve_Retornar_Componentes_Curriculares_Quando_Sucesso()
        {
            var codigosTurmas = new[] { "111111", "222222" };
            var query = new ObterComponentesCurricularesEOLPorTurmasCodigoQuery(codigosTurmas);

            var componentesEsperados = new List<ComponenteCurricularEol>
            {
                new ComponenteCurricularEol { Codigo = 1, Descricao = "Matemática", TurmaCodigo = "111111" },
                new ComponenteCurricularEol { Codigo = 2, Descricao = "Português", TurmaCodigo = "222222" }
            };

            var respostaJson = JsonConvert.SerializeObject(componentesEsperados);

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

            var httpClient = new HttpClient(handlerMock.Object)
            {
                BaseAddress = new Uri("http://localhost")
            };

            var httpClientFactoryMock = new Mock<IHttpClientFactory>();
            httpClientFactoryMock.Setup(f => f.CreateClient(It.IsAny<string>())).Returns(httpClient);

            var mediatorMock = new Mock<IMediator>();
            mediatorMock
                .Setup(m => m.Send(It.IsAny<ObterInfoPedagogicasComponentesCurricularesPorIdsQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<InfoComponenteCurricular>());

            var handler = new ObterComponentesCurricularesEOLPorTurmasCodigoQueryHandler(httpClientFactoryMock.Object, mediatorMock.Object);

            var resultado = await handler.Handle(query, CancellationToken.None);

            Assert.NotNull(resultado);
            var lista = new List<ComponenteCurricularEol>(resultado);
            Assert.Equal(2, lista.Count);
            Assert.Equal("Matemática", lista[0].Descricao);
            Assert.Equal("111111", lista[0].TurmaCodigo);
            Assert.Equal("Português", lista[1].Descricao);
            Assert.Equal("222222", lista[1].TurmaCodigo);
        }

        [Fact]
        public async Task Deve_Lancar_Excecao_Quando_Nao_Sucesso()
        {
            var codigosTurmas = new[] { "111111" };
            var query = new ObterComponentesCurricularesEOLPorTurmasCodigoQuery(codigosTurmas);

            var handlerMock = new Mock<HttpMessageHandler>();
            handlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.BadRequest
                });

            var httpClient = new HttpClient(handlerMock.Object)
            {
                BaseAddress = new Uri("http://localhost")
            };

            var httpClientFactoryMock = new Mock<IHttpClientFactory>();
            httpClientFactoryMock.Setup(f => f.CreateClient(It.IsAny<string>())).Returns(httpClient);

            var mediatorMock = new Mock<IMediator>();

            var handler = new ObterComponentesCurricularesEOLPorTurmasCodigoQueryHandler(httpClientFactoryMock.Object, mediatorMock.Object);

            await Assert.ThrowsAsync<NegocioException>(() => handler.Handle(query, CancellationToken.None));
        }
    }
}

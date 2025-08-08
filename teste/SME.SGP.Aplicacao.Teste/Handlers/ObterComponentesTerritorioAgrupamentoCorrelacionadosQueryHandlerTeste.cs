using FluentAssertions;
using MediatR;
using Moq;
using Moq.Protected;
using Newtonsoft.Json;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.Handlers
{
    public class ObterComponentesTerritorioAgrupamentoCorrelacionadosQueryHandlerTeste
    {
        [Fact]
        public async Task Deve_Retornar_Componentes_Corretamente_Quando_Servico_Responder_Sucesso()
        {
            var codigosComponentes = new List<long> { 123, 456 };
            var componentesEol = new List<ComponenteCurricularEol>
        {
            new ComponenteCurricularEol { Codigo = 123, Descricao = "Matemática" },
            new ComponenteCurricularEol { Codigo = 456, Descricao = "Português" }
        };

            var componentesSgp = new List<ComponenteCurricularDto>
        {
            new ComponenteCurricularDto { Codigo = "123", Regencia = false },
            new ComponenteCurricularDto { Codigo = "456", Regencia = true }
        };

            var request = new ObterComponentesTerritorioAgrupamentoCorrelacionadosQuery(codigosComponentes.ToArray());

            var mockHttpHandler = new Mock<HttpMessageHandler>(MockBehavior.Strict);
            mockHttpHandler
                .Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(JsonConvert.SerializeObject(componentesEol), Encoding.UTF8, "application/json")
                });

            var httpClient = new HttpClient(mockHttpHandler.Object)
            {
                BaseAddress = new Uri("https://fake-eol.api")
            };

            var httpClientFactory = new Mock<IHttpClientFactory>();
            httpClientFactory.Setup(f => f.CreateClient(It.IsAny<string>())).Returns(httpClient);

            var mediator = new Mock<IMediator>();
            mediator.Setup(m => m.Send(It.IsAny<InfoComponenteCurricular>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(componentesSgp);

            var handler = new ObterComponentesTerritorioAgrupamentoCorrelacionadosQueryHandler(httpClientFactory.Object, mediator.Object);

            var resultado = await handler.Handle(request, CancellationToken.None);

            resultado.Should().NotBeNull();
            resultado.Should().HaveCount(2);
            resultado.Should().Contain(r => r.Codigo == 123 && r.Descricao == "Matemática");
            resultado.Should().Contain(r => r.Codigo == 456 && r.Descricao == "Português");
        }
    }
}

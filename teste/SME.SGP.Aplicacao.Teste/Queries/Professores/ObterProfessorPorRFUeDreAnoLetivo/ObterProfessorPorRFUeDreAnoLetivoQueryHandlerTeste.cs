using MediatR;
using Moq;
using Moq.Protected;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using SME.SGP.Infra.Enumerados;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.Queries.Professores.ObterProfessorPorRFUeDreAnoLetivo
{
    public class ObterProfessorPorRFUeDreAnoLetivoQueryHandlerTeste
    {
        private readonly Mock<IHttpClientFactory> httpClientFactory;
        private readonly Mock<IMediator> mediator;
        private readonly ObterProfessorPorRFUeDreAnoLetivoQueryHandler queryHandler;

        public ObterProfessorPorRFUeDreAnoLetivoQueryHandlerTeste()
        {
            httpClientFactory = new Mock<IHttpClientFactory>();
            mediator = new Mock<IMediator>();
            queryHandler = new ObterProfessorPorRFUeDreAnoLetivoQueryHandler(httpClientFactory.Object, mediator.Object);
        }

        [Fact(DisplayName = "ObterProfessorPorRFUeDreAnoLetivo - Deve retornar dados mesmo sem atribuições ativas")]
        public async Task DeveRetornarDadosMesmoSemAtribuicoesAtivas()
        {
            var anoLetivo = DateTimeExtension.HorarioBrasilia().Year;
            MockRepository mockRepository = new(MockBehavior.Default);
            var handlerMock = mockRepository.Create<HttpMessageHandler>();
            var url = string.Format(ServicosEolConstants.URL_PROFESSORES_BUSCAR_POR_RF_DRE_UE, "1", anoLetivo);

            handlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage { StatusCode = HttpStatusCode.NoContent }).Verifiable();

            HttpClient httpClient = new(handlerMock.Object) { BaseAddress = new System.Uri("http://localhost") };
            httpClientFactory.Setup(x => x.CreateClient(ServicosEolConstants.SERVICO))
                .Returns(httpClient);

            var usuario = new Usuario() { Id = 1, PerfilAtual = Perfis.PERFIL_CJ_INFANTIL, Nome = "usuário" };
            usuario.DefinirPerfis(new List<PrioridadePerfil>() { new() { CodigoPerfil = Perfis.PERFIL_CJ_INFANTIL } });
            mediator.Setup(x => x.Send(It.IsAny<ObterUsuarioLogadoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(usuario);

            mediator.Setup(x => x.Send(It.Is<ObterAtribuicoesCJAtivasQuery>(y => y.CodigoRf == "1" && !y.Historico), It.IsAny<CancellationToken>()))
                .ReturnsAsync(Enumerable.Empty<AtribuicaoCJ>());

            var resultado = await queryHandler
                .Handle(new ObterProfessorPorRFUeDreAnoLetivoQuery("1", anoLetivo, "1", "1", true, false), It.IsAny<CancellationToken>());

            Assert.NotNull(resultado);
            Assert.Equal("1", resultado.CodigoRF);
            Assert.Equal("usuário", resultado.Nome);
            Assert.Equal(1, resultado.UsuarioId);
        }
    }
}

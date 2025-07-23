using MediatR;
using Moq;
using SME.SGP.Aplicacao.CasosDeUso.EscolaAqui.Dashboard.ObterDadosDeLeituraDeComunicadosAgrupadosPorDre;
using SME.SGP.Infra.Dtos.EscolaAqui.DadosDeLeituraDeComunicados;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso.EscolaAqui.Dashboard.ObterDadosDeLeituraDeComunicadosAgrupadosPorDre
{
    public class ObterDadosDeLeituraDeComunicadosAgrupadosPorDreUseCaseTeste
    {
        private readonly Mock<IMediator> mediatorMock;
        private readonly ObterDadosDeLeituraDeComunicadosAgrupadosPorDreUseCase useCase;

        public ObterDadosDeLeituraDeComunicadosAgrupadosPorDreUseCaseTeste()
        {
            mediatorMock = new Mock<IMediator>();
            useCase = new ObterDadosDeLeituraDeComunicadosAgrupadosPorDreUseCase(mediatorMock.Object);
        }

        [Fact]
        public void Construtor_Com_Mediator_Nulo_Deve_Lancar_ArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() =>
                new ObterDadosDeLeituraDeComunicadosAgrupadosPorDreUseCase(null));
        }

        [Fact]
        public async Task Executar_Deve_Chamar_Mediator_Com_Query_Correta()
        {
            var dto = new ObterDadosDeLeituraDeComunicadosAgrupadosPorDreDto
            {
                NotificacaoId = 12345,
                ModoVisualizacao = 2
            };

            mediatorMock
                .Setup(m => m.Send(It.IsAny<ObterDadosDeLeituraDeComunicadosAgrupadosPorDreQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<DadosDeLeituraDoComunicadoDto>());

            await useCase.Executar(dto);

            mediatorMock.Verify(m => m.Send(
                It.Is<ObterDadosDeLeituraDeComunicadosAgrupadosPorDreQuery>(q =>
                    q.ComunicadoId == dto.NotificacaoId &&
                    q.ModoVisualizacao == dto.ModoVisualizacao),
                It.IsAny<CancellationToken>()),
                Times.Once);
        }

        [Fact]
        public async Task Executar_Deve_Retornar_Resultado_Do_Mediator()
        {
            var dto = new ObterDadosDeLeituraDeComunicadosAgrupadosPorDreDto
            {
                NotificacaoId = 1001,
                ModoVisualizacao = 1
            };

            var resultadoEsperado = new List<DadosDeLeituraDoComunicadoDto>
            {
                new DadosDeLeituraDoComunicadoDto
                {
                    NomeAbreviadoDre = "DRE01",
                    NomeAbreviadoUe = null,
                    NaoReceberamComunicado = 10,
                    ReceberamENaoVisualizaram = 20,
                    VisualizaramComunicado = 30
                }
            };

            mediatorMock
                .Setup(m => m.Send(It.IsAny<ObterDadosDeLeituraDeComunicadosAgrupadosPorDreQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(resultadoEsperado);

            var resultado = await useCase.Executar(dto);

            Assert.NotNull(resultado);
            var lista = Assert.IsAssignableFrom<IEnumerable<DadosDeLeituraDoComunicadoDto>>(resultado);
            Assert.Single(lista);

            var item = Assert.Single(lista);
            Assert.Equal("DRE01", item.NomeAbreviadoDre);
            Assert.Null(item.NomeAbreviadoUe); 
            Assert.Equal(10, item.NaoReceberamComunicado);
            Assert.Equal(20, item.ReceberamENaoVisualizaram);
            Assert.Equal(30, item.VisualizaramComunicado);
        }
    }
}

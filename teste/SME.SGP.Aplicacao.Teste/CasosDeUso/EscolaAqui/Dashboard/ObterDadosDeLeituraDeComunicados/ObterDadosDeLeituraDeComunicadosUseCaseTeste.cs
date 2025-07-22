using MediatR;
using Moq;
using SME.SGP.Aplicacao.CasosDeUso.EscolaAqui.Dashboard.ObterDadosDeLeituraDeComunicados;
using SME.SGP.Infra.Dtos.EscolaAqui.DadosDeLeituraDeComunicados;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso.EscolaAqui.Dashboard.ObterDadosDeLeituraDeComunicados
{
    public class ObterDadosDeLeituraDeComunicadosUseCaseTeste
    {
        private readonly Mock<IMediator> mediatorMock;
        private readonly ObterDadosDeLeituraDeComunicadosUseCase useCase;

        public ObterDadosDeLeituraDeComunicadosUseCaseTeste()
        {
            mediatorMock = new Mock<IMediator>();
            useCase = new ObterDadosDeLeituraDeComunicadosUseCase(mediatorMock.Object);
        }

        [Fact]
        public void Construtor_Com_Mediator_Nulo_Deve_Lancar_ArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new ObterDadosDeLeituraDeComunicadosUseCase(null));
        }

        [Fact]
        public async Task Executar_Deve_Chamar_Mediator_Com_Query_Correta()
        {
            var dto = new ObterDadosDeLeituraDeComunicadosDto
            {
                CodigoDre = "DRE01",
                CodigoUe = "UE01",
                NotificacaoId = 123,
                ModoVisualizacao = 2,
                AgruparModalidade = true
            };

            mediatorMock
                .Setup(m => m.Send(It.IsAny<ObterDadosDeLeituraDeComunicadosQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<DadosDeLeituraDoComunicadoDto>());

            await useCase.Executar(dto);

            mediatorMock.Verify(m => m.Send(
                It.Is<ObterDadosDeLeituraDeComunicadosQuery>(q =>
                    q.CodigoDre == dto.CodigoDre &&
                    q.CodigoUe == dto.CodigoUe &&
                    q.ComunicadoId == dto.NotificacaoId &&
                    q.ModoVisualizacao == dto.ModoVisualizacao &&
                    q.AgruparModalidade == dto.AgruparModalidade
                ),
                It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Executar_Deve_Retornar_Lista_Do_Mediator()
        {
            var dto = new ObterDadosDeLeituraDeComunicadosDto
            {
                CodigoDre = "DRE01",
                CodigoUe = "UE01",
                NotificacaoId = 123,
                ModoVisualizacao = 1,
                AgruparModalidade = false
            };

            var resultadoEsperado = new List<DadosDeLeituraDoComunicadoDto>
            {
                new DadosDeLeituraDoComunicadoDto
                {
                    NomeAbreviadoDre = "DRE01",
                    NomeAbreviadoUe = "UE01",
                    NaoReceberamComunicado = 10,
                    ReceberamENaoVisualizaram = 5,
                    VisualizaramComunicado = 20
                }
            };

            mediatorMock
                .Setup(m => m.Send(It.IsAny<ObterDadosDeLeituraDeComunicadosQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(resultadoEsperado);

            var resultado = await useCase.Executar(dto);

            Assert.NotNull(resultado);
            var lista = Assert.IsAssignableFrom<IEnumerable<DadosDeLeituraDoComunicadoDto>>(resultado);
            Assert.Single(lista);

            var item = Assert.Single(lista);
            Assert.Equal("DRE01", item.NomeAbreviadoDre);
            Assert.Equal("UE01", item.NomeAbreviadoUe);
            Assert.Equal(10, item.NaoReceberamComunicado);
            Assert.Equal(5, item.ReceberamENaoVisualizaram);
            Assert.Equal(20, item.VisualizaramComunicado);
        }
    }
}

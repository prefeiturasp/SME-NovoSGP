using MediatR;
using Moq;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso.DashboardDevolutivas
{
    public class ObterGraficoTotalDevolutivasPorDreUseCaseTeste
    {
        private readonly Mock<IMediator> mediatorMock;
        private readonly ObterGraficoTotalDevolutivasPorDreUseCase useCase;

        public ObterGraficoTotalDevolutivasPorDreUseCaseTeste()
        {
            mediatorMock = new Mock<IMediator>();
            useCase = new ObterGraficoTotalDevolutivasPorDreUseCase(mediatorMock.Object);
        }

        [Fact]
        public async Task Executar_Quando_Filtro_Valido_Deve_Retornar_Resultados()
        {
            var filtro = new FiltroTotalDevolutivasPorDreDto
            {
                AnoLetivo = 2025,
                Ano = "1º"
            };

            var resultadoEsperado = new List<GraficoBaseDto>
            {
                new GraficoBaseDto("DRE1", 10, "Descricao1")
            };

            mediatorMock.Setup(m => m.Send(It.IsAny<ObterTotalDevolutivasPorDreQuery>(), It.IsAny<CancellationToken>()))
                        .ReturnsAsync(resultadoEsperado);

            var resultado = await useCase.Executar(filtro);

            Assert.NotNull(resultado);
            Assert.Single(resultadoEsperado);
            mediatorMock.Verify(m => m.Send(It.Is<ObterTotalDevolutivasPorDreQuery>(q =>
                q.AnoLetivo == filtro.AnoLetivo &&
                q.Ano == filtro.Ano
            ), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Executar_Quando_Mediator_Retornar_Vazio_Deve_Retornar_Lista_Vazia()
        {
            var filtro = new FiltroTotalDevolutivasPorDreDto
            {
                AnoLetivo = 2025,
                Ano = "2º"
            };

            mediatorMock.Setup(m => m.Send(It.IsAny<ObterTotalDevolutivasPorDreQuery>(), It.IsAny<CancellationToken>()))
                        .ReturnsAsync(new List<GraficoBaseDto>());

            var resultado = await useCase.Executar(filtro);

            Assert.Empty(resultado);
            mediatorMock.Verify(m => m.Send(It.IsAny<ObterTotalDevolutivasPorDreQuery>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public void Construtor_Quando_Mediator_Nulo_Deve_Lancar_Excecao()
        {
            Assert.Throws<ArgumentNullException>(() => new ObterGraficoTotalDevolutivasPorDreUseCase(null));
        }
    }
}

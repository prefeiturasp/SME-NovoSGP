using MediatR;
using Moq;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso.DashboardDevolutivas
{
    public class ObterDevolutivasEstimadasEConfirmadasUseCaseTeste
    {
        private readonly Mock<IMediator> mediatorMock;
        private readonly ObterDevolutivasEstimadasEConfirmadasUseCase useCase;

        public ObterDevolutivasEstimadasEConfirmadasUseCaseTeste()
        {
            mediatorMock = new Mock<IMediator>();
            useCase = new ObterDevolutivasEstimadasEConfirmadasUseCase(mediatorMock.Object);
        }

        [Fact]
        public async Task Executar_Quando_Filtro_Valido_Deve_Retornar_Resultados()
        {
            var filtro = new FiltroGraficoDevolutivasEstimadasEConfirmadasDto
            {
                AnoLetivo = 2025,
                Modalidade = Modalidade.EducacaoInfantil,
                DreId = 1,
                UeId = 2
            };

            var resultadoEsperado = new List<GraficoDevolutivasEstimadasEConfirmadasDto>
            {
                new GraficoDevolutivasEstimadasEConfirmadasDto
                {
                    Grupo = "Grupo1",
                    Quantidade = 5,
                    Descricao = "Descricao1",
                    TurmaAno = "1ºA"
                }
            };

            mediatorMock.Setup(m => m.Send(It.IsAny<ObterDevolutivasEstimadasEConfirmadasQuery>(), It.IsAny<CancellationToken>()))
                        .ReturnsAsync(resultadoEsperado);

            var resultado = await useCase.Executar(filtro);

            Assert.NotNull(resultado);
            Assert.Single(resultadoEsperado);
            mediatorMock.Verify(m => m.Send(It.Is<ObterDevolutivasEstimadasEConfirmadasQuery>(q =>
                q.AnoLetivo == filtro.AnoLetivo &&
                q.Modalidade == filtro.Modalidade &&
                q.DreId == filtro.DreId &&
                q.UeId == filtro.UeId
            ), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Executar_Quando_Mediator_Retornar_Vazio_Deve_Retornar_Lista_Vazia()
        {
            var filtro = new FiltroGraficoDevolutivasEstimadasEConfirmadasDto
            {
                AnoLetivo = 2025,
                Modalidade = Modalidade.EducacaoInfantil
            };

            mediatorMock.Setup(m => m.Send(It.IsAny<ObterDevolutivasEstimadasEConfirmadasQuery>(), It.IsAny<CancellationToken>()))
                        .ReturnsAsync(new List<GraficoDevolutivasEstimadasEConfirmadasDto>());

            var resultado = await useCase.Executar(filtro);

            Assert.Empty(resultado);
            mediatorMock.Verify(m => m.Send(It.IsAny<ObterDevolutivasEstimadasEConfirmadasQuery>(), It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}

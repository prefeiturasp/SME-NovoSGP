using MediatR;
using Moq;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso.DashboardDevolutivas
{
    public class ObterGraficoDiariosDeBordoComDevolutivaEDevolutivaPendenteUseCaseTeste
    {
        private readonly Mock<IMediator> mediatorMock;
        private readonly ObterGraficoDiariosDeBordoComDevolutivaEDevolutivaPendenteUseCase useCase;

        public ObterGraficoDiariosDeBordoComDevolutivaEDevolutivaPendenteUseCaseTeste()
        {
            mediatorMock = new Mock<IMediator>();
            useCase = new ObterGraficoDiariosDeBordoComDevolutivaEDevolutivaPendenteUseCase(mediatorMock.Object);
        }

        [Fact]
        public async Task Executar_Quando_Filtro_Valido_Deve_Retornar_Resultados()
        {
            var filtro = new FiltroGraficoDiariosDeBordoComDevolutivaEDevolutivaPendenteDto
            {
                AnoLetivo = 2025,
                Modalidade = Modalidade.EducacaoInfantil,
                DreId = 1,
                UeId = 2
            };

            var resultadoEsperado = new List<GraficoDiariosDeBordoComDevolutivaEDevolutivaPendenteDto>
            {
                new GraficoDiariosDeBordoComDevolutivaEDevolutivaPendenteDto
                {
                    Grupo = "Grupo1",
                    Quantidade = 10,
                    Descricao = "Descricao1"
                }
            };

            mediatorMock.Setup(m => m.Send(It.IsAny<ObterDiariosDeBordoComDevolutivaEDevolutivaPendenteQuery>(), It.IsAny<CancellationToken>()))
                        .ReturnsAsync(resultadoEsperado);

            var resultado = await useCase.Executar(filtro);

            Assert.NotNull(resultado);
            Assert.Single(resultadoEsperado);
            mediatorMock.Verify(m => m.Send(It.Is<ObterDiariosDeBordoComDevolutivaEDevolutivaPendenteQuery>(q =>
                q.AnoLetivo == filtro.AnoLetivo &&
                q.Modalidade == filtro.Modalidade &&
                q.DreId == filtro.DreId &&
                q.UeId == filtro.UeId &&
                q.DataAula.Date == DateTime.Today
            ), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Executar_Quando_Mediator_Retornar_Vazio_Deve_Retornar_Lista_Vazia()
        {
            var filtro = new FiltroGraficoDiariosDeBordoComDevolutivaEDevolutivaPendenteDto
            {
                AnoLetivo = 2025,
                Modalidade = Modalidade.EducacaoInfantil
            };

            mediatorMock.Setup(m => m.Send(It.IsAny<ObterDiariosDeBordoComDevolutivaEDevolutivaPendenteQuery>(), It.IsAny<CancellationToken>()))
                        .ReturnsAsync(new List<GraficoDiariosDeBordoComDevolutivaEDevolutivaPendenteDto>());

            var resultado = await useCase.Executar(filtro);

            Assert.Empty(resultado);
            mediatorMock.Verify(m => m.Send(It.IsAny<ObterDiariosDeBordoComDevolutivaEDevolutivaPendenteQuery>(), It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}

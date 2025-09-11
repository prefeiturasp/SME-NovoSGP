using Moq;
using Xunit;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SME.SGP.Infra;
using SME.SGP.Dominio;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso.DashboardFrequencia
{
    public class ObterDashboardFrequenciaPorAnoUseCaseTeste
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly ObterDashboardFrequenciaPorAnoUseCase _useCase;

        public ObterDashboardFrequenciaPorAnoUseCaseTeste()
        {
            _mediatorMock = new Mock<IMediator>();
            _useCase = new ObterDashboardFrequenciaPorAnoUseCase(_mediatorMock.Object);
        }

        [Fact]
        public async Task Executar_DeveChamarMediatorComParametrosCorretosERetornarDadosEsperados()
        {
            // Arrange
            int anoLetivo = 2025;
            long dreId = 1;
            long ueId = 123;
            Modalidade modalidade = Modalidade.Fundamental;
            int semestre = 2;

            var resultadoEsperado = new List<GraficoFrequenciaGlobalPorAnoDto>
            {
                new GraficoFrequenciaGlobalPorAnoDto
                {
                    Turma = "5A",
                    Grupo = "Grupo 1",
                    Quantidade = 10,
                    Descricao = "Descrição 1"
                },
                new GraficoFrequenciaGlobalPorAnoDto
                {
                    Turma = "6A",
                    Grupo = "Grupo 2",
                    Quantidade = 8,
                    Descricao = "Descrição 2"
                }
            };

            ObterDadosDashboardFrequenciaPorAnoQuery queryCapturada = null;

            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterDadosDashboardFrequenciaPorAnoQuery>(), It.IsAny<CancellationToken>()))
                .Callback<IRequest<IEnumerable<GraficoFrequenciaGlobalPorAnoDto>>, CancellationToken>((query, token) =>
                {
                    queryCapturada = query as ObterDadosDashboardFrequenciaPorAnoQuery;
                })
                .ReturnsAsync(resultadoEsperado);

            // Act
            var resultado = await _useCase.Executar(anoLetivo, dreId, ueId, modalidade, semestre);

            // Assert
            Assert.NotNull(queryCapturada);
            Assert.Equal(anoLetivo, queryCapturada.AnoLetivo);
            Assert.Equal(dreId, queryCapturada.DreId);
            Assert.Equal(ueId, queryCapturada.UeId);
            Assert.Equal(modalidade, queryCapturada.Modalidade);
            Assert.Equal(semestre, queryCapturada.Semestre);

            Assert.NotNull(resultado);
            Assert.Equal(resultadoEsperado.Count, (resultado as List<GraficoFrequenciaGlobalPorAnoDto>).Count);

            for (int i = 0; i < resultadoEsperado.Count; i++)
            {
                Assert.Equal(resultadoEsperado[i].Turma, (resultado as List<GraficoFrequenciaGlobalPorAnoDto>)[i].Turma);
                Assert.Equal(resultadoEsperado[i].Grupo, (resultado as List<GraficoFrequenciaGlobalPorAnoDto>)[i].Grupo);
                Assert.Equal(resultadoEsperado[i].Quantidade, (resultado as List<GraficoFrequenciaGlobalPorAnoDto>)[i].Quantidade);
                Assert.Equal(resultadoEsperado[i].Descricao, (resultado as List<GraficoFrequenciaGlobalPorAnoDto>)[i].Descricao);
            }

            _mediatorMock.Verify(m => m.Send(It.IsAny<ObterDadosDashboardFrequenciaPorAnoQuery>(), It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}

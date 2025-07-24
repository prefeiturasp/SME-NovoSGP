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
    public class ObterDadosDashboardFrequenciaPorDreUseCaseTeste
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly ObterDadosDashboardFrequenciaPorDreUseCase _useCase;

        public ObterDadosDashboardFrequenciaPorDreUseCaseTeste()
        {
            _mediatorMock = new Mock<IMediator>();
            _useCase = new ObterDadosDashboardFrequenciaPorDreUseCase(_mediatorMock.Object);
        }

        [Fact]
        public async Task Executar_DeveChamarMediatorComParametrosCorretosERetornarDadosEsperados()
        {
            // Arrange
            var filtro = new FiltroGraficoFrequenciaGlobalPorDREDto
            {
                AnoLetivo = 2025,
                Modalidade = Modalidade.Fundamental,
                Ano = "5A",
                Semestre = 2
            };

            var resultadoEsperado = new List<GraficoFrequenciaGlobalPorDREDto>
            {
                new GraficoFrequenciaGlobalPorDREDto
                {
                    Dre = "DRE01",
                    Grupo = "Grupo 1",
                    Quantidade = 100,
                    Descricao = "Descrição 1"
                },
                new GraficoFrequenciaGlobalPorDREDto
                {
                    Dre = "DRE02",
                    Grupo = "Grupo 2",
                    Quantidade = 80,
                    Descricao = "Descrição 2"
                }
            };

            ObterDadosDashboardFrequenciaPorDreQuery queryCapturada = null;

            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterDadosDashboardFrequenciaPorDreQuery>(), It.IsAny<CancellationToken>()))
                .Callback<IRequest<IEnumerable<GraficoFrequenciaGlobalPorDREDto>>, CancellationToken>((query, token) =>
                {
                    queryCapturada = query as ObterDadosDashboardFrequenciaPorDreQuery;
                })
                .ReturnsAsync(resultadoEsperado);

            // Act
            var resultado = await _useCase.Executar(filtro);

            // Assert
            Assert.NotNull(queryCapturada);
            Assert.Equal(filtro.AnoLetivo, queryCapturada.AnoLetivo);
            Assert.Equal(filtro.Modalidade, queryCapturada.Modalidade);
            Assert.Equal(filtro.Ano, queryCapturada.Ano);
            Assert.Equal(filtro.Semestre, queryCapturada.Semestre);

            Assert.NotNull(resultado);
            Assert.Equal(resultadoEsperado.Count, (resultado as List<GraficoFrequenciaGlobalPorDREDto>).Count);

            for (int i = 0; i < resultadoEsperado.Count; i++)
            {
                Assert.Equal(resultadoEsperado[i].Dre, (resultado as List<GraficoFrequenciaGlobalPorDREDto>)[i].Dre);
                Assert.Equal(resultadoEsperado[i].Grupo, (resultado as List<GraficoFrequenciaGlobalPorDREDto>)[i].Grupo);
                Assert.Equal(resultadoEsperado[i].Quantidade, (resultado as List<GraficoFrequenciaGlobalPorDREDto>)[i].Quantidade);
                Assert.Equal(resultadoEsperado[i].Descricao, (resultado as List<GraficoFrequenciaGlobalPorDREDto>)[i].Descricao);
            }

            _mediatorMock.Verify(m => m.Send(It.IsAny<ObterDadosDashboardFrequenciaPorDreQuery>(), It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}

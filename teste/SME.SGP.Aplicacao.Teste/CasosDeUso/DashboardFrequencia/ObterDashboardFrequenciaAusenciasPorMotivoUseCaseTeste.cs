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
    public class ObterDashboardFrequenciaAusenciasPorMotivoUseCaseTeste
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly ObterDashboardFrequenciaAusenciasPorMotivoUseCase _useCase;

        public ObterDashboardFrequenciaAusenciasPorMotivoUseCaseTeste()
        {
            _mediatorMock = new Mock<IMediator>();
            _useCase = new ObterDashboardFrequenciaAusenciasPorMotivoUseCase(_mediatorMock.Object);
        }

        [Fact]
        public async Task Executar_DeveChamarMediatorComParametrosCorretosERetornarDadosEsperados()
        {
            // Arrange
            int anoLetivo = 2025;
            long dreId = 1;
            long ueId = 123;
            Modalidade modalidade = Modalidade.Fundamental;
            string ano = "5A";
            long turmaId = 456;
            int semestre = 2;

            var resultadoEsperado = new List<GraficoBaseDto>
            {
                new GraficoBaseDto("Grupo 1", 10, "Descrição 1"),
                new GraficoBaseDto("Grupo 2", 5, "Descrição 2")
            };

            ObterDashboardFrequenciaAusenciasPorMotivoQuery queryCapturada = null;

            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterDashboardFrequenciaAusenciasPorMotivoQuery>(), It.IsAny<CancellationToken>()))
                .Callback<IRequest<IEnumerable<GraficoBaseDto>>, CancellationToken>((query, token) =>
                {
                    queryCapturada = query as ObterDashboardFrequenciaAusenciasPorMotivoQuery;
                })
                .ReturnsAsync(resultadoEsperado);

            // Act
            var resultado = await _useCase.Executar(anoLetivo, dreId, ueId, modalidade, ano, turmaId, semestre);

            // Assert
            Assert.NotNull(queryCapturada);
            Assert.Equal(anoLetivo, queryCapturada.AnoLetivo);
            Assert.Equal(dreId, queryCapturada.DreId);
            Assert.Equal(ueId, queryCapturada.UeId);
            Assert.Equal(modalidade, queryCapturada.Modalidade);
            Assert.Equal(ano, queryCapturada.Ano);
            Assert.Equal(turmaId, queryCapturada.TurmaId);
            Assert.Equal(semestre, queryCapturada.Semestre);

            Assert.NotNull(resultado);
            Assert.Equal(resultadoEsperado.Count, (resultado as List<GraficoBaseDto>).Count);

            for (int i = 0; i < resultadoEsperado.Count; i++)
            {
                Assert.Equal(resultadoEsperado[i].Grupo, (resultado as List<GraficoBaseDto>)[i].Grupo);
                Assert.Equal(resultadoEsperado[i].Quantidade, (resultado as List<GraficoBaseDto>)[i].Quantidade);
                Assert.Equal(resultadoEsperado[i].Descricao, (resultado as List<GraficoBaseDto>)[i].Descricao);
            }

            _mediatorMock.Verify(m => m.Send(It.IsAny<ObterDashboardFrequenciaAusenciasPorMotivoQuery>(), It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}

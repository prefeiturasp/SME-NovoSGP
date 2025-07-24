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
    public class ObterDadosDashboardAusenciasComJustificativaUseCaseTeste
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly ObterDadosDashboardAusenciasComJustificativaUseCase _useCase;

        public ObterDadosDashboardAusenciasComJustificativaUseCaseTeste()
        {
            _mediatorMock = new Mock<IMediator>();
            _useCase = new ObterDadosDashboardAusenciasComJustificativaUseCase(_mediatorMock.Object);
        }

        [Fact]
        public async Task Executar_DeveRetornarDadosCorretosEChamarMediatorComParametrosEsperados()
        {
            // Arrange
            int anoLetivo = 2025;
            long dreId = 1;
            long ueId = 123;
            Modalidade modalidade = Modalidade.Medio;
            int semestre = 2;

            var resultadoEsperado = new List<GraficoAusenciasComJustificativaResultadoDto>
            {
                new GraficoAusenciasComJustificativaResultadoDto
                {
                    Grupo = "Grupo 1",
                    Quantidade = 10,
                    Descricao = "Descrição 1",
                    ModalidadeAno = "EM-2025"
                },
                new GraficoAusenciasComJustificativaResultadoDto
                {
                    Grupo = "Grupo 2",
                    Quantidade = 5,
                    Descricao = "Descrição 2",
                    ModalidadeAno = "EM-2025"
                }
            };

            ObterDadosDashboardAusenciasComJustificativaQuery queryCapturada = null;

            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterDadosDashboardAusenciasComJustificativaQuery>(), It.IsAny<CancellationToken>()))
                .Callback<IRequest<IEnumerable<GraficoAusenciasComJustificativaResultadoDto>>, CancellationToken>((query, token) =>
                {
                    queryCapturada = query as ObterDadosDashboardAusenciasComJustificativaQuery;
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
            Assert.Equal(semestre, queryCapturada.Semstre);

            Assert.NotNull(resultado);
            Assert.Equal(resultadoEsperado.Count, (resultado as List<GraficoAusenciasComJustificativaResultadoDto>).Count);

            for (int i = 0; i < resultadoEsperado.Count; i++)
            {
                Assert.Equal(resultadoEsperado[i].Grupo, (resultado as List<GraficoAusenciasComJustificativaResultadoDto>)[i].Grupo);
                Assert.Equal(resultadoEsperado[i].Quantidade, (resultado as List<GraficoAusenciasComJustificativaResultadoDto>)[i].Quantidade);
                Assert.Equal(resultadoEsperado[i].Descricao, (resultado as List<GraficoAusenciasComJustificativaResultadoDto>)[i].Descricao);
                Assert.Equal(resultadoEsperado[i].ModalidadeAno, (resultado as List<GraficoAusenciasComJustificativaResultadoDto>)[i].ModalidadeAno);
            }

            _mediatorMock.Verify(m => m.Send(It.IsAny<ObterDadosDashboardAusenciasComJustificativaQuery>(), It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}

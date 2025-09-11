using Moq;
using Xunit;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso
{
    public class ObterListagemDiariosDeBordoPorPeriodoUseCaseTeste
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly ObterListagemDiariosDeBordoPorPeriodoUseCase _useCase;

        public ObterListagemDiariosDeBordoPorPeriodoUseCaseTeste()
        {
            _mediatorMock = new Mock<IMediator>();
            _useCase = new ObterListagemDiariosDeBordoPorPeriodoUseCase(_mediatorMock.Object);
        }

        [Fact]
        public async Task Executar_DeveRetornarPaginacaoResultadoDto_ComDadosCompletos()
        {
            // Arrange
            var filtro = new FiltroListagemDiarioBordoDto(
                turmaId: 123,
                componenteCurricularId: 456,
                dataInicio: new DateTime(2025, 5, 1),
                dataFim: new DateTime(2025, 6, 30)
            );

            string componentePai = "789";

            var resultadoEsperado = new PaginacaoResultadoDto<DiarioBordoTituloDto>
            {
                TotalRegistros = 1,
                Items = new List<DiarioBordoTituloDto>
            {
                new DiarioBordoTituloDto(
                    id: 1001,
                    titulo: "Diário de Aula 01",
                    pendente: true,
                    aulaId: 2001
                )
            }
            };

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<ObterCodigoComponentePaiQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(componentePai);

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<ObterListagemDiariosDeBordoPorPeriodoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(resultadoEsperado);

            // Act
            var resultado = await _useCase.Executar(filtro);

            // Assert
            Assert.NotNull(resultado);
            Assert.Equal(1, resultado.TotalRegistros);

            var diario = Assert.Single(resultado.Items);
            Assert.Equal(1001, diario.Id);
            Assert.Equal("Diário de Aula 01", diario.Titulo);
            Assert.True(diario.Pendente);
            Assert.Equal(2001, diario.AulaId);

            // Verifica se o mediator foi chamado com o query correto para obter o componente pai
            _mediatorMock.Verify(m => m.Send(It.Is<ObterCodigoComponentePaiQuery>(
                q => q.ComponenteCurricularId == filtro.ComponenteCurricularId), It.IsAny<CancellationToken>()), Times.Once);

            // Verifica se o mediator foi chamado com o query correto para obter os diários
            _mediatorMock.Verify(m => m.Send(It.Is<ObterListagemDiariosDeBordoPorPeriodoQuery>(
                q => q.TurmaId == filtro.TurmaId &&
                     q.ComponenteCurricularPaiId == componentePai &&
                     q.ComponenteCurricularFilhoId == filtro.ComponenteCurricularId &&
                     q.DataInicio == filtro.DataInicio &&
                     q.DataFim == filtro.DataFim
            ), It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}

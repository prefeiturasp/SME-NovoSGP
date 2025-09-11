using MediatR;
using Moq;
using SME.SGP.Dominio;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso.PeriodosEscolares
{
    public class PeriodoFechamentoUseCaseTests
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly PeriodoFechamentoUseCase _useCase;

        public PeriodoFechamentoUseCaseTests()
        {
            _mediatorMock = new Mock<IMediator>();
            _useCase = new PeriodoFechamentoUseCase(_mediatorMock.Object);
        }

        [Fact]
        public async Task Executar_DeveRetornarTrueQuandoTurmaEstaEmPeriodoDeFechamento()
        {
            var turmaCodigo = "COD123";
            var dataReferencia = DateTime.Now;
            var bimestre = 1;

            var turmaMock = new Turma { CodigoTurma = turmaCodigo };
            _mediatorMock.Setup(m => m.Send(It.Is<ObterTurmaPorCodigoQuery>(q => q.TurmaCodigo == turmaCodigo), It.IsAny<CancellationToken>()))
                .ReturnsAsync(turmaMock);

            _mediatorMock.Setup(m => m.Send(It.Is<ObterTurmaEmPeriodoDeFechamentoQuery>(q =>
                q.Turma == turmaMock && q.DataReferencia == dataReferencia && q.Bimestre == bimestre), It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            var result = await _useCase.Executar(turmaCodigo, dataReferencia, bimestre);

            Assert.True(result);
            _mediatorMock.Verify(m => m.Send(It.Is<ObterTurmaPorCodigoQuery>(q => q.TurmaCodigo == turmaCodigo), It.IsAny<CancellationToken>()), Times.Once);
            _mediatorMock.Verify(m => m.Send(It.Is<ObterTurmaEmPeriodoDeFechamentoQuery>(q =>
                q.Turma == turmaMock && q.DataReferencia == dataReferencia && q.Bimestre == bimestre), It.IsAny<CancellationToken>()), Times.Once);
            _mediatorMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task Executar_DeveRetornarFalseQuandoTurmaNaoEstaEmPeriodoDeFechamento()
        {
            var turmaCodigo = "COD456";
            var dataReferencia = DateTime.Now;
            var bimestre = 2;

            var turmaMock = new Turma { CodigoTurma = turmaCodigo };
            _mediatorMock.Setup(m => m.Send(It.Is<ObterTurmaPorCodigoQuery>(q => q.TurmaCodigo == turmaCodigo), It.IsAny<CancellationToken>()))
                .ReturnsAsync(turmaMock);

            _mediatorMock.Setup(m => m.Send(It.Is<ObterTurmaEmPeriodoDeFechamentoQuery>(q =>
                q.Turma == turmaMock && q.DataReferencia == dataReferencia && q.Bimestre == bimestre), It.IsAny<CancellationToken>()))
                .ReturnsAsync(false);

            var result = await _useCase.Executar(turmaCodigo, dataReferencia, bimestre);

            Assert.False(result);
            _mediatorMock.Verify(m => m.Send(It.Is<ObterTurmaPorCodigoQuery>(q => q.TurmaCodigo == turmaCodigo), It.IsAny<CancellationToken>()), Times.Once);
            _mediatorMock.Verify(m => m.Send(It.Is<ObterTurmaEmPeriodoDeFechamentoQuery>(q =>
                q.Turma == turmaMock && q.DataReferencia == dataReferencia && q.Bimestre == bimestre), It.IsAny<CancellationToken>()), Times.Once);
            _mediatorMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task Executar_DeveLancarExcecaoQuandoObterTurmaPorCodigoQueryFalha()
        {
            var turmaCodigo = "COD789";
            var dataReferencia = DateTime.Now;
            var bimestre = 3;

            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterTurmaPorCodigoQuery>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new Exception("Erro ao obter turma"));

            var ex = await Assert.ThrowsAsync<Exception>(() => _useCase.Executar(turmaCodigo, dataReferencia, bimestre));
            Assert.Contains("Erro ao obter turma", ex.Message);

            _mediatorMock.Verify(m => m.Send(It.Is<ObterTurmaPorCodigoQuery>(q => q.TurmaCodigo == turmaCodigo), It.IsAny<CancellationToken>()), Times.Once);
            _mediatorMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task Executar_DeveLancarExcecaoQuandoObterTurmaEmPeriodoDeFechamentoQueryFalha()
        {
            var turmaCodigo = "COD000";
            var dataReferencia = DateTime.Now;
            var bimestre = 4;

            var turmaMock = new Turma { CodigoTurma = turmaCodigo };
            _mediatorMock.Setup(m => m.Send(It.Is<ObterTurmaPorCodigoQuery>(q => q.TurmaCodigo == turmaCodigo), It.IsAny<CancellationToken>()))
                .ReturnsAsync(turmaMock);

            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterTurmaEmPeriodoDeFechamentoQuery>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new Exception("Erro ao verificar período de fechamento"));

            var ex = await Assert.ThrowsAsync<Exception>(() => _useCase.Executar(turmaCodigo, dataReferencia, bimestre));
            Assert.Contains("Erro ao verificar período de fechamento", ex.Message);

            _mediatorMock.Verify(m => m.Send(It.Is<ObterTurmaPorCodigoQuery>(q => q.TurmaCodigo == turmaCodigo), It.IsAny<CancellationToken>()), Times.Once);
            _mediatorMock.Verify(m => m.Send(It.Is<ObterTurmaEmPeriodoDeFechamentoQuery>(q =>
                q.Turma == turmaMock && q.DataReferencia == dataReferencia && q.Bimestre == bimestre), It.IsAny<CancellationToken>()), Times.Once);
            _mediatorMock.VerifyNoOtherCalls();
        }
    }
}
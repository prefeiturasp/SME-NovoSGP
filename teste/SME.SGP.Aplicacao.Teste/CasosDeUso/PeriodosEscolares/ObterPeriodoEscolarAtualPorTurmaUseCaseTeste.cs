using MediatR;
using Moq;
using SME.SGP.Dominio;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso.PeriodosEscolares
{
    public class ObterPeriodoEscolarAtualPorTurmaUseCaseTeste
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly ObterPeriodoEscolarAtualPorTurmaUseCase _useCase;

        public ObterPeriodoEscolarAtualPorTurmaUseCaseTeste()
        {
            _mediatorMock = new Mock<IMediator>();
            _useCase = new ObterPeriodoEscolarAtualPorTurmaUseCase(_mediatorMock.Object);
        }

        [Fact]
        public async Task Executar_DeveRetornarPeriodoDtoComSucesso()
        {
            // Arrange
            long turmaId = 123;
            DateTime dataReferencia = new DateTime(2025, 5, 10);

            var turmaMock = new Turma { Id = turmaId, CodigoTurma = "ABC001" };
            var periodoEscolarMock = new PeriodoEscolar
            {
                Bimestre = 2,
                PeriodoInicio = new DateTime(2025, 4, 1),
                PeriodoFim = new DateTime(2025, 6, 30)
            };

            _mediatorMock.Setup(m => m.Send(It.Is<ObterTurmaPorIdQuery>(q => q.TurmaId == turmaId), It.IsAny<CancellationToken>()))
                .ReturnsAsync(turmaMock);

            _mediatorMock.Setup(m => m.Send(It.Is<ObterPeriodoEscolarAtualPorTurmaQuery>(q =>
                q.Turma == turmaMock && q.DataReferencia == dataReferencia
            ), It.IsAny<CancellationToken>()))
            .ReturnsAsync(periodoEscolarMock);

            // Act
            var result = await _useCase.Executar(turmaId, dataReferencia);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(periodoEscolarMock.PeriodoInicio, result.Inicio);
            Assert.Equal(periodoEscolarMock.PeriodoFim, result.Fim);

            _mediatorMock.Verify(m => m.Send(It.Is<ObterTurmaPorIdQuery>(q => q.TurmaId == turmaId), It.IsAny<CancellationToken>()), Times.Once);
            _mediatorMock.Verify(m => m.Send(It.Is<ObterPeriodoEscolarAtualPorTurmaQuery>(q =>
                q.Turma == turmaMock && q.DataReferencia == dataReferencia
            ), It.IsAny<CancellationToken>()), Times.Once);
            _mediatorMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task Executar_DeveLancarExcecaoSeObterTurmaPorIdQueryFalhar()
        {
            // Arrange
            long turmaId = 789;
            DateTime dataReferencia = new DateTime(2025, 5, 10);

            _mediatorMock.Setup(m => m.Send(It.Is<ObterTurmaPorIdQuery>(q => q.TurmaId == turmaId), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new Exception("Erro ao buscar turma"));

            // Act
            var ex = await Record.ExceptionAsync(() => _useCase.Executar(turmaId, dataReferencia));

            // Assert
            Assert.NotNull(ex);
            Assert.IsType<Exception>(ex);
            Assert.Contains("Erro ao buscar turma", ex.Message);

            _mediatorMock.Verify(m => m.Send(It.Is<ObterTurmaPorIdQuery>(q => q.TurmaId == turmaId), It.IsAny<CancellationToken>()), Times.Once);
            _mediatorMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task Executar_DeveLancarExcecaoSeObterPeriodoEscolarAtualPorTurmaQueryFalhar()
        {
            // Arrange
            long turmaId = 101;
            DateTime dataReferencia = new DateTime(2025, 5, 10);

            var turmaMock = new Turma { Id = turmaId, CodigoTurma = "GHI003" };

            _mediatorMock.Setup(m => m.Send(It.Is<ObterTurmaPorIdQuery>(q => q.TurmaId == turmaId), It.IsAny<CancellationToken>()))
                .ReturnsAsync(turmaMock);

            _mediatorMock.Setup(m => m.Send(It.Is<ObterPeriodoEscolarAtualPorTurmaQuery>(q =>
                q.Turma == turmaMock && q.DataReferencia == dataReferencia
            ), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception("Erro ao buscar período escolar"));

            // Act
            var ex = await Record.ExceptionAsync(() => _useCase.Executar(turmaId, dataReferencia));

            // Assert
            Assert.NotNull(ex);
            Assert.IsType<Exception>(ex);
            Assert.Contains("Erro ao buscar período escolar", ex.Message);

            _mediatorMock.Verify(m => m.Send(It.Is<ObterTurmaPorIdQuery>(q => q.TurmaId == turmaId), It.IsAny<CancellationToken>()), Times.Once);
            _mediatorMock.Verify(m => m.Send(It.Is<ObterPeriodoEscolarAtualPorTurmaQuery>(q =>
                q.Turma == turmaMock && q.DataReferencia == dataReferencia
            ), It.IsAny<CancellationToken>()), Times.Once);
            _mediatorMock.VerifyNoOtherCalls();
        }
    }
}
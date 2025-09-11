using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Moq;
using Newtonsoft.Json;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso.Aula
{
    public class AlterarAulaFrequenciaTratarUseCaseTeste
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly AlterarAulaFrequenciaTratarUseCase _useCase;

        public AlterarAulaFrequenciaTratarUseCaseTeste()
        {
            _mediatorMock = new Mock<IMediator>();
            _useCase = new AlterarAulaFrequenciaTratarUseCase(_mediatorMock.Object);
        }

        [Fact]
        public async Task Executar_DeveExecutarComandos_QuandoAulaForEncontrada()
        {
            // Arrange
            var aulaId = 123;
            var quantidadeAnterior = 2;

            var dto = new AulaAlterarFrequenciaRequestDto(aulaId, quantidadeAnterior);
            var mensagem = new MensagemRabbit(JsonConvert.SerializeObject(dto));

            var aulaMock = new SME.SGP.Dominio.Aula
            {
                Id = aulaId,
                TurmaId = "TURMA1",
                DisciplinaId = "DISC1",
                DataAula = new DateTime(2025, 5, 10)
            };

            var turmaMock = new Turma
            {
                Id = 1,
                CodigoTurma = "TURMA1",
                ModalidadeCodigo = Modalidade.EducacaoInfantil,
                AnoLetivo = 2025
            };

            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterAulaPorIdQuery>(), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(aulaMock);

            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterTurmaPorCodigoQuery>(), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(turmaMock);

            _mediatorMock.Setup(m => m.Send(It.IsAny<AlterarAulaFrequenciaTratarCommand>(), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(true);

            _mediatorMock.Setup(m => m.Send(It.IsAny<RecalcularFrequenciaPorTurmaCommand>(), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(true);

            _mediatorMock.Setup(m => m.Send(It.IsAny<IncluirFilaConsolidacaoDiariaDashBoardFrequenciaCommand>(), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(true);

            _mediatorMock.Setup(m => m.Send(It.IsAny<IncluirFilaConsolidacaoSemanalMensalDashBoardFrequenciaCommand>(), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(true);

            // Act
            var resultado = await _useCase.Executar(mensagem);

            // Assert
            Assert.True(resultado);

            _mediatorMock.Verify(m => m.Send(It.Is<ObterAulaPorIdQuery>(q => q.AulaId == aulaId), It.IsAny<CancellationToken>()), Times.Once);
            _mediatorMock.Verify(m => m.Send(It.IsAny<ObterTurmaPorCodigoQuery>(), It.IsAny<CancellationToken>()), Times.Once);
            _mediatorMock.Verify(m => m.Send(It.IsAny<AlterarAulaFrequenciaTratarCommand>(), It.IsAny<CancellationToken>()), Times.Once);
            _mediatorMock.Verify(m => m.Send(It.IsAny<RecalcularFrequenciaPorTurmaCommand>(), It.IsAny<CancellationToken>()), Times.Once);
            _mediatorMock.Verify(m => m.Send(It.IsAny<IncluirFilaConsolidacaoDiariaDashBoardFrequenciaCommand>(), It.IsAny<CancellationToken>()), Times.Once);
            _mediatorMock.Verify(m => m.Send(It.IsAny<IncluirFilaConsolidacaoSemanalMensalDashBoardFrequenciaCommand>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Executar_DeveRetornarTrue_QuandoAulaNaoForEncontrada()
        {
            // Arrange
            var dto = new AulaAlterarFrequenciaRequestDto(999, 3);
            var mensagem = new MensagemRabbit(JsonConvert.SerializeObject(dto));

            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterAulaPorIdQuery>(), It.IsAny<CancellationToken>()))
                         .ReturnsAsync((SME.SGP.Dominio.Aula)null);

            // Act
            var resultado = await _useCase.Executar(mensagem);

            // Assert
            Assert.True(resultado);

            _mediatorMock.Verify(m => m.Send(It.Is<ObterAulaPorIdQuery>(q => q.AulaId == dto.AulaId), It.IsAny<CancellationToken>()), Times.Once);
            _mediatorMock.VerifyNoOtherCalls();
        }
    }
}

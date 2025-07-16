using FluentAssertions;
using MediatR;
using Moq;
using Newtonsoft.Json;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso.Frequencia
{
    public class ExcluirFrequenciaPorAulaIdUseCaseTeste
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly ExcluirFrequenciaPorAulaIdUseCase _useCase;

        public ExcluirFrequenciaPorAulaIdUseCaseTeste()
        {
            _mediatorMock = new Mock<IMediator>();
            _useCase = new ExcluirFrequenciaPorAulaIdUseCase(_mediatorMock.Object);
        }

        [Fact]
        public async Task Executar_DeveExcluirFrequenciaEConsolidarDashboards()
        {
            // Arrange
            long aulaId = 123;
            long turmaId = 456;
            string turmaCodigo = "TURMAABC";
            DateTime dataAula = new DateTime(2025, 7, 15);
            int anoLetivo = 2025;
            Modalidade modalidade = Modalidade.Fundamental;

            var filtroId = new FiltroIdDto(aulaId);
            var mensagemRabbit = new MensagemRabbit("ExcluirFrequencia", JsonConvert.SerializeObject(filtroId), Guid.NewGuid(), "RF12345");

            var aulaMock = new Dominio.Aula
            {
                DataAula = dataAula,
                Quantidade = 1,
                TurmaId = turmaId.ToString(),
                DisciplinaId = Guid.NewGuid().ToString(),
                TipoAula = TipoAula.Normal,
                TipoCalendarioId = 1
            };
            aulaMock.Id = aulaId;

            var turmaMock = new Turma
            {
                CodigoTurma = turmaCodigo,
                AnoLetivo = anoLetivo,
                ModalidadeCodigo = modalidade
            };
            turmaMock.Id = turmaId;

            _mediatorMock.Setup(m => m.Send(It.IsAny<ExcluirFrequenciaDaAulaCommand>(), It.IsAny<CancellationToken>())).ReturnsAsync(true);
            _mediatorMock.Setup(m => m.Send(It.Is<ObterAulaPorIdQuery>(q => q.AulaId == aulaId), It.IsAny<CancellationToken>())).ReturnsAsync(aulaMock);
            _mediatorMock.Setup(m => m.Send(It.Is<ObterTurmaPorCodigoQuery>(q => q.TurmaCodigo == aulaMock.TurmaId), It.IsAny<CancellationToken>())).ReturnsAsync(turmaMock);
            _mediatorMock.Setup(m => m.Send(It.IsAny<IncluirFilaConsolidacaoDiariaDashBoardFrequenciaCommand>(), It.IsAny<CancellationToken>())).ReturnsAsync(true);
            _mediatorMock.Setup(m => m.Send(It.IsAny<IncluirFilaConsolidacaoSemanalMensalDashBoardFrequenciaCommand>(), It.IsAny<CancellationToken>())).ReturnsAsync(true);

            // Act
            var result = await _useCase.Executar(mensagemRabbit);

            // Assert
            result.Should().BeTrue();

            _mediatorMock.Verify(m => m.Send(It.Is<ExcluirFrequenciaDaAulaCommand>(cmd => cmd.AulaId == aulaId), It.IsAny<CancellationToken>()), Times.Once);
            _mediatorMock.Verify(m => m.Send(It.Is<ObterAulaPorIdQuery>(query => query.AulaId == aulaId), It.IsAny<CancellationToken>()), Times.Once);
            _mediatorMock.Verify(m => m.Send(It.Is<ObterTurmaPorCodigoQuery>(query => query.TurmaCodigo == aulaMock.TurmaId), It.IsAny<CancellationToken>()), Times.Once);

            _mediatorMock.Verify(m => m.Send(It.Is<IncluirFilaConsolidacaoDiariaDashBoardFrequenciaCommand>(cmd =>
                cmd.TurmaId == turmaId &&
                cmd.DataAula == dataAula
            ), It.IsAny<CancellationToken>()), Times.Once);

            _mediatorMock.Verify(m => m.Send(It.Is<IncluirFilaConsolidacaoSemanalMensalDashBoardFrequenciaCommand>(cmd =>
                cmd.TurmaId == turmaId &&
                cmd.CodigoTurma == turmaCodigo &&
                cmd.EhModalidadeInfantil == (modalidade == Modalidade.EducacaoInfantil) &&
                cmd.AnoLetivo == anoLetivo &&
                cmd.DataAula == dataAula
            ), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Executar_DeveLancarExcecao_QuandoFiltroIdInvalido()
        {
            // Arrange
            var mensagemRabbit = new MensagemRabbit("ExcluirFrequencia", null, Guid.NewGuid(), "RF12345");

            // Act
            Func<Task> act = async () => await _useCase.Executar(mensagemRabbit);

            // Assert
            await act.Should().ThrowAsync<NullReferenceException>();
        }

        [Fact]
        public async Task Executar_DeveLancarExcecao_QuandoObterAulaFalhar()
        {
            // Arrange
            long aulaId = 123;
            var filtroId = new FiltroIdDto(aulaId);
            var mensagemRabbit = new MensagemRabbit("ExcluirFrequencia", JsonConvert.SerializeObject(filtroId), Guid.NewGuid(), "RF12345");

            _mediatorMock.Setup(m => m.Send(It.IsAny<ExcluirFrequenciaDaAulaCommand>(), It.IsAny<CancellationToken>())).ReturnsAsync(true);
            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterAulaPorIdQuery>(), It.IsAny<CancellationToken>())).ThrowsAsync(new Exception("Erro ao obter aula"));

            // Act
            Func<Task> act = async () => await _useCase.Executar(mensagemRabbit);

            // Assert
            await act.Should().ThrowAsync<Exception>().WithMessage("Erro ao obter aula");

            _mediatorMock.Verify(m => m.Send(It.IsAny<ExcluirFrequenciaDaAulaCommand>(), It.IsAny<CancellationToken>()), Times.Once);
            _mediatorMock.Verify(m => m.Send(It.IsAny<ObterAulaPorIdQuery>(), It.IsAny<CancellationToken>()), Times.Once);
            _mediatorMock.Verify(m => m.Send(It.IsAny<ObterTurmaPorCodigoQuery>(), It.IsAny<CancellationToken>()), Times.Never);
            _mediatorMock.Verify(m => m.Send(It.IsAny<IncluirFilaConsolidacaoDiariaDashBoardFrequenciaCommand>(), It.IsAny<CancellationToken>()), Times.Never);
            _mediatorMock.Verify(m => m.Send(It.IsAny<IncluirFilaConsolidacaoSemanalMensalDashBoardFrequenciaCommand>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task Executar_DeveLancarExcecao_QuandoObterTurmaFalhar()
        {
            // Arrange
            long aulaId = 123;
            long turmaId = 456;
            DateTime dataAula = new DateTime(2025, 7, 15);
            var filtroId = new FiltroIdDto(aulaId);
            var mensagemRabbit = new MensagemRabbit("ExcluirFrequencia", JsonConvert.SerializeObject(filtroId), Guid.NewGuid(), "RF12345");

            var aulaMock = new Dominio.Aula
            {
                DataAula = dataAula,
                Quantidade = 1,
                TurmaId = turmaId.ToString(),
                DisciplinaId = Guid.NewGuid().ToString(),
                TipoAula = TipoAula.Normal,
                TipoCalendarioId = 1
            };
            aulaMock.Id = aulaId;

            _mediatorMock.Setup(m => m.Send(It.IsAny<ExcluirFrequenciaDaAulaCommand>(), It.IsAny<CancellationToken>())).ReturnsAsync(true);
            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterAulaPorIdQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync(aulaMock);
            _mediatorMock.Setup(m => m.Send(It.Is<ObterTurmaPorCodigoQuery>(q => q.TurmaCodigo == aulaMock.TurmaId), It.IsAny<CancellationToken>())).ThrowsAsync(new Exception("Erro ao obter turma"));

            // Act
            Func<Task> act = async () => await _useCase.Executar(mensagemRabbit);

            // Assert
            await act.Should().ThrowAsync<Exception>().WithMessage("Erro ao obter turma");

            _mediatorMock.Verify(m => m.Send(It.IsAny<ExcluirFrequenciaDaAulaCommand>(), It.IsAny<CancellationToken>()), Times.Once);
            _mediatorMock.Verify(m => m.Send(It.IsAny<ObterAulaPorIdQuery>(), It.IsAny<CancellationToken>()), Times.Once);
            _mediatorMock.Verify(m => m.Send(It.IsAny<ObterTurmaPorCodigoQuery>(), It.IsAny<CancellationToken>()), Times.Once);
            _mediatorMock.Verify(m => m.Send(It.IsAny<IncluirFilaConsolidacaoDiariaDashBoardFrequenciaCommand>(), It.IsAny<CancellationToken>()), Times.Never);
            _mediatorMock.Verify(m => m.Send(It.IsAny<IncluirFilaConsolidacaoSemanalMensalDashBoardFrequenciaCommand>(), It.IsAny<CancellationToken>()), Times.Never);
        }
    }
}
using Bogus;
using MediatR;
using Moq;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.Queries.Frequencia
{
    public class ObterConsultaFrequenciaGeralAlunoQueryHandlerTeste
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly ObterConsultaFrequenciaGeralAlunoQueryHandler _handler;
        private readonly Faker _faker;

        public ObterConsultaFrequenciaGeralAlunoQueryHandlerTeste()
        {
            _mediatorMock = new Mock<IMediator>();
            _handler = new ObterConsultaFrequenciaGeralAlunoQueryHandler(_mediatorMock.Object);
            _faker = new Faker("pt_BR");
        }

        [Fact(DisplayName = "Deve lançar exceção quando o mediator for nulo")]
        public void DeveLancarExcecao_QuandoMediatorNulo()
        {
            // Arrange & Act & Assert
            Assert.Throws<ArgumentNullException>(() => new ObterConsultaFrequenciaGeralAlunoQueryHandler(null));
        }

        [Fact(DisplayName = "Deve incluir apenas a turma da query quando não for regular ou de itinerário")]
        public async Task Handle_QuandoTurmaNaoRegularOuItinerario_DeveConsultarApenasTurmaAtual()
        {
            // Arrange
            var query = new ObterConsultaFrequenciaGeralAlunoQuery("ALUNO1", "TURMA_PROGRAMA");
            var turma = new Turma { CodigoTurma = query.TurmaCodigo, TipoTurma = TipoTurma.Programa };
            var retornoFrequencia = "95,00%";

            _mediatorMock.Setup(m => m.Send(It.Is<ObterTurmaComUeEDrePorCodigoQuery>(q => q.TurmaCodigo == query.TurmaCodigo), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(turma);
            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterTurmaItinerarioEnsinoMedioQuery>(), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(new List<TurmaItinerarioEnsinoMedioDto>());
            _mediatorMock.Setup(m => m.Send(It.Is<ObterAlunoPorTurmaAlunoCodigoQuery>(q => q.TurmaCodigo == query.TurmaCodigo), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(new AlunoPorTurmaResposta { CodigoSituacaoMatricula = SituacaoMatriculaAluno.Ativo });
            _mediatorMock.Setup(m => m.Send(It.Is<ObterConsultaFrequenciaGeralAlunoPorTurmasQuery>(q => q.TurmaCodigo.Length == 1 && q.TurmaCodigo[0] == query.TurmaCodigo), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(retornoFrequencia);

            // Act
            var resultado = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.Equal(retornoFrequencia, resultado);
            _mediatorMock.Verify(m => m.Send(It.IsAny<ObterTurmaCodigosAlunoPorAnoLetivoAlunoTipoTurmaQuery>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact(DisplayName = "Deve buscar outras turmas regulares quando a turma principal for regular e aluno estiver ativo")]
        public async Task Handle_QuandoTurmaRegularEAlunoAtivo_DeveBuscarOutrasTurmas()
        {
            // Arrange
            var query = new ObterConsultaFrequenciaGeralAlunoQuery("ALUNO1", "TURMA_REGULAR");
            var turma = new Turma { CodigoTurma = query.TurmaCodigo, TipoTurma = TipoTurma.Regular };
            var turmasDoAluno = new[] { "TURMA_ED_FISICA", "TURMA_ITINERARIO" };
            var retornoFrequencia = "98,00%";

            _mediatorMock.Setup(m => m.Send(It.Is<ObterTurmaComUeEDrePorCodigoQuery>(q => q.TurmaCodigo == query.TurmaCodigo), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(turma);
            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterTurmaItinerarioEnsinoMedioQuery>(), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(new List<TurmaItinerarioEnsinoMedioDto> { new TurmaItinerarioEnsinoMedioDto { Id = (int)TipoTurma.ItinerarioEnsMedio } });
            _mediatorMock.Setup(m => m.Send(It.Is<ObterAlunoPorTurmaAlunoCodigoQuery>(q => q.TurmaCodigo == query.TurmaCodigo), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(new AlunoPorTurmaResposta { CodigoSituacaoMatricula = SituacaoMatriculaAluno.Ativo });
            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterTurmaCodigosAlunoPorAnoLetivoAlunoTipoTurmaQuery>(), It.IsAny<CancellationToken>()))
                        .ReturnsAsync(turmasDoAluno);
            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterTurmasPorCodigosQuery>(), It.IsAny<CancellationToken>()))
                        .ReturnsAsync(new List<Turma>());
            _mediatorMock.Setup(m => m.Send(It.Is<ObterConsultaFrequenciaGeralAlunoPorTurmasQuery>(q => q.TurmaCodigo.Length == turmasDoAluno.Length), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(retornoFrequencia);

            // Act
            var resultado = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.Equal(retornoFrequencia, resultado);
            _mediatorMock.Verify(m => m.Send(It.IsAny<ObterTurmaCodigosAlunoPorAnoLetivoAlunoTipoTurmaQuery>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact(DisplayName = "Deve incluir turma principal na busca quando aluno estiver inativo na turma regular")]
        public async Task Handle_QuandoTurmaRegularEAlunoInativo_DeveIncluirTurmaPrincipalNaBusca()
        {
            // Arrange
            var query = new ObterConsultaFrequenciaGeralAlunoQuery("ALUNO1", "TURMA_REGULAR");
            var turma = new Turma { CodigoTurma = query.TurmaCodigo, TipoTurma = TipoTurma.Regular };
            var retornoFrequencia = "75,00%";

            _mediatorMock.Setup(m => m.Send(It.Is<ObterTurmaComUeEDrePorCodigoQuery>(q => q.TurmaCodigo == query.TurmaCodigo), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(turma);
            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterTurmaItinerarioEnsinoMedioQuery>(), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(new List<TurmaItinerarioEnsinoMedioDto>());
            // Aluno inativo na turma da query
            _mediatorMock.Setup(m => m.Send(It.Is<ObterAlunoPorTurmaAlunoCodigoQuery>(q => q.TurmaCodigo == query.TurmaCodigo), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(new AlunoPorTurmaResposta { CodigoSituacaoMatricula = SituacaoMatriculaAluno.Transferido });
            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterTurmaCodigosAlunoPorAnoLetivoAlunoTipoTurmaQuery>(), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(new string[0]); // Sem outras turmas
            // A verificação deve incluir a turma da query
            _mediatorMock.Setup(m => m.Send(It.Is<ObterConsultaFrequenciaGeralAlunoPorTurmasQuery>(q => q.TurmaCodigo.Contains(query.TurmaCodigo)), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(retornoFrequencia);

            // Act
            var resultado = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.Equal(retornoFrequencia, resultado);
            _mediatorMock.Verify(m => m.Send(It.Is<ObterConsultaFrequenciaGeralAlunoPorTurmasQuery>(q => q.TurmaCodigo.Length == 1 && q.TurmaCodigo[0] == query.TurmaCodigo), It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}

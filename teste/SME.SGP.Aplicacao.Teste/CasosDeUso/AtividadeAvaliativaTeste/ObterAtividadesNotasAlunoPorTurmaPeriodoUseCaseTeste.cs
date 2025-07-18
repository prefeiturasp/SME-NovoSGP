using MediatR;
using Moq;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Constantes.MensagensNegocio;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso.AtividadeAvaliativaTeste
{
    public class ObterAtividadesNotasAlunoPorTurmaPeriodoUseCaseTests
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly ObterAtividadesNotasAlunoPorTurmaPeriodoUseCase _useCase;

        public ObterAtividadesNotasAlunoPorTurmaPeriodoUseCaseTests()
        {
            _mediatorMock = new Mock<IMediator>();
            _useCase = new ObterAtividadesNotasAlunoPorTurmaPeriodoUseCase(_mediatorMock.Object);
        }

        [Fact]
        public async Task Executar_ShouldReturnEvaluations_WhenAllDependenciesAreMet()
        {
            var param = new FiltroTurmaAlunoPeriodoEscolarDto(1, 1, "aluno123", "456");
            var atividadesNotas = new List<AvaliacaoNotaAlunoDto>
        {
            new AvaliacaoNotaAlunoDto { Id = 101, Data = DateTime.Today.AddDays(-5), Regencia = false },
            new AvaliacaoNotaAlunoDto { Id = 102, Data = DateTime.Today.AddDays(-2), Regencia = true }
        };
            var turma = new Turma { Id = 1, CodigoTurma = "codigoTurma123", Nome = "Nome Turma", TipoTurma = TipoTurma.Regular, AnoLetivo = DateTime.Now.Year };
            var componentesCurriculares = new List<DisciplinaDto>
        {
            new DisciplinaDto { CodigoComponenteCurricular = 456, Regencia = true, LancaNota = true, RegistraFrequencia = true, Nome = "Componente Teste" }
        };
            var disciplinasAtividade102 = new List<AtividadeAvaliativaDisciplina>
        {
            new AtividadeAvaliativaDisciplina(102, "789")
        };
            var disciplinasPorId = new List<DisciplinaDto>
        {
            new DisciplinaDto { Id = 789, Nome = "Disciplina Regencia Teste" }
        };
            var turmaComUeEDre = new Turma { Id = 1, CodigoTurma = "codigoTurma123", Nome = "Nome Turma", TipoTurma = TipoTurma.Regular, AnoLetivo = DateTime.Now.Year };
            var ausencias = new List<AusenciaAlunoDto>
        {
            new AusenciaAlunoDto { AlunoCodigo = "aluno123", AulaData = DateTime.Today.AddDays(-5) }
        };

            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterAtividadesNotasAlunoPorTurmaPeriodoQuery>(), default))
                .ReturnsAsync(atividadesNotas);
            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterTurmaPorIdQuery>(), default))
                .ReturnsAsync(turma);
            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterComponentesCurricularesPorIdsUsuarioLogadoQuery>(), default))
                .ReturnsAsync(componentesCurriculares);
            _mediatorMock.Setup(m => m.Send(It.Is<ObterDisciplinasAtividadeAvaliativaQuery>(q => q.Avaliacao_id == 102), default))
                .ReturnsAsync(disciplinasAtividade102);
            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterComponentesCurricularesPorIdsQuery>(), default))
                .ReturnsAsync(disciplinasPorId);
            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterTurmaComUeEDrePorIdQuery>(), default))
                .ReturnsAsync(turmaComUeEDre);
            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterAusenciasDaAtividadesAvaliativasPorAlunoQuery>(), default))
                .ReturnsAsync(ausencias);

            var result = await _useCase.Executar(param);

            _mediatorMock.Verify(m => m.Send(It.Is<ObterAtividadesNotasAlunoPorTurmaPeriodoQuery>(q =>
                q.TurmaId == param.TurmaId &&
                q.PeriodoEscolarId == param.PeriodoEscolarId &&
                q.AlunoCodigo == param.AlunoCodigo &&
                q.ComponenteCurricular == param.ComponenteCurricular), default), Times.Once);

            _mediatorMock.Verify(m => m.Send(It.Is<ObterTurmaPorIdQuery>(q => q.TurmaId == param.TurmaId), default), Times.Exactly(2));
            _mediatorMock.Verify(m => m.Send(It.Is<ObterComponentesCurricularesPorIdsUsuarioLogadoQuery>(q =>
                q.Ids.Contains(long.Parse(param.ComponenteCurricular)) &&
                q.CodigoTurma == turma.CodigoTurma), default), Times.Once);
            _mediatorMock.Verify(m => m.Send(It.Is<ObterTurmaComUeEDrePorIdQuery>(q => q.TurmaId == param.TurmaId), default), Times.Once);
            _mediatorMock.Verify(m => m.Send(It.Is<ObterDisciplinasAtividadeAvaliativaQuery>(q => q.Avaliacao_id == 102 && q.EhRegencia == true), default), Times.Once);
            _mediatorMock.Verify(m => m.Send(It.Is<ObterComponentesCurricularesPorIdsQuery>(q => q.Ids.Contains(789)), default), Times.Once);
            _mediatorMock.Verify(m => m.Send(It.Is<ObterAusenciasDaAtividadesAvaliativasPorAlunoQuery>(q =>
                q.TurmaCodigo == turma.CodigoTurma &&
                q.AtividadesAvaliativasData.Length == atividadesNotas.Count &&
                q.ComponenteCurricularCodigo == param.ComponenteCurricular &&
                q.CodigoAluno == param.AlunoCodigo), default), Times.Once);

            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
            Assert.True(result.First().Ausente);
            Assert.Contains("Disciplina Regencia Teste", result.Last().Disciplinas);
        }

        [Fact]
        public async Task Executar_ShouldThrowNegocioException_WhenComponenteCurricularNotFound()
        {
            var param = new FiltroTurmaAlunoPeriodoEscolarDto(1, 1, "aluno123", "456");
            var atividadesNotas = new List<AvaliacaoNotaAlunoDto>();
            var turma = new Turma { Id = 1, CodigoTurma = "codigoTurma123", Nome = "Nome Turma", TipoTurma = TipoTurma.Regular, AnoLetivo = DateTime.Now.Year };
            var componentesCurriculares = new List<DisciplinaDto>();

            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterAtividadesNotasAlunoPorTurmaPeriodoQuery>(), default))
                .ReturnsAsync(atividadesNotas);
            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterTurmaPorIdQuery>(), default))
                .ReturnsAsync(turma);
            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterComponentesCurricularesPorIdsUsuarioLogadoQuery>(), default))
                .ReturnsAsync(componentesCurriculares);

            var exception = await Assert.ThrowsAsync<NegocioException>(() => _useCase.Executar(param));

            Assert.Equal(MensagemNegocioEOL.COMPONENTE_CURRICULAR_NAO_LOCALIZADO_INFORMACOES_EOL, exception.Message);
        }

        [Fact]
        public async Task Executar_ShouldThrowNegocioException_WhenTurmaComUeEDreNotFoundAndRegenciaIsTrue()
        {
            var param = new FiltroTurmaAlunoPeriodoEscolarDto(1, 1, "aluno123", "456");
            var atividadesNotas = new List<AvaliacaoNotaAlunoDto>
        {
            new AvaliacaoNotaAlunoDto { Id = 101, Data = DateTime.Today, Regencia = true }
        };
            var turma = new Turma { Id = 1, CodigoTurma = "codigoTurma123", Nome = "Nome Turma", TipoTurma = TipoTurma.Regular, AnoLetivo = DateTime.Now.Year };
            var componentesCurriculares = new List<DisciplinaDto>
        {
            new DisciplinaDto { CodigoComponenteCurricular = 456, Regencia = true }
        };
            Turma turmaComUeEDre = null;

            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterAtividadesNotasAlunoPorTurmaPeriodoQuery>(), default))
                .ReturnsAsync(atividadesNotas);
            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterTurmaPorIdQuery>(), default))
                .ReturnsAsync(turma);
            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterComponentesCurricularesPorIdsUsuarioLogadoQuery>(), default))
                .ReturnsAsync(componentesCurriculares);
            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterTurmaComUeEDrePorIdQuery>(), default))
                .ReturnsAsync(turmaComUeEDre);

            var exception = await Assert.ThrowsAsync<NegocioException>(() => _useCase.Executar(param));

            Assert.Equal(MensagemNegocioTurma.NAO_FOI_POSSIVEL_OBTER_TURMA, exception.Message);
        }

        [Fact]
        public async Task Executar_ShouldHandleNoDisciplinasForRegenciaCorrectly()
        {
            var param = new FiltroTurmaAlunoPeriodoEscolarDto(1, 1, "aluno123", "456");
            var atividadesNotas = new List<AvaliacaoNotaAlunoDto>
        {
            new AvaliacaoNotaAlunoDto { Id = 101, Data = DateTime.Today, Regencia = true }
        };
            var turma = new Turma { Id = 1, CodigoTurma = "codigoTurma123", Nome = "Nome Turma", TipoTurma = TipoTurma.Regular, AnoLetivo = DateTime.Now.Year };
            var componentesCurriculares = new List<DisciplinaDto>
        {
            new DisciplinaDto { CodigoComponenteCurricular = 456, Regencia = true }
        };
            var turmaComUeEDre = new Turma { Id = 1, CodigoTurma = "codigoTurma123", Nome = "Nome Turma", TipoTurma = TipoTurma.Regular, AnoLetivo = DateTime.Now.Year };
            var ausencias = new List<AusenciaAlunoDto>();

            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterAtividadesNotasAlunoPorTurmaPeriodoQuery>(), default))
                .ReturnsAsync(atividadesNotas);
            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterTurmaPorIdQuery>(), default))
                .ReturnsAsync(turma);
            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterComponentesCurricularesPorIdsUsuarioLogadoQuery>(), default))
                .ReturnsAsync(componentesCurriculares);
            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterTurmaComUeEDrePorIdQuery>(), default))
                .ReturnsAsync(turmaComUeEDre);
            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterDisciplinasAtividadeAvaliativaQuery>(), default))
                .ReturnsAsync(new List<AtividadeAvaliativaDisciplina>()); // No disciplines for activity
            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterComponentesCurricularesPorProfessorETurmaParaPlanejamentoQuery>(), default))
                .ReturnsAsync(new List<DisciplinaDto> { new DisciplinaDto { Nome = "Disciplina Planejamento" } });
            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterAusenciasDaAtividadesAvaliativasPorAlunoQuery>(), default))
                .ReturnsAsync(ausencias);

            var result = await _useCase.Executar(param);

            Assert.NotNull(result);
            Assert.Single(result);
            Assert.Contains("Disciplina Planejamento", result.First().Disciplinas);
        }
    }
}
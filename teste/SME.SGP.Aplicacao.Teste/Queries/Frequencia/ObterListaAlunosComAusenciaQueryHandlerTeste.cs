using Bogus;
using MediatR;
using Moq;
using SME.SGP.Dominio.Interfaces;
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
    public class ObterListaAlunosComAusenciaQueryHandlerTeste
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly Mock<IRepositorioFrequenciaAlunoDisciplinaPeriodoConsulta> _repositorioFrequenciaMock;
        private readonly ObterListaAlunosComAusenciaQueryHandler _handler;
        private readonly Faker _faker;

        public ObterListaAlunosComAusenciaQueryHandlerTeste()
        {
            _mediatorMock = new Mock<IMediator>();
            _repositorioFrequenciaMock = new Mock<IRepositorioFrequenciaAlunoDisciplinaPeriodoConsulta>();
            _handler = new ObterListaAlunosComAusenciaQueryHandler(_mediatorMock.Object, _repositorioFrequenciaMock.Object);
            _faker = new Faker("pt_BR");
        }

        [Fact(DisplayName = "Deve lançar exceção quando as dependências forem nulas")]
        public void DeveLancarExcecao_QuandoDependenciasForemNulas()
        {
            // Arrange & Act & Assert
            Assert.Throws<ArgumentNullException>(() => new ObterListaAlunosComAusenciaQueryHandler(null, _repositorioFrequenciaMock.Object));
            Assert.Throws<ArgumentNullException>(() => new ObterListaAlunosComAusenciaQueryHandler(_mediatorMock.Object, null));
        }

        [Fact(DisplayName = "Deve retornar a lista de alunos ausentes quando encontrados")]
        public async Task DeveRetornarListaDeAlunosAusentes_QuandoEncontrados()
        {
            // Arrange
            var query = new ObterListaAlunosComAusenciaQuery("1", "123", 1);
            var alunoCodigo = "987";

            var turma = new Turma { CodigoTurma = query.TurmaId, AnoLetivo = 2025, ModalidadeCodigo = Modalidade.Fundamental };
            var periodo = new PeriodoEscolar { Id = 1, Bimestre = 1, PeriodoInicio = DateTime.Now, PeriodoFim = DateTime.Now.AddMonths(2) };
            var alunosAtivos = new List<AlunoPorTurmaResposta> { new AlunoPorTurmaResposta { CodigoAluno = alunoCodigo, NomeAluno = "Aluno Teste" } };
            var componentes = new List<DisciplinaDto> { new DisciplinaDto { Id = long.Parse(query.DisciplinaId), Regencia = true } };
            var frequenciaAluno = CriarFrequenciaAluno(totalAulas: 20, totalAusencias: 5, totalCompensacoes: 1); // 4 faltas não compensadas

            ConfigurarMocks(turma, periodo, alunosAtivos, componentes);

            _repositorioFrequenciaMock.Setup(r => r.ObterPorAlunoDisciplinaPeriodo(alunoCodigo, It.IsAny<string[]>(), periodo.Id, turma.CodigoTurma, It.IsAny<string>()))
                .Returns(frequenciaAluno);

            // Act
            var resultado = await _handler.Handle(query, CancellationToken.None);

            // Assert
            var alunoAusente = Assert.Single(resultado);
            Assert.Equal(alunoCodigo, alunoAusente.Id);
            Assert.Equal(4, alunoAusente.QuantidadeFaltasTotais);
            Assert.Equal(80, alunoAusente.PercentualFrequencia);
            _mediatorMock.Verify(m => m.Send(It.IsAny<ObterTurmaPorCodigoQuery>(), It.IsAny<CancellationToken>()), Times.Once);
            _repositorioFrequenciaMock.Verify(r => r.ObterPorAlunoDisciplinaPeriodo(It.IsAny<string>(), It.IsAny<string[]>(), It.IsAny<long>(), It.IsAny<string>(), It.IsAny<string>()), Times.Once);
        }

        [Fact(DisplayName = "Deve retornar lista vazia quando aluno não possui faltas não compensadas")]
        public async Task DeveRetornarListaVazia_QuandoAlunoNaoPossuiFaltas()
        {
            // Arrange
            var query = new ObterListaAlunosComAusenciaQuery("1", "123", 1);
            var alunoCodigo = "987";

            var turma = new Turma { CodigoTurma = query.TurmaId, AnoLetivo = 2025, ModalidadeCodigo = Modalidade.Fundamental };
            var periodo = new PeriodoEscolar { Id = 1, Bimestre = 1, PeriodoInicio = DateTime.Now, PeriodoFim = DateTime.Now.AddMonths(2) };
            var alunosAtivos = new List<AlunoPorTurmaResposta> { new AlunoPorTurmaResposta { CodigoAluno = alunoCodigo, NomeAluno = "Aluno Teste" } };
            var componentes = new List<DisciplinaDto> { new DisciplinaDto { Id = long.Parse(query.DisciplinaId), Regencia = true } };
            var frequenciaAluno = CriarFrequenciaAluno(totalAulas: 20, totalAusencias: 5, totalCompensacoes: 5); // 0 faltas não compensadas

            ConfigurarMocks(turma, periodo, alunosAtivos, componentes);
            _repositorioFrequenciaMock.Setup(r => r.ObterPorAlunoDisciplinaPeriodo(alunoCodigo, It.IsAny<string[]>(), periodo.Id, turma.CodigoTurma, It.IsAny<string>()))
                .Returns(frequenciaAluno);

            // Act
            var resultado = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(resultado);
            Assert.Empty(resultado);
        }

        [Fact(DisplayName = "Deve lançar exceção de negócio quando não encontrar alunos ativos")]
        public async Task DeveLancarExcecao_QuandoNaoEncontrarAlunosAtivos()
        {
            // Arrange
            var query = new ObterListaAlunosComAusenciaQuery("1", "123", 1);

            var turma = new Turma { CodigoTurma = query.TurmaId, AnoLetivo = 2025, ModalidadeCodigo = Modalidade.Fundamental };
            var periodo = new PeriodoEscolar { Id = 1, Bimestre = 1, PeriodoInicio = DateTime.Now, PeriodoFim = DateTime.Now.AddMonths(2) };

            ConfigurarMocks(turma, periodo, new List<AlunoPorTurmaResposta>(), new List<DisciplinaDto>());

            // Act & Assert
            var exception = await Assert.ThrowsAsync<NegocioException>(() => _handler.Handle(query, CancellationToken.None));
            Assert.Equal("Não foram localizados alunos com matrícula ativa na turma, no período escolar selecionado.", exception.Message);
        }

        private void ConfigurarMocks(Turma turma, PeriodoEscolar periodo, List<AlunoPorTurmaResposta> alunosAtivos, List<DisciplinaDto> componentes)
        {
            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterTurmaPorCodigoQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync(turma);
            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterTipoCalendarioPorAnoLetivoEModalidadeQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync(new TipoCalendario());
            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterPeridosEscolaresPorTipoCalendarioIdQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync(new List<PeriodoEscolar> { periodo });
            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterAlunosDentroPeriodoQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync(alunosAtivos);
            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterComponentesCurricularesPorIdsQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync(componentes);
            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterValorParametroSistemaTipoEAnoQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync("10");
            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterAlunosAtivosTurmaProgramaPapEolQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync(new List<AlunosTurmaProgramaPapDto>());
        }

        private FrequenciaAluno CriarFrequenciaAluno(int totalAulas, int totalAusencias, int totalCompensacoes) => new FrequenciaAluno
        {
            TotalAulas = totalAulas,
            TotalAusencias = totalAusencias,
            TotalCompensacoes = totalCompensacoes
        };
    }
}
using Bogus;
using MediatR;
using Moq;
using SME.SGP.Aplicacao.Queries;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.Queries.Nota.VerificaNotasTodosComponentesCurriculares
{
    public class VerificaNotasTodosComponentesCurricularesQueryHandlerTeste
    {
        private readonly Mock<IMediator> mediator;
        private readonly VerificaNotasTodosComponentesCurricularesQueryHandler verificaNotasTodosComponentesCurricularesQueryHandler;
        private readonly Faker faker;

        public VerificaNotasTodosComponentesCurricularesQueryHandlerTeste()
        {
            this.mediator = new Mock<IMediator>();
            this.verificaNotasTodosComponentesCurricularesQueryHandler = new VerificaNotasTodosComponentesCurricularesQueryHandler(mediator.Object);
            this.faker = new Faker("pt_BR");
        }

        [Fact(DisplayName = "Verifica se há nota somente para turmas ativas no período")]
        public async Task Verifica_Se_Ha_Nota_Somente_Para_Turmas_Ativas_No_Periodo()
        {
            // Arrange
            var alunoCodigo = faker.Random.AlphaNumeric(10);
            var turmaAtivaCodigo = "TURMA-ATIVA";
            var turmaInativaCodigo = "TURMA-INATIVA";
            var bimestre = 2;

            var turmaPrincipal = new Faker<Turma>("pt_BR")
                .RuleFor(t => t.CodigoTurma, turmaAtivaCodigo)
                .RuleFor(t => t.ModalidadeCodigo, Modalidade.EJA)
                .RuleFor(t => t.AnoLetivo, 2023)
                .RuleFor(t => t.Semestre, 1)
                .RuleFor(t => t.TipoTurma, Dominio.Enumerados.TipoTurma.Regular)
                .Generate();

            var periodoEscolar = new Faker<PeriodoEscolar>("pt_BR")
                .RuleFor(p => p.Id, f => f.Random.Long(1, 100))
                .RuleFor(p => p.Bimestre, bimestre)
                .RuleFor(p => p.PeriodoInicio, new DateTime(2023, 04, 01))
                .RuleFor(p => p.PeriodoFim, new DateTime(2023, 06, 30))
                .Generate();

            var query = new VerificaNotasTodosComponentesCurricularesQuery(alunoCodigo, turmaPrincipal, bimestre, false, periodoEscolar);

            // Mocks Essenciais
            ConfigurarMockTipoCalendario(turmaPrincipal, periodoEscolar);
            ConfigurarMockTurmasDoAluno(new[] { turmaAtivaCodigo, turmaInativaCodigo });
            ConfigurarMockTurmasAtivasNoPeriodo(new[] { turmaAtivaCodigo });

            var componenteTurmaAtiva = ConfigurarComponente("Componente Ativo", turmaAtivaCodigo, true);
            var componenteTurmaInativa = ConfigurarComponente("Componente Inativo", turmaInativaCodigo, true);
            ConfigurarMockComponentesDasTurmas(new List<DisciplinaDto> { componenteTurmaAtiva, componenteTurmaInativa });

            var notaComponenteAtivo = ConfigurarNota(alunoCodigo, componenteTurmaAtiva.CodigoComponenteCurricular, bimestre, 8);
            ConfigurarMockNotas(new List<NotaConceitoBimestreComponenteDto> { notaComponenteAtivo });

            var usuario = new Faker<Usuario>()
                .RuleFor(u => u.Login, f => f.Internet.UserName())
                .RuleFor(u => u.PerfilAtual, Perfis.PERFIL_PROFESSOR)
                .Generate();

            mediator.Setup(m => m.Send(ObterUsuarioLogadoQuery.Instance, It.IsAny<CancellationToken>()))
                    .ReturnsAsync(usuario);

            mediator.Setup(m => m.Send(It.IsAny<ObterConselhoClasseIdsPorTurmaEBimestreQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync(new long[] { 1 });
            mediator.Setup(m => m.Send(It.IsAny<ObterTurmaItinerarioEnsinoMedioQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync(new List<TurmaItinerarioEnsinoMedioDto>());
            mediator.Setup(m => m.Send(It.IsAny<ObterComponentesCurricularesPorIdsUsuarioLogadoQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync(new List<DisciplinaDto> { componenteTurmaAtiva });

            // Act
            var resultado = await verificaNotasTodosComponentesCurricularesQueryHandler.Handle(query, CancellationToken.None);

            // Assert
            Assert.True(resultado);
        }

        #region Métodos de Configuração de Mocks
        private void ConfigurarMockTipoCalendario(Turma turma, PeriodoEscolar periodoEscolar)
        {
            var tipoCalendario = new Faker<TipoCalendario>()
                .RuleFor(c => c.Id, f => f.Random.Long(1, 100))
                .RuleFor(c => c.AnoLetivo, turma.AnoLetivo)
                .RuleFor(c => c.Modalidade, turma.ModalidadeTipoCalendario)
                .Generate();
            periodoEscolar.TipoCalendarioId = tipoCalendario.Id;

            mediator.Setup(m => m.Send(It.Is<ObterTipoCalendarioPorAnoLetivoEModalidadeQuery>(q => q.AnoLetivo == turma.AnoLetivo && q.Modalidade == turma.ModalidadeTipoCalendario), It.IsAny<CancellationToken>()))
                    .ReturnsAsync(tipoCalendario);

            mediator.Setup(m => m.Send(It.Is<ObterPeriodosEscolaresPorTipoCalendarioQuery>(q => q.TipoCalendarioId == tipoCalendario.Id), It.IsAny<CancellationToken>()))
                    .ReturnsAsync(new List<PeriodoEscolar> { periodoEscolar });
        }

        private void ConfigurarMockTurmasDoAluno(string[] turmasCodigos)
        {
            mediator.Setup(a => a.Send(It.IsAny<ObterTurmaCodigosAlunoPorAnoLetivoAlunoTipoTurmaQuery>(), It.IsAny<CancellationToken>()))
                    .ReturnsAsync(turmasCodigos);
        }

        private void ConfigurarMockTurmasAtivasNoPeriodo(string[] turmasCodigos)
        {
            mediator.Setup(m => m.Send(It.IsAny<ObterTurmasComMatriculasValidasPeriodoQuery>(), It.IsAny<CancellationToken>()))
                    .ReturnsAsync(turmasCodigos);
        }

        private static DisciplinaDto ConfigurarComponente(string nome, string turmaCodigo, bool lancaNota)
        {
            return new Faker<DisciplinaDto>()
                .RuleFor(d => d.CodigoComponenteCurricular, f => f.Random.Long(1, 1000))
                .RuleFor(d => d.Nome, nome)
                .RuleFor(d => d.TurmaCodigo, turmaCodigo)
                .RuleFor(d => d.LancaNota, lancaNota)
                .RuleFor(d => d.GrupoMatrizNome, "Grupo Matriz Teste")
                .Generate();
        }

        private void ConfigurarMockComponentesDasTurmas(List<DisciplinaDto> componentes)
        {
            mediator.Setup(a => a.Send(It.IsAny<ObterComponentesCurricularesPorTurmasCodigoQuery>(), It.IsAny<CancellationToken>()))
                    .ReturnsAsync(componentes);
        }

        private static NotaConceitoBimestreComponenteDto ConfigurarNota(string alunoCodigo, long componenteCodigo, int bimestre, double? nota)
        {
            return new Faker<NotaConceitoBimestreComponenteDto>()
               .RuleFor(n => n.AlunoCodigo, alunoCodigo)
               .RuleFor(n => n.ComponenteCurricularCodigo, componenteCodigo)
               .RuleFor(n => n.Bimestre, bimestre)
               .RuleFor(n => n.Nota, nota)
               .Generate();
        }

        private void ConfigurarMockNotas(List<NotaConceitoBimestreComponenteDto> notas)
        {
            mediator.Setup(a => a.Send(It.IsAny<ObterConselhoClasseNotasAlunoQuery>(), It.IsAny<CancellationToken>()))
                    .ReturnsAsync((ObterConselhoClasseNotasAlunoQuery q, CancellationToken ct) => notas.Where(n => n.Bimestre == q.Bimestre && n.AlunoCodigo == q.AlunoCodigo));

            mediator.Setup(a => a.Send(It.IsAny<ObterNotasFinaisBimestresAlunoQuery>(), It.IsAny<CancellationToken>()))
                    .ReturnsAsync(notas);
        }
        #endregion
    }
}
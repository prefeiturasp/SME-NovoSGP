using Bogus;
using MediatR;
using Moq;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.Queries
{
    public class ObterTurmasComMatriculasValidasQueryHandlerTeste
    {
        private readonly Mock<IMediator> mediator;
        private readonly ObterTurmasComMatriculasValidasQueryHandler query;

        public ObterTurmasComMatriculasValidasQueryHandlerTeste()
        {
            mediator = new Mock<IMediator>();
            query = new ObterTurmasComMatriculasValidasQueryHandler(mediator.Object);
        }

        [Fact(DisplayName = "ObterTurmasComMatriculasValidasQueryHandler -  Obter Turmas com matrículas válidas")]
        public async Task Deve_Obter_Somente_matriculas_validas_conforme_periodo()
        {
            // Arrange
            var periodoInicio = new DateTime(2023, 02, 03);
            var periodoFim = new DateTime(2023, 06, 05);

            var matriculaValida = new Faker<AlunoPorTurmaResposta>("pt_BR")
                .RuleFor(m => m.CodigoAluno, "1")
                .RuleFor(m => m.CodigoTurma, 1)
                .RuleFor(m => m.DataMatricula, f => f.Date.Between(new DateTime(2023, 01, 01), periodoInicio))
                .RuleFor(m => m.DataSituacao, f => f.Date.Between(periodoInicio, periodoFim))
                .RuleFor(m => m.CodigoSituacaoMatricula, SituacaoMatriculaAluno.Concluido) // Situação que se enquadra em PossuiSituacaoAtiva()
                .Generate();

            var matriculaInvalida = new Faker<AlunoPorTurmaResposta>("pt_BR")
                .RuleFor(m => m.CodigoAluno, "1")
                .RuleFor(m => m.CodigoTurma, 2)
                .RuleFor(m => m.DataMatricula, f => f.Date.Soon(10, periodoFim)) // Data de matrícula fora do período
                .RuleFor(m => m.CodigoSituacaoMatricula, SituacaoMatriculaAluno.Ativo)
                .Generate();

            var turmas = new List<Turma>
            {
                new Turma { CodigoTurma = "1", TipoTurma = TipoTurma.Regular },
                new Turma { CodigoTurma = "2", TipoTurma = TipoTurma.Regular }
            };

            ConfigurarMocks(new List<AlunoPorTurmaResposta> { matriculaValida, matriculaInvalida }, turmas);
            var request = new ObterTurmasComMatriculasValidasQuery("1", new string[] { "1", "2" }, periodoInicio, periodoFim);

            // Act
            var retornoConsulta = await query.Handle(request, new CancellationToken());

            // Assert
            Assert.NotNull(retornoConsulta);
            Assert.Single(retornoConsulta);
            Assert.Equal("1", retornoConsulta.First());
        }

        [Fact(DisplayName = "ObterTurmasComMatriculasValidasQueryHandler -  Obter Turmas com matrículas válidas dentro do periodo de fechamento")]
        public async Task Deve_Obter_Somente_matriculas_validas_conforme_periodo_fechamento()
        {
            // Arrange
            var matriculaValida = new Faker<AlunoPorTurmaResposta>("pt_BR")
                .RuleFor(m => m.CodigoAluno, "1")
                .RuleFor(m => m.CodigoTurma, 1)
                .RuleFor(m => m.DataMatricula, new DateTime(2023, 01, 12))
                .RuleFor(m => m.DataSituacao, new DateTime(2023, 12, 20))
                .RuleFor(m => m.CodigoSituacaoMatricula, SituacaoMatriculaAluno.Ativo)
                .Generate();

            var matriculaInvalidaDispensada = new Faker<AlunoPorTurmaResposta>("pt_BR")
                .RuleFor(m => m.CodigoAluno, "1")
                .RuleFor(m => m.CodigoTurma, 2)
                .RuleFor(m => m.DataMatricula, new DateTime(2023, 02, 26))
                .RuleFor(m => m.DataSituacao, new DateTime(2023, 02, 26))
                .RuleFor(m => m.CodigoSituacaoMatricula, SituacaoMatriculaAluno.DispensadoEdFisica)
                .Generate();

            var turmaRegular = new Turma { CodigoTurma = "1", TipoTurma = TipoTurma.Regular };
            var turmaEdFisica = new Turma { CodigoTurma = "2", TipoTurma = TipoTurma.EdFisica };

            ConfigurarMocks(new List<AlunoPorTurmaResposta> { matriculaValida, matriculaInvalidaDispensada }, new List<Turma> { turmaRegular, turmaEdFisica });

            var periodoFechamentoInicio = new DateTime(2023, 04, 12);
            var periodoFechamentoFinal = new DateTime(2023, 05, 17);
            var request = new ObterTurmasComMatriculasValidasQuery("1", new string[] { "1", "2" }, periodoFechamentoInicio, periodoFechamentoFinal);

            // Act
            var retornoConsulta = await query.Handle(request, new CancellationToken());

            // Assert
            Assert.NotNull(retornoConsulta);
            Assert.Single(retornoConsulta);
            Assert.Equal("1", retornoConsulta.First());
        }

        private void ConfigurarMocks(IEnumerable<AlunoPorTurmaResposta> matriculas, IEnumerable<Turma> turmas)
        {
            mediator.Setup(m => m.Send(It.IsAny<ObterTodosAlunosNaTurmaQuery>(), It.IsAny<CancellationToken>()))
                    .ReturnsAsync((ObterTodosAlunosNaTurmaQuery query, CancellationToken token) =>
                        matriculas.Where(t => t.CodigoTurma.ToString() == query.CodigoTurma.ToString()));

            mediator.Setup(m => m.Send(It.IsAny<ObterTurmaPorCodigoQuery>(), It.IsAny<CancellationToken>()))
                    .ReturnsAsync((ObterTurmaPorCodigoQuery query, CancellationToken token) =>
                        turmas.FirstOrDefault(t => t.CodigoTurma == query.TurmaCodigo));
        }
    }
}
using MediatR;
using Moq;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.Queries.Aluno.ObterDadosAlunos
{
    public class ObterDadosAlunosQueryHandlerTeste
    {
        private readonly Mock<IMediator> mediator;
        private readonly ObterDadosAlunosQueryHandler queryHandler;

        public ObterDadosAlunosQueryHandlerTeste()
        {
            mediator = new Mock<IMediator>();
            queryHandler = new ObterDadosAlunosQueryHandler(mediator.Object);
        }

        [Fact(DisplayName = "ObterDadosAlunosQueryHandler - Deve obter dados dos alunos acionando obter todos alunos na turma considerando inativos")]
        public async Task DeveObterDadosAlunosAcionandoObterTodosAlunosNaTurmaConsiderandoInativos()
        {
            var anoAtual = DateTimeExtension.HorarioBrasilia().Year;
            var alunos = new List<AlunoPorTurmaResposta>()
            {
                new() { CodigoAluno = "1", CodigoSituacaoMatricula = SituacaoMatriculaAluno.Ativo },
                new() { CodigoAluno = "2", CodigoSituacaoMatricula = SituacaoMatriculaAluno.Transferido },
                new() { CodigoAluno = "3", CodigoSituacaoMatricula = SituacaoMatriculaAluno.Ativo },
            };

            mediator.Setup(x => x.Send(It.Is<ObterTodosAlunosNaTurmaQuery>(y => y.CodigoTurma == 1), It.IsAny<CancellationToken>()))
                .ReturnsAsync(alunos);

            var resultado = await queryHandler.Handle(new ObterDadosAlunosQuery("1", anoAtual, consideraInativos: true), It.IsAny<CancellationToken>());

            Assert.NotNull(resultado);
            Assert.Equal(alunos.Count, resultado.Count());

            mediator.Verify(x => x.Send(It.Is<ObterTodosAlunosNaTurmaQuery>(y => y.CodigoTurma == 1), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact(DisplayName = "ObterDadosAlunosQueryHandler - Deve obter dados dos alunos acionando obter todos alunos na turma desconsiderando inativos")]
        public async Task DeveObterDadosAlunosAcionandoObterTodosAlunosNaTurmaDesconsiderandoInativos()
        {
            var anoAtual = DateTimeExtension.HorarioBrasilia().Year;
            var alunos = new List<AlunoPorTurmaResposta>()
            {
                new() { CodigoAluno = "1", CodigoSituacaoMatricula = SituacaoMatriculaAluno.Ativo },
                new() { CodigoAluno = "2", CodigoSituacaoMatricula = SituacaoMatriculaAluno.Transferido },
                new() { CodigoAluno = "3", CodigoSituacaoMatricula = SituacaoMatriculaAluno.Ativo },
            };

            mediator.Setup(x => x.Send(It.Is<ObterTodosAlunosNaTurmaQuery>(y => y.CodigoTurma == 1), It.IsAny<CancellationToken>()))
                .ReturnsAsync(alunos);

            var resultado = await queryHandler.Handle(new ObterDadosAlunosQuery("1", anoAtual), It.IsAny<CancellationToken>());

            Assert.NotNull(resultado);
            Assert.Equal(alunos.Count(a => a.Ativo), resultado.Count());

            mediator.Verify(x => x.Send(It.Is<ObterTodosAlunosNaTurmaQuery>(y => y.CodigoTurma == 1), It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}

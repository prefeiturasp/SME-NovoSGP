using MediatR;
using Moq;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.Queries.EOL.Aluno.Turmas.ObterTurmasComMatriculasValidas
{
    public class ObterTurmasComMatriculasValidasQueryHandlerTeste
    {
        private readonly Mock<IMediator> mediator;
        private readonly ObterTurmasComMatriculasValidasQueryHandler queryHandler;

        public ObterTurmasComMatriculasValidasQueryHandlerTeste()
        {
            mediator = new Mock<IMediator>();
            queryHandler = new ObterTurmasComMatriculasValidasQueryHandler(mediator.Object);
        }

        [Fact(DisplayName = "ObterTurmasComMatriculasValidasQuery - Dever obter as turmas desconsiderando matriculas com vínculo indevido")]
        public async Task DeveObterTurmasDesconsiderandoMatriculasComVinculoIndevido()
        {
            var alunosPorTurma = new List<AlunoPorTurmaResposta>()
            {
                new()
                {
                    CodigoAluno = "1",
                    CodigoTurma = 1,
                    CodigoSituacaoMatricula = SituacaoMatriculaAluno.Ativo,
                    DataMatricula = new DateTime(DateTimeExtension.HorarioBrasilia().AddYears(-1).Year, 12, 05)
                },
                new()
                {
                    CodigoAluno = "2",
                    CodigoTurma = 2,
                    CodigoSituacaoMatricula = SituacaoMatriculaAluno.VinculoIndevido                    
                },
                new()
                {
                    CodigoAluno = "3",
                    CodigoTurma = 3,
                    CodigoSituacaoMatricula = SituacaoMatriculaAluno.Ativo,
                    DataMatricula = new DateTime(DateTimeExtension.HorarioBrasilia().Year, 02, 10)
                },
            };

            var codigosTurmas = new string[] { "1", "2", "3" };

            mediator.Setup(x => x.Send(It.Is<ObterMatriculasAlunoNaTurmaQuery>(y => codigosTurmas.Contains(y.CodigoTurma) && y.CodigoAluno.EhNulo()), It.IsAny<CancellationToken>()))
                .ReturnsAsync(alunosPorTurma);

            var periodoInicio = new DateTime(DateTimeExtension.HorarioBrasilia().Year, 02, 05);
            var periodoFim = new DateTime(DateTimeExtension.HorarioBrasilia().Year, 04, 30);
            var query = new ObterTurmasComMatriculasValidasQuery(null, codigosTurmas, periodoInicio, periodoFim); 

            var resultado = await queryHandler.Handle(query, It.IsAny<CancellationToken>());

            Assert.NotNull(resultado);
            Assert.NotEmpty(resultado);
            Assert.Equal(alunosPorTurma.Count(a => a.CodigoSituacaoMatricula != SituacaoMatriculaAluno.VinculoIndevido), resultado.Count());

            foreach (var aluno in alunosPorTurma)
            {
                if (aluno.CodigoSituacaoMatricula == SituacaoMatriculaAluno.VinculoIndevido)
                {
                    Assert.DoesNotContain(aluno.CodigoTurma.ToString(), resultado);
                    continue;
                }

                Assert.Contains(aluno.CodigoTurma.ToString(), resultado);
            }
        }
    }
}

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

namespace SME.SGP.Aplicacao.Teste.Queries.EOL.Aluno.Turmas.ObterTurmasComMatriculaValidasParaValidarConselho
{
    public class ObterTurmasComMatriculaValidasParaValidarConselhoQueryHandlerTeste
    {
        private readonly Mock<IMediator> mediator;
        private readonly ObterTurmasComMatriculaValidasParaValidarConselhoQueryHandler queryHandler;

        public ObterTurmasComMatriculaValidasParaValidarConselhoQueryHandlerTeste()
        {
            mediator = new Mock<IMediator>();
            queryHandler = new ObterTurmasComMatriculaValidasParaValidarConselhoQueryHandler(mediator.Object);
        }

        [Fact(DisplayName = "ObterTurmasComMatriculaValidasParaValidarConselhoQuery - Não considerar matrícula ativa iniciada após o período escolar")]
        public async Task NaoDeveConsiderarMatriculaAtivaIniciadaAposPeriodoEscolar()
        {
            var anoAtual = DateTime.Now.Year;
            var turma = new SME.SGP.Dominio.Turma { CodigoTurma = "1", TipoTurma = TipoTurma.Regular };
            var matriculas = new List<AlunoPorTurmaResposta>
            {
                new()
                {
                    CodigoAluno = "1",
                    DataMatricula = new DateTime(anoAtual, 5, 4),
                    CodigoSituacaoMatricula = SituacaoMatriculaAluno.Ativo
                }
            };

            mediator.Setup(x => x.Send(It.IsAny<ObterTurmaPorCodigoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(turma);
            mediator.Setup(x => x.Send(It.IsAny<ObterMatriculasAlunoNaTurmaQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(matriculas);

            var query = new ObterTurmasComMatriculaValidasParaValidarConselhoQuery(
                "1",
                new[] { turma.CodigoTurma },
                new DateTime(anoAtual, 2, 4),
                new DateTime(anoAtual, 4, 30));

            var retorno = await queryHandler.Handle(query, CancellationToken.None);

            Assert.Empty(retorno);
        }

        [Fact(DisplayName = "ObterTurmasComMatriculaValidasParaValidarConselhoQuery - Considerar matrícula ativa iniciada dentro do período escolar")]
        public async Task DeveConsiderarMatriculaAtivaIniciadaDentroPeriodoEscolar()
        {
            var anoAtual = DateTime.Now.Year;
            var turma = new SME.SGP.Dominio.Turma { CodigoTurma = "1", TipoTurma = TipoTurma.Regular };
            var matriculas = new List<AlunoPorTurmaResposta>
            {
                new()
                {
                    CodigoAluno = "1",
                    DataMatricula = new DateTime(anoAtual, 2, 10),
                    CodigoSituacaoMatricula = SituacaoMatriculaAluno.Ativo
                }
            };

            mediator.Setup(x => x.Send(It.IsAny<ObterTurmaPorCodigoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(turma);
            mediator.Setup(x => x.Send(It.IsAny<ObterMatriculasAlunoNaTurmaQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(matriculas);

            var query = new ObterTurmasComMatriculaValidasParaValidarConselhoQuery(
                "1",
                new[] { turma.CodigoTurma },
                new DateTime(anoAtual, 2, 4),
                new DateTime(anoAtual, 4, 30));

            var retorno = (await queryHandler.Handle(query, CancellationToken.None)).ToArray();

            Assert.Single(retorno);
            Assert.Contains(turma.CodigoTurma, retorno);
        }

        [Fact(DisplayName = "ObterTurmasComMatriculaValidasParaValidarConselhoQuery - Considerar matrícula a partir do dia posterior")]
        public async Task DeveConsiderarMatriculaAPartirDoDiaPosterior()
        {
            var anoAtual = DateTime.Now.Year;
            var turma = new SME.SGP.Dominio.Turma { CodigoTurma = "1", TipoTurma = TipoTurma.Regular };
            var periodoInicio = new DateTime(anoAtual, 2, 4);
            var periodoFim = new DateTime(anoAtual, 4, 30);

            mediator.Setup(x => x.Send(It.IsAny<ObterTurmaPorCodigoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(turma);

            var query = new ObterTurmasComMatriculaValidasParaValidarConselhoQuery(
                "1",
                new[] { turma.CodigoTurma },
                periodoInicio,
                periodoFim);

            mediator.Setup(x => x.Send(It.IsAny<ObterMatriculasAlunoNaTurmaQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new[]
                {
                    new AlunoPorTurmaResposta
                    {
                        CodigoAluno = "1",
                        DataMatricula = new DateTime(anoAtual, 4, 29, 23, 59, 59),
                        CodigoSituacaoMatricula = SituacaoMatriculaAluno.Ativo
                    }
                });

            var retornoMatriculaDiaAnterior = (await queryHandler.Handle(query, CancellationToken.None)).ToArray();

            mediator.Setup(x => x.Send(It.IsAny<ObterMatriculasAlunoNaTurmaQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new[]
                {
                    new AlunoPorTurmaResposta
                    {
                        CodigoAluno = "1",
                        DataMatricula = new DateTime(anoAtual, 4, 30, 7, 14, 47),
                        CodigoSituacaoMatricula = SituacaoMatriculaAluno.Ativo
                    }
                });

            var retornoMatriculaUltimoDia = (await queryHandler.Handle(query, CancellationToken.None)).ToArray();

            Assert.Single(retornoMatriculaDiaAnterior);
            Assert.Contains(turma.CodigoTurma, retornoMatriculaDiaAnterior);
            Assert.Empty(retornoMatriculaUltimoDia);
        }
    }
}

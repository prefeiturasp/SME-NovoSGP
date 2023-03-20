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

namespace SME.SGP.Aplicacao.Teste.Queries.Aluno.ObterAlunosDentroPeriodo
{
    public class ObterAlunosDentroPeriodoQueryHandlerTeste
    {
        private readonly Mock<IMediator> mediator = new Mock<IMediator>();
        private readonly ObterAlunosDentroPeriodoQueryHandler obterAlunosDentroPeriodoQueryHandler;
        private readonly int anoAtual;

        public ObterAlunosDentroPeriodoQueryHandlerTeste()
        {
            mediator = new Mock<IMediator>();
            obterAlunosDentroPeriodoQueryHandler = new ObterAlunosDentroPeriodoQueryHandler(mediator.Object);
            anoAtual = DateTime.Now.Year;
        }

        [Fact(DisplayName = "ObterAlunosDentroPeriodoQuery - Deve retornar exceção de alunos não localizados")]
        public async Task DeveRetornarErroAlunosNaoLocalizados()
        {
            mediator.Setup(x => x.Send(It.IsAny<ObterTodosAlunosNaTurmaQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(Enumerable.Empty<AlunoPorTurmaResposta>);

            await Assert.ThrowsAsync<NegocioException>(() =>
                obterAlunosDentroPeriodoQueryHandler.Handle(new ObterAlunosDentroPeriodoQuery("1", It.IsAny<(DateTime, DateTime)>()), It.IsAny<CancellationToken>()));
        }

        [Fact(DisplayName = "ObterAlunosDentroPeriodoQuery - Obter somente alunos considerados ativos no EOL")]
        public async Task DeveRetornarSomenteAlunosAtivos()
        {
            var listaAlunos = new List<AlunoPorTurmaResposta>()
            {
                new AlunoPorTurmaResposta() { CodigoSituacaoMatricula = SituacaoMatriculaAluno.Ativo },
                new AlunoPorTurmaResposta() { CodigoSituacaoMatricula = SituacaoMatriculaAluno.PendenteRematricula },
                new AlunoPorTurmaResposta() { CodigoSituacaoMatricula = SituacaoMatriculaAluno.Desistente },
                new AlunoPorTurmaResposta() { CodigoSituacaoMatricula = SituacaoMatriculaAluno.Rematriculado },
                new AlunoPorTurmaResposta() { CodigoSituacaoMatricula = SituacaoMatriculaAluno.Transferido },
                new AlunoPorTurmaResposta() { CodigoSituacaoMatricula = SituacaoMatriculaAluno.SemContinuidade },
                new AlunoPorTurmaResposta() { CodigoSituacaoMatricula = SituacaoMatriculaAluno.Concluido },
                new AlunoPorTurmaResposta() { CodigoSituacaoMatricula = SituacaoMatriculaAluno.VinculoIndevido }
            };

            mediator.Setup(x => x.Send(It.IsAny<ObterTodosAlunosNaTurmaQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(listaAlunos.AsEnumerable());

            var query = new ObterAlunosDentroPeriodoQuery("1", consideraSomenteAtivos: true);

            var retorno = await obterAlunosDentroPeriodoQueryHandler
                .Handle(query, It.IsAny<CancellationToken>());

            Assert.NotNull(retorno);
            Assert.NotEmpty(retorno);
            Assert.Equal(5, retorno.Count());
            Assert.Contains(listaAlunos[0], retorno);
            Assert.Contains(listaAlunos[1], retorno);
            Assert.DoesNotContain(listaAlunos[2], retorno);
            Assert.Contains(listaAlunos[3], retorno);
            Assert.DoesNotContain(listaAlunos[4], retorno);
            Assert.Contains(listaAlunos[5], retorno);
            Assert.Contains(listaAlunos[6], retorno);
            Assert.DoesNotContain(listaAlunos[7], retorno);
        }

        [Fact(DisplayName = "ObterAlunosDentroPeriodoQuery - Considerar alunos com período de matrícula condizente com a data única do período e todos alunos ativos no EOL")]
        public async Task ConsiderarAlunosNoPeriodoDataUnicaComListaTodosAlunosAtivos()
        {
            var listaAlunos = new List<AlunoPorTurmaResposta>()
            {
                new AlunoPorTurmaResposta() { DataMatricula = new DateTime(anoAtual, 3, 1), CodigoSituacaoMatricula = SituacaoMatriculaAluno.Ativo },
                new AlunoPorTurmaResposta() { DataMatricula = new DateTime(anoAtual, 3, 3), CodigoSituacaoMatricula = SituacaoMatriculaAluno.Ativo },
                new AlunoPorTurmaResposta() { DataMatricula = new DateTime(anoAtual, 3, 4), CodigoSituacaoMatricula = SituacaoMatriculaAluno.Ativo },
                new AlunoPorTurmaResposta() { DataMatricula = new DateTime(anoAtual, 3, 5), CodigoSituacaoMatricula = SituacaoMatriculaAluno.Ativo },
                new AlunoPorTurmaResposta() { DataMatricula = new DateTime(anoAtual, 3, 6), CodigoSituacaoMatricula = SituacaoMatriculaAluno.Ativo },
                new AlunoPorTurmaResposta() { DataMatricula = new DateTime(anoAtual, 3, 7), CodigoSituacaoMatricula = SituacaoMatriculaAluno.Ativo }
            };

            mediator.Setup(x => x.Send(It.IsAny<ObterTodosAlunosNaTurmaQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(listaAlunos.AsEnumerable());

            var dataUnica = new DateTime(anoAtual, 3, 5);
            var query = new ObterAlunosDentroPeriodoQuery("1", (dataUnica, dataUnica));

            var retorno = await obterAlunosDentroPeriodoQueryHandler
                .Handle(query, It.IsAny<CancellationToken>());

            Assert.NotNull(retorno);
            Assert.NotEmpty(retorno);
            Assert.Equal(3, retorno.Count());
            Assert.Contains(listaAlunos[0], retorno);
            Assert.Contains(listaAlunos[1], retorno);
            Assert.Contains(listaAlunos[2], retorno);
            Assert.DoesNotContain(listaAlunos[3], retorno); // não deve constar, pois considera o aluno na turma no dia posterior a matrícula
            Assert.DoesNotContain(listaAlunos[4], retorno); // não deve constar, pois foi matriculado posteriormente a data de referência apesar de ser considerado como ativo
            Assert.DoesNotContain(listaAlunos[5], retorno); // idem ao aluno acima
        }

        [Fact(DisplayName = "ObterAlunosDentroPeriodoQuery - Considerar alunos com período de matrícula condizente com a data única do período em que alunos foram inativados anteriormente e posteriormente a data do período no EOL")]
        public async Task ConsiderarAlunosNoPeriodoComListaAlgunsAlunosInativosAnteriormentePosteriormente()
        {
            var listaAlunos = new List<AlunoPorTurmaResposta>()
            {
                new AlunoPorTurmaResposta() { DataMatricula = new DateTime(anoAtual, 3, 1), DataSituacao = new DateTime(anoAtual, 3, 3), CodigoSituacaoMatricula = SituacaoMatriculaAluno.Cessado },
                new AlunoPorTurmaResposta() { DataMatricula = new DateTime(anoAtual, 3, 3), CodigoSituacaoMatricula = SituacaoMatriculaAluno.Ativo },
                new AlunoPorTurmaResposta() { DataMatricula = new DateTime(anoAtual, 3, 4), DataSituacao = new DateTime(anoAtual, 3, 8), CodigoSituacaoMatricula = SituacaoMatriculaAluno.Deslocamento },
                new AlunoPorTurmaResposta() { DataMatricula = new DateTime(anoAtual, 3, 5), CodigoSituacaoMatricula = SituacaoMatriculaAluno.Ativo },
                new AlunoPorTurmaResposta() { DataMatricula = new DateTime(anoAtual, 3, 6), CodigoSituacaoMatricula = SituacaoMatriculaAluno.Ativo },
                new AlunoPorTurmaResposta() { DataMatricula = new DateTime(anoAtual, 3, 7), CodigoSituacaoMatricula = SituacaoMatriculaAluno.Ativo },
                new AlunoPorTurmaResposta() { DataMatricula = new DateTime(anoAtual, 3, 8), DataSituacao = new DateTime(anoAtual, 3, 10), CodigoSituacaoMatricula = SituacaoMatriculaAluno.NaoCompareceu },
                new AlunoPorTurmaResposta() { DataMatricula = new DateTime(anoAtual, 3, 9), CodigoSituacaoMatricula = SituacaoMatriculaAluno.Ativo },
                new AlunoPorTurmaResposta() { DataMatricula = new DateTime(anoAtual, 3, 10), DataSituacao = new DateTime(anoAtual, 8, 20), CodigoSituacaoMatricula = SituacaoMatriculaAluno.ReclassificadoSaida }
            };

            mediator.Setup(x => x.Send(It.IsAny<ObterTodosAlunosNaTurmaQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(listaAlunos.AsEnumerable());

            var dataUnica = new DateTime(anoAtual, 3, 8);
            var query = new ObterAlunosDentroPeriodoQuery("1", (dataUnica, dataUnica));

            var retorno = await obterAlunosDentroPeriodoQueryHandler
                .Handle(query, It.IsAny<CancellationToken>());

            Assert.NotNull(retorno);
            Assert.NotEmpty(retorno);
            Assert.Equal(5, retorno.Count());
            Assert.DoesNotContain(listaAlunos[0], retorno); // não deve constar, pois foi considerado inativado antes da data de referência
            Assert.Contains(listaAlunos[1], retorno); // deve constar, pois é considerado ativo na data de referência
            Assert.Contains(listaAlunos[2], retorno); // deve constar, pois mesmo considerado inativado na data de referência só estará fora da turma no dia posterior
            Assert.Contains(listaAlunos[3], retorno); // deve constar, pois é considerado ativo na data de referência
            Assert.Contains(listaAlunos[4], retorno); // idem ao aluno acima
            Assert.Contains(listaAlunos[5], retorno); // idem ao aluno acima
            Assert.DoesNotContain(listaAlunos[6], retorno); // não deve constar, pois considera o aluno na turma no dia posterior a matrícula
            Assert.DoesNotContain(listaAlunos[7], retorno); // não deve constar, pois foi matriculado posteriormente a data de referência apesar de ser considerado como ativo
            Assert.DoesNotContain(listaAlunos[8], retorno); // idem ao aluno acima
        }

        [Fact(DisplayName = "ObterAlunosDentroPeriodoQuery - Considerar alunos com período de matrícula condizente com o período informado e todos alunos ativos no EOL")]
        public async Task ConsiderarAlunosNoPeriodoComListaTodosAlunosAtivos()
        {
            var listaAlunos = new List<AlunoPorTurmaResposta>()
            {
                new AlunoPorTurmaResposta() { DataMatricula = new DateTime(anoAtual, 2, 1), CodigoSituacaoMatricula = SituacaoMatriculaAluno.Ativo },
                new AlunoPorTurmaResposta() { DataMatricula = new DateTime(anoAtual, 3, 3), CodigoSituacaoMatricula = SituacaoMatriculaAluno.Ativo },
                new AlunoPorTurmaResposta() { DataMatricula = new DateTime(anoAtual, 4, 5), CodigoSituacaoMatricula = SituacaoMatriculaAluno.Ativo },
                new AlunoPorTurmaResposta() { DataMatricula = new DateTime(anoAtual, 5, 30), CodigoSituacaoMatricula = SituacaoMatriculaAluno.Ativo },
                new AlunoPorTurmaResposta() { DataMatricula = new DateTime(anoAtual, 5, 31), CodigoSituacaoMatricula = SituacaoMatriculaAluno.Ativo },
                new AlunoPorTurmaResposta() { DataMatricula = new DateTime(anoAtual, 6, 1), CodigoSituacaoMatricula = SituacaoMatriculaAluno.Ativo },
                new AlunoPorTurmaResposta() { DataMatricula = new DateTime(anoAtual, 8, 10), CodigoSituacaoMatricula = SituacaoMatriculaAluno.Ativo }
            };

            mediator.Setup(x => x.Send(It.IsAny<ObterTodosAlunosNaTurmaQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(listaAlunos.AsEnumerable());

            var periodo = (new DateTime(anoAtual, 4, 1), new DateTime(anoAtual, 5, 31));
            var query = new ObterAlunosDentroPeriodoQuery("1", periodo);

            var retorno = await obterAlunosDentroPeriodoQueryHandler
                .Handle(query, It.IsAny<CancellationToken>());

            Assert.NotNull(retorno);
            Assert.NotEmpty(retorno);
            Assert.Equal(4, retorno.Count());
            Assert.Contains(listaAlunos[0], retorno); // deve constar, pois é considerado ativo no período
            Assert.Contains(listaAlunos[1], retorno); // idem ao aluno acima
            Assert.Contains(listaAlunos[2], retorno); // idem ao aluno acima
            Assert.Contains(listaAlunos[3], retorno); // idem ao aluno acima
            Assert.DoesNotContain(listaAlunos[4], retorno); // não deve constar, pois considera o aluno na turma no dia posterior a matrícula
            Assert.DoesNotContain(listaAlunos[5], retorno); // não deve constar, pois foi matriculado posteriormente ao período apesar de ser considerado como ativo
            Assert.DoesNotContain(listaAlunos[6], retorno); // idem ao aluno acima
        }

        [Fact(DisplayName = "ObterAlunosDentroPeriodoQuery - Considerar alunos com período de matrícula condizente com o período informado em que alunos foram inativados anteriormente, dentro e posteriormente ao período no EOL")]
        public async Task ConsiderarAlunosNoPeriodoDataUnicaComListaAlgunsAlunosInativosAnteriormentePosteriormente()
        {
            var listaAlunos = new List<AlunoPorTurmaResposta>()
            {
                new AlunoPorTurmaResposta() { DataMatricula = new DateTime(anoAtual, 3, 1), DataSituacao = new DateTime(anoAtual, 7, 1), CodigoSituacaoMatricula = SituacaoMatriculaAluno.Cessado },
                new AlunoPorTurmaResposta() { DataMatricula = new DateTime(anoAtual, 3, 3), CodigoSituacaoMatricula = SituacaoMatriculaAluno.Ativo },
                new AlunoPorTurmaResposta() { DataMatricula = new DateTime(anoAtual, 3, 4), DataSituacao = new DateTime(anoAtual, 10, 15), CodigoSituacaoMatricula = SituacaoMatriculaAluno.Deslocamento },
                new AlunoPorTurmaResposta() { DataMatricula = new DateTime(anoAtual, 3, 5), CodigoSituacaoMatricula = SituacaoMatriculaAluno.Ativo },
                new AlunoPorTurmaResposta() { DataMatricula = new DateTime(anoAtual, 3, 6), CodigoSituacaoMatricula = SituacaoMatriculaAluno.Ativo },
                new AlunoPorTurmaResposta() { DataMatricula = new DateTime(anoAtual, 3, 7), CodigoSituacaoMatricula = SituacaoMatriculaAluno.Ativo },
                new AlunoPorTurmaResposta() { DataMatricula = new DateTime(anoAtual, 10, 15), DataSituacao = new DateTime(anoAtual, 3, 10), CodigoSituacaoMatricula = SituacaoMatriculaAluno.NaoCompareceu },
                new AlunoPorTurmaResposta() { DataMatricula = new DateTime(anoAtual, 10, 20), CodigoSituacaoMatricula = SituacaoMatriculaAluno.Ativo },
                new AlunoPorTurmaResposta() { DataMatricula = new DateTime(anoAtual, 11, 5), DataSituacao = new DateTime(anoAtual, 8, 20), CodigoSituacaoMatricula = SituacaoMatriculaAluno.ReclassificadoSaida },
                new AlunoPorTurmaResposta() { DataMatricula = new DateTime(anoAtual, 8, 2), DataSituacao = new DateTime(anoAtual, 10, 10), CodigoSituacaoMatricula = SituacaoMatriculaAluno.ReclassificadoSaida }
            };

            mediator.Setup(x => x.Send(It.IsAny<ObterTodosAlunosNaTurmaQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(listaAlunos.AsEnumerable());

            var periodo = (new DateTime(anoAtual, 8, 1), new DateTime(anoAtual, 10, 15));
            var query = new ObterAlunosDentroPeriodoQuery("1", periodo);

            var retorno = await obterAlunosDentroPeriodoQueryHandler
                .Handle(query, It.IsAny<CancellationToken>());

            Assert.NotNull(retorno);
            Assert.NotEmpty(retorno);
            Assert.Equal(6, retorno.Count());
            Assert.DoesNotContain(listaAlunos[0], retorno); // não deve constar, pois foi considerado inativado antes do período
            Assert.Contains(listaAlunos[1], retorno); // deve constar, pois é considerado ativo no período
            Assert.Contains(listaAlunos[2], retorno); // deve constar, pois mesmo considerado inativado no período só estará fora da turma no dia posterior
            Assert.Contains(listaAlunos[3], retorno); // deve constar, pois é considerado ativo no período
            Assert.Contains(listaAlunos[4], retorno); // idem ao aluno acima
            Assert.Contains(listaAlunos[5], retorno); // idem ao aluno acima
            Assert.DoesNotContain(listaAlunos[6], retorno); // não deve constar, pois considera na turma no dia posterior a matrícula
            Assert.DoesNotContain(listaAlunos[7], retorno); // não deve constar, pois foi matriculado posteriormente a data de referência apesar de ser considerado como ativo
            Assert.DoesNotContain(listaAlunos[8], retorno); // idem ao aluno acima
            Assert.Contains(listaAlunos[9], retorno); // deve constar, pois seu período de matrícula está dentro do período definido
        }
    }
}
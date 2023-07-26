using MediatR;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.TesteIntegracao.ServicosFakes
{
    public class ObterAlunosDentroPeriodoQueryHandlerFake : IRequestHandler<ObterAlunosDentroPeriodoQuery, IEnumerable<AlunoPorTurmaResposta>>
    {
        private const string CODIGO_ALUNO_1 = "1";
        private const string CODIGO_ALUNO_2 = "2";
        private const string CODIGO_ALUNO_3 = "3";
        private const string CODIGO_ALUNO_4 = "4";
        private const string CODIGO_ALUNO_5 = "5";
        private const string CODIGO_ALUNO_6 = "6";
        private const string CODIGO_ALUNO_7 = "7";
        private const string CODIGO_ALUNO_8 = "8";
        private const string CODIGO_ALUNO_9 = "9";
        private const string CODIGO_ALUNO_10 = "10";
        private const string CODIGO_ALUNO_11 = "11";
        private const string SITUACAO_MATRICULA_1 = "1";
        private const string SITUACAO_MATRICULA_2 = "2";
        private const string SITUACAO_MATRICULA_4 = "4";
        private const string SITUACAO_MATRICULA_15 = "15";
        
        public async Task<IEnumerable<AlunoPorTurmaResposta>> Handle(ObterAlunosDentroPeriodoQuery request, CancellationToken cancellationToken)
        {
            var dataAtual = DateTime.Now;

            return new List<AlunoPorTurmaResposta>()
            {
                new AlunoPorTurmaResposta()
                {
                    NomeAluno = "Aluno 1",
                    NomeResponsavel = "Responsavel 1",
                    Ano = DateTime.Now.Year,
                    DataSituacao = request.Periodo.dataInicio > dataAtual ? dataAtual : request.Periodo.dataFim,
                    CodigoAluno = CODIGO_ALUNO_1,
                    CodigoComponenteCurricular = 138,
                    CodigoSituacaoMatricula = SituacaoMatriculaAluno.Ativo,
                    SituacaoMatricula = SITUACAO_MATRICULA_1
                },
                new AlunoPorTurmaResposta()
                {
                    NomeAluno = "Aluno 2",
                    Ano = DateTime.Now.Year,
                    DataSituacao = DateTime.Now.Date.AddDays(-50),
                    CodigoAluno = CODIGO_ALUNO_4,
                    CodigoComponenteCurricular = 138,
                    CodigoSituacaoMatricula = SituacaoMatriculaAluno.Ativo,
                    SituacaoMatricula = SITUACAO_MATRICULA_1
                },
                new AlunoPorTurmaResposta()
                {
                    NomeAluno = "Aluno 3",
                    NomeResponsavel = "Responsavel 1",
                    Ano = DateTime.Now.Year,
                    DataSituacao = DateTime.Now.Date.AddDays(-50),
                    CodigoAluno = CODIGO_ALUNO_2,
                    CodigoComponenteCurricular = 138,
                    CodigoSituacaoMatricula = SituacaoMatriculaAluno.Ativo,
                    SituacaoMatricula = SITUACAO_MATRICULA_15
                },
                new AlunoPorTurmaResposta()
                {
                    NomeAluno = "Aluno 4",
                    NomeResponsavel = "Responsavel 1",
                    Ano = DateTime.Now.Year,
                    DataSituacao = request.Periodo.dataInicio.AddDays(-1),
                    CodigoAluno = CODIGO_ALUNO_3,
                    CodigoComponenteCurricular = 138,
                    CodigoSituacaoMatricula = SituacaoMatriculaAluno.Ativo,
                    SituacaoMatricula = SITUACAO_MATRICULA_4
                },
                new AlunoPorTurmaResposta()
                {
                    NomeAluno = "Aluno 5",
                    NomeResponsavel = "Responsavel 1",
                    Ano = DateTime.Now.Year,
                    DataSituacao = request.Periodo.dataFim.AddDays(-1),
                    CodigoAluno = CODIGO_ALUNO_5,
                    CodigoComponenteCurricular = 138,
                    CodigoSituacaoMatricula = SituacaoMatriculaAluno.Desistente,
                    SituacaoMatricula = SITUACAO_MATRICULA_2
                },
                new AlunoPorTurmaResposta()
                {
                    NomeAluno = "Aluno 6",
                    NomeResponsavel = "Responsavel 1",
                    Ano = DateTime.Now.Year,
                    DataSituacao = request.Periodo.dataFim.AddDays(-2),
                    CodigoAluno = CODIGO_ALUNO_6,
                    CodigoComponenteCurricular = 138,
                    CodigoSituacaoMatricula = SituacaoMatriculaAluno.Desistente,
                    SituacaoMatricula = SITUACAO_MATRICULA_2
                },
                new AlunoPorTurmaResposta()
                {
                    NomeAluno = "Aluno 7",
                    NomeResponsavel = "Responsavel 1",
                    Ano = DateTime.Now.Year,
                    DataSituacao = request.Periodo.dataFim.AddDays(-3),
                    CodigoAluno = CODIGO_ALUNO_7,
                    CodigoComponenteCurricular = 138,
                    CodigoSituacaoMatricula = SituacaoMatriculaAluno.Desistente,
                    SituacaoMatricula = SITUACAO_MATRICULA_2
                },
                new AlunoPorTurmaResposta()
                {
                    NomeAluno = "Aluno 8",
                    NomeResponsavel = "Responsavel 1",
                    Ano = DateTime.Now.Year,
                    DataSituacao = request.Periodo.dataFim.AddDays(-4),
                    CodigoAluno = CODIGO_ALUNO_8,
                    CodigoComponenteCurricular = 138,
                    CodigoSituacaoMatricula = SituacaoMatriculaAluno.Desistente,
                    SituacaoMatricula = SITUACAO_MATRICULA_2
                },
                new AlunoPorTurmaResposta()
                {
                    NomeAluno = "Aluno 9",
                    NomeResponsavel = "Responsavel 1",
                    Ano = DateTime.Now.Year,
                    DataSituacao = request.Periodo.dataFim.AddDays(-5),
                    CodigoAluno = CODIGO_ALUNO_9,
                    CodigoComponenteCurricular = 138,
                    CodigoSituacaoMatricula = SituacaoMatriculaAluno.Desistente,
                    SituacaoMatricula = SITUACAO_MATRICULA_2
                },
                new AlunoPorTurmaResposta()
                {
                    NomeAluno = "Aluno 10",
                    NomeResponsavel = "Responsavel 1",
                    Ano = DateTime.Now.Year,
                    DataSituacao = request.Periodo.dataFim.AddDays(-6),
                    CodigoAluno = CODIGO_ALUNO_10,
                    CodigoComponenteCurricular = 138,
                    CodigoSituacaoMatricula = SituacaoMatriculaAluno.Desistente,
                    SituacaoMatricula = SITUACAO_MATRICULA_2
                },
                new AlunoPorTurmaResposta()
                {
                    NomeAluno = "Aluno 11",
                    NomeResponsavel = "Responsavel 1",
                    Ano = DateTime.Now.Year,
                    DataSituacao = request.Periodo.dataFim.AddDays(-7),
                    CodigoAluno = CODIGO_ALUNO_11,
                    CodigoComponenteCurricular = 138,
                    CodigoSituacaoMatricula = SituacaoMatriculaAluno.Desistente,
                    SituacaoMatricula = SITUACAO_MATRICULA_2
                }
            };
        }
    }
}

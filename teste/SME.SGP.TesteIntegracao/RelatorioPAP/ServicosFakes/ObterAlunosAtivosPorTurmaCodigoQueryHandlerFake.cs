using global::SME.SGP.Aplicacao;
using global::SME.SGP.Dominio;
using global::SME.SGP.Infra;
using MediatR;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.TesteIntegracao.RelatorioPAP.ServicosFakes
{
    public class ObterAlunosAtivosPorTurmaCodigoQueryHandlerFake : IRequestHandler<ObterAlunosAtivosPorTurmaCodigoQuery, IEnumerable<AlunoPorTurmaResposta>>
    {
        private const string CODIGO_ALUNO_1 = "1";

        private const long CODIGO_TURMA_1 = 1;
        private const string NOME_ALUNO_CODIGO_1 = "NOME_ALUNO_CODIGO_1";

        private const string CODIGO_ALUNO_2 = "2";
        private const string NOME_ALUNO_CODIGO_2 = "NOME_ALUNO_CODIGO_2";

        private const string CODIGO_ALUNO_3 = "3";
        private const string NOME_ALUNO_CODIGO_3 = "NOME_ALUNO_CODIGO_3";

        private const string CODIGO_ALUNO_4 = "4";
        private const string NOME_ALUNO_CODIGO_4 = "NOME_ALUNO_CODIGO_4";

        private const string ATIVO = "ATIVO";
        private const string INATIVO = "INATIVO";
        private const string NOME_RESPONSAVEL_ALUNO_CODIGO_1 = "NOME_RESPONSAVEL_ALUNO_CODIGO_1";
        private const string TIPO_RESPONSAVEL_1 = "1";

        public ObterAlunosAtivosPorTurmaCodigoQueryHandlerFake()
        { }

        public async Task<IEnumerable<AlunoPorTurmaResposta>> Handle(ObterAlunosAtivosPorTurmaCodigoQuery request, CancellationToken cancellationToken)
        {
            return await Task.FromResult(new List<AlunoPorTurmaResposta>()
            {
                new AlunoPorTurmaResposta()
                {
                    CodigoAluno = CODIGO_ALUNO_1,
                    CodigoTurma = CODIGO_TURMA_1,
                    NomeAluno = NOME_ALUNO_CODIGO_1,
                    DataNascimento = DateTime.Now.AddYears(-15).Date,
                    CodigoSituacaoMatricula = SituacaoMatriculaAluno.Ativo,
                    SituacaoMatricula = ATIVO,
                    DataSituacao = DateTime.Now.AddMonths(-1).Date,
                    DataMatricula = DateTime.Now.AddMonths(-10).Date,
                    NumeroAlunoChamada = 1,
                    NomeResponsavel = NOME_RESPONSAVEL_ALUNO_CODIGO_1,
                    TipoResponsavel = TIPO_RESPONSAVEL_1,
                },
                new AlunoPorTurmaResposta()
                {
                    CodigoAluno = CODIGO_ALUNO_2,
                    CodigoTurma = CODIGO_TURMA_1,
                    NomeAluno = NOME_ALUNO_CODIGO_2,
                    DataNascimento = DateTime.Now.AddYears(-12).Date,
                    CodigoSituacaoMatricula = SituacaoMatriculaAluno.Ativo,
                    SituacaoMatricula = ATIVO,
                    DataSituacao = DateTime.Now.AddMonths(-4).Date,
                    DataMatricula = DateTime.Now.AddMonths(-11).Date,
                    NumeroAlunoChamada = 2,
                    NomeResponsavel = NOME_RESPONSAVEL_ALUNO_CODIGO_1,
                    TipoResponsavel = TIPO_RESPONSAVEL_1,
                },
                new AlunoPorTurmaResposta()
                {
                    CodigoAluno = CODIGO_ALUNO_3,
                    CodigoTurma = CODIGO_TURMA_1,
                    NomeAluno = NOME_ALUNO_CODIGO_3,
                    DataNascimento = DateTime.Now.AddYears(-14).Date,
                    CodigoSituacaoMatricula = SituacaoMatriculaAluno.Desistente,
                    SituacaoMatricula = INATIVO,
                    DataSituacao = DateTime.Now.AddMonths(-12).Date,
                    DataMatricula = DateTime.Now.AddMonths(-11).Date,
                    NumeroAlunoChamada = 3,
                    NomeResponsavel = NOME_RESPONSAVEL_ALUNO_CODIGO_1,
                    TipoResponsavel = TIPO_RESPONSAVEL_1,
                },
                new AlunoPorTurmaResposta()
                {
                    CodigoAluno = CODIGO_ALUNO_4,
                    CodigoTurma = CODIGO_TURMA_1,
                    NomeAluno = NOME_ALUNO_CODIGO_4,
                    DataNascimento = DateTime.Now.AddYears(-14).Date,
                    CodigoSituacaoMatricula = SituacaoMatriculaAluno.Ativo,
                    SituacaoMatricula = ATIVO,
                    DataSituacao = DateTime.Now.AddMonths(-6).Date,
                    DataMatricula = DateTime.Now.AddMonths(-11).Date,
                    NumeroAlunoChamada = 4,
                    NomeResponsavel = NOME_RESPONSAVEL_ALUNO_CODIGO_1,
                    TipoResponsavel = TIPO_RESPONSAVEL_1,
                }
            });
        }
    }
}

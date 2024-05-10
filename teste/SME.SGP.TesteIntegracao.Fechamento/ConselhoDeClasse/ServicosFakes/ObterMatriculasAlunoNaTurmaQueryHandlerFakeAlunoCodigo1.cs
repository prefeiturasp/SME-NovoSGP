using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio;
using SME.SGP.Infra;

namespace SME.SGP.TesteIntegracao.ConselhoDeClasse.ServicosFakes
{
    public class ObterMatriculasAlunoNaTurmaQueryHandlerFakeAlunoCodigo1 : IRequestHandler<ObterMatriculasAlunoNaTurmaQuery, IEnumerable<AlunoPorTurmaResposta>>
    {
        private const string CODIGO_ALUNO_1 = "1";
        private const long CODIGO_TURMA_1 = 1;
        private const string NOME_ALUNO_CODIGO_1 = "NOME_ALUNO_CODIGO_1";
        private const string ATIVO = "ATIVO";
        private const string NOME_RESPONSAVEL_ALUNO_CODIGO_1 = "NOME_RESPONSAVEL_ALUNO_CODIGO_1";
        private const string TIPO_RESPONSAVEL_1 = "TIPO_RESPONSAVEL_1";
        
        public ObterMatriculasAlunoNaTurmaQueryHandlerFakeAlunoCodigo1()
        {}

        public async Task<IEnumerable<AlunoPorTurmaResposta>> Handle(ObterMatriculasAlunoNaTurmaQuery request, CancellationToken cancellationToken)
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
                    DataSituacao = DateTime.Now.AddMonths(-5).Date,
                    DataMatricula = DateTime.Now.AddMonths(-10).Date,
                    NumeroAlunoChamada = 1,
                    NomeResponsavel = NOME_RESPONSAVEL_ALUNO_CODIGO_1,
                    TipoResponsavel = TIPO_RESPONSAVEL_1,
                }
            });
        }
    }
}
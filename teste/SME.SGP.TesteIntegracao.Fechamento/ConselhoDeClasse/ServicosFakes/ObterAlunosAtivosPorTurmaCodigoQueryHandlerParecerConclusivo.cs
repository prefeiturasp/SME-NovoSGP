using MediatR;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.TesteIntegracao.ConselhoDeClasse.ServicosFakes
{
    public class ObterAlunosAtivosPorTurmaCodigoQueryHandlerParecerConclusivo : IRequestHandler<ObterAlunosAtivosPorTurmaCodigoQuery, IEnumerable<AlunoPorTurmaResposta>>
    {
        private const string CODIGO_ALUNO_1 = "1";
        private const long CODIGO_TURMA_1 = 1;
        private const string NOME_ALUNO_CODIGO_1 = "NOME_ALUNO_CODIGO_1";
        private const string NOME_RESPONSAVEL_ALUNO_CODIGO_1 = "NOME_RESPONSAVEL_ALUNO_CODIGO_1";
        private const string TIPO_RESPONSAVEL_1 = "TIPO_RESPONSAVEL_1";

        public async Task<IEnumerable<AlunoPorTurmaResposta>> Handle(ObterAlunosAtivosPorTurmaCodigoQuery request, CancellationToken cancellationToken)
        {
            var dataReferencia = DateTimeExtension.HorarioBrasilia().Date;

            return await Task.FromResult(new List<AlunoPorTurmaResposta>()
            {
                new AlunoPorTurmaResposta()
                {
                    CodigoAluno = CODIGO_ALUNO_1,
                    CodigoTurma = CODIGO_TURMA_1,
                    NomeAluno = NOME_ALUNO_CODIGO_1,
                    DataNascimento = DateTime.Now.AddYears(-15).Date,
                    CodigoSituacaoMatricula = SituacaoMatriculaAluno.Ativo,
                    SituacaoMatricula = "ATIVO",
                    DataSituacao = dataReferencia,
                    DataMatricula = dataReferencia.AddDays(-2),
                    NumeroAlunoChamada = 1,
                    NomeResponsavel = NOME_RESPONSAVEL_ALUNO_CODIGO_1,
                    TipoResponsavel = TIPO_RESPONSAVEL_1,
                }
            });
        }
    }
}

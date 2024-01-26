using MediatR;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.TesteIntegracao.ConselhoDeClasse.ServicosFakes
{
    public class ObterAlunoPorTurmaAlunoCodigoQueryHandlerParecerConclusivo : IRequestHandler<ObterAlunoPorTurmaAlunoCodigoQuery, AlunoPorTurmaResposta>
    {
        public Task<AlunoPorTurmaResposta> Handle(ObterAlunoPorTurmaAlunoCodigoQuery request, CancellationToken cancellationToken)
        {
            var dataReferencia = DateTimeExtension.HorarioBrasilia().Date;

            return Task.FromResult(new AlunoPorTurmaResposta()
            {
                CodigoAluno = "1",
                NomeAluno = "Nome aluno 1",
                DataMatricula = dataReferencia.AddDays(-2),
                CodigoSituacaoMatricula = SituacaoMatriculaAluno.Ativo,
                SituacaoMatricula = "ATIVO",
                DataSituacao = dataReferencia,
                DataNascimento = new DateTime(1959, 01, 16, 00, 00, 00),
                NumeroAlunoChamada = 1
            });
        }
    }
}

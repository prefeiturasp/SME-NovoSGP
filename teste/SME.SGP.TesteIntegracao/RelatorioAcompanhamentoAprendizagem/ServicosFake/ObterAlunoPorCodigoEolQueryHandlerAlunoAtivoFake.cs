using MediatR;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.TesteIntegracao.RelatorioAcompanhamentoAprendizagem.ServicosFake
{
    public class ObterAlunoPorCodigoEolQueryHandlerAlunoAtivoFake : IRequestHandler<ObterAlunoPorCodigoEolQuery, AlunoPorTurmaResposta>
    {
        public async Task<AlunoPorTurmaResposta> Handle(ObterAlunoPorCodigoEolQuery request, CancellationToken cancellationToken)
        {
            var dataReferencia = DateTimeExtension.HorarioBrasilia().Date;

            return new AlunoPorTurmaResposta()
            {
                CodigoAluno = "1",
                CodigoSituacaoMatricula = SituacaoMatriculaAluno.Ativo,
                CodigoTurma = 1,
                DataNascimento = dataReferencia.AddYears(-5),
                DataSituacao = dataReferencia.AddDays(-5),
                DataMatricula = dataReferencia.AddDays(-5),
                NomeAluno = "Nome 2",
                NumeroAlunoChamada = 1,
                SituacaoMatricula = "Ativo",
                DataAtualizacaoContato = dataReferencia,
            };
        }
    }
}

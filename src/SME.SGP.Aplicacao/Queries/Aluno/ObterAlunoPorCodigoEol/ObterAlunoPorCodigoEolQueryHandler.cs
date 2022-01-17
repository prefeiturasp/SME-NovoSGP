using MediatR;
using SME.SGP.Aplicacao.Integracoes;
using SME.SGP.Infra;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using SME.SGP.Dominio;

namespace SME.SGP.Aplicacao
{
    public class
        ObterAlunoPorCodigoEolQueryHandler : IRequestHandler<ObterAlunoPorCodigoEolQuery, AlunoPorTurmaResposta>
    {
        private readonly IServicoEol servicoEol;

        public ObterAlunoPorCodigoEolQueryHandler(IServicoEol servicoEol)
        {
            this.servicoEol = servicoEol ?? throw new System.ArgumentNullException(nameof(servicoEol));
        }

        public async Task<AlunoPorTurmaResposta> Handle(ObterAlunoPorCodigoEolQuery request,
            CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(request.CodigoTurma))
            {
                return (await servicoEol.ObterDadosAluno(request.CodigoAluno, request.AnoLetivo,
                        request.ConsideraHistorico, request.FiltrarSituacao))
                    .OrderByDescending(a => a.DataSituacao)?.FirstOrDefault();
            }

            return await ObterAluno(request.CodigoAluno, request.AnoLetivo, request.ConsideraHistorico,
                request.FiltrarSituacao, request.CodigoTurma);
        }


        public async Task<AlunoPorTurmaResposta> ObterAluno(string codigoAluno, int anoLetivo,
            bool historica, bool filtrarSituacao, string codigoTurma)
        {
            var response =
                (await servicoEol.ObterDadosAluno(codigoAluno, anoLetivo, historica, filtrarSituacao))
                .OrderByDescending(a => a.DataSituacao);

            var retorno = response
                .Where(da => da.CodigoTurma.ToString().Equals(codigoTurma));

            if (!retorno.Any())
            {
                return await ObterAluno(codigoAluno, anoLetivo, !historica, filtrarSituacao, codigoTurma);
            }

            return retorno.FirstOrDefault(a => a.CodigoSituacaoMatricula == SituacaoMatriculaAluno.Ativo);
        }
    }
}
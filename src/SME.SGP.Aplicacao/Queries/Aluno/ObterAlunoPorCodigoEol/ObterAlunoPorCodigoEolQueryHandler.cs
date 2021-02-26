using MediatR;
using SME.SGP.Aplicacao.Integracoes;
using SME.SGP.Infra;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterAlunoPorCodigoEolQueryHandler : IRequestHandler<ObterAlunoPorCodigoEolQuery, AlunoPorTurmaResposta>
    {
        private readonly IServicoEol servicoEol;

        public ObterAlunoPorCodigoEolQueryHandler(IServicoEol servicoEol)
        {
            this.servicoEol = servicoEol ?? throw new System.ArgumentNullException(nameof(servicoEol));
        }

        public async Task<AlunoPorTurmaResposta> Handle(ObterAlunoPorCodigoEolQuery request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(request.CodigoTurma))
                return (await servicoEol.ObterDadosAluno(request.CodigoAluno, request.AnoLetivo))?.FirstOrDefault();
            else
            {
                var retorno = await servicoEol.ObterDadosAluno(request.CodigoAluno, request.AnoLetivo);
                return retorno?.FirstOrDefault(da => da.CodigoTurma.ToString().Equals(request.CodigoTurma));
            }
        }
    }
}

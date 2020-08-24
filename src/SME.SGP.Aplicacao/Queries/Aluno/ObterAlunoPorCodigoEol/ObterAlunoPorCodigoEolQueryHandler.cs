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
            return (await servicoEol.ObterDadosAluno(request.CodigoAluno, request.AnoLetivo))?.FirstOrDefault();
        }
    }
}

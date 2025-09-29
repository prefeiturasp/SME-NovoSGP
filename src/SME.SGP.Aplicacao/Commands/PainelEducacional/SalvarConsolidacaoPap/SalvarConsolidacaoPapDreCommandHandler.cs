using MediatR;
using SME.SGP.Dominio.Interfaces.Repositorios;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Commands.PainelEducacional.SalvarConsolidacaoPap
{
    public class SalvarConsolidacaoPapDreCommandHandler : IRequestHandler<SalvarConsolidacaoPapDreCommand, bool>
    {
        private readonly IRepositorioPainelEducacionalPap repositorioPainelEducacionalPap;

        public SalvarConsolidacaoPapDreCommandHandler(IRepositorioPainelEducacionalPap repositorioPainelEducacionalPap)
        {
            this.repositorioPainelEducacionalPap = repositorioPainelEducacionalPap;
        }

        public async Task<bool> Handle(SalvarConsolidacaoPapDreCommand request, CancellationToken cancellationToken)
        {
            await repositorioPainelEducacionalPap.SalvarConsolidacaoDre(request.Consolidacao);
            return true;
        }
    }
}

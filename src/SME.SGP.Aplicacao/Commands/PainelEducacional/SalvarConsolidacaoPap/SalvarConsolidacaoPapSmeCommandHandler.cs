using MediatR;
using SME.SGP.Dominio.Interfaces.Repositorios;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Commands.PainelEducacional.SalvarConsolidacaoPap
{
    public class SalvarConsolidacaoPapSmeCommandHandler : IRequestHandler<SalvarConsolidacaoPapSmeCommand, bool>
    {
        private readonly IRepositorioPainelEducacionalPap repositorioPainelEducacionalPap;

        public SalvarConsolidacaoPapSmeCommandHandler(IRepositorioPainelEducacionalPap repositorioPainelEducacionalPap)
        {
            this.repositorioPainelEducacionalPap = repositorioPainelEducacionalPap;
        }

        public async Task<bool> Handle(SalvarConsolidacaoPapSmeCommand request, CancellationToken cancellationToken)
        {
            await repositorioPainelEducacionalPap.SalvarConsolidacaoSme(request.Consolidacao);
            return true;
        }
    }
}

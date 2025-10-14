using MediatR;
using SME.SGP.Dominio.Interfaces.Repositorios;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Commands.PainelEducacional.SalvarConsolidacaoPap
{
    public class SalvarConsolidacaoPapUeCommandHandler : IRequestHandler<SalvarConsolidacaoPapUeCommand, bool>
    {
        private readonly IRepositorioPainelEducacionalConsolidacaoIndicadoresPap repositorioPainelEducacionalPap;

        public SalvarConsolidacaoPapUeCommandHandler(IRepositorioPainelEducacionalConsolidacaoIndicadoresPap repositorioPainelEducacionalPap)
        {
            this.repositorioPainelEducacionalPap = repositorioPainelEducacionalPap;
        }

        public async Task<bool> Handle(SalvarConsolidacaoPapUeCommand request, CancellationToken cancellationToken)
        {
            await repositorioPainelEducacionalPap.SalvarConsolidacaoUe(request.Consolidacao);
            return true;
        }
    }
}

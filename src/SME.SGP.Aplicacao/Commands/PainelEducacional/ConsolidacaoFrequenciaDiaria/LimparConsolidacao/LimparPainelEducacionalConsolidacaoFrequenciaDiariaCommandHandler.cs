using MediatR;
using SME.SGP.Dominio.Interfaces.Repositorios;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Commands.PainelEducacional.ConsolidacaoFrequenciaDiaria.LimparConsolidacao
{
    public class LimparPainelEducacionalConsolidacaoFrequenciaDiariaCommandHandler : IRequestHandler<LimparPainelEducacionalConsolidacaoFrequenciaDiariaCommand, bool>
    {
        private readonly IRepositorioPainelEducacionalConsolidacaoFrequenciaDiariaDre repositorioPainelEducacionalConsolidacaoFrequenciaDiariaDre;

        public LimparPainelEducacionalConsolidacaoFrequenciaDiariaCommandHandler(IRepositorioPainelEducacionalConsolidacaoFrequenciaDiariaDre repositorioPainelEducacionalConsolidacaoFrequenciaDiariaDre)
        {
            this.repositorioPainelEducacionalConsolidacaoFrequenciaDiariaDre = repositorioPainelEducacionalConsolidacaoFrequenciaDiariaDre;
        }

        public async Task<bool> Handle(LimparPainelEducacionalConsolidacaoFrequenciaDiariaCommand request, CancellationToken cancellationToken)
        {
            await repositorioPainelEducacionalConsolidacaoFrequenciaDiariaDre.LimparConsolidacao();
            return true;
        }
    }
}

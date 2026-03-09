using MediatR;
using SME.SGP.Dominio.Interfaces.Repositorios;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Commands.PainelEducacional.ConsolidacaoFrequenciaDiaria.LimparConsolidacao
{
    public class LimparPainelEducacionalConsolidacaoFrequenciaDiariaTurmaCommandHandler : IRequestHandler<LimparPainelEducacionalConsolidacaoFrequenciaDiariaTurmaCommand, bool>
    {
        private readonly IRepositorioPainelEducacionalConsolidacaoFrequenciaDiaria repositorioPainelEducacionalConsolidacaoFrequenciaDiaria;

        public LimparPainelEducacionalConsolidacaoFrequenciaDiariaTurmaCommandHandler(IRepositorioPainelEducacionalConsolidacaoFrequenciaDiaria repositorioPainelEducacionalConsolidacaoFrequenciaDiaria)
        {
            this.repositorioPainelEducacionalConsolidacaoFrequenciaDiaria = repositorioPainelEducacionalConsolidacaoFrequenciaDiaria;
        }

        public async Task<bool> Handle(LimparPainelEducacionalConsolidacaoFrequenciaDiariaTurmaCommand request, CancellationToken cancellationToken)
        {
            await repositorioPainelEducacionalConsolidacaoFrequenciaDiaria.LimparConsolidacao();
            return true;
        }
    }
}

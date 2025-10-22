using MediatR;
using SME.SGP.Dominio.Interfaces.Repositorios;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Commands.PainelEducacional.SalvarConsolidacaoNota
{
    public class SalvarPainelEducacionalConsolidacaoNotaCommandHandler : IRequestHandler<SalvarPainelEducacionalConsolidacaoNotaCommand, bool>
    {
        private readonly IRepositorioPainelEducacionalConsolidacaoNota _repositorioPainelEducacionalConsolidacaoNota;

        public SalvarPainelEducacionalConsolidacaoNotaCommandHandler(IRepositorioPainelEducacionalConsolidacaoNota repositorioPainelEducacionalConsolidacaoNota)
        {
            _repositorioPainelEducacionalConsolidacaoNota = repositorioPainelEducacionalConsolidacaoNota;
        }

        public async Task<bool> Handle(SalvarPainelEducacionalConsolidacaoNotaCommand request, CancellationToken cancellationToken)
        {
            if(request.NotasConsolidadasDre?.Any() != true)
                return false;

            var menorAnoLetivo = request.NotasConsolidadasDre.Min(c => c.AnoLetivo);

            await _repositorioPainelEducacionalConsolidacaoNota.LimparConsolidacaoAsync(menorAnoLetivo);

            await  _repositorioPainelEducacionalConsolidacaoNota.SalvarConsolidacaoAsync(request.NotasConsolidadasDre.ToList());
            await  _repositorioPainelEducacionalConsolidacaoNota.SalvarConsolidacaoUeAsync(request.NotasConsolidadasUe.ToList());
            return true;
        }
    }
}

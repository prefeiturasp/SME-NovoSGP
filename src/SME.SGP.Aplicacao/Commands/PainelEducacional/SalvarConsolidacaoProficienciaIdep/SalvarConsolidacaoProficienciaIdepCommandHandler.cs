using MediatR;
using SME.SGP.Dominio.Interfaces.Repositorios;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Commands.PainelEducacional.SalvarConsolidacaoProficienciaIdep
{
    public class SalvarConsolidacaoProficienciaIdepCommandHandler : IRequestHandler<SalvarConsolidacaoProficienciaIdepCommand, bool>
    {
        private readonly IRepositorioPainelEducacionalConsolidacaoProficienciaIdep _repositorioPainelEducacionalConsolidacaoProficienciaIdep;
        public SalvarConsolidacaoProficienciaIdepCommandHandler(IRepositorioPainelEducacionalConsolidacaoProficienciaIdep repositorioPainelEducacionalConsolidacaoProficienciaIdep)
        {
            _repositorioPainelEducacionalConsolidacaoProficienciaIdep = repositorioPainelEducacionalConsolidacaoProficienciaIdep;
        }
        public async Task<bool> Handle(SalvarConsolidacaoProficienciaIdepCommand request, CancellationToken cancellationToken)
        {
            if (request.consolidacaoIdepUe == null)
                return false;

            var anoLetivo = request.consolidacaoIdepUe.FirstOrDefault().AnoLetivo;
            await _repositorioPainelEducacionalConsolidacaoProficienciaIdep.LimparConsolidacaoPorAnoAsync(anoLetivo);
            await _repositorioPainelEducacionalConsolidacaoProficienciaIdep.SalvarConsolidacaoAsync(request.consolidacaoIdepUe.ToList());
            return true;
        }
    }
}
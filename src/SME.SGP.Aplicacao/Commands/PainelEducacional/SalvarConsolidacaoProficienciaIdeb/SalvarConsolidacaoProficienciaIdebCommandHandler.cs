using MediatR;
using SME.SGP.Dominio.Interfaces.Repositorios;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Commands.PainelEducacional.SalvarConsolidacaoProficienciaIdeb
{
    public class SalvarConsolidacaoProficienciaIdebCommandHandler : IRequestHandler<SalvarConsolidacaoProficienciaIdebCommand, bool>
    {
        private readonly IRepositorioPainelEducacionalConsolidacaoProficienciaIdeb _repositorioPainelEducacionalConsolidacaoProficienciaIdeb;
        public SalvarConsolidacaoProficienciaIdebCommandHandler(IRepositorioPainelEducacionalConsolidacaoProficienciaIdeb repositorioPainelEducacionalConsolidacaoProficienciaIdeb)
        {
            _repositorioPainelEducacionalConsolidacaoProficienciaIdeb = repositorioPainelEducacionalConsolidacaoProficienciaIdeb;
        }
        public async Task<bool> Handle(SalvarConsolidacaoProficienciaIdebCommand request, CancellationToken cancellationToken)
        {
            if (request.consolidacaoIdebUe == null)
                return false;

            var anoLetivo = request.consolidacaoIdebUe.FirstOrDefault().AnoLetivo;
            await _repositorioPainelEducacionalConsolidacaoProficienciaIdeb.LimparConsolidacaoPorAnoAsync(anoLetivo);
            await _repositorioPainelEducacionalConsolidacaoProficienciaIdeb.SalvarConsolidacaoAsync(request.consolidacaoIdebUe.ToList());
            return true;
        }
    }
}
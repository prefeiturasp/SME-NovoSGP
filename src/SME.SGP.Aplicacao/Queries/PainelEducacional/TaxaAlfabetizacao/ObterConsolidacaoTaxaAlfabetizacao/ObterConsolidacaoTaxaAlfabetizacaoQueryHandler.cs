using MediatR;
using SME.SGP.Dominio.Entidades;
using SME.SGP.Dominio.Interfaces.Repositorios;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Queries.PainelEducacional.TaxaAlfabetizacao.ObterConsolidacaoTaxaAlfabetizacao
{
    public class ObterConsolidacaoTaxaAlfabetizacaoQueryHandler : IRequestHandler<ObterConsolidacaoTaxaAlfabetizacaoQuery, IEnumerable<PainelEducacionalConsolidacaoTaxaAlfabetizacao>>
    {
        private readonly IRepositorioPainelEducacionalConsolidacaoTaxaAlfabetizacao repositorioPainelEducacionalConsolidacaoTaxaAlfabetizacao;

        public ObterConsolidacaoTaxaAlfabetizacaoQueryHandler(IRepositorioPainelEducacionalConsolidacaoTaxaAlfabetizacao repositorioPainelEducacionalConsolidacaoTaxaAlfabetizacao)
        {
            this.repositorioPainelEducacionalConsolidacaoTaxaAlfabetizacao = repositorioPainelEducacionalConsolidacaoTaxaAlfabetizacao;
        }

        public Task<IEnumerable<PainelEducacionalConsolidacaoTaxaAlfabetizacao>> Handle(ObterConsolidacaoTaxaAlfabetizacaoQuery request, CancellationToken cancellationToken)
        {
            return repositorioPainelEducacionalConsolidacaoTaxaAlfabetizacao.ObterConsolidacaoAsync(request.AnoLetivo, request.CodigoDre, request.CodigoUe);
        }
    }
}

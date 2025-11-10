using MediatR;
using SME.SGP.Aplicacao.Interfaces.CasosDeUso.PainelEducacional;
using SME.SGP.Aplicacao.Queries.PainelEducacional.TaxaAlfabetizacao.ObterConsolidacaoTaxaAlfabetizacao;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.CasosDeUso.PainelEducacional
{
    public class ConsultasPainelEducacionalTaxaAlfabetizacaoUseCase : IConsultasPainelEducacionalTaxaAlfabetizacaoUseCase
    {
        private readonly IMediator mediator;

        public ConsultasPainelEducacionalTaxaAlfabetizacaoUseCase(IMediator mediator)
        {
            this.mediator = mediator;
        }

        public async Task<decimal> Executar(int anoLetivo, string codigoDre, string codigoUe)
        {
            var taxas = await mediator.Send(new ObterConsolidacaoTaxaAlfabetizacaoQuery(anoLetivo, codigoDre, codigoUe));

            if (taxas?.Count() == 0)
                return 0;
            return taxas.Sum(t => t.TaxaAlfabetizacao) / taxas.Count();
        }
    }
}

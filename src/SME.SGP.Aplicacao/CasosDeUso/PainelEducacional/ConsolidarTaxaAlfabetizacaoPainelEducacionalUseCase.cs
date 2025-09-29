using MediatR;
using SME.SGP.Aplicacao.Commands.PainelEducacional.SalvarConsolidacaoTaxaAlfabetizacao;
using SME.SGP.Aplicacao.Interfaces.CasosDeUso.PainelEducacional;
using SME.SGP.Aplicacao.Queries.PainelEducacional.TaxaAlfabetizacao.ObterTaxaAlfabetizacao;
using SME.SGP.Dominio.Entidades;
using SME.SGP.Infra;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.CasosDeUso.PainelEducacional
{
    public class ConsolidarTaxaAlfabetizacaoPainelEducacionalUseCase : AbstractUseCase, IConsolidarTaxaAlfabetizacaoPainelEducacionalUseCase
    {
        public ConsolidarTaxaAlfabetizacaoPainelEducacionalUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<bool> Executar(MensagemRabbit param)
        {
            var taxasAlfabetizacao = await mediator.Send(new ObterTaxaAlfabetizacaoPorAnoLetivoQuery());

            if (taxasAlfabetizacao?.Any() != true)
                return false;

            var registroConsolidado = taxasAlfabetizacao
                    .GroupBy(r => new { r.AnoLetivo, r.CodigoUe })
                    .Select(g => new PainelEducacionalConsolidacaoTaxaAlfabetizacao
                    {
                        CodigoUe = g.Key.CodigoUe,
                        CodigoDre = g.Select(x => x.CodigoDre).FirstOrDefault(),
                        AnoLetivo = g.Key.AnoLetivo,
                        TaxaAlfabetizacao = g.Sum(x => x.Taxa)
                    })
                    .OrderBy(x => x.AnoLetivo)
                    .ThenBy(x => x.CodigoUe).ToList();

            await mediator.Send(new SalvarPainelEducacionalConsolidacaoTaxaAlfabetizacaoCommand(registroConsolidado));

            return true;
        }
    }
}

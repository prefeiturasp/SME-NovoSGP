using MediatR;
using SME.SGP.Infra.Dtos.PainelEducacional.TaxaAlfabetizacao;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao.Queries.PainelEducacional.TaxaAlfabetizacao.ObterTaxaAlfabetizacao
{
    public class ObterTaxaAlfabetizacaoPorAnoLetivoQuery : IRequest<IEnumerable<TaxaAlfabetizacaoDto>>
    {
       
    }
}

using MediatR;
using SME.SGP.Infra.Dtos.PainelEducacional.SondagemEscrita;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao.Queries.PainelEducacional.TaxaAlfabetizacao.ObterSondagemEscrita
{
    public class ObterSondagemEscritaUePorAnoLetivoPeriodoQuery : IRequest<IEnumerable<SondagemEscritaUeDto>>
    {
       
    }
}

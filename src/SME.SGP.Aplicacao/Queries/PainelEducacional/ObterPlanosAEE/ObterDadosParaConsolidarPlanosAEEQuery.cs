using MediatR;
using SME.SGP.Infra.Dtos.PainelEducacional.ConsolidacaoPlanoAEE;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao.Queries.PainelEducacional.ObterConsolidacaoPlanosAEE
{
    public class ObterDadosParaConsolidarPlanosAEEQuery : IRequest<IEnumerable<DadosParaConsolidarPlanosAEEDto>>
    {
    }
}

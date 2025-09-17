using MediatR;
using SME.SGP.Dominio;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao.Queries.PainelEducacional.ObterIndicadoresPap
{
    public class ObterIndicadoresPapQuery : IRequest<IEnumerable<ConsolidacaoInformacoesPap>>
    {
    }
}
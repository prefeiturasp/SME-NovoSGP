using MediatR;
using SME.SGP.Infra.Dtos.Sondagem;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao.Queries.Sondagem.ObterConsolidacaoNivelEscritaAlfabetizacao
{
    public class ObterConsolidacaoNivelEscritaAlfabetizacaoQuery : IRequest<IEnumerable<SondagemConsolidacaoNivelEscritaAlfabetizacaoDto>>
    {
    }
}

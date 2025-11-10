using MediatR;
using SME.SGP.Infra.Dtos.Sondagem;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao.Queries.Sondagem.ObterConsolidacaoAlfabetizacaoCriticaEscrita
{
    public class ObterConsolidacaoAlfabetizacaoCriticaEscritaQuery : IRequest<IEnumerable<SondagemConsolidacaoAlfabetizacaoCriticaEscritaDto>>
    {
    }
}
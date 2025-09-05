using MediatR;
using SME.SGP.Infra.Dtos.Sondagem;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao.Commands.PainelEducacional.SalvarConsolidacaoAlfabetizacaoCriticaEscrita
{
    public class SalvarConsolidacaoAlfabetizacaoCriticaEscritaCommand : IRequest<bool>
    {
        public IEnumerable<SondagemConsolidacaoAlfabetizacaoCriticaEscritaDto> ConsolidacaoAlfabetizacaoCriticaEscrita { get; set; }

        public SalvarConsolidacaoAlfabetizacaoCriticaEscritaCommand(IEnumerable<SondagemConsolidacaoAlfabetizacaoCriticaEscritaDto> consolidacaoAlfabetizacaoCriticaEscrita)
        {
            ConsolidacaoAlfabetizacaoCriticaEscrita = consolidacaoAlfabetizacaoCriticaEscrita;
        }
    }
}
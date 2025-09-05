using MediatR;
using SME.SGP.Infra.Dtos.Sondagem;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao.Commands.PainelEducacional.SalvarConsolidacaoNivelEscritaAlfabetizacao
{
    public class SalvarConsolidacaoNivelEscritaAlfabetizacaoPainelEducacionalCommand : IRequest<bool>
    {
        public IEnumerable<SondagemConsolidacaoNivelEscritaAlfabetizacaoDto> ConsolidacaoNivelEscritaAlfabetizacao { get; set; }
        public SalvarConsolidacaoNivelEscritaAlfabetizacaoPainelEducacionalCommand(IEnumerable<SondagemConsolidacaoNivelEscritaAlfabetizacaoDto> consolidacaoNivelEscritaAlfabetizacao)
        {
            ConsolidacaoNivelEscritaAlfabetizacao = consolidacaoNivelEscritaAlfabetizacao;
        }
    }
}

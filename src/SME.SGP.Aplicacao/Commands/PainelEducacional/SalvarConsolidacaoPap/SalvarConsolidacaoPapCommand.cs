using MediatR;
using SME.SGP.Dominio;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao.Commands.PainelEducacional.SalvarConsolidacaoPap
{
    public class SalvarConsolidacaoPapCommand : IRequest<bool>
    {
        public SalvarConsolidacaoPapCommand(IEnumerable<ConsolidacaoInformacoesPap> indicadoresPap) 
        {
            IndicadoresPap = indicadoresPap;    
        }

        public IEnumerable<ConsolidacaoInformacoesPap> IndicadoresPap { get; set; } 
    }
}
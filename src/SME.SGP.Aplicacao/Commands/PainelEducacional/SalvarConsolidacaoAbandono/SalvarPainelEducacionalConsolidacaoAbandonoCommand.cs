using MediatR;
using SME.SGP.Infra.Dtos.PainelEducacional.ConsolidacaoAbandono;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao.Commands.PainelEducacional.SalvarConsolidacaoAbandono
{
    public class SalvarPainelEducacionalConsolidacaoAbandonoCommand : IRequest<bool>
    {

        public SalvarPainelEducacionalConsolidacaoAbandonoCommand(
            IEnumerable<ConsolidacaoAbandonoDto> indicadoresDre,
            IEnumerable<ConsolidacaoAbandonoUeDto> indicadoresUe)
        {
            IndicadoresDre = indicadoresDre;
            IndicadoresUe = indicadoresUe;
        }

        public IEnumerable<ConsolidacaoAbandonoDto> IndicadoresDre { get; set; }
        public IEnumerable<ConsolidacaoAbandonoUeDto> IndicadoresUe { get; set; }
    }
}

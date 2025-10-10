using MediatR;
using SME.SGP.Infra.Dtos.PainelEducacional.ConsolidacaoAbandono;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao.Commands.PainelEducacional.SalvarConsolidacaoAbandono
{
    public class SalvarPainelEducacionalConsolidacaoAbandonoCommand : IRequest<bool>
    {

        public SalvarPainelEducacionalConsolidacaoAbandonoCommand(IEnumerable<ConsolidacaoAbandonoDto> indicadores) => Indicadores = indicadores;
        public IEnumerable<ConsolidacaoAbandonoDto> Indicadores { get; set; }
    }
}

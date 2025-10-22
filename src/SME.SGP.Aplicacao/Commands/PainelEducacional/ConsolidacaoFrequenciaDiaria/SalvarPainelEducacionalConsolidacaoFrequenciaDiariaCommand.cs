using MediatR;
using SME.SGP.Infra.Dtos.PainelEducacional.Frequencia;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao.Commands.PainelEducacional.ConsolidacaoFrequenciaDiaria
{
    public class SalvarPainelEducacionalConsolidacaoFrequenciaDiariaCommand : IRequest<bool>
    {
        public SalvarPainelEducacionalConsolidacaoFrequenciaDiariaCommand(IEnumerable<ConsolidacaoFrequenciaDiariaDreDto> indicadores)
        {
            Indicadores = indicadores;
        }
        public IEnumerable<ConsolidacaoFrequenciaDiariaDreDto> Indicadores { get; set; }
    }
}

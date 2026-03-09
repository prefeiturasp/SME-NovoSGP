using MediatR;
using SME.SGP.Infra.Dtos.PainelEducacional.Frequencia;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao.Commands.PainelEducacional.ConsolidacaoFrequenciaSemanal
{
    public class SalvarPainelEducacionalConsolidacaoFrequenciaSemanalCommand : IRequest<bool>
    {
        public SalvarPainelEducacionalConsolidacaoFrequenciaSemanalCommand(IEnumerable<ConsolidacaoFrequenciaSemanalDto> indicadores)
        {
            Indicadores = indicadores;
        }

        public IEnumerable<ConsolidacaoFrequenciaSemanalDto> Indicadores { get; set; }
    }
}

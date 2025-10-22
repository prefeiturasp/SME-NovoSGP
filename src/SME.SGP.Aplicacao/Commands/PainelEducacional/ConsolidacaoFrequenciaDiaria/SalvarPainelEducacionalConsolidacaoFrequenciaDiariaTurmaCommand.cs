using MediatR;
using SME.SGP.Infra.Dtos.PainelEducacional.Frequencia;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao.Commands.PainelEducacional.ConsolidacaoFrequenciaDiaria
{
    public class SalvarPainelEducacionalConsolidacaoFrequenciaDiariaTurmaCommand : IRequest<bool>
    {
        public SalvarPainelEducacionalConsolidacaoFrequenciaDiariaTurmaCommand(IEnumerable<ConsolidacaoFrequenciaDiariaTurmaDto> indicadores)
        {
            Indicadores = indicadores;
        }
        public IEnumerable<ConsolidacaoFrequenciaDiariaTurmaDto> Indicadores { get; set; }
    }
}

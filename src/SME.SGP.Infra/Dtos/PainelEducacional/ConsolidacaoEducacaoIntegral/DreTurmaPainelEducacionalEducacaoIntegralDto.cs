using System.Collections.Generic;

namespace SME.SGP.Infra.Dtos.PainelEducacional.ConsolidacaoEducacaoIntegral
{
    public class DreTurmaPainelEducacionalEducacaoIntegralDto
    {
        public string CodigoDre { get; set; }
        public List<TurmaPainelEducacionalDto> Turmas { get; set; }
    }
}

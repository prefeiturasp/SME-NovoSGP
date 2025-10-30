using System.Collections.Generic;

namespace SME.SGP.Infra.Dtos.PainelEducacional.ConsolidacaoDistorcaoIdade
{
    public class DreTurmaPainelEducacionalDistorcaoIdadeDto
    {
        public string CodigoDre { get; set; }
        public List<TurmaPainelEducacionalDto> Turmas { get; set; }
    }
}

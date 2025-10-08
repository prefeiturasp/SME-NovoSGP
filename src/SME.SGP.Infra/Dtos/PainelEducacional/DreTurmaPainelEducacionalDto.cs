using System.Collections.Generic;

namespace SME.SGP.Infra.Dtos.PainelEducacional
{
    public class DreTurmaPainelEducacionalDto
    {
        public string CodigoDre { get; set; }
        public List<TurmaPainelEducacionalDto> Turmas { get; set; }
    }
}

using System.Collections.Generic;

namespace SME.SGP.Infra.Dtos.PainelEducacional.ConsolidacaoFluenciaLeitoraUe
{
    public class DreTurmasFluenciaLeitoraUePainelEducacionalDto
    {
        public string CodigoDre { get; set; }
        public List<TurmaPainelEducacionalDto> Turmas { get; set; }
    }
}

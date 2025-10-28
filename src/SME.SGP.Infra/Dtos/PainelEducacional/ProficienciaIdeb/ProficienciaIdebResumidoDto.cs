using System.Collections.Generic;

namespace SME.SGP.Infra.Dtos.PainelEducacional.ProficienciaIdeb
{
    public class ProficienciaIdebResumidoDto
    {
        public List<ComponenteCurricularIdebResumidoDto> AnosIniciais { get; set; }
        public List<ComponenteCurricularIdebResumidoDto> AnosFinais { get; set; }
        public List<ComponenteCurricularIdebResumidoDto> EnsinoMedio { get; set; }
    }
}
using System.Collections.Generic;

namespace SME.SGP.Infra.Dtos.PainelEducacional
{
    public class ProficienciaIdebResumidoDto
    {
        public List<ComponenteCurricularIdebResumidoDto> AnosIniciais { get; set; }
        public List<ComponenteCurricularIdebResumidoDto> AnosFinais { get; set; }
    }

    public class ComponenteCurricularIdebResumidoDto
    {
        public int ComponenteCurricular { get; set; }
    }
}

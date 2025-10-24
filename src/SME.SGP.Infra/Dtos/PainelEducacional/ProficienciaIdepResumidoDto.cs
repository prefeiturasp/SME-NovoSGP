using System.Collections.Generic;

namespace SME.SGP.Infra.Dtos.PainelEducacional
{
    public class ProficienciaIdepResumidoDto
    {
        public List<ComponenteCurricularIdepResumidoDto> AnosIniciais { get; set; }
        public List<ComponenteCurricularIdepResumidoDto> AnosFinais { get; set; }
    }

    public class ComponenteCurricularIdepResumidoDto
    {
        public string ComponenteCurricular { get; set; }
        public decimal Percentual { get; set; }
    }
}

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
        public string ComponenteCurricular { get; set; }
        public decimal Percentual { get; set; }
    }
}

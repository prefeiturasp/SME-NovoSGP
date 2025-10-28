using SME.SGP.Dominio.Enumerados;
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
        public ComponenteCurricularEnum? ComponenteCurricular { get; set; }
        public decimal? Percentual { get; set; }
    }
}

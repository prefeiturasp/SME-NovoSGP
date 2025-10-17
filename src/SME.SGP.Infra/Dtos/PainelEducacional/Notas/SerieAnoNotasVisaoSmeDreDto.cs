using System.Collections.Generic;

namespace SME.SGP.Infra.Dtos.PainelEducacional.Notas
{
    public class SerieAnoNotasVisaoSmeDreDto
    {
        public string Nome { get; set; }
        public IEnumerable<ComponenteCurricularNotasVisaoSmeDreDto> ComponentesCurriculares { get; set; }
    }
}

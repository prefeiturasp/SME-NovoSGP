using System.Collections.Generic;

namespace SME.SGP.Infra.Dtos.PainelEducacional.Notas.VisaoSmeDre
{
    public class SerieAnoNotasVisaoSmeDreDto
    {
        public string Nome { get; set; }
        public IEnumerable<ComponenteCurricularNotasDto> ComponentesCurriculares { get; set; }
    }
}

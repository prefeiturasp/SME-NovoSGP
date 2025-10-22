using System.Collections.Generic;

namespace SME.SGP.Infra.Dtos.PainelEducacional.Notas.VisaoUe
{
    public class TurmaNotasVisaoUeDto
    {
        public string Nome { get; set; }
        public IEnumerable<ComponenteCurricularNotasDto> ComponentesCurriculares { get; set; }
    }
}

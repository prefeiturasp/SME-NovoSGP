using System.Collections.Generic;

namespace SME.SGP.Infra.Dtos.PainelEducacional.Notas.VisaoUe
{
    public class ModalidadeNotasVisaoUeDto
    {
        public string Nome { get; set; }
        public IEnumerable<TurmaNotasVisaoUeDto> Turmas { get; set; }
    }
}

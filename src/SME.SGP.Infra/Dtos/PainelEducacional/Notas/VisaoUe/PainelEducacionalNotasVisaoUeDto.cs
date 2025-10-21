using System.Collections.Generic;

namespace SME.SGP.Infra.Dtos.PainelEducacional.Notas.VisaoUe
{
    public class PainelEducacionalNotasVisaoUeDto
    {
        public IEnumerable<ModalidadeNotasVisaoUeDto> Modalidades { get; set; }
    }
}

using System.Collections.Generic;

namespace SME.SGP.Infra.Dtos.PainelEducacional.Notas
{
    public class PainelEducacionalNotasVisaoSmeDreDto
    {
        public IEnumerable<ModalidadeNotasVisaoSmeDreDto> Modalidades { get; set; }
    }
}

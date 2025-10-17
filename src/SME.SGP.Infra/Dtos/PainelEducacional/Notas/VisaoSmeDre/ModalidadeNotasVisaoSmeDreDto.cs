using System.Collections.Generic;

namespace SME.SGP.Infra.Dtos.PainelEducacional.Notas.VisaoSmeDre
{
    public class ModalidadeNotasVisaoSmeDreDto
    {
        public string Nome { get; set; }
        public IEnumerable<SerieAnoNotasVisaoSmeDreDto> SerieAno { get; set; }
    }
}

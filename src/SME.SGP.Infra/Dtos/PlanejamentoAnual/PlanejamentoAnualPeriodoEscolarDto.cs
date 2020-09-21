using System.Collections;
using System.Collections.Generic;

namespace SME.SGP.Infra
{
    public class PlanejamentoAnualPeriodoEscolarDto
    {
        public long Id { get; set; }
        public long PeriodoEscolarId { get; set; }
        public int Bimestre { get; set; }
        public IEnumerable<PlanejamentoAnualComponenteDto> Componentes { get; set; }
    }
}

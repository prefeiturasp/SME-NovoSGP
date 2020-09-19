using System.Collections.Generic;

namespace SME.SGP.Infra
{
    public class SalvarPlanejamentoAnualDto
    {
        public long Id { get; set; }
        public long PeriodoEscolarId { get; set; }
        public IEnumerable<PlanejamentoAnualComponenteDto> Componentes { get; set; }
    }
}

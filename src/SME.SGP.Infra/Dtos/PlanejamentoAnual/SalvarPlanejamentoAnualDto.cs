using System.Collections.Generic;

namespace SME.SGP.Infra
{
    public class SalvarPlanejamentoAnualDto
    {
        public IEnumerable<PlanejamentoAnualPeriodoEscolarDto> PeriodosEscolares { get; set; }
        public long Id { get; set; }
        //public long PeriodoEscolarId { get; set; }
        //public List<PlanejamentoAnualComponenteDto> Componentes { get; set; }
    }
}

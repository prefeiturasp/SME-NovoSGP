using System.Collections.Generic;

namespace SME.SGP.Infra
{
    public class PlanejamentoAnualPeriodoEscolarDto
    {
        public PlanejamentoAnualPeriodoEscolarDto()
        {
            Componentes = new List<PlanejamentoAnualComponenteDto>();
        }
        public long PeriodoEscolarId { get; set; }
        public int Bimestre { get; set; }
        public List<PlanejamentoAnualComponenteDto> Componentes { get; set; }
    }
}

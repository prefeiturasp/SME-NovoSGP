using System;

namespace SME.SGP.Infra.Dtos
{
    public class PeriodoCalendarioBimestrePorAnoLetivoModalidadeDto
    {
        public long PeriodoEscolarId { get; set; }
        public int Bimestre { get; set; }
        public DateTime PeriodoInicio { get; set; }
        public DateTime PeriodoFim { get; set; }
    }
}
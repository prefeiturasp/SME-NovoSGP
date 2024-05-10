using System;

namespace SME.SGP.Infra
{
    public class AusenciaPorDisciplinaDto
    {
        public int Bimestre { get; set; }
        public long? PeriodoEscolarId { get; set; }
        public DateTime PeriodoFim { get; set; }
        public DateTime PeriodoInicio { get; set; }
        public int TotalAusencias { get; set; }
    }
}
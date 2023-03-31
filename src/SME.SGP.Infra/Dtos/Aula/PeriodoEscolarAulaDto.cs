using System;

namespace SME.SGP.Infra
{
    public class PeriodoEscolarAulaDto
    {
        public long Id { get; set; }
        public DateTime DataInicio { get; set; }
        public DateTime DataFim { get; set; }
        public int Bimestre { get; set; }
        public long AulaId { get; set; }
        public DateTime DataAula { get; set; }
    }
}

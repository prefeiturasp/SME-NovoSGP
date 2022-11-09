using System;

namespace SME.SGP.Infra
{
    public class TurmaDataAulaComponenteQtdeAulasDto
    {
        public DateTime DataAula { get; set; }
        public string ComponenteCurricularCodigo { get; set; }
        public string TurmaCodigo { get; set; }
        public int AulasQuantidade { get; set; }
        public int Bimestre { get; set; }
        public long PeriodoEscolarId { get; set; }
    }
}

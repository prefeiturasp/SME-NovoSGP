using System;

namespace SME.SGP.Infra.Dtos
{
    public class TurmaComponenteQntAulasDto
    {
        public string ComponenteCurricularCodigo { get; set; }
        public string TurmaCodigo { get; set; }
        public int AulasQuantidade { get; set; }
        public int Bimestre { get; set; }
        public long PeriodoEscolarId { get; set; }
        public DateTime PeriodoInicio { get; set; }
        public DateTime PeriodoFim { get; set; }
        public string Professor { get; set; }
    }
}

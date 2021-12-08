using System;

namespace SME.SGP.Infra
{
    public class FiltroFrequenciaPorPeriodoDto
    {
        public DateTime DataInicio { get; set; }
        public DateTime DataFim { get; set; }
        public string TurmaId { get; set; }
        public string DisciplinaId { get; set; }
    }
}

using System;

namespace SME.SGP.Infra
{
    public class DiarioBordoSemDevolutivaDto
    {
        public int Bimestre { get; set; }
        public DateTime PeriodoInicio { get; set; }
        public DateTime PeriodoFim { get; set; }
        public DateTime DataAula { get; set; }
    }
}

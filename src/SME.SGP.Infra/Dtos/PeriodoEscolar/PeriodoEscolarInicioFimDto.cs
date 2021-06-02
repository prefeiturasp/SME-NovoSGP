using System;

namespace SME.SGP.Infra
{
    public class PeriodoEscolarInicioFimDto
    {
        public long Id { get; set; }
        public DateTime DataInicio { get; set; }
        public DateTime DataFim { get; set; }
        public int Bimestre { get; set; }
    }
}

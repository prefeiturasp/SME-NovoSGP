using System;

namespace SME.SGP.Infra
{
    public class FechamentoBimestreDto
    {
        public int Bimestre { get; set; }
        public DateTime FinalDoFechamento { get; set; }
        public long Id { get; set; }
        public DateTime InicioDoFechamento { get; set; }
        public DateTime FinalMaximo { get; set; }
        public DateTime InicioMinimo { get; set; }
    }
}
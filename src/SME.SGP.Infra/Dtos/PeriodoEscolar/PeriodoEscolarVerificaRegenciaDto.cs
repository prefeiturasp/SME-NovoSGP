using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Infra
{
    public class PeriodoEscolarVerificaRegenciaDto
    {
        public long Id { get; set; }
        public DateTime DataInicio { get; set; }
        public DateTime DataFim { get; set; }
        public int Bimestre { get; set; }
        public bool EhRegencia { get; set; }
        public long AulaId { get; set; }
        public DateTime DataAula { get; set; }
    }
}

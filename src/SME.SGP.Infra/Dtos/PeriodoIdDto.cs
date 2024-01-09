using System;

namespace SME.SGP.Infra
{
    public class PeriodoIdDto
    {
        public PeriodoIdDto()
        {}

        public long Id { get; set; }
        public DateTime Inicio { get; set; }
        public DateTime Fim { get; set; }
    }
}

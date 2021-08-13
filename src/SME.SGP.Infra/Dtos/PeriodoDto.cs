using System;

namespace SME.SGP.Infra
{
    public class PeriodoDto
    {
        public PeriodoDto(DateTime inicio, DateTime fim)
        {
            Inicio = inicio;
            Fim = fim;
        }

        public DateTime Inicio { get; set; }
        public DateTime Fim { get; set; }
    }
}

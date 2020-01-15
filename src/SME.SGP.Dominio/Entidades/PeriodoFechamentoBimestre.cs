using System;

namespace SME.SGP.Dominio
{
    public class PeriodoFechamentoBimestre
    {
        public PeriodoFechamentoBimestre(PeriodoEscolar periodoEscolar, DateTime inicioDoFechamento, DateTime finalDoFechamento)
        {
            FinalDoFechamento = finalDoFechamento;
            InicioDoFechamento = inicioDoFechamento;
            PeriodoEscolar = periodoEscolar;
            PeriodoEscolarId = periodoEscolar.Id;
        }

        public DateTime FinalDoFechamento { get; set; }
        public DateTime InicioDoFechamento { get; set; }
        public PeriodoEscolar PeriodoEscolar { get; set; }
        public long PeriodoEscolarId { get; set; }
    }
}
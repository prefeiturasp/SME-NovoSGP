using System;

namespace SME.SGP.Dominio
{
    public class FechamentoBimestre
    {
        public FechamentoBimestre(PeriodoEscolar periodoEscolar, DateTime inicioDoFechamento, DateTime finalDoFechamento)
        {
            FinalDoFechamento = finalDoFechamento;
            InicioDoFechamento = inicioDoFechamento;
            PeriodoEscolar = periodoEscolar;
            PeriodoEscolarId = periodoEscolar.Id;
        }

        protected FechamentoBimestre()
        {
        }

        public DateTime FinalDoFechamento { get; set; }
        public long Id { get; set; }
        public DateTime InicioDoFechamento { get; set; }
        public PeriodoEscolar PeriodoEscolar { get; set; }
        public long PeriodoEscolarId { get; set; }

        internal void AdicionarPeriodoEscolar(PeriodoEscolar periodoEscolar)
        {
            PeriodoEscolar = periodoEscolar;
        }
    }
}
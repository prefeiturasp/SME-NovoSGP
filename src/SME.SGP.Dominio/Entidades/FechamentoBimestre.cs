using System;

namespace SME.SGP.Dominio
{
    public class FechamentoBimestre
    {
        public FechamentoBimestre(long fechamentoId,
                                  PeriodoEscolar periodoEscolar,
                                  DateTime inicioDoFechamento,
                                  DateTime finalDoFechamento)
        {
            FechamentoId = fechamentoId;
            FinalDoFechamento = finalDoFechamento;
            InicioDoFechamento = inicioDoFechamento;
            PeriodoEscolar = periodoEscolar;
            PeriodoEscolarId = periodoEscolar.Id;
        }

        protected FechamentoBimestre()
        {
        }

        public Fechamento Fechamento { get; set; }
        public long FechamentoId { get; set; }
        public DateTime FinalDoFechamento { get; set; }
        public long Id { get; set; }
        public DateTime InicioDoFechamento { get; set; }
        public PeriodoEscolar PeriodoEscolar { get; set; }
        public long PeriodoEscolarId { get; set; }

        public void AdicionarPeriodoEscolar(PeriodoEscolar periodoEscolar)
        {
            PeriodoEscolar = periodoEscolar;
        }

        public void AtualizarDatas(DateTime inicioDoFechamento, DateTime finalDoFechamento)
        {
            if (inicioDoFechamento > finalDoFechamento)
            {
                throw new NegocioException("A data de início deve ser menor que a data final.");
            }
            InicioDoFechamento = inicioDoFechamento;
            FinalDoFechamento = finalDoFechamento;
        }
    }
}
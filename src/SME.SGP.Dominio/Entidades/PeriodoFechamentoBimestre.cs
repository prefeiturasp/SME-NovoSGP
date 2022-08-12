using System;

namespace SME.SGP.Dominio
{
    public class PeriodoFechamentoBimestre
    {
        public PeriodoFechamentoBimestre(long fechamentoId,
                                  PeriodoEscolar periodoEscolar,
                                  DateTime? inicioDoFechamento,
                                  DateTime? finalDoFechamento)
        {
            PeriodoFechamentoId = fechamentoId;
            if (inicioDoFechamento.HasValue)
                InicioDoFechamento = inicioDoFechamento.Value;
            if (finalDoFechamento.HasValue)
                FinalDoFechamento = finalDoFechamento.Value;
            PeriodoEscolar = periodoEscolar;
            PeriodoEscolarId = periodoEscolar.Id;
        }

        protected PeriodoFechamentoBimestre()
        {
        }

        public PeriodoFechamento PeriodoFechamento { get; set; }
        public long PeriodoFechamentoId { get; set; }
        public DateTime FinalDoFechamento { get; set; }
        public long Id { get; set; }
        public DateTime InicioDoFechamento { get; set; }
        public PeriodoEscolar PeriodoEscolar { get; set; }

        public long PeriodoEscolarId { get; set; }

        public void AdicionarPeriodoEscolar(PeriodoEscolar periodoEscolar)
        {
            PeriodoEscolar = periodoEscolar;
        }

        public void AtualizarDatas(DateTime? inicioDoFechamento, DateTime? finalDoFechamento)
        {
            if (inicioDoFechamento.HasValue)
                InicioDoFechamento = inicioDoFechamento.Value;

            if (finalDoFechamento.HasValue)
                FinalDoFechamento = finalDoFechamento.Value;
        }

        public bool DataDentroPeriodo(DateTime data)
        {
            return InicioDoFechamento.Date <= data.Date && FinalDoFechamento >= data.Date;
        }
    }
}
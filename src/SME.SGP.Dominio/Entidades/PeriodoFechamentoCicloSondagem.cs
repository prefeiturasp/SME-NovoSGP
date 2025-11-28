using System;

namespace SME.SGP.Dominio
{
    public class PeriodoFechamentoCicloSondagem
    {
        public PeriodoFechamentoCicloSondagem(long fechamentoId,
                                  DateTime? inicioDoFechamento,
                                  DateTime? finalDoFechamento)
        {
            PeriodoFechamentoId = fechamentoId;
            if (inicioDoFechamento.HasValue)
                InicioDoFechamento = inicioDoFechamento.Value;
            if (finalDoFechamento.HasValue)
                FinalDoFechamento = finalDoFechamento.Value;
        }

        public PeriodoFechamentoCicloSondagem()
        {
        }

        public long Id { get; set; }
        public int Ciclo { get; set; }
        public PeriodoFechamento PeriodoFechamento { get; set; }
        public long PeriodoFechamentoId { get; set; }
        public TipoCalendario TipoCalendario { get; set; }
        public long TipoCalendarioId { get; set; }
        public DateTime InicioDoFechamento { get; set; }
        public DateTime FinalDoFechamento { get; set; }

        public void AtualizarDatas(DateTime? inicioDoFechamento, DateTime? finalDoFechamento)
        {
            if (inicioDoFechamento.HasValue)
                InicioDoFechamento = inicioDoFechamento.Value;

            if (finalDoFechamento.HasValue)
                FinalDoFechamento = finalDoFechamento.Value;
        }
    }
}
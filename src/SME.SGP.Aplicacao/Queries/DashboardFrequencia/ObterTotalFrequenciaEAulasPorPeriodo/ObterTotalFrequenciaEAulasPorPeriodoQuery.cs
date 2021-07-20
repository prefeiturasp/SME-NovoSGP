using MediatR;
using SME.SGP.Infra;
using System;

namespace SME.SGP.Aplicacao
{
    public class ObterTotalFrequenciaEAulasPorPeriodoQuery : IRequest<TotalFrequenciaEAulasPorPeriodoDto>
    {
        public ObterTotalFrequenciaEAulasPorPeriodoQuery(int anoLetivo, long dreId, long ueId, int modalidade, int semestre, string anoTurma, DateTime dataInicio, DateTime dataFim, int mes, int tipoPeriodoDashboard)
        {
            AnoLetivo = anoLetivo;
            DreId = dreId;
            UeId = ueId;
            Modalidade = modalidade;
            Semestre = semestre;
            AnoTurma = anoTurma;
            DataInicio = dataInicio;
            DataFim = dataFim;
            Mes = mes;
            TipoPeriodoDashboard = tipoPeriodoDashboard;
        }

        public int AnoLetivo { get; set; }
        public long DreId { get; set; }
        public long UeId { get; set; }
        public int Modalidade { get; set; }
        public int Semestre { get; set; }
        public string AnoTurma { get; set; }
        public DateTime DataInicio { get; set; }
        public DateTime DataFim { get; set; }
        public int Mes { get; set; }
        public int TipoPeriodoDashboard { get; set; }
    }
}

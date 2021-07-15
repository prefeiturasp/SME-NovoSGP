using MediatR;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterDadosDashboardFrequenciaPorAnoTurmaQuery : IRequest<IEnumerable<FrequenciaAlunoDashboardDto>>
    {
        public ObterDadosDashboardFrequenciaPorAnoTurmaQuery(int anoLetivo, long dreId, long ueId, int modalidade, int semestre, int anoTurma, DateTime dataInicio, DateTime dataFim, int mes, int tipoPeriodoDashboard, bool visaoDre = false)
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
            VisaoDre = visaoDre;
        }

        public int AnoLetivo { get; set; }
        public long DreId { get; set; }
        public long UeId { get; set; }
        public int Modalidade { get; set; }
        public int Semestre { get; set; }
        public int AnoTurma { get; set; }
        public DateTime DataInicio { get; set; }
        public DateTime DataFim { get; set; }
        public int Mes { get; set; }
        public int TipoPeriodoDashboard { get; set; }
        public bool VisaoDre { get; set; }
    }
}

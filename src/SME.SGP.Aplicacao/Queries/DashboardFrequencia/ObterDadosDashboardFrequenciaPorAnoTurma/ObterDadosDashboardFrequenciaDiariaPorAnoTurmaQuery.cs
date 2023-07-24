using MediatR;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterDadosDashboardFrequenciaDiariaPorAnoTurmaQuery : IRequest<IEnumerable<FrequenciaAlunoDashboardDto>>
    {
        public ObterDadosDashboardFrequenciaDiariaPorAnoTurmaQuery(int anoLetivo, long dreId, long ueId, int modalidade, int semestre, long[] turmaIds, DateTime dataAula, bool visaoDre)
        {
            AnoLetivo = anoLetivo;
            DreId = dreId;
            UeId = ueId;
            Modalidade = modalidade;
            Semestre = semestre;
            TurmaIds = turmaIds;
            DataAula = dataAula;
            VisaoDre = visaoDre;
        }

        public int AnoLetivo { get; set; }
        public long DreId { get; set; }
        public long UeId { get; set; }
        public int Modalidade { get; set; }
        public int Semestre { get; set; }
        public long[] TurmaIds { get; set; }
        public DateTime DataAula { get; set; }
        public bool VisaoDre { get; set; }
    }
}

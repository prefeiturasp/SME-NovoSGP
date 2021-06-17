using System;

namespace SME.SGP.Infra.Dtos
{
    public class ConciliacaoFrequenciaTurmasSyncDto
    {
        public DateTime DataPeriodo { get; set; }

        public string TurmaCodigo { get; set; }

        public ConciliacaoFrequenciaTurmasSyncDto(DateTime dataPeriodo, string turmaCodigo)
        {
            this.DataPeriodo = dataPeriodo;
            this.TurmaCodigo = turmaCodigo;
        }
    }
}

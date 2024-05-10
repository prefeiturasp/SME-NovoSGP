using System;

namespace SME.SGP.Infra.Dtos
{
    public class ConciliacaoFrequenciaTurmasSyncDto
    {
        public DateTime DataPeriodo { get; set; }

        public string TurmaCodigo { get; set; }

        public bool Bimestral { get; set; }

        public bool Mensal {  get; set; }

        public ConciliacaoFrequenciaTurmasSyncDto(DateTime dataPeriodo, string turmaCodigo, bool bimestral, bool mensal)
        {
            this.DataPeriodo = dataPeriodo;
            this.TurmaCodigo = turmaCodigo;
            this.Bimestral = bimestral;
            this.Mensal = mensal;
        }
    }
}

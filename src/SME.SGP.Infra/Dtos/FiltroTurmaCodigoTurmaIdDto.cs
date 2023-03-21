using System;

namespace SME.SGP.Infra
{
    public class FiltroTurmaCodigoTurmaIdDto
    {
        public FiltroTurmaCodigoTurmaIdDto() { }
        public FiltroTurmaCodigoTurmaIdDto(string turmaCodigo, long turmaId, DateTime dataStatusTurmaEscola, DateTime? dataInicioPeriodo = null)
        {
            TurmaCodigo = turmaCodigo;
            TurmaId = turmaId;
            DataStatusTurmaEscola = dataStatusTurmaEscola;
            DataInicioPeriodo = dataInicioPeriodo;
        }

        public string TurmaCodigo { get; set; }
        public long TurmaId { get; set; }
        public DateTime DataStatusTurmaEscola { get; set; }
        public DateTime? DataInicioPeriodo { get; set; }
    }
}
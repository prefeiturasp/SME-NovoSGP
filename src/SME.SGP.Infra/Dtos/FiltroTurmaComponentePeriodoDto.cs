using System;

namespace SME.SGP.Infra
{
    public class FiltroTurmaComponentePeriodoDto : FiltroTurmaComponenteDto
    {
        public FiltroTurmaComponentePeriodoDto(string turmaCodigo, long componenteCurricularCodigo, DateTime periodoInicio, DateTime periodoFim) : base(turmaCodigo, componenteCurricularCodigo)
        {
            PeriodoInicio = periodoInicio;
            PeriodoFim = periodoFim;
        }

        public DateTime PeriodoInicio { get; set; }
        public DateTime PeriodoFim { get; set; }
    }
}

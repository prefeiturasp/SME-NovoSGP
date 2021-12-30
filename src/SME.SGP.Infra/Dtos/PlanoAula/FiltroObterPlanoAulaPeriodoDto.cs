using System;
using System.Diagnostics;

namespace SME.SGP.Infra
{
    [DebuggerDisplay("turmaCodigo:{turmaCodigo} - componenteCurricularCodigo:{componenteCurricularCodigo} - aulaInicio:{aulaInicio} - aulaFim:{aulaFim}")]
    public class FiltroObterPlanoAulaPeriodoDto
    {
        public FiltroObterPlanoAulaPeriodoDto(string turmaCodigo, string componenteCurricularCodigo, DateTime aulaInicio, DateTime aulaFim)
        {
            TurmaCodigo = turmaCodigo;
            ComponenteCurricularCodigo = componenteCurricularCodigo;
            AulaInicio = aulaInicio;
            AulaFim = aulaFim;
        }

        public string TurmaCodigo { get; set; }
        public string ComponenteCurricularCodigo { get; set; }
        public DateTime AulaInicio { get; set; }
        public DateTime AulaFim { get; set; }
    }
}

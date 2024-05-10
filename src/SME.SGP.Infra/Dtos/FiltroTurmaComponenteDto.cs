using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Infra
{
    public class FiltroTurmaComponenteDto
    {
        public FiltroTurmaComponenteDto() {}
        public FiltroTurmaComponenteDto(string turmaCodigo, long componenteCurricularCodigo) 
        {
            TurmaCodigo = turmaCodigo;
            ComponenteCurricularCodigo = componenteCurricularCodigo;
        }

        public string TurmaCodigo { get; set; }

        public long ComponenteCurricularCodigo { get; set; }
    }
}

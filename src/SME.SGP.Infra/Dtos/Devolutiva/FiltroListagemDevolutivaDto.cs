using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Infra
{
    public class FiltroListagemDevolutivaDto
    {
        public FiltroListagemDevolutivaDto(string turmaCodigo, long componenteCurricularCodigo, DateTime? dataReferencia)
        {
            TurmaCodigo = turmaCodigo;
            ComponenteCurricularCodigo = componenteCurricularCodigo;
            DataReferencia = dataReferencia;
        }

        public string TurmaCodigo { get; set; }
        public long ComponenteCurricularCodigo { get; set; }
        public DateTime? DataReferencia { get; set; }
    }
}

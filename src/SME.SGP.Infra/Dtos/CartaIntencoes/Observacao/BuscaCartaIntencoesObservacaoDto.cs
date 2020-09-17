using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Infra
{
    public class BuscaCartaIntencoesObservacaoDto
    {
        public BuscaCartaIntencoesObservacaoDto(string turmaCodigo, long componenteCurricularId)
        {
            TurmaCodigo = turmaCodigo;
            ComponenteCurricularId = componenteCurricularId;
        }

        public string TurmaCodigo { get; set; }
        public long ComponenteCurricularId { get; set; }
    }
}

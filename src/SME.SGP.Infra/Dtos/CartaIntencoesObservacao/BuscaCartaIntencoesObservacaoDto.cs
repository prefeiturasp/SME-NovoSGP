using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Infra
{
    public class BuscaCartaIntencoesObservacaoDto
    {
        public BuscaCartaIntencoesObservacaoDto(long turmaId, long componenteCurricularId)
        {
            TurmaId = turmaId;
            ComponenteCurricularId = componenteCurricularId;
        }

        public long TurmaId { get; set; }
        public long ComponenteCurricularId { get; set; }
    }
}

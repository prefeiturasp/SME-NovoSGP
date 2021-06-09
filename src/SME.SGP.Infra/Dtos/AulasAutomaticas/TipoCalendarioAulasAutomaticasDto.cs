using SME.SGP.Dominio;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Infra
{
    public class TipoCalendarioAulasAutomaticasDto
    {
        public TipoCalendarioAulasAutomaticasDto(long tipoCalendarioId, Modalidade modalidade)
        {
            TipoCalendarioId = tipoCalendarioId;
            Modalidade = modalidade;
        }

        public long TipoCalendarioId { get; set; }
        public Modalidade Modalidade { get; set; }
    }
}

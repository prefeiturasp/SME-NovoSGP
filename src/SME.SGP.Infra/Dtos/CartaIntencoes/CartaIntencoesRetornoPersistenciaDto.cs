using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Infra.Dtos
{
    public class CartaIntencoesRetornoPersistenciaDto
    {
        public long PeriodoEscolarId { get; set; }

        public AuditoriaDto Auditoria { get; set; }
    }
}

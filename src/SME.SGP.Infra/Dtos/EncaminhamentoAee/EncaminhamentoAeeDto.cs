using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Infra
{
    public class EncaminhamentoAEEDto
    {
        public long Id { get; set; }
        public long TurmaId { get; set; }
        public string AlunoCodigo { get; set; }
        public AuditoriaDto Auditoria { get; set; }
    }
}

using System;
using SME.SGP.Dominio;
using System.Collections.Generic;

namespace SME.SGP.Infra
{
    public class FrequenciaAuditoriaAulaDto
    {
        public FrequenciaAuditoriaAulaDto()
        {}

        public long? AulaIdComErro { get; set; }
        public DateTime? DataAulaComErro { get; set; }
        public AuditoriaDto Auditoria { get; set; }
    }
}

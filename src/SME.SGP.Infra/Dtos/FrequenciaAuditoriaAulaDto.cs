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
        public DateTime? DataAula { get; set; }
        public string TurmaId { get; set; }
        public string DisciplinaId { get; set; }
        public AuditoriaDto Auditoria { get; set; }
    }
}

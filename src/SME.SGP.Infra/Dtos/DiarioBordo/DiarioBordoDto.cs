using SME.SGP.Dominio;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Infra.Dtos
{
    public class DiarioBordoDto
    {
        public long AulaId { get; set; }
        public AulaDto Aula { get; set; }
        public long? DevolutivaId { get; set; }
        public string Devolutivas { get; set; }

        public string Planejamento { get; set; }
        public string ReflexoesReplanejamento { get; set; }

        public bool Excluido { get; set; }
        public bool Migrado { get; set; }

        public bool TemPeriodoAberto { get; set; }

        public AuditoriaDto Auditoria { get; set; }
    }
}

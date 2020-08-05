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
        public DevolutivaDiarioBordoDto Devolutiva { get; set; }

        public string Planejamento { get; set; }
        public string ReflexoesReplanejamento { get; set; }

        public bool Excluido { get; set; }
        public bool Migrado { get; set; }
    }
}

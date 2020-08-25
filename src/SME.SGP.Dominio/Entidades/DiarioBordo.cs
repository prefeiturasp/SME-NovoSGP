using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Dominio
{
    public class DiarioBordo: EntidadeBase
    {
        public long AulaId { get; set; }
        public Aula Aula { get; set; }
        public long? DevolutivaId { get; set; }
        public DevolutivaDiarioBordo Devolutiva { get; set; }

        public string Planejamento { get; set; }
        public string ReflexoesReplanejamento { get; set; }

        public bool Excluido { get; set; }
        public bool Migrado { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Dominio
{
    public class ProcessoExecutando: EntidadeBase
    {
        public TipoProcesso TipoProcesso { get; set; }
        public string TurmaId { get; set; }
        public string DisciplinaId { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Dominio
{
    public class AcompanhamentoAluno : EntidadeBase
    {
        public Turma Turma { get; set; }
        public long TurmaId { get; set; }

        public string AlunoCodigo { get; set; }

        public bool Excluido { get; set; }
    }
}

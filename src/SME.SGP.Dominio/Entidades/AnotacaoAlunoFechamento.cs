using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Dominio
{
    public class AnotacaoAlunoFechamento : EntidadeBase
    {
        public long FechamentoTurmaDisciplinaId { get; set; }
        public string AlunoCodigo { get; set; }
        public string Anotacao { get; set; }

        public bool Excluido { get; set; }
    }
}

using SME.SGP.Dominio.Enumerados;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Dominio
{
    public class EncaminhamentoAEE : EntidadeBase
    {
        public Turma Turma { get; set; }
        public long TurmaId { get; set; }

        public string AlunoCodigo { get; set; }
        public string AlunoNome { get; set; }
        public SituacaoAEE Situacao { get; set; }
        public bool Excluido { get; set; }
    }
}

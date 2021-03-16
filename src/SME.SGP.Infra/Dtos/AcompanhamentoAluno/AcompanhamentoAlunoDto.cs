using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Infra
{
    public class AcompanhamentoAlunoDto
    {
        public long AcompanhamentoAlunoId { get; set; }
        public long AcompanhamentoAlunoSemestreId { get; set; }
        public long TurmaId { get; set; }
        public int Semestre { get; set; }
        public string AlunoCodigo { get; set; }
        public string Observacoes { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Infra
{
    public class AlunosFaltososDto
    {
        public string TurmaId { get; set; }
        public DateTime DataAula { get; set; }
        public string CodigoAluno { get; set; }
        public int QuantidadeAulas { get; set; }
        public int QuantidadeFaltas { get; set; }
    }
}

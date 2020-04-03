using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SME.SGP.Infra
{
    public class AnotacaoAlunoDto
    {
        public long FechamentoId { get; set; }
        public string CodigoAluno { get; set; }
        public string Anotacao { get; set; }
    }
}

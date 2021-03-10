using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Dominio
{
    public class Questionario : EntidadeBase
    {
        public string Nome { get; set; }
        public TipoQuestionario Tipo { get; set; }
        public bool Excluido { get; set; }
    }
}

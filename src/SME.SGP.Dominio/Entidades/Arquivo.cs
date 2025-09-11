using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Dominio
{
    public class Arquivo : EntidadeBase
    {
        public string Nome { get; set; }
        public Guid Codigo { get; set; }
        public string TipoConteudo { get; set; }
        public TipoArquivo Tipo { get; set; }
    }
}

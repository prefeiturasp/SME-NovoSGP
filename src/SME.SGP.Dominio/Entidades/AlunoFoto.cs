using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Dominio
{
    public class AlunoFoto : EntidadeBase
    {
        public Arquivo Arquivo { get; set; }
        public long ArquivoId { get; set; }
        public Arquivo Miniatura { get; set; }
        public long? MiniaturaId { get; set; }
        public string AlunoCodigo { get; set; }
        public bool Excluido { get; set; }
    }
}

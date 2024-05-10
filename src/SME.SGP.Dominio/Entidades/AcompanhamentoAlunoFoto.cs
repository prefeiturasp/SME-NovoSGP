using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Dominio
{
    public class AcompanhamentoAlunoFoto : EntidadeBase
    {
        public AcompanhamentoAlunoSemestre AcompanhamentoAlunoSemestre { get; set; }
        public long AcompanhamentoAlunoSemestreId { get; set; }

        public Arquivo Arquivo { get; set; }
        public long ArquivoId { get; set; }

        public Arquivo Miniatura { get; set; }
        public long? MiniaturaId { get; set; }

        public AcompanhamentoAlunoFoto FotoOriginal { get; set; }

        public bool Excluido { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Dominio
{
    public class SecaoEncaminhamentoNAAPA : EntidadeBase
    {
        public Questionario Questionario { get; set; }
        public long QuestionarioId { get; set; }

        public string Nome { get; set; }
        public int Ordem { get; set; }
        public int Etapa { get; set; }
        public bool Excluido { get; set; }
        public string? NomeComponente { get; set; }
        public EncaminhamentoNAAPASecao EncaminhamentoNAAPASecao { get; set; }
    }
}

using SME.SGP.Dominio.Entidades;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace SME.SGP.Dominio
{
    [ExcludeFromCodeCoverage]
    public class EncaminhamentoNAAPASecao : EntidadeBase
    {
        public EncaminhamentoNAAPASecao()
        {
            Questoes = new List<QuestaoEncaminhamentoNAAPA>();
            QuestoesEscolar = new List<QuestaoEncaminhamentoEscolar>();
        }

        public EncaminhamentoNAAPA EncaminhamentoNAAPA { get; set; }
        public long? EncaminhamentoNAAPAId { get; set; }

        public SecaoEncaminhamentoNAAPA SecaoEncaminhamentoNAAPA { get; set; }
        public long SecaoEncaminhamentoNAAPAId { get; set; }

        public bool Concluido { get; set; }
        public bool Excluido { get; set; }

        public List<QuestaoEncaminhamentoNAAPA> Questoes { get; set; }

        public List<QuestaoEncaminhamentoEscolar> QuestoesEscolar { get; set; }

        public EncaminhamentoEscolar EncaminhamentoEscolar { get; set; }
        public long? EncaminhamentoEscolarId { get; set; }
    }
}

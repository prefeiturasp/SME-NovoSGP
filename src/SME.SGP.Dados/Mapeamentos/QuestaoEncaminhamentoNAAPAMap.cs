using SME.SGP.Dominio;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Dados
{
    public class QuestaoEncaminhamentoNAAPAMap : BaseMap<QuestaoEncaminhamentoNAAPA>
    {
        public QuestaoEncaminhamentoNAAPAMap()
        {
            ToTable("encaminhamento_naapa_questao");
            Map(c => c.EncaminhamentoNAAPASecaoId).ToColumn("encaminhamento_naapa_secao_id");
            Map(c => c.QuestaoId).ToColumn("questao_id");
            Map(c => c.Excluido).ToColumn("excluido");
        }
    }
}

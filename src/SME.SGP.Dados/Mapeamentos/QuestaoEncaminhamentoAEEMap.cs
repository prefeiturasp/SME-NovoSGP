using SME.SGP.Dominio;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Dados
{
    public class QuestaoEncaminhamentoAEEMap : BaseMap<QuestaoEncaminhamentoAEE>
    {
        public QuestaoEncaminhamentoAEEMap()
        {
            ToTable("questao_encaminhamento_aee");
            Map(c => c.EncaminhamentoAEESecaoId).ToColumn("encaminhamento_aee_secao_id");
            Map(c => c.QuestaoId).ToColumn("questao_id");
            Map(c => c.Excluido).ToColumn("excluido");
        }
    }
}

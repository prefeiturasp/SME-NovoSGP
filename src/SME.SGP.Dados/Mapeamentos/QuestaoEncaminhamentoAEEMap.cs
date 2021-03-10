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
            Map(a => a.EncaminhamentoAEESecaoId).ToColumn("encaminhamento_aee_secao_id");
            Map(a => a.QuestaoId).ToColumn("questao_id");
        }
    }
}

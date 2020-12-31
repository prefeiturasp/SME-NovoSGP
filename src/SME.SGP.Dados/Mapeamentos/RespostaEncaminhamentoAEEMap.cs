using SME.SGP.Dominio;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Dados
{
    public class RespostaEncaminhamentoAEEMap : BaseMap<RespostaEncaminhamentoAEE>
    {
        public RespostaEncaminhamentoAEEMap()
        {
            ToTable("resposta_encaminhamento_aee");
            Map(a => a.QuestaoEncaminhamentoId).ToColumn("questao_encaminhamento_id");
            Map(a => a.RespostaId).ToColumn("resposta_id");
            Map(a => a.ArquivoId).ToColumn("arquivo_id");
        }
    }
}

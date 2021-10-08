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
            Map(c => c.QuestaoEncaminhamentoId).ToColumn("questao_encaminhamento_id");
            Map(c => c.RespostaId).ToColumn("resposta_id");
            Map(c => c.ArquivoId).ToColumn("arquivo_id");
            Map(c => c.Texto).ToColumn("texto");
            Map(c => c.Excluido).ToColumn("excluido");
        }
    }
}

using SME.SGP.Dominio;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Dados
{
    public class RespostaEncaminhamentoNAAPAMap : BaseMap<RespostaEncaminhamentoNAAPA>
    {
        public RespostaEncaminhamentoNAAPAMap()
        {
            ToTable("encaminhamento_naapa_resposta");
            Map(c => c.QuestaoEncaminhamentoId).ToColumn("questao_encaminhamento_id");
            Map(c => c.RespostaId).ToColumn("resposta_id");
            Map(c => c.ArquivoId).ToColumn("arquivo_id");
            Map(c => c.Texto).ToColumn("texto");
            Map(c => c.Excluido).ToColumn("excluido");
        }
    }
}

using SME.SGP.Dominio;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Dados
{
    public class SecaoEncaminhamentoAEEMap : BaseMap<SecaoEncaminhamentoAEE>
    {
        public SecaoEncaminhamentoAEEMap()
        {
            ToTable("secao_encaminhamento_aee");
            Map(c => c.QuestionarioId).ToColumn("questionario_id");
            Map(c => c.Nome).ToColumn("nome");
            Map(c => c.Ordem).ToColumn("ordem");
            Map(c => c.Etapa).ToColumn("etapa");
            Map(c => c.Excluido).ToColumn("excluido");
        }
    }
}

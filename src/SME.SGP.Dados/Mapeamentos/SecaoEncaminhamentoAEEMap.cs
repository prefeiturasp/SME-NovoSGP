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
            Map(a => a.QuestionarioId).ToColumn("questionario_id");
        }
    }
}

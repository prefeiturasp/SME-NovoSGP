using SME.SGP.Dominio;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Dados
{
    public class EncaminhamentoAEESecaoMap : BaseMap<EncaminhamentoAEESecao>
    {
        public EncaminhamentoAEESecaoMap()
        {
            ToTable("encaminhamento_aee_secao");
            Map(c => c.EncaminhamentoAEEId).ToColumn("encaminhamento_aee_id");
            Map(c => c.SecaoEncaminhamentoAEEId).ToColumn("secao_encaminhamento_id");
            Map(c => c.Concluido).ToColumn("concluido");
            Map(c => c.Excluido).ToColumn("excluido");
        }
    }
}

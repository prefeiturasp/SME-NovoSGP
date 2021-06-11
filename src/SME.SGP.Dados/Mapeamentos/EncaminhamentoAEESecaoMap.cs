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
            Map(a => a.EncaminhamentoAEEId).ToColumn("encaminhamento_aee_id");
            Map(a => a.SecaoEncaminhamentoAEEId).ToColumn("secao_encaminhamento_id");
        }
    }
}

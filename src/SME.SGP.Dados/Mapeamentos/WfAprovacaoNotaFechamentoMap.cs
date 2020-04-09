using Dapper.FluentMap.Dommel.Mapping;
using SME.SGP.Dominio;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Dados.Mapeamentos
{
    public class WfAprovacaoNotaFechamentoMap: DommelEntityMap<WfAprovacaoNotaFechamento>
    {
        public WfAprovacaoNotaFechamentoMap()
        {
            ToTable("wf_aprovacao_nota_fechamento");
            Map(c => c.WfAprovacaoId).ToColumn("wf_aprovacao_id");
            Map(c => c.FechamentoNotaId).ToColumn("fechamento_nota_id");
            Map(c => c.ConceitoId).ToColumn("conceito_id");
        }
    }
}

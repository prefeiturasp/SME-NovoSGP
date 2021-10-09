using Dapper.FluentMap.Dommel.Mapping;
using SME.SGP.Dominio;

namespace SME.SGP.Dados.Mapeamentos
{
    public class WfAprovacaoNotaFechamentoMap: DommelEntityMap<WfAprovacaoNotaFechamento>
    {
        public WfAprovacaoNotaFechamentoMap()
        {
            ToTable("wf_aprovacao_nota_fechamento");
            Map(c => c.Id).ToColumn("id").IsIdentity().IsKey();
            Map(c => c.WfAprovacaoId).ToColumn("wf_aprovacao_id");
            Map(c => c.FechamentoNotaId).ToColumn("fechamento_nota_id");
            Map(c => c.Nota).ToColumn("nota");
            Map(c => c.ConceitoId).ToColumn("conceito_id");
        }
    }
}

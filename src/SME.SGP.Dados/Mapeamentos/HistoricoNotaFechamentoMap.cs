using Dapper.FluentMap.Dommel.Mapping;
using SME.SGP.Dominio;

namespace SME.SGP.Dados.Mapeamentos
{
    public class HistoricoNotaFechamentoMap : DommelEntityMap<HistoricoNotaFechamento>
    {
        public HistoricoNotaFechamentoMap()
        {
            ToTable("historico_nota_fechamento");
            Map(c => c.HistoricoNotaId).ToColumn("historico_nota_id");
            Map(c => c.Id).ToColumn("id").IsIdentity().IsKey();
            Map(c => c.FechamentoNotaId).ToColumn("fechamento_nota_id");
            Map(c => c.WorkFlowId).ToColumn("wf_aprovacao_id");
        }
    }
}

using Dapper.FluentMap.Dommel.Mapping;
using SME.SGP.Dominio;

namespace SME.SGP.Dados.Mapeamentos
{
    public class HistoricoNotaFechamentoMap : DommelEntityMap<HistoricoNotaFechamento>
    {
        public HistoricoNotaFechamentoMap()
        {
            ToTable("historico_nota_fechamento");
            Map(e => e.HistoricoNotaId).ToColumn("historico_nota_id");
            Map(e => e.FechamentoNotaId).ToColumn("fechamento_nota_id");
            Map(e => e.WorkFlowId).ToColumn("wf_aprovacao_id");
        }
    }
}

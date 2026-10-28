using SME.SGP.Dominio;

namespace SME.SGP.Dados.Mapeamentos
{
    public class HistoricoNotaFechamentoMap : SimpleMap<HistoricoNotaFechamento>
    {
        public HistoricoNotaFechamentoMap()
        {
            ToTable("historico_nota_fechamento");
            Map(nameof(HistoricoNotaFechamento.HistoricoNotaId), "historico_nota_id");
            Map(nameof(HistoricoNotaFechamento.FechamentoNotaId), "fechamento_nota_id");
            Map(nameof(HistoricoNotaFechamento.WorkFlowId), "wf_aprovacao_id");
        }
    }
}
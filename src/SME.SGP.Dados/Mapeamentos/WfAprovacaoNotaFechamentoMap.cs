using Dapper.FluentMap.Dommel.Mapping;
using SME.SGP.Dominio;

namespace SME.SGP.Dados.Mapeamentos
{
    public class WfAprovacaoNotaFechamentoMap: BaseMap<WfAprovacaoNotaFechamento>
    {
        public WfAprovacaoNotaFechamentoMap()
        {
            ToTable("wf_aprovacao_nota_fechamento");
            Map(c => c.WfAprovacaoId).ToColumn("wf_aprovacao_id");
            Map(c => c.FechamentoNotaId).ToColumn("fechamento_nota_id");
            Map(c => c.Nota).ToColumn("nota");
            Map(c => c.ConceitoId).ToColumn("conceito_id");
            Map(c => c.Excluido).ToColumn("excluido");
            Map(c => c.NotaAnterior).ToColumn("nota_anterior");
            Map(c => c.ConceitoIdAnterior).ToColumn("conceito_id_anterior");
        }
    }
}

using SME.SGP.Dominio;

namespace SME.SGP.Dados.Mapeamentos
{
    public class WfAprovacaoNotaFechamentoMap : BaseMap<WfAprovacaoNotaFechamento>
    {
        public WfAprovacaoNotaFechamentoMap()
        {
            ToTable("wf_aprovacao_nota_fechamento");
            Map(nameof(WfAprovacaoNotaFechamento.WfAprovacaoId), "wf_aprovacao_id");
            Map(nameof(WfAprovacaoNotaFechamento.FechamentoNotaId), "fechamento_nota_id");
            Map(nameof(WfAprovacaoNotaFechamento.Nota), "nota");
            Map(nameof(WfAprovacaoNotaFechamento.ConceitoId), "conceito_id");
            Map(nameof(WfAprovacaoNotaFechamento.Excluido), "excluido");
            Map(nameof(WfAprovacaoNotaFechamento.NotaAnterior), "nota_anterior");
            Map(nameof(WfAprovacaoNotaFechamento.ConceitoIdAnterior), "conceito_id_anterior");
        }
    }
}
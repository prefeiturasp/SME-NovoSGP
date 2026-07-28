using SME.SGP.Dominio;

namespace SME.SGP.Dados.Mapeamentos
{
    public class WfAprovacaoNotaConselhoMap : BaseEntityMap<WFAprovacaoNotaConselho>
    {
        public WfAprovacaoNotaConselhoMap()
        {
            ToTable("wf_aprovacao_nota_conselho");
            Map(nameof(WFAprovacaoNotaConselho.WfAprovacaoId), "wf_aprovacao_id");
            Map(nameof(WFAprovacaoNotaConselho.ConselhoClasseNotaId), "conselho_classe_nota_id");
            Map(nameof(WFAprovacaoNotaConselho.UsuarioSolicitanteId), "usuario_solicitante_id");
            Map(nameof(WFAprovacaoNotaConselho.Nota), "nota");
            Map(nameof(WFAprovacaoNotaConselho.ConceitoId), "conceito_id");
            Map(nameof(WFAprovacaoNotaConselho.NotaAnterior), "nota_anterior");
            Map(nameof(WFAprovacaoNotaConselho.ConceitoIdAnterior), "conceito_id_anterior");
            Map(nameof(WFAprovacaoNotaConselho.Excluido), "excluido");
        }
    }
}
using SME.SGP.Dominio;

namespace SME.SGP.Dados.Mapeamentos
{
    public class WfAprovacaoNotaConselhoMap : BaseMap<WFAprovacaoNotaConselho>
    {
        public WfAprovacaoNotaConselhoMap()
        {
            ToTable("wf_aprovacao_nota_conselho");
            Map(c => c.WfAprovacaoId).ToColumn("wf_aprovacao_id");
            Map(c => c.ConselhoClasseNotaId).ToColumn("conselho_classe_nota_id");
            Map(c => c.UsuarioSolicitanteId).ToColumn("usuario_solicitante_id");
            Map(c => c.Nota).ToColumn("nota");
            Map(c => c.ConceitoId).ToColumn("conceito_id");
            Map(c => c.NotaAnterior).ToColumn("nota_anterior");
            Map(c => c.ConceitoIdAnterior).ToColumn("conceito_id_anterior");
            Map(c => c.Excluido).ToColumn("excluido");
        }
    }
}

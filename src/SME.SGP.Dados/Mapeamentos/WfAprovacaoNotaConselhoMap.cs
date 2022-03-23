using Dapper.FluentMap.Dommel.Mapping;
using SME.SGP.Dominio;

namespace SME.SGP.Dados.Mapeamentos
{
    public class WfAprovacaoNotaConselhoMap : DommelEntityMap<WFAprovacaoNotaConselho>
    {
        public WfAprovacaoNotaConselhoMap()
        {
            ToTable("wf_aprovacao_nota_conselho");
            Map(c => c.WfAprovacaoId).ToColumn("wf_aprovacao_id");
            Map(c => c.ConselhoClasseNotaId).ToColumn("conselho_classe_nota_id");
            Map(c => c.ConceitoId).ToColumn("conceito_id");
            Map(c => c.UsuarioSolicitanteId).ToColumn("usuario_solicitante_id");
        }
    }
}

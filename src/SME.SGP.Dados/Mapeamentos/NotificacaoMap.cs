using SME.SGP.Dominio;

namespace SME.SGP.Dados.Mapeamentos
{
    public class NotificacaoMap : BaseMap<Notificacao>
    {
        public NotificacaoMap()
        {
            ToTable("notificacao");
            Map(c => c.Usuario).Ignore();

            Map(c => c.UeId).ToColumn("ue_id");
            Map(c => c.DreId).ToColumn("dre_id");
            Map(c => c.TurmaId).ToColumn("turma_id");
            Map(c => c.UsuarioId).ToColumn("usuario_id");

            Map(c => c.WorkflowAprovacaoNivel).Ignore();
        }
    }
}

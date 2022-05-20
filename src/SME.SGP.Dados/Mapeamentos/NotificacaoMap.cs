using SME.SGP.Dominio;

namespace SME.SGP.Dados.Mapeamentos
{
    public class NotificacaoMap : BaseMap<Notificacao>
    {
        public NotificacaoMap()
        {
            ToTable("notificacao");
            Map(c => c.Ano).ToColumn("ano");
            Map(c => c.Categoria).ToColumn("categoria");
            Map(c => c.Codigo).ToColumn("codigo");
            Map(c => c.DreId).ToColumn("dre_id");
            Map(c => c.Excluida).ToColumn("excluida");
            Map(c => c.Mensagem).ToColumn("mensagem");
            Map(c => c.Status).ToColumn("status");
            Map(c => c.Tipo).ToColumn("tipo");
            Map(c => c.Titulo).ToColumn("titulo");
            Map(c => c.TurmaId).ToColumn("turma_id");
            Map(c => c.UeId).ToColumn("ue_id");
            Map(c => c.Usuario).Ignore();
            Map(c => c.UsuarioId).ToColumn("usuario_id");
            Map(c => c.DeveAprovar).Ignore();
            Map(c => c.WorkflowAprovacaoNivel).Ignore();
            Map(c => c.DeveMarcarComoLido).Ignore();
            Map(c => c.PodeRemover).Ignore();
        }
    }
}

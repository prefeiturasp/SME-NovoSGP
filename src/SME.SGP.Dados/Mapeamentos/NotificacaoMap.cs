using SME.SGP.Dominio;

namespace SME.SGP.Dados.Mapeamentos
{
    public class NotificacaoMap : BaseEntityMap<Notificacao>
    {
        public NotificacaoMap()
        {
            ToTable("notificacao");
            Map(nameof(Notificacao.Ano), "ano");
            Map(nameof(Notificacao.Categoria), "categoria");
            Map(nameof(Notificacao.Codigo), "codigo");
            Map(nameof(Notificacao.DreId), "dre_id");
            Map(nameof(Notificacao.Excluida), "excluida");
            Map(nameof(Notificacao.Mensagem), "mensagem");
            Map(nameof(Notificacao.Status), "status");
            Map(nameof(Notificacao.Tipo), "tipo");
            Map(nameof(Notificacao.Titulo), "titulo");
            Map(nameof(Notificacao.TurmaId), "turma_id");
            Map(nameof(Notificacao.UeId), "ue_id");
            Map(nameof(Notificacao.UsuarioId), "usuario_id");
        }
    }
}
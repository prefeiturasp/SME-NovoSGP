using SME.SGP.Dominio.Entidades;

namespace SME.SGP.Dados.Mapeamentos
{
    public class NotificacaoMap : BaseMap<Notificacao>
    {
        public NotificacaoMap()
        {
            ToTable("notificacao");
            Map(c => c.UsuarioId).ToColumn("usuario_id");
            Map(c => c.EscolaId).ToColumn("escola_id");
            Map(c => c.DreId).ToColumn("dre_id");

        }
    }
}

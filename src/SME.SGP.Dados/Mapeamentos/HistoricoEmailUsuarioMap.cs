using SME.SGP.Dominio;

namespace SME.SGP.Dados.Mapeamentos
{
    public class HistoricoEmailUsuarioMap : BaseMap<HistoricoEmailUsuario>
    {
        public HistoricoEmailUsuarioMap()
        {
            Map(c => c.UsuarioId).ToColumn("usuario_id");
            Map(c => c.Email).ToColumn("email");
            Map(c => c.Acao).ToColumn("tipo_acao");
            ToTable("historico_email_usuario");
        }
    }
}

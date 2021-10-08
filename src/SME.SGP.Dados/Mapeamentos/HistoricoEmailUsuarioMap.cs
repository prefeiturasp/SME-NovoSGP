using SME.SGP.Dominio;

namespace SME.SGP.Dados.Mapeamentos
{
    public class HistoricoEmailUsuarioMap : BaseMap<HistoricoEmailUsuario>
    {
        public HistoricoEmailUsuarioMap()
        {
            ToTable("historico_email_usuario");
            Map(c => c.UsuarioId).ToColumn("usuario_id");
            Map(c => c.Email).ToColumn("email");
            Map(c => c.Acao).ToColumn("tipo_acao");
        }
    }
}

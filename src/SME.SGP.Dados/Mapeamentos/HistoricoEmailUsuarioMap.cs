using SME.SGP.Dominio;

namespace SME.SGP.Dados.Mapeamentos
{
    public class HistoricoEmailUsuarioMap : BaseEntityMap<HistoricoEmailUsuario>
    {
        public HistoricoEmailUsuarioMap()
        {
            ToTable("historico_email_usuario");
            Map(nameof(HistoricoEmailUsuario.UsuarioId), "usuario_id");
            Map(nameof(HistoricoEmailUsuario.Email), "email");
            Map(nameof(HistoricoEmailUsuario.Acao), "tipo_acao");
        }
    }
}
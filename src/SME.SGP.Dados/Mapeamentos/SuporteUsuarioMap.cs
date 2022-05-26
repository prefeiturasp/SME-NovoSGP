using SME.SGP.Dominio;

namespace SME.SGP.Dados.Mapeamentos
{
    public class SuporteUsuarioMap : BaseMap<SuporteUsuario>
    {
        public SuporteUsuarioMap()
        {
            ToTable("suporte_usuario");
            Map(suporte => suporte.UsuarioAdministrador).ToColumn("usuario_administrador");
            Map(suporte => suporte.UsuarioSimulado).ToColumn("usuario_simulado");
            Map(suporte => suporte.DataAcesso).ToColumn("data_acesso");
            Map(suporte => suporte.TokenAcesso).ToColumn("token_acesso");
        }
    }
}

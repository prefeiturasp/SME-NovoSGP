using SME.SGP.Dominio;

namespace SME.SGP.Dados.Mapeamentos
{
    public class SuporteUsuarioMap : SimpleEntityMap<SuporteUsuario>
    {
        public SuporteUsuarioMap()
        {
            ToTable("suporte_usuario");

            Map(nameof(SuporteUsuario.UsuarioAdministrador), "usuario_administrador");
            Map(nameof(SuporteUsuario.UsuarioSimulado), "usuario_simulado");
            Map(nameof(SuporteUsuario.DataAcesso), "data_acesso");
            Map(nameof(SuporteUsuario.TokenAcesso), "token_acesso");
        }
    }
}
using SME.SGP.Dominio;

namespace SME.SGP.Dados.Mapeamentos
{
    public class UsuarioMap : BaseMap<Usuario>
    {
        public UsuarioMap()
        {
            ToTable("usuario");
            Map(nameof(Usuario.CodigoRf), "rf_codigo");
            Map(nameof(Usuario.ExpiracaoRecuperacaoSenha), "expiracao_recuperacao_senha");
            Map(nameof(Usuario.Login), "login");
            Map(nameof(Usuario.Nome), "nome");
            Map(nameof(Usuario.TokenRecuperacaoSenha), "token_recuperacao_senha");
            Map(nameof(Usuario.UltimoLogin), "ultimo_login");
        }
    }
}
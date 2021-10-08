using SME.SGP.Dominio;

namespace SME.SGP.Dados.Mapeamentos
{
    public class UsuarioMap : BaseMap<Usuario>
    {
        public UsuarioMap()
        {
            ToTable("usuario");
            Map(a => a.PerfilAtual).Ignore();
            Map(c => c.CodigoRf).ToColumn("codigo_rf");
            Map(c => c.ExpiracaoRecuperacaoSenha).ToColumn("expiracao_recuperacao_senha");
            Map(c => c.Login).ToColumn("login");
            Map(c => c.Nome).ToColumn("nome");
            Map(c => c.TokenRecuperacaoSenha).ToColumn("token_recuperacao_senha");
            Map(c => c.UltimoLogin).ToColumn("ultimo_login");
        }
    }
}
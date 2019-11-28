using SME.SGP.Dominio;

namespace SME.SGP.Dados.Mapeamentos
{
    public class UsuarioMap : BaseMap<Usuario>
    {
        public UsuarioMap()
        {
            ToTable("usuario");
            Map(a => a.CodigoRf).ToColumn("rf_codigo");
            Map(a => a.TokenRecuperacaoSenha).ToColumn("token_recuperacao_senha");
            Map(a => a.ExpiracaoRecuperacaoSenha).ToColumn("expiracao_recuperacao_senha");
            Map(a => a.UltimoLogin).ToColumn("ultimo_login");
            Map(a => a.PerfilAtual).Ignore();
        }
    }
}
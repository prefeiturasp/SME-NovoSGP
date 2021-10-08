using SME.SGP.Dominio;

namespace SME.SGP.Dados.Mapeamentos
{
    public class ConfiguracaoEmailMap : BaseMap<ConfiguracaoEmail>
    {
        public ConfiguracaoEmailMap()
        {
            ToTable("configuracao_email");
            Map(c => c.EmailRemetente).ToColumn("email_remetente");
            Map(c => c.NomeRemetente).ToColumn("nome_remetente");
            Map(c => c.Porta).ToColumn("porta");
            Map(c => c.Senha).ToColumn("senha");
            Map(c => c.ServidorSmtp).ToColumn("servidor_smtp");
            Map(c => c.UsarTls).ToColumn("usar_tls");
            Map(c => c.Usuario).ToColumn("usuario");
        }
    }
}
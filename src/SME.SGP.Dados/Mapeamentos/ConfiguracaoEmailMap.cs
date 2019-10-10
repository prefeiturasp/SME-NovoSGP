using SME.SGP.Dominio;

namespace SME.SGP.Dados.Mapeamentos
{
    public class ConfiguracaoEmailMap : BaseMap<ConfiguracaoEmail>
    {
        public ConfiguracaoEmailMap()
        {
            ToTable("configuracao_email");
            Map(a => a.EmailRemetente).ToColumn("email_remetente");
            Map(a => a.NomeRemetente).ToColumn("nome_remetente");
            Map(a => a.ServidorSmtp).ToColumn("servidor_smtp");
            Map(a => a.UsarTls).ToColumn("usar_tls");
        }
    }
}
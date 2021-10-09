using Dapper.FluentMap.Dommel.Mapping;
using SME.SGP.Dominio;

namespace SME.SGP.Dados
{
    public class AuditoriaMap : DommelEntityMap<Auditoria>
    {
        public AuditoriaMap()
        {
            ToTable("auditoria");
            Map(c => c.Acao).ToColumn("acao");
            Map(c => c.Chave).ToColumn("chave");
            Map(c => c.Data).ToColumn("data");
            Map(c => c.Entidade).ToColumn("entidade");
            Map(c => c.Id).ToColumn("id").IsIdentity().IsKey();
            Map(c => c.RF).ToColumn("rf");
            Map(c => c.Usuario).ToColumn("usuario");
            Map(c => c.Perfil).ToColumn("perfil");
        }
    }
}
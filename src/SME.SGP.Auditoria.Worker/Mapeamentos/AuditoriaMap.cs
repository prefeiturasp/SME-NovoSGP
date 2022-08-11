using Dapper.FluentMap.Dommel.Mapping;

namespace SME.SGP.Auditoria.Worker
{
    public class AuditoriaMap : DommelEntityMap<Entidade.Auditoria>
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
            Map(c => c.Administrador).ToColumn("administrador");
        }
    }
}

using Dapper.FluentMap.Dommel.Mapping;
using SME.SGP.Dominio;

namespace SME.SGP.Dados
{
    public class AuditoriaMap : DommelEntityMap<Auditoria>
    {
        public AuditoriaMap()
        {
            ToTable("auditoria");
        }
    }
}
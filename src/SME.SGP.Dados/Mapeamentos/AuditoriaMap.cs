using Dapper.FluentMap.Dommel.Mapping;
using SME.SGP.Dominio.Entidades;

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
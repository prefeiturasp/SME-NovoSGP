using Dapper.FluentMap.Dommel.Mapping;
using SME.SGP.Dominio;

namespace SME.SGP.Dados.Mapeamentos
{
    public class ProfessorMap : DommelEntityMap<Professor>
    {
        public ProfessorMap()
        {
            ToTable("professor");
        }
    }
}
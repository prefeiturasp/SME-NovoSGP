using Dapper.FluentMap.Dommel.Mapping;
using SME.SGP.Dominio;

namespace SME.SGP.Dados.Mapeamentos
{
    public class PlanoCicloMap : DommelEntityMap<PlanoCiclo>
    {
        public PlanoCicloMap()
        {
            ToTable("plano_ciclo");
        }
    }
}
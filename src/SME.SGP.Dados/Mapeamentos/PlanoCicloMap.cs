using SME.SGP.Dominio;

namespace SME.SGP.Dados.Mapeamentos
{
    public class PlanoCicloMap : BaseMap<PlanoCiclo>
    {
        public PlanoCicloMap()
        {
            ToTable("plano_ciclo");
            Map(c => c.CicloId).ToColumn("ciclo_id");
            Map(c => c.EscolaId).ToColumn("escola_id");
        }
    }
}
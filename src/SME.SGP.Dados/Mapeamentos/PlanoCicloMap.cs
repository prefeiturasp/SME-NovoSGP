using SME.SGP.Dominio;

namespace SME.SGP.Dados.Mapeamentos
{
    public class PlanoCicloMap : BaseMap<PlanoCiclo>
    {
        public PlanoCicloMap()
        {
            ToTable("plano_ciclo");
            Map(nameof(PlanoCiclo.Ano), "ano");
            Map(nameof(PlanoCiclo.CicloId), "ciclo_id");
            Map(nameof(PlanoCiclo.Descricao), "descricao");
            Map(nameof(PlanoCiclo.EscolaId), "escola_id");
            Map(nameof(PlanoCiclo.Migrado), "migrado");
        }
    }
}
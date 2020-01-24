using Dapper.FluentMap.Dommel.Mapping;
using SME.SGP.Dominio;

namespace SME.SGP.Dados.Mapeamentos
{
    public class FechamentoBimestreMap : DommelEntityMap<FechamentoBimestre>
    {
        public FechamentoBimestreMap()
        {
            ToTable("fechamento_bimestre");
            Map(c => c.Id).ToColumn("id");
            Map(c => c.InicioDoFechamento).ToColumn("inicio_fechamento");
            Map(c => c.FinalDoFechamento).ToColumn("final_fechamento");
            Map(c => c.PeriodoEscolarId).ToColumn("periodo_escolar_id");
            Map(c => c.FechamentoId).ToColumn("fechamento_id");
            Map(c => c.PeriodoEscolar).Ignore();
        }
    }
}
using Dapper.FluentMap.Dommel.Mapping;
using SME.SGP.Dominio;

namespace SME.SGP.Dados.Mapeamentos
{
    public class PeriodoFechamentoBimestreMap : DommelEntityMap<PeriodoFechamentoBimestre>
    {
        public PeriodoFechamentoBimestreMap()
        {
            ToTable("periodo_fechamento_bimestre");
            Map(c => c.Id).ToColumn("id");
            Map(c => c.InicioDoFechamento).ToColumn("inicio_fechamento");
            Map(c => c.FinalDoFechamento).ToColumn("final_fechamento");
            Map(c => c.PeriodoEscolarId).ToColumn("periodo_escolar_id");
            Map(c => c.PeriodoFechamentoId).ToColumn("periodo_fechamento_id");
            Map(c => c.PeriodoEscolar).Ignore();
        }
    }
}
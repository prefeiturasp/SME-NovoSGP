using Dapper.FluentMap.Dommel.Mapping;
using SME.SGP.Dominio;

namespace SME.SGP.Dados.Mapeamentos
{
    public class PeriodoFechamentoBimestreMap : DommelEntityMap<PeriodoFechamentoBimestre>
    {
        public PeriodoFechamentoBimestreMap()
        {
            ToTable("periodo_fechamento_bimestre");
            Map(c => c.PeriodoEscolar).Ignore();
            Map(c => c.PeriodoFechamentoId).ToColumn("periodo_fechamento_id");
            Map(c => c.FinalDoFechamento).Ignore();
            Map(c => c.Id).ToColumn("id").IsIdentity().IsKey();
            Map(c => c.InicioDoFechamento).ToColumn("inicio_fechamento");
            Map(c => c.PeriodoEscolarId).ToColumn("periodo_escolar_id");
        }
    }
}
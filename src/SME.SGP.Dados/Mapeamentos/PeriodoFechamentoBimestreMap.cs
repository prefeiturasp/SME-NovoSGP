using SME.SGP.Dominio;

namespace SME.SGP.Dados.Mapeamentos
{
    public class PeriodoFechamentoBimestreMap : SimpleMap<PeriodoFechamentoBimestre>
    {
        public PeriodoFechamentoBimestreMap()
        {
            ToTable("periodo_fechamento_bimestre");
            Map(nameof(PeriodoFechamentoBimestre.PeriodoFechamentoId), "periodo_fechamento_id");
            Map(nameof(PeriodoFechamentoBimestre.FinalDoFechamento), "final_fechamento");
            Map(nameof(PeriodoFechamentoBimestre.InicioDoFechamento), "inicio_fechamento");
            Map(nameof(PeriodoFechamentoBimestre.PeriodoEscolarId), "periodo_escolar_id");
        }
    }
}
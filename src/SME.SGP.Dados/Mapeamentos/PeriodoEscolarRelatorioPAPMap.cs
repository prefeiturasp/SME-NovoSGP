using SME.SGP.Dominio;

namespace SME.SGP.Dados.Mapeamentos
{
    public class PeriodoEscolarRelatorioPAPMap : BaseMap<PeriodoEscolarRelatorioPAP>
    {
        public PeriodoEscolarRelatorioPAPMap()
        {
            ToTable("periodo_escolar_relatorio_pap");

            Map(c => c.PeriodoRelatorioId).ToColumn("periodo_relatorio_pap_id");
            Map(c => c.PeriodoEscolarId).ToColumn("periodo_escolar_id");
        }
    }
}

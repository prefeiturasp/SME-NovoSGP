using SME.SGP.Dominio;

namespace SME.SGP.Dados.Mapeamentos
{
    public class PeriodoEscolarRelatorioPAPMap : BaseMap<PeriodoEscolarRelatorioPAP>
    {
        public PeriodoEscolarRelatorioPAPMap()
        {
            ToTable("periodo_escolar_relatorio_pap");
            Map(nameof(PeriodoEscolarRelatorioPAP.PeriodoRelatorioId), "periodo_relatorio_pap_id");
            Map(nameof(PeriodoEscolarRelatorioPAP.PeriodoEscolarId), "periodo_escolar_id");
        }
    }
}
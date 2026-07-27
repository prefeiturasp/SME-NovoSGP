using SME.SGP.Dominio;

namespace SME.SGP.Dados.Mapeamentos
{
    public class PeriodoRelatorioPAPMap : SimpleEntityMap<PeriodoRelatorioPAP>
    {
        public PeriodoRelatorioPAPMap()
        {
            ToTable("periodo_relatorio_pap");
            Map(nameof(PeriodoRelatorioPAP.ConfiguracaoId), "configuracao_relatorio_pap_id");
            Map(nameof(PeriodoRelatorioPAP.Periodo), "periodo");
        }
    }
}
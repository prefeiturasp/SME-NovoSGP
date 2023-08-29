using SME.SGP.Dominio;

namespace SME.SGP.Dados.Mapeamentos
{
    public class PeriodoRelatorioPAPMap : BaseMap<PeriodoRelatorioPAP>
    {
        public PeriodoRelatorioPAPMap() 
        {
            ToTable("periodo_relatorio_pap");

            Map(c => c.ConfiguracaoId).ToColumn("configuracao_relatorio_pap_id");
            Map(c => c.Periodo).ToColumn("periodo");
        }
    }
}

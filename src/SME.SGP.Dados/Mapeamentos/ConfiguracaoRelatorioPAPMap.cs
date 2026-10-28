using SME.SGP.Dominio;

namespace SME.SGP.Dados.Mapeamentos
{
    public class ConfiguracaoRelatorioPAPMap : BaseMap<ConfiguracaoRelatorioPAP>
    {
        public ConfiguracaoRelatorioPAPMap()
        {
            ToTable("configuracao_relatorio_pap");
            Map(nameof(ConfiguracaoRelatorioPAP.InicioVigencia), "inicio_vigencia");
            Map(nameof(ConfiguracaoRelatorioPAP.FimVigencia), "fim_vigencia");
            Map(nameof(ConfiguracaoRelatorioPAP.TipoPeriocidade), "tipo_periodicidade");
        }
    }
}
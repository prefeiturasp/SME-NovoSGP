using SME.SGP.Dominio;

namespace SME.SGP.Dados.Mapeamentos
{
    public class ConfiguracaoRelatorioPAPMap : BaseMap<ConfiguracaoRelatorioPAP>
    {
        public ConfiguracaoRelatorioPAPMap()
        {
            ToTable("configuracao_relatorio_pap");

            Map(c => c.InicioVigencia).ToColumn("inicio_vigencia");
            Map(c => c.FimVigencia).ToColumn("fim_vigencia");
            Map(c => c.TipoPeriocidade).ToColumn("tipo_periodicidade");
        }
    }
}

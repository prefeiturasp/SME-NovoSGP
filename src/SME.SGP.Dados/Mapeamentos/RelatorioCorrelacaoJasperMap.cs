using Dapper.FluentMap.Dommel.Mapping;
using SME.SGP.Dominio;

namespace SME.SGP.Dados.Mapeamentos
{
    public class RelatorioCorrelacaoJasperMap : DommelEntityMap<RelatorioCorrelacaoJasper>
    {
        public RelatorioCorrelacaoJasperMap()
        {
            ToTable("relatorio_correlacao_jasper");
            Map(c => c.RelatorioCorrelacao).Ignore();
            Map(c => c.ExportId).ToColumn("export_id");
            Map(c => c.Id).ToColumn("id");
            Map(c => c.JSessionId).ToColumn("jsession_id");
            Map(c => c.RelatorioCorrelacaoId).ToColumn("relatorio_correlacao_id");
            Map(c => c.RequestId).ToColumn("request_id");
        }
    }
}

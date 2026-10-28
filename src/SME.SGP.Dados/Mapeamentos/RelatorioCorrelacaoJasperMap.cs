using SME.SGP.Dominio;

namespace SME.SGP.Dados.Mapeamentos
{
    public class RelatorioCorrelacaoJasperMap : SimpleMap<RelatorioCorrelacaoJasper>
    {
        public RelatorioCorrelacaoJasperMap()
        {
            ToTable("relatorio_correlacao_jasper");
            Map(nameof(RelatorioCorrelacaoJasper.ExportId), "export_id");
            Map(nameof(RelatorioCorrelacaoJasper.JSessionId), "jsession_id");
            Map(nameof(RelatorioCorrelacaoJasper.RelatorioCorrelacaoId), "relatorio_correlacao_id");
            Map(nameof(RelatorioCorrelacaoJasper.RequestId), "request_id");
        }
    }
}
using SME.SGP.Dominio.Entidades;

namespace SME.SGP.Dados.Mapeamentos
{
    public partial class RelatorioCorrelacaoMap
    {
        public class RelatorioCorrelacaoJasperMap : BaseMap<RelatorioCorrelacaoJasper>
        {
            public RelatorioCorrelacaoJasperMap()
            {
                ToTable("relatorio_correlacao_jasper");
                Map(c => c.Id).ToColumn("id");
                Map(c => c.JSessionId).ToColumn("jsession_id");
                Map(c => c.RequestId).ToColumn("request_id");
                Map(c => c.ExportId).ToColumn("export_id");
                Map(c => c.RelatorioCorrelacaoId).ToColumn("relatorio_correlacao_id");
                Map(c => c.RelatorioCorrelacao).Ignore();
            }
        }
    }
}

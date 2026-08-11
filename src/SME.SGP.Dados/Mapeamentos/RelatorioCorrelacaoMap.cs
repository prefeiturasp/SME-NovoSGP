using SME.SGP.Dominio;

namespace SME.SGP.Dados.Mapeamentos
{
    public partial class RelatorioCorrelacaoMap : BaseMap<RelatorioCorrelacao>
    {
        public RelatorioCorrelacaoMap()
        {
            ToTable("relatorio_correlacao");
            Map(nameof(RelatorioCorrelacao.Formato), "tipo_formato");
            Map(nameof(RelatorioCorrelacao.Codigo), "codigo");
            Map(nameof(RelatorioCorrelacao.TipoRelatorio), "tipo_relatorio");
            Map(nameof(RelatorioCorrelacao.UsuarioSolicitanteId), "usuario_solicitante_id");
            Map(nameof(RelatorioCorrelacao.UrlRelatorio), "url_relatorio");
        }
    }
}
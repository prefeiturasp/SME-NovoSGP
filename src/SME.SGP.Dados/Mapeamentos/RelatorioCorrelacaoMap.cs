using SME.SGP.Dominio;

namespace SME.SGP.Dados.Mapeamentos
{
    public partial class RelatorioCorrelacaoMap : BaseMap<RelatorioCorrelacao>
    {
        public RelatorioCorrelacaoMap()
        {
            ToTable("relatorio_correlacao");
            Map(c => c.Formato).ToColumn("tipo_formato");
            Map(c => c.UsuarioSolicitante).Ignore();
            Map(c => c.CorrelacaoJasper).Ignore();
            Map(c => c.Codigo).ToColumn("codigo");
            Map(c => c.TipoRelatorio).ToColumn("tipo_relatorio");
            Map(c => c.UsuarioSolicitanteId).ToColumn("usuario_solicitante_id");
        }
    }
}

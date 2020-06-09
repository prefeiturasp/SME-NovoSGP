using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using SME.SGP.Dominio.Entidades;

namespace SME.SGP.Dados.Mapeamentos
{
    public partial class RelatorioCorrelacaoMap : BaseMap<RelatorioCorrelacao>
    {
        public RelatorioCorrelacaoMap()
        {
            ToTable("relatorio_correlacao");
            Map(c => c.Id).ToColumn("id");
            Map(c => c.Codigo).ToColumn("codigo");
            Map(c => c.TipoRelatorio).ToColumn("codigo_tipo_relatorio");
            Map(c => c.UsuarioSolicitanteId).ToColumn("usuario_solicitante_id");
            Map(c => c.UsuarioSolicitante).Ignore();
        }
    }
}

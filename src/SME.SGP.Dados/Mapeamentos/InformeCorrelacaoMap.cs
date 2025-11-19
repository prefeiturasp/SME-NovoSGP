using Dapper.FluentMap.Dommel.Mapping;
using SME.SGP.Dominio;

namespace SME.SGP.Dados.Mapeamentos
{
    public class InformeCorrelacaoMap : DommelEntityMap<InformeCorrelacao>
    {
        public InformeCorrelacaoMap()
        {
            ToTable("informativo_correlacao");
            Map(c => c.Id).ToColumn("id").IsKey();
            Map(c => c.InformeId).ToColumn("informe_id");
            Map(c => c.Codigo).ToColumn("codigo");
            Map(c => c.UsuarioSolicitanteId).ToColumn("usuario_solicitante_id");
            Map(c => c.CriadoEm).ToColumn("criado_em");
            Map(c => c.CriadoPor).ToColumn("criado_por");
            Map(c => c.CriadoRF).ToColumn("criado_rf");
            Map(c => c.AlteradoEm).ToColumn("alterado_em");
            Map(c => c.AlteradoPor).ToColumn("alterado_por");
            Map(c => c.AlteradoRF).ToColumn("alterado_rf");
        }
    }
}
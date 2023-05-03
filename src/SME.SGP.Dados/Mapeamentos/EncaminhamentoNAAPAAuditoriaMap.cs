using Dapper.FluentMap.Dommel.Mapping;
using SME.SGP.Dominio;

namespace SME.SGP.Dados
{
    public class EncaminhamentoNAAPAAuditoriaMap : DommelEntityMap<EncaminhamentoNAAPAAuditoria>
    {
        public EncaminhamentoNAAPAAuditoriaMap()
        {
            ToTable("encaminhamento_naapa_auditoria");

            Map(c => c.Id).ToColumn("id").IsIdentity().IsKey();
            Map(c => c.EncaminhamentoNAAPAId).ToColumn("encaminhamento_naapa_id");
            Map(c => c.EncaminhamentoNAAPASecaoId).ToColumn("encaminhamento_naapa_secao_id");
            Map(c => c.UsuarioId).ToColumn("usuario_id");
            Map(c => c.CamposInseridos).ToColumn("campos_inseridos");
            Map(c => c.CamposInseridos).ToColumn("campos_alterados");
            Map(c => c.DataAuditoria).ToColumn("data_auditoria");
            Map(c => c.TipoAuditoria).ToColumn("tipo_auditoria");
        }
    }
}

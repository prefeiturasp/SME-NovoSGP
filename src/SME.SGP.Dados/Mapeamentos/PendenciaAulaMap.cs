using Dapper.FluentMap.Dommel.Mapping;
using SME.SGP.Dominio;

namespace SME.SGP.Dados.Mapeamentos
{
    public class PendenciaAulaMap : DommelEntityMap<PendenciaAula>
    {
        public PendenciaAulaMap()
        {
            ToTable("pendencia_aula");
            Map(c => c.Id).ToColumn("id").IsIdentity().IsKey();
            Map(c => c.AulaId).ToColumn("aula_id");
            Map(c => c.PendenciaId).ToColumn("pendencia_id");
            Map(c => c.Motivo).ToColumn("motivo");
        }
    }
}
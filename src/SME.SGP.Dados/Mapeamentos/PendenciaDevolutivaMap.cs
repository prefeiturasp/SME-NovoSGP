using Dapper.FluentMap.Dommel.Mapping;
using SME.SGP.Dominio;

namespace SME.SGP.Dados
{
    public class PendenciaDevolutivaMap : DommelEntityMap<PendenciaDevolutiva>
    {
        public PendenciaDevolutivaMap()
        {
            ToTable("pendencia_devolutiva");
            Map(c => c.Id).ToColumn("id").IsIdentity().IsKey();
            Map(c => c.PedenciaId).ToColumn("pendencia_id");
            Map(c => c.ComponenteCurricularId).ToColumn("componente_curricular_id");
            Map(c => c.TurmaId).ToColumn("turma_id");
        }
    }
}

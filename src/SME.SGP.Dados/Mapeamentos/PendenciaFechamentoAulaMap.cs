using Dapper.FluentMap.Dommel.Mapping;
using SME.SGP.Dominio;

namespace SME.SGP.Dados.Mapeamentos
{
    public class PendenciaFechamentoAulaMap : DommelEntityMap<PendenciaFechamentoAula>
    {
        public PendenciaFechamentoAulaMap()
        {
            ToTable("pendencia_fechamento_aula");
            Map(c => c.Id).ToColumn("id").IsIdentity().IsKey();
            Map(c => c.AulaId).ToColumn("aula_id");
            Map(c => c.PendenciaFechamentoId).ToColumn("pendencia_fechamento_id");
        }
    }
}

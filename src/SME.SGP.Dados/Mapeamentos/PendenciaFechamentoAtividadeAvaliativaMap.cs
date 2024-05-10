using Dapper.FluentMap.Dommel.Mapping;
using SME.SGP.Dominio;

namespace SME.SGP.Dados.Mapeamentos
{
    public class PendenciaFechamentoAtividadeAvaliativaMap : DommelEntityMap<PendenciaFechamentoAtividadeAvaliativa>
    {
        public PendenciaFechamentoAtividadeAvaliativaMap()
        {
            ToTable("pendencia_fechamento_atividade_avaliativa");
            Map(c => c.Id).ToColumn("id").IsIdentity().IsKey();
            Map(c => c.AtividadeAvaliativaId).ToColumn("atividade_avaliativa_id");
            Map(c => c.PendenciaFechamentoId).ToColumn("pendencia_fechamento_id");
        }
    }
}

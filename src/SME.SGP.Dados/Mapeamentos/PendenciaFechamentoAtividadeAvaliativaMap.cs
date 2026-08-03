using SME.SGP.Dominio;

namespace SME.SGP.Dados.Mapeamentos
{
    public class PendenciaFechamentoAtividadeAvaliativaMap : SimpleMap<PendenciaFechamentoAtividadeAvaliativa>
    {
        public PendenciaFechamentoAtividadeAvaliativaMap()
        {
            ToTable("pendencia_fechamento_atividade_avaliativa");
            Map(nameof(PendenciaFechamentoAtividadeAvaliativa.AtividadeAvaliativaId), "atividade_avaliativa_id");
            Map(nameof(PendenciaFechamentoAtividadeAvaliativa.PendenciaFechamentoId), "pendencia_fechamento_id");
        }
    }
}
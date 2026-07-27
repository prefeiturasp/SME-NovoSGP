using SME.SGP.Dominio;

namespace SME.SGP.Dados
{
    public class EventoFechamentoMap : BaseEntityMap<EventoFechamento>
    {
        public EventoFechamentoMap()
        {
            ToTable("evento_fechamento");
            Map(nameof(EventoFechamento.EventoId), "evento_id");
            Map(nameof(EventoFechamento.FechamentoId), "fechamento_id");
            Map(nameof(EventoFechamento.Excluido), "excluido");
        }
    }
}
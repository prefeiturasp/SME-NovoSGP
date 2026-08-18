using SME.SGP.Dominio;
using SME.SGP.Dados.Mapeamentos;

namespace SME.SGP.Dados
{
    public class EventoFechamentoMap : BaseMap<EventoFechamento>
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
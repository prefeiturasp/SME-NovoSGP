using SME.SGP.Dominio.Entidades;

namespace SME.SGP.Dados.Mapeamentos
{
    public class EventoBimestreMap : BaseEntityMap<EventoBimestre>
    {
        public EventoBimestreMap()
        {
            ToTable("evento_bimestre");
            Map(nameof(EventoBimestre.EventoId), "evento_id");
            Map(nameof(EventoBimestre.Bimestre), "bimestre");
        }
    }
}
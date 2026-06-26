using SME.SGP.Dominio.Entidades;

namespace SME.SGP.Dados.Mapeamentos
{
    public class EventoBimestreMap : BaseMap<EventoBimestre>
    {
        public EventoBimestreMap()
        {
            ToTable("evento_bimestre");
            Map(c => c.EventoId).ToColumn("evento_id");
            Map(c => c.Bimestre).ToColumn("bimestre");
        }
    }
}


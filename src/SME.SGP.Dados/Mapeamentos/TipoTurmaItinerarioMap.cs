using Dapper.FluentMap.Dommel.Mapping;
using SME.SGP.Dominio;

namespace SME.SGP.Dados.Mapeamentos
{
    public class TipoTurmaItinerarioMap : DommelEntityMap<TipoTurmaItinerario>
    {
        public TipoTurmaItinerarioMap()
        {
            ToTable("tipos_turma_itinerario");
            Map(c => c.Id);
            Map(c => c.Nome);
            Map(c => c.Serie);
        }
    }
}
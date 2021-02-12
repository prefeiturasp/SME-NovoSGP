using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioItinerancia : IRepositorioBase<Itinerancia>
    {
        Task<IEnumerable<ItineranciaObjetivosBaseDto>> ObterObjetivosBase();
        Task<IEnumerable<ItineranciaAlunoQuestaoDto>> ObterQuestoesItineranciaAluno(long id);
        Task<IEnumerable<ItineranciaQuestaoBaseDto>> ObterItineranciaQuestaoBase();
        Task<ItineranciaDto> ObterItineranciaPorId(long id);
        Task<IEnumerable<ItineranciaAlunoDto>> ObterItineranciaAlunoPorId(long id);
        Task<IEnumerable<ItineranciaObjetivoDto>> ObterObjetivosItineranciaPorId(long id);
        Task<IEnumerable<ItineranciaQuestaoDto>> ObterQuestoesItineranciaPorId(long id);
        Task<IEnumerable<ItineranciaUeDto>> ObterUesItineranciaPorId(long id);
        Task<Itinerancia> ObterEntidadeCompleta(long id);
    }
}
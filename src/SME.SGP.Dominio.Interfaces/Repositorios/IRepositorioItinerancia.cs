using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dados.Repositorios
{
    public interface IRepositorioItinerancia : IRepositorioBase<Itinerancia>
    {
        Task<IEnumerable<ItineranciaObjetivosBaseDto>> ObterObjetivosBase();
        Task<IEnumerable<ItineranciaAlunoQuestaoDto>> ObterQuestoesItineranciaAluno(long id);
        Task<IEnumerable<ItineranciaQuestaoBaseDto>> ObterItineranciaQuestaoBase();
    }
}
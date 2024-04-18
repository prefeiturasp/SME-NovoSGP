using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioRespostaMapeamentoEstudante : IRepositorioBase<RespostaMapeamentoEstudante>
    {
        Task<IEnumerable<RespostaMapeamentoEstudante>> ObterPorQuestaoMapeamentoEstudanteId(long questaoMapeamentoEstudanteId);
    }
}

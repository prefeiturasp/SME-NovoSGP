using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioObjetivoAprendizagem
    {
        Task Atualizar(ObjetivoAprendizagem objetivoAprendizagem);

        Task<IEnumerable<ObjetivoAprendizagem>> Listar();

        Task<ObjetivoAprendizagem> ObterPorId(long id);

        Task ReativarAsync(long id);

        Task SalvarAsync(ObjetivoAprendizagem objetivoAprendizagem);
    }
}
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioObjetivoAprendizagem
    {
        Task AtualizarAsync(ObjetivoAprendizagem objetivoAprendizagem);

        Task<IEnumerable<ObjetivoAprendizagem>> ListarAsync();

        Task<ObjetivoAprendizagem> ObterPorIdAsync(long id);

        Task ReativarAsync(long id);

        Task SalvarAsync(ObjetivoAprendizagem objetivoAprendizagem);
    }
}
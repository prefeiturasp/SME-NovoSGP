using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Interfaces
{
    public interface IObterListaSituacoesUsuarioUseCase
    {
        Task<IEnumerable<KeyValuePair<int, string>>> Executar();
    }
}

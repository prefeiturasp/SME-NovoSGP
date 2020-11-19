using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Interfaces
{
    public interface IObterListaPerfisUsuarioUseCase
    {
        Task<IEnumerable<KeyValuePair<Guid, string>>> Executar();
    }
}

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Interfaces
{
    public interface IObterHierarquiaPerfisUsuarioUseCase
    {
        Task<IEnumerable<KeyValuePair<Guid, string>>> Executar();
    }
}

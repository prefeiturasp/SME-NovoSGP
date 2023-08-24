using System.Collections.Generic;

namespace SME.SGP.Aplicacao.Interfaces
{
    public interface IObterPrefixosCacheUseCase
    {
        IEnumerable<string> Executar();
    }
}

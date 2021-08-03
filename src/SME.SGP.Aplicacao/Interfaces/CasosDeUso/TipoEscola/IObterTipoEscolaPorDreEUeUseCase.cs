using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Interfaces
{
    public interface IObterTipoEscolaPorDreEUeUseCase
    {
        Task<IEnumerable<TipoEscolaDto>> Executar(string dreCodigo, string ueCodigo);
    }
}
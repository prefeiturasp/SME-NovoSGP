using SME.SGP.Infra;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public interface IObterPareceresConclusivosUseCase
    {
        Task<IEnumerable<ConselhoClasseParecerConclusivoDto>> Executar();
    }
}

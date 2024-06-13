using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public interface IObterPareceresConclusivosTurmaUseCase 
    {
        Task<IEnumerable<ParecerConclusivoDto>> Executar(long turmaId, bool anoLetivoAnterior);
    }
}

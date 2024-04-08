using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public interface IObterTurmaSondagemUseCase 
    {
        Task<IEnumerable<TurmaRetornoDto>> Executar(string ueCodigo, int anoLetivo);
    }
}

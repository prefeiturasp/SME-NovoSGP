using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public interface IObterRecomendacoesAlunoFamiliaUseCase
    {
        Task<IEnumerable<RecomendacoesAlunoFamiliaDto>> Executar();
    }
}

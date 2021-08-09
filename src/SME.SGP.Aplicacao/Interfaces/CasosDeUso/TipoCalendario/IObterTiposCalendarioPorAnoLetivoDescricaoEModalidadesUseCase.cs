using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Interfaces
{
    public interface IObterTiposCalendarioPorAnoLetivoDescricaoEModalidadesUseCase
    {
        Task<IEnumerable<TipoCalendarioRetornoDto>> Executar(int anoLetivo, IEnumerable<int> modalidades, string descricao);
    }
}
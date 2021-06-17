using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Interfaces
{
    public interface IObterModalidadesPorAnoUseCase
    {
        Task<IEnumerable<EnumeradoRetornoDto>> Executar(int anoLetivo, bool consideraHistorico, bool consideraNovasModalidades);
    }
}
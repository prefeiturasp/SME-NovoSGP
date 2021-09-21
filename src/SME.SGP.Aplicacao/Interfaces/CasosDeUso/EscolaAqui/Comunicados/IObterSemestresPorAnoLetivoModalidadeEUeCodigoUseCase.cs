using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Interfaces
{
    public interface IObterSemestresPorAnoLetivoModalidadeEUeCodigoUseCase
    {
        Task<IEnumerable<int>> Executar(bool consideraHistorico, int modalidade, int anoLetivo, string ueCodigo);
    }
}
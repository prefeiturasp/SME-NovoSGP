using SME.SGP.Dominio;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public interface IObterModalidadesAnoUseCase
    {
        Task<IEnumerable<RetornoModalidadesPorAnoDto>> Executar(int anoLetivo, long dreId, long ueId, int modalidade, int semestre);
    }
}

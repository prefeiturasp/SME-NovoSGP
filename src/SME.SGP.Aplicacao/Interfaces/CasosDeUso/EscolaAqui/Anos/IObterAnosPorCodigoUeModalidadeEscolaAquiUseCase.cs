using SME.SGP.Infra.Dtos.EscolaAqui.Anos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Interfaces
{
    public interface IObterAnosPorCodigoUeModalidadeEscolaAquiUseCase
    {
        Task<IEnumerable<AnosPorCodigoUeModalidadeEscolaAquiResult>> Executar(string codigoUe, int[] modalidade);
    }
}

using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public interface IObterModalidadesAnoUseCase
    {
        Task<IEnumerable<ModalidadesPorAnoDto>> Executar(List<string> anos);
    }
    

}

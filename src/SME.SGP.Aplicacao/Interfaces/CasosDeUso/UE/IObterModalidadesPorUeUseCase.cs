using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Interfaces
{
    public interface IObterModalidadesPorUeUseCase
    {
        Task<IEnumerable<ModalidadeRetornoDto>> Executar(string ueCodigo, int anoLetivo, bool consideraNovasModalidades);
    }
}
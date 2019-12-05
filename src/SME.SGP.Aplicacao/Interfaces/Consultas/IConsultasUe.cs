using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public interface IConsultasUe
    {
        Task<IEnumerable<ModalidadeRetornoDto>> ObterModalidadesPorUe(string ueCodigo);
    }
}
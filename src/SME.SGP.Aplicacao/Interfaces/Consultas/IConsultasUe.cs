using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public interface IConsultasUe
    {
        Task<IEnumerable<ModalidadeRetornoDto>> ObterModalidadesPorUe(string ueCodigo, int ano);

        Task<IEnumerable<TurmaRetornoDto>> ObterTurmas(string ueCodigo, int modalidadeId, int ano);
    }
}
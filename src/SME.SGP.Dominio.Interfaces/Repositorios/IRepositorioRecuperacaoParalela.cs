using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioRecuperacaoParalela : IRepositorioBase<RecuperacaoParalela>
    {
        Task<IEnumerable<RetornoRecuperacaoParalela>> Listar(long turmaId);
    }
}
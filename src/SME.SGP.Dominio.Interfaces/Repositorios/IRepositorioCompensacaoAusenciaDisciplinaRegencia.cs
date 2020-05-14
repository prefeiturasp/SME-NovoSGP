using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioCompensacaoAusenciaDisciplinaRegencia: IRepositorioBase<CompensacaoAusenciaDisciplinaRegencia>
    {
        Task<IEnumerable<CompensacaoAusenciaDisciplinaRegencia>> ObterPorCompensacao(long compensacaoId);
    }
}

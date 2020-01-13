using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioCompensacaoAusencia: IRepositorioBase<CompensacaoAusencia>
    {
        Task<IEnumerable<CompensacaoAusencia>> Listar(string turmaId, string disciplinaId, int bimestre, string nomeAtividade);
    }
}

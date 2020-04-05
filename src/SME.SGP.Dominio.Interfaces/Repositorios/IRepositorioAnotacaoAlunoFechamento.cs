using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioFechamentoAluno: IRepositorioBase<FechamentoAluno>
    {
        Task<FechamentoAluno> ObterFechamentoAluno(long fechamentoId, string codigoAluno);
        Task<IEnumerable<FechamentoAluno>> ObterPorFechamentoTurmaDisciplina(long fechamentoDisciplinaId);
    }
}

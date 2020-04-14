using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioFechamentoTurma : IRepositorioBase<FechamentoTurma>
    {
        Task<FechamentoTurma> ObterPorTurmaPeriodo(long turmaId, long periodoId = 0);
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioFechamentoTurma : IRepositorioBase<FechamentoTurma>
    {
        Task<FechamentoTurma> ObterPorTurmaCodigoBimestreAsync(string turmaCodigo, int bimestre = 0);
        Task<FechamentoTurma> ObterPorTurmaPeriodo(long turmaId, long periodoId = 0);
        Task<FechamentoTurma> ObterCompletoPorIdAsync(long fechamentoTurmaId);
    }
}

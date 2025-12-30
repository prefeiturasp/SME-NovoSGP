using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioFechamentoNota : IRepositorioBase<FechamentoNota>
    {
        Task<IEnumerable<FechamentoNotaAlunoAprovacaoDto>> ObterPorFechamentosTurma(long[] fechamentosTurmaDisciplinaId);
    }
}
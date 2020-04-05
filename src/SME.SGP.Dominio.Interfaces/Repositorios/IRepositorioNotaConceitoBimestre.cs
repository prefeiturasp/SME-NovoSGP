using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioFechamentoNota : IRepositorioBase<FechamentoNota>
    {
        Task<IEnumerable<FechamentoNota>> ObterPorFechamentoTurma(long fechamentoId);
        Task<FechamentoNota> ObterPorAlunoEFechamento(long fechamentoId, string codigoAluno);
    }
}
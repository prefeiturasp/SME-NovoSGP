using System.Collections.Generic;
using System.Threading.Tasks;
using SME.SGP.Infra;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioAnotacaoFrequenciaAluno : IRepositorioBase<AnotacaoFrequenciaAluno>
    {
        Task<AnotacaoFrequenciaAluno> ObterPorAlunoAula(string codigoAluno, long aulaId);
        Task<bool> ExcluirAnotacoesDaAula(long aulaId);
        Task<IEnumerable<string>> ListarAlunosComAnotacaoFrequenciaNaAula(long aulaId);
    }
}
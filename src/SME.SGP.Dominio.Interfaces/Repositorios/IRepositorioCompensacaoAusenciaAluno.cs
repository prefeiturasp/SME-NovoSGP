using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioCompensacaoAusenciaAluno: IRepositorioBase<CompensacaoAusenciaAluno>
    {
        Task<IEnumerable<CompensacaoAusenciaAluno>> ObterPorCompensacao(long compensacaoId);
        int ObterTotalCompensacoesPorAlunoETurma(int bimestre, string codigoAluno, string disciplinaId, string turmaId);
        Task<IEnumerable<CompensacaoAusenciaAluno>> ObterCompensacoesAluno(string codigoAluno, long compensacaoIgnorada, int bimestre);
    }
}

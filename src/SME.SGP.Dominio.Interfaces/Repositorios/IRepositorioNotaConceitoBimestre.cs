using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioFechamentoNota : IRepositorioBase<FechamentoNota>
    {
        Task<IEnumerable<FechamentoNota>> ObterPorFechamentoTurma(long fechamentoTurmaDisciplinaId);
        Task<FechamentoNota> ObterPorAlunoEFechamento(long fechamentoTurmaDisciplinaId, string codigoAluno);
        Task<IEnumerable<WfAprovacaoNotaFechamento>> ObterNotasEmAprovacaoWf(long workFlowId);
        Task<IEnumerable<WfAprovacaoNotaFechamento>> ObterNotasEmAprovacaoPorFechamento(long fechamentoTurmaDisciplinaId);
        Task<IEnumerable<NotaConceitoBimestreComponenteDto>> ObterNotasAlunoBimestre(long fechamentoTurmaId, string alunoCodigo);
        Task<IEnumerable<NotaConceitoBimestreComponenteDto>> ObterNotasAlunoAno(string turmaCodigo, string alunoCodigo);
    }
}
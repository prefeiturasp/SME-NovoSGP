using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioFechamentoNotaConsulta : IRepositorioBase<FechamentoNota>
    {
        Task<IEnumerable<FechamentoNotaAlunoAprovacaoDto>> ObterPorFechamentoTurma(long fechamentoTurmaDisciplinaId);
        Task<IEnumerable<FechamentoNotaAlunoAprovacaoDto>> ObterPorFechamentosTurma(long[] fechamentosTurmaDisciplinaId);
        Task<FechamentoNota> ObterPorAlunoEFechamento(long fechamentoTurmaDisciplinaId, string codigoAluno);
        Task<IEnumerable<WfAprovacaoNotaFechamento>> ObterNotasEmAprovacaoWf(long workFlowId);
        Task<IEnumerable<WfAprovacaoNotaFechamento>> ObterNotasEmAprovacaoPorFechamento(long fechamentoTurmaDisciplinaId);
        Task<IEnumerable<NotaConceitoBimestreComponenteDto>> ObterNotasAlunoBimestreAsync(long fechamentoTurmaId, string alunoCodigo);
        Task<IEnumerable<NotaConceitoBimestreComponenteDto>> ObterNotasAlunoPorTurmasCodigosBimestreAsync(string[] turmasCodigos, string alunoCodigo, int bimestre);
        Task<IEnumerable<NotaConceitoBimestreComponenteDto>> ObterNotasFinaisAlunoAsync(string[] turmasCodigos, string alunoCodigo);
        Task<IEnumerable<NotaConceitoBimestreComponenteDto>> ObterNotasAlunoAnoAsync(string turmaCodigo, string alunoCodigo);
        Task<IEnumerable<AlunosFechamentoNotaDto>> ObterComNotaLancadaPorPeriodoEscolarUE(long ueId, long periodoEscolarId);
    }
}
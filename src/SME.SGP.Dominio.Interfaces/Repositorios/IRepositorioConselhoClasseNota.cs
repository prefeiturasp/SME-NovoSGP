using System.Threading.Tasks;
using SME.SGP.Infra;
using System.Collections.Generic;
using SME.SGP.Dominio.Entidades;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioConselhoClasseNota : IRepositorioBase<ConselhoClasseNota>
    {
        Task<ConselhoClasseNota> ObterPorConselhoClasseAlunoComponenteCurricularAsync(long conselhoClasseAlunoId, long componenteCurricularCodigo);
        Task<IEnumerable<NotaConceitoBimestreComponenteDto>> ObterNotasAlunoAsync(long conselhoClasseId, string alunoCodigo);
        Task<IEnumerable<NotaConceitoBimestreComponenteDto>> ObterNotasAlunoAsync(string alunoCodigo, string turmaCodigo, long? periodoEscolarId = null);
        Task<IEnumerable<NotaConceitoBimestreComponenteDto>> ObterNotasFinaisBimestresAlunoAsync(string alunoCodigo, string[] turmasCodigo, int bimestre = 0);
        Task<IEnumerable<NotaConceitoBimestreComponenteDto>> ObterNotasConselhoAlunoAsync(string alunoCodigo, string[] turmasCodigos, long? periodoEscolarId = null);
        Task<IEnumerable<NotaConceitoBimestreComponenteDto>> ObterNotasAlunoPorTurmasAsync(string alunoCodigo, IEnumerable<string> turmasCodigos, long? periodoEscolarId);
        Task<IEnumerable<NotaConceitoBimestreComponenteDto>> ObterNotasBimestresAluno(string alunoCodigo, string ueCodigo, string turmaCodigo, int[] bimestres);
        Task<int> ObterBimestreEmAprovacaoWf(long workFlowId);
        Task<WFAprovacaoNotaConselho> ObterNotaEmAprovacaoWf(long workFlowId);
        Task<double> VerificaNotaConselhoEmAprovacao(long conselhoClasseNotaId);
    }
}
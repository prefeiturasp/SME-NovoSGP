using System.Threading.Tasks;
using SME.SGP.Infra;
using System.Collections.Generic;
using System;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioConselhoClasseNotaConsulta : IRepositorioBase<ConselhoClasseNota>
    {
        Task<ConselhoClasseNota> ObterPorConselhoClasseAlunoComponenteCurricularAsync(long conselhoClasseAlunoId, long componenteCurricularCodigo);
        Task<IEnumerable<NotaConceitoBimestreComponenteDto>> ObterNotasAlunoAsync(string alunoCodigo, string turmaCodigo, long? periodoEscolarId = null);
        Task<IEnumerable<NotaConceitoBimestreComponenteDto>> ObterNotasFinaisBimestresAlunoAsync(string alunoCodigo, string[] turmasCodigo, int bimestre = 0, DateTime? dataMatricula = null, DateTime? dataSituacao = null, bool validaMatricula = true);
        Task<IEnumerable<NotaConceitoBimestreComponenteDto>> ObterNotasConselhoAlunoAsync(string alunoCodigo, string[] turmasCodigos, long? periodoEscolarId = null);
        Task<IEnumerable<NotaConceitoBimestreComponenteDto>> ObterNotasAlunoPorTurmasAsync(string alunoCodigo, IEnumerable<string> turmasCodigos, long? periodoEscolarId);
        Task<IEnumerable<NotaConceitoBimestreComponenteDto>> ObterNotasConceitosFechamentoPorTurmaCodigoEBimestreAsync(string turmaCodigo, int bimestre = 0, DateTime? dataMatricula = null, DateTime? dataSituacao = null, long? tipoCalendario = null, string alunoCodigo = null);
        Task<IEnumerable<NotaConceitoBimestreComponenteDto>> ObterNotasConceitosConselhoClassePorTurmaCodigoEBimestreAsync(string turmaCodigo, int? bimestre,DateTime? dataMatricula = null, DateTime? dataSituacao = null, long? tipoCalendario = null, string alunoCodigo = null);
        Task<IEnumerable<NotaConceitoBimestreComponenteDto>> ObterNotasBimestresAluno(string alunoCodigo, string ueCodigo, string turmaCodigo, int[] bimestres);
        Task<double?> VerificaNotaConselhoEmAprovacao(long conselhoClasseNotaId);
        Task<WFAprovacaoNotaConselho> ObterNotaEmAprovacaoWf(long workFlowId);
    }
}
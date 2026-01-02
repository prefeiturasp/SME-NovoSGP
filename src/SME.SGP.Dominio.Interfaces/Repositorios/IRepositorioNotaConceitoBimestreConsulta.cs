using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioFechamentoNotaConsulta : IRepositorioBase<FechamentoNota>
    {
        Task<IEnumerable<WfAprovacaoNotaFechamentoTurmaDto>> ObterNotasEmAprovacaoWf(long wfAprovacaoId);
        Task<IEnumerable<WfAprovacaoNotaFechamento>> ObterNotasEmAprovacaoPorFechamento(long fechamentoTurmaDisciplinaId);
        Task<IEnumerable<NotaConceitoBimestreComponenteDto>> ObterNotasAlunoBimestreAsync(long fechamentoTurmaId, string alunoCodigo);
        Task<IEnumerable<NotaConceitoBimestreComponenteDto>> ObterNotasFinaisAlunoAsync(string[] turmasCodigos, string alunoCodigo);
        Task<IEnumerable<NotaConceitoBimestreComponenteDto>> ObterNotasAlunoAnoAsync(string turmaCodigo, string alunoCodigo);
        Task<IEnumerable<AlunosFechamentoNotaDto>> ObterComNotaLancadaPorPeriodoEscolarUE(long ueId, long periodoEscolarId);
        Task<IEnumerable<FechamentoNotaAprovacaoDto>> ObterNotasEmAprovacaoPorIdsFechamento(IEnumerable<long> Ids);        
        Task<IEnumerable<FechamentoNotaAlunoAprovacaoDto>> ObterPorFechamentosTurmaAlunoCodigo(long[] fechamentosTurmaDisciplinaId, string alunoCodigo);
        Task<IEnumerable<NotaConceitoBimestreComponenteDto>> ObterNotasPorTurmaCodigoEBimestreAsync(string turmaCodigo, int bimestre, DateTime? dataMatricula = null,
            DateTime? dataSituacao = null, int? anoLetivo = null, long? tipoCalendario = null);        
        Task<IEnumerable<NotaConceitoBimestreComponenteDto>> ObterPorFechamentoTurmaAlunoEDisciplinaAsync(long fechamentoTurmaId, string alunoCodigo, long componenteCurricularId);
    }
}
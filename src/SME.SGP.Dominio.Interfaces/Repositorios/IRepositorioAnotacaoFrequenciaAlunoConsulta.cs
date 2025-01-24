using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos.AnotacaoFrequenciaAluno;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioAnotacaoFrequenciaAlunoConsulta : IRepositorioBase<AnotacaoFrequenciaAluno>
    {
        Task<AnotacaoFrequenciaAluno> ObterPorAlunoAula(string codigoAluno, long aulaId);
        Task<IEnumerable<AnotacaoFrequenciaAluno>> ObterPorAulaIdRegistroExcluido(long aulaId);
        Task<IEnumerable<string>> ListarAlunosComAnotacaoFrequenciaNaAula(long aulaId);
        Task<IEnumerable<JustificativaAlunoDto>> ObterPorTurmaAlunoComponenteCurricular(long turmaId, long codigoAluno, long componenteCurricularId);
        Task<IEnumerable<JustificativaAlunoDto>> ObterPorTurmaAlunoComponenteCurricularBimestre(long turmaId, long alunoCodigo, long componenteCurricularId, int bimestre);
        Task<PaginacaoResultadoDto<JustificativaAlunoDto>> ObterPorTurmaAlunoComponenteCurricularBimestrePaginado(long turmaId, long alunoCodigo, long componenteCurricularId, int bimestre,Paginacao paginacao, int? semestre);
        Task<IEnumerable<AnotacaoAlunoAulaDto>> ListarAlunosComAnotacaoFrequenciaPorPeriodo(string turmaCodigo, DateTime dataInicio, DateTime dataFim);
        Task<IEnumerable<AnotacaoAlunoAulaPorPeriodoDto>> ObterPorAlunoPorPeriodo(string codigoAluno, DateTime dataInicio, DateTime dataFim);
    }
}
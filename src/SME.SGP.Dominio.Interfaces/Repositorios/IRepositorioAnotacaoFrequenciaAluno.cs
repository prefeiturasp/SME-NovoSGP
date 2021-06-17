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
        Task<IEnumerable<JustificativaAlunoDto>> ObterPorTurmaAlunoComponenteCurricular(long turmaId, long codigoAluno, long componenteCurricularId);
        Task<IEnumerable<JustificativaAlunoDto>> ObterPorTurmaAlunoComponenteCurricularBimestre(long turmaId, long alunoCodigo, long componenteCurricularId, int bimestre);
        Task<PaginacaoResultadoDto<JustificativaAlunoDto>> ObterPorTurmaAlunoComponenteCurricularBimestrePaginado(long turmaId, long alunoCodigo, long componenteCurricularId, int bimestre,Paginacao paginacao);
    }
}
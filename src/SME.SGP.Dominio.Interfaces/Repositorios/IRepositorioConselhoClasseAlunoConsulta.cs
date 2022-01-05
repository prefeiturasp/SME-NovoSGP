using SME.SGP.Infra;
using SME.SGP.Infra.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioConselhoClasseAlunoConsulta : IRepositorioBase<ConselhoClasseAluno>
    {
        Task<ConselhoClasseAluno> ObterPorFechamentoAsync(long fechamentoTurmaId, string alunoCodigo);
        Task<ConselhoClasseAluno> ObterPorFiltrosAsync(string codigoTurma, string codigoAluno, int bimestre, bool EhFinal);
        Task<ConselhoClasseAluno> ObterPorConselhoClasseAlunoCodigoAsync(long conselhoClasseId, string alunoCodigo);
        Task<ConselhoClasseAluno> ObterPorPeriodoAsync(string alunoCodigo, long turmaId, long periodoEscolarId);
        Task<IEnumerable<NotaConceitoFechamentoConselhoFinalDto>> ObterNotasFinaisAlunoAsync(string[] turmasCodigos, string alunoCodigo);
        Task<IEnumerable<long>> ObterComponentesPorAlunoTurmaBimestreAsync(string alunoCodigo, int bimestre, long turmaId);
        Task<long> ObterConselhoClasseAlunoIdAsync(long conselhoClasseId, string alunoCodigo);
        Task<IEnumerable<ConselhoClasseFechamentoAlunoDto>> ObterConselhoClasseAlunosPorTurma(string turmaCodigo);
    }
}
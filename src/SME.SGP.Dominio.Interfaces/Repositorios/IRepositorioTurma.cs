using System.Collections.Generic;
using System.Threading.Tasks;
using SME.SGP.Infra;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioTurma
    {
        IEnumerable<Turma> MaterializarCodigosTurma(string[] idTurmas, out string[] codigosNaoEncontrados);

        Task<Turma> ObterPorCodigo(string turmaCodigo);

        Task<Turma> ObterPorId(long id);

        Task<Turma> ObterTurmaComUeEDrePorCodigo(string turmaCodigo);

        Task<Turma> ObterTurmaComUeEDrePorId(long turmaId);       

        Task<bool> ObterTurmaEspecialPorCodigo(string turmaCodigo);

        Task<IEnumerable<Turma>> SincronizarAsync(IEnumerable<Turma> entidades, IEnumerable<Ue> ues);

        Task<long> ObterTurmaIdPorCodigo(string turmaCodigo);
        Task<IEnumerable<Turma>> ObterTurmasInfantilNaoDeProgramaPorAnoLetivoAsync(int anoLetivo);
        Task<IEnumerable<Turma>> ObterPorIdsAsync(long[] ids);
        Task<ObterTurmaSimplesPorIdRetornoDto> ObterTurmaSimplesPorId(long id);
        Task<IEnumerable<Turma>> ObterPorCodigosAsync(string[] codigos);
        Task<IEnumerable<long>> ObterTurmasPorUeAnos(string ueCodigo, int anoLetivo, string[] anos, int modalidadeId);
    }
}
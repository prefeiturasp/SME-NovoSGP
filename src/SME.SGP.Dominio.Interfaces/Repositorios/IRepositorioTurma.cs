using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioTurma
    {
        IEnumerable<Turma> MaterializarCodigosTurma(string[] idTurmas, out string[] codigosNaoEncontrados);

        Task<Turma> ObterPorCodigo(string turmaCodigo);

        Task<Turma> ObterPorId(long id);

        Task<Turma> ObterTurmaComUeEDrePorCodigo(string turmaCodigo);

        Task<Turma> ObterTurmaComUeEDrePorId(long turmaId);

        Task<IEnumerable<Turma>> Sincronizar(IEnumerable<Turma> entidades, IEnumerable<Ue> ues);
        Task<bool> ObterTurmaEspecialPorCodigo(string turmaCodigo);
    }
}
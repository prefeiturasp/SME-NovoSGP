using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioTurma
    {
        IEnumerable<Turma> MaterializarCodigosTurma(string[] idTurmas, out string[] codigosNaoEncontrados);

        Turma ObterPorCodigo(string turmaCodigo);

        Turma ObterPorId(long id);

        Turma ObterTurmaComUeEDrePorCodigo(string turmaCodigo);

        Turma ObterTurmaComUeEDrePorId(long turmaId);
        Task<IEnumerable<Turma>> SincronizarAsync(IEnumerable<Turma> entidades, IEnumerable<Ue> ues);
    }
}
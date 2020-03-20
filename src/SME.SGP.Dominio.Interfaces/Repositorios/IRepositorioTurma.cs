using System.Collections.Generic;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioTurma
    {
        IEnumerable<Turma> MaterializarCodigosTurma(string[] idTurmas, out string[] codigosNaoEncontrados);

        Turma ObterPorCodigo(string turmaCodigo);

        Turma ObterPorId(long id);

        Turma ObterTurmaComUeEDrePorId(string turmaId);

        IEnumerable<Turma> Sincronizar(IEnumerable<Turma> entidades, IEnumerable<Ue> ues);
    }
}
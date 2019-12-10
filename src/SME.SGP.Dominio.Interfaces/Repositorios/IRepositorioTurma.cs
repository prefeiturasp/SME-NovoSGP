using System.Collections.Generic;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioTurma
    {
        Turma ObterPorId(string turmaId);

        Turma ObterTurmaComUeEDrePorId(string turmaId);

        IEnumerable<Turma> Sincronizar(IEnumerable<Turma> entidades, IEnumerable<Ue> ues);
    }
}
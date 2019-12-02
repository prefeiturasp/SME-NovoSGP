using SME.SGP.Dominio.Entidades;
using System.Collections.Generic;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioUe
    {
        Ue ObterUEPorTurma(string turmaId);

        IEnumerable<Ue> Sincronizar(IEnumerable<Ue> entidades, IEnumerable<Dre> dres);
    }
}
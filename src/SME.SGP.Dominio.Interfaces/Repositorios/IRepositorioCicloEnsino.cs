using System.Collections.Generic;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioCicloEnsino : IRepositorioBase<CicloEnsino>

    {
        void Sincronizar(IEnumerable<CicloEnsino> ciclosEnsino);
    }
}
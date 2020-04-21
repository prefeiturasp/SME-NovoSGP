using System.Collections.Generic;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioTipoEscola : IRepositorioBase<TipoEscolaEol>
    {
        void Sincronizar(IEnumerable<TipoEscolaEol> tiposEscolas);
    }
}
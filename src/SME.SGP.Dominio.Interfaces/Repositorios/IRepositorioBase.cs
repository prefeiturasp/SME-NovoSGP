using System.Collections.Generic;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioBase<T> where T : EntidadeBase
    {
        IEnumerable<T> Listar();

        T ObterPorId(long id);

        void Remover(long id);

        void Remover(T entidade);

        long Salvar(T entidade);
    }
}
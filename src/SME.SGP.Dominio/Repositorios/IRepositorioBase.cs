using System.Collections.Generic;

namespace SME.SGP.Dominio
{
    public interface IRepositorioBase<T> where T : EntidadeBase
    {
        IEnumerable<T> Listar();
        T ObterPorId(long id);

        void Remover(long id);

        long Salvar(T entidade);
    }
}
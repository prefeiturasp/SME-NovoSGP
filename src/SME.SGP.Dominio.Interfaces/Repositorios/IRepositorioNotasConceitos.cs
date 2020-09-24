using System.Collections.Generic;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioNotasConceitos : IRepositorioBase<NotaConceito>
    {
        IEnumerable<NotaConceito> ObterNotasPorAlunosAtividadesAvaliativas(IEnumerable<long> atividadesAvaliativas, IEnumerable<string> alunosIds, long disciplinaId);
    }
}
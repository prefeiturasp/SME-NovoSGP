using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioNotasConceitos : IRepositorioBase<NotaConceito>
    {
        IEnumerable<NotaConceito> ObterNotasPorAlunosAtividadesAvaliativas(IEnumerable<long> atividadesAvaliativas,
            IEnumerable<string> alunosIds, string disciplinaId);

        Task<IEnumerable<NotaConceito>> ObterNotasPorAlunosAtividadesAvaliativasAsync(long[] atividadesAvaliativasId,
            string[] alunosIds, string componenteCurricularId);

        Task<NotaConceito> ObterNotasPorAtividadeIdCodigoAluno(long atividadeId,string codigoAluno);
        Task<NotaConceito> ObterNotasPorId(long id);
    }
}
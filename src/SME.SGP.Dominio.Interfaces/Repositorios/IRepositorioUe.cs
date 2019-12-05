using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioUe
    {
        Task<IEnumerable<Modalidade>> ObterModalidadesPorUe(string ueCodigo);

        Ue ObterUEPorTurma(string turmaId);

        IEnumerable<Ue> Sincronizar(IEnumerable<Ue> entidades, IEnumerable<Dre> dres);
    }
}
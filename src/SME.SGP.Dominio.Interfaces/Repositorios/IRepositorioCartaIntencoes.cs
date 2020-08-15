using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioCartaIntencoes : IRepositorioBase<CartaIntencoes>
    {
        Task<IEnumerable<CartaIntencoes>> ObterPorTurmaEComponente(string turmaCodigo, long componenteCurricularId);
    }
}

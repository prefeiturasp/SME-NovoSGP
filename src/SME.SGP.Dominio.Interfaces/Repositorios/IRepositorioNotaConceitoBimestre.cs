using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioNotaConceitoBimestre : IRepositorioBase<NotaConceitoBimestre>
    {
        Task<IEnumerable<NotaConceitoBimestre>> ObterPorFechamentoTurma(long fechamentoId);
    }
}
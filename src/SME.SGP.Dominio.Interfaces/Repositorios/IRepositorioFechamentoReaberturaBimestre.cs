using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioFechamentoReaberturaBimestre : IRepositorioBase<FechamentoReaberturaBimestre>
    {
        Task<IEnumerable<FechamentoReaberturaBimestre>> ObterPorFechamentoReaberturaIdAsync(long fechamentoReaberturaId);
    }
}

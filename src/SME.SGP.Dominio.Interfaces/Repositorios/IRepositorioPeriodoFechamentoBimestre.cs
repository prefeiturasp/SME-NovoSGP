using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioPeriodoFechamentoBimestre
    {
        Task<IEnumerable<PeriodoFechamentoBimestre>> ObterBimestreParaAlteracaoHierarquicaAsync(long periodoEscolarId, long? dreId, DateTime inicioDoFechamento, DateTime finalDoFechamento);
        Task<long> SalvarAsync(PeriodoFechamentoBimestre entidade);
    }
}
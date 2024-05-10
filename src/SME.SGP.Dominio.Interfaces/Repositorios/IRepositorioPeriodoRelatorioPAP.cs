using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioPeriodoRelatorioPAP : IRepositorioBase<PeriodoRelatorioPAP>
    {
        Task<IEnumerable<PeriodosPAPDto>> ObterPeriodos(int anoLetivo);
        Task<PeriodoRelatorioPAP> ObterComPeriodosEscolares(long id);
        Task<bool> PeriodoEmAberto(long periodoRelatorioId, DateTime dataReferencia);
        Task<long> ObterIdPeriodoRelatorioPAP(int anoLetivo, int semestre, string tipoPeriodo);
    }
}

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioPeriodoFechamentoBimestre
    {
        Task<IEnumerable<PeriodoFechamentoBimestre>> ObterBimestreParaAlteracaoHierarquicaAsync(long periodoEscolarId, long? dreId, DateTime inicioDoFechamento, DateTime finalDoFechamento);
        Task<long> SalvarAsync(PeriodoFechamentoBimestre entidade);
        Task<PeriodoFechamentoBimestre> ObterPeridoFechamentoBimestrePorDreUeEData(ModalidadeTipoCalendario modalidadeTipoCalendario, DateTime dataInicio, int bimestre, long? dreId, long? ueId);
        Task<bool> ExistePeriodoFechamentoPorDataPeriodoEscolar(long periodoEscolarId, DateTime dataReferencia);
        Task<bool> ExistePeriodoFechamentoPorDataPeriodoIdEscolar(long periodoEscolarId, DateTime dataReferencia);
        Task<PeriodoFechamentoBimestre> ObterPeriodoFechamanentoPorCalendarioDreUeBimestre(long tipoCalendarioId, int bimestre, long dreId, long ueId);
    }
}
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioPeriodoFechamento : IRepositorioBase<PeriodoFechamento>
    {
        void AlterarPeriodosComHierarquiaInferior(DateTime inicioDoFechamento, DateTime finalDoFechamento, long periodoEscolarId, long? dreId);

        PeriodoFechamento ObterPorFiltros(long? tipoCalendarioId, long? dreId, long? ueId, long? turmaId);

        void SalvarBimestres(IEnumerable<PeriodoFechamentoBimestre> fechamentosBimestre, long fechamentoId);

        bool ValidaRegistrosForaDoPeriodo(DateTime inicioDoFechamento, DateTime finalDoFechamento, long fechamentoId, long periodoEscolarId, long? dreId);

        Task<bool> ExistePeriodoPorUeDataBimestre(long ueId, DateTime dataReferencia, int bimestre);
        Task<PeriodoFechamentoBimestre> ObterPeriodoFechamentoTurmaAsync(long ueId, long dreId, int bimestre);
    }
}
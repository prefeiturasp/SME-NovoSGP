using SME.SGP.Infra;
using SME.SGP.Infra.Dtos;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioPeriodoFechamento : IRepositorioBase<PeriodoFechamento>
    {
        Task<bool> ExistePeriodoPorUeDataBimestre(long ueId, DateTime dataReferencia, int bimestre);

        PeriodoFechamento ObterPorFiltros(long? tipoCalendarioId, long? turmaId);

        void SalvarBimestres(IEnumerable<PeriodoFechamentoBimestre> fechamentosBimestre, long fechamentoId);

        bool ValidaRegistrosForaDoPeriodo(DateTime inicioDoFechamento, DateTime finalDoFechamento, long fechamentoId, long periodoEscolarId, long? dreId);

        Task<PeriodoFechamento> ObterPeriodoPorUeDataBimestreAsync(long ueId, DateTime dataReferencia, int bimestre);
        
        Task<PeriodoFechamentoBimestre> ObterPeriodoFechamentoTurmaAsync(long ueId, long dreId, int anoLetivo, int bimestre, long? periodoEscolarId);
        Task<PeriodoFechamentoVigenteDto> ObterPeriodoVigentePorAnoModalidade(int anoLetivo, int modalidadeTipoCalendario);
        Task<IEnumerable<PeriodoFechamentoBimestre>> ObterPeriodosFechamentoEscolasPorDataFinal(DateTime dataFinal);
        Task<IEnumerable<PeriodoFechamentoBimestre>> ObterPeriodosFechamentoBimestrePorDataFinal(int modalidade, DateTime dataEncerramento);
        Task<IEnumerable<PeriodoFechamentoBimestre>> ObterPeriodosFechamentoBimestrePorDataInicio(int modalidade, DateTime dataAbertura);
    }
}
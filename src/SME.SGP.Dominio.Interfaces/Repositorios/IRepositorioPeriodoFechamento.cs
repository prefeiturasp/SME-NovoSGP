using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioPeriodoFechamento : IRepositorioBase<PeriodoFechamento>
    {
        PeriodoFechamento ObterPorFiltros(long? tipoCalendarioId, long? turmaId, Aplicacao aplicacao);
        Task<PeriodoFechamento> ObterPorFiltrosAsync(long? tipoCalendarioId, long? turmaId);
        void SalvarBimestres(IEnumerable<PeriodoFechamentoBimestre> fechamentosBimestre, long fechamentoId);
        void SalvarCiclosSondagem(IEnumerable<PeriodoFechamentoCicloSondagem> fechamentosBimestre, long fechamentoId);
        bool ValidaRegistrosForaDoPeriodo(DateTime inicioDoFechamento, DateTime finalDoFechamento, long fechamentoId, long periodoEscolarId, long? dreId);
        Task<PeriodoFechamentoVigenteDto> ObterPeriodoVigentePorAnoModalidade(int anoLetivo, int modalidadeTipoCalendario);
        Task<IEnumerable<PeriodoFechamentoBimestre>> ObterPeriodosFechamentoEscolasPorDataFinal(DateTime dataFinal);
        Task<IEnumerable<PeriodoFechamentoBimestre>> ObterPeriodosFechamentoBimestrePorDataFinal(int modalidade, DateTime dataEncerramento);
        Task<IEnumerable<PeriodoFechamentoBimestre>> ObterPeriodosFechamentoBimestrePorDataInicio(int modalidade, DateTime dataAbertura);        
    }
}
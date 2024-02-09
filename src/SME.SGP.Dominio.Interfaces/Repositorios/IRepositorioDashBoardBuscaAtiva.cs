using SME.SGP.Dominio.Enumerados;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioDashBoardBuscaAtiva
    {
        Task<IEnumerable<DadosGraficoMotivoAusenciaBuscaAtivaDto>> ObterDadosGraficoMotivoAusencia(int anoLetivo, Modalidade modalidade, long? ueId, long? dreId, int? semestre);
        Task<IEnumerable<DadosGraficoProcedimentoTrabalhoDreBuscaAtivaDto>> ObterDadosGraficoProcedimentoTrabalho(EnumProcedimentoTrabalhoBuscaAtiva tipoProcedimentoRealizado, int anoLetivo, Modalidade modalidade, long? ueId, long? dreId, int? semestre);
        Task<IEnumerable<DadosGraficoReflexoFrequenciaAnoTurmaBuscaAtivaDto>> ObterDadosGraficoReflexoFrequencia(int mes, int anoLetivo, Modalidade modalidade, long? ueId, long? dreId, int? semestre);
        Task<DateTime?> ObterDataUltimaConsolidacaoReflexoFrequencia();
    }
}
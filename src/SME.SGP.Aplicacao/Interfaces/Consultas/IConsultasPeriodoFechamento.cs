using SME.SGP.Infra;
using System;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public interface IConsultasFechamento
    {
        Task<FechamentoDto> ObterPorTipoCalendarioDreEUe(FiltroFechamentoDto fechamentoDto);
        Task<bool> TurmaEmPeriodoDeFechamento(string turmaCodigo, DateTime dataReferencia, int bimestre);
    }
}
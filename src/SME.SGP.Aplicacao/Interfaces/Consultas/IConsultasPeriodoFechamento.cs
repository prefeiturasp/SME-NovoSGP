using SME.SGP.Infra;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public interface IConsultasFechamento
    {
        Task<FechamentoDto> ObterPorTipoCalendarioDreEUe(FiltroFechamentoDto fechamentoDto);
    }
}
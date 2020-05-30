using SME.SGP.Infra;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IServicoPeriodoFechamento
    {
        void AlterarPeriodosComHierarquiaInferior(PeriodoFechamento fechamento);

        Task<FechamentoDto> ObterPorTipoCalendarioDreEUe(long tipoCalendarioId, Dre dre, Ue ue);
        Task<FechamentoDto> ObterPorTipoCalendarioDreEUe(long tipoCalendarioId, string dreId, string ueId);

        Task Salvar(FechamentoDto fechamentoDto);
    }
}
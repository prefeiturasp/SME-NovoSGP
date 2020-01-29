using SME.SGP.Infra;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IServicoFechamento
    {
        Task AlterarPeriodosComHierarquiaInferior(PeriodoFechamento fechamento);

        Task<FechamentoDto> ObterPorTipoCalendarioDreEUe(long tipoCalendarioId, string dreId, string ueId);

        Task Salvar(FechamentoDto fechamentoDto);
    }
}
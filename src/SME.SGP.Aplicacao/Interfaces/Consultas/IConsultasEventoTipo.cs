using SME.SGP.Dto;
using SME.SGP.Infra;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Interfaces.Consultas
{
    public interface IConsultasEventoTipo
    {
        Task<PaginacaoResultadoDto<EventoTipoDto>> Listar(FiltroEventoTipoDto Filtro);

        EventoTipoDto ObterPorId(long id);
    }
}
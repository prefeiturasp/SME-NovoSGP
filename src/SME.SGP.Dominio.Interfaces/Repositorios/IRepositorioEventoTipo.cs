using SME.SGP.Dominio.Entidades;
using SME.SGP.Infra;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioEventoTipo : IRepositorioBase<EventoTipo>
    {
        Task<PaginacaoResultadoDto<EventoTipo>> ListarTipos(EventoLocalOcorrencia eventoLocalOcorrencia, EventoLetivo eventoLetivo, string descricao, Paginacao paginacao);

        EventoTipo ObtenhaTipoEventoFeriado();

        EventoTipo ObterTipoEventoPorTipo(TipoEvento tipoEvento);

        EventoTipo ObterPorCodigo(long id);
    }
}
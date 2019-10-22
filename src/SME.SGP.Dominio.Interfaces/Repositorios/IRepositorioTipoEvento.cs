using SME.SGP.Dominio.Entidades;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Infra;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces.Repositorios
{
    public interface IRepositorioEventoTipo : IRepositorioBase<EventoTipo>
    {
        Task<PaginacaoResultadoDto<EventoTipo>> ListarTipos(EventoLocalOcorrencia eventoLocalOcorrencia, EventoLetivo eventoLetivo, string descricao, Paginacao paginacao);
    }
}
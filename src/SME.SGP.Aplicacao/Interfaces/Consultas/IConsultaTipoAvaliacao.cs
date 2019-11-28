using SME.SGP.Infra;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public interface IConsultaTipoAvaliacao
    {
        Task<PaginacaoResultadoDto<TipoAvaliacaoDto>> ListarPaginado(string nome);

        TipoAtividadeAvaliativaCompletaDto ObterPorId(long id);
    }
}
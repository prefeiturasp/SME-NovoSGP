using SME.SGP.Infra;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public interface IConsultaTipoAvaliacao
    {
        Task<PaginacaoResultadoDto<TipoAvaliacaoCompletaDto>> ListarPaginado(string nome);

        TipoAvaliacaoCompletaDto ObterPorId(long id);
    }
}
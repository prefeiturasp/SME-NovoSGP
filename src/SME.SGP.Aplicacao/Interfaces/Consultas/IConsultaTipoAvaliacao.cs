using SME.SGP.Infra;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public interface IConsultaTipoAvaliacao
    {
        Task<PaginacaoResultadoDto<TipoAvaliacaoCompletaDto>> ListarPaginado(string nome, string descricao, bool? situacao);

        TipoAvaliacaoCompletaDto ObterPorId(long id);

        Task<TipoAvaliacaoCompletaDto> ObterTipoAvaliacaoBimestral();
    }
}
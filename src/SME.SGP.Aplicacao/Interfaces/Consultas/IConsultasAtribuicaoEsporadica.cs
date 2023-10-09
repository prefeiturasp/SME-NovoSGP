using SME.SGP.Infra;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Interfaces
{
    public interface IConsultasAtribuicaoEsporadica
    {
        Task<PaginacaoResultadoDto<AtribuicaoEsporadicaDto>> Listar(FiltroAtribuicaoEsporadicaDto filtro);

        Task<AtribuicaoEsporadicaCompletaDto> ObterPorId(long id);
    }
}
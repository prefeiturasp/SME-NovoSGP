using SME.SGP.Infra;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Interfaces
{
    public interface IConsultasAtribuicaoEsporadica
    {
        Task<PaginacaoResultadoDto<AtribuicaoEsporadicaDto>> Listar(FiltroAtribuicaoEsporadicaDto filtro);

        AtribuicaoEsporadicaDto ObterPorId(long id);
    }
}
using SME.SGP.Infra;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Interfaces
{
    public interface IConsultasRegistroPoa
    {
        RegistroPoaCompletoDto ObterPorId(long id);

        Task<PaginacaoResultadoDto<RegistroPoaDto>> ListarPaginado(RegistroPoaFiltroDto registroPoaFiltroDto);
    }
}